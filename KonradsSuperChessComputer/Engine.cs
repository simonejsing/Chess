using GameModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KonradsSuperChessComputer
{
    public class Engine: IChessEngine
    {
        private ChessPieceColor color;
        private Random random = new Random();

        public Engine(ChessPieceColor color)
        {
            this.color = color;
        }

        public void MakeMove(ChessModel model)
        {
            var myPieces = FindMyPieces(model);
            var validMoves = GetValidMoves(model, myPieces).ToList();

            if (!validMoves.Any())
                // This is stalemate
                return;

            var bestMove = GetBestMove(model, validMoves);
            model.MovePiece(bestMove);
        }

        private ChessMove GetBestMove(ChessModel model, IList<ChessMove> moves)
        {
            var evaluations = moves.Select(m => Tuple.Create(m, EvaluateMove(model, m)));
            var bestScore = evaluations.Max(e => e.Item2);
            var candidateMoves = evaluations.Where(e => e.Item2 == bestScore).Select(m => m.Item1).ToList();

            var randomMoveNumber = random.Next(0, candidateMoves.Count);
            return candidateMoves[randomMoveNumber];
        }

        private double EvaluateMove(ChessModel model, ChessMove move)
        {
            var simulateModel = new ChessModel(model);
            simulateModel.MovePiece(move);
            var materialScore = CalculateMaterialAdvantage(simulateModel);
            var positionalScore = CalculatePositionalAdvantage(simulateModel);

            return materialScore + positionalScore;
        }

        private double CalculatePositionalAdvantage(ChessModel position)
        {
            var score = 0.0;
            var pieceLocations = position.GetPieceLocations().Where(pl => pl.Piece.Color == color).ToList();

            // Calculate number of legal moves expressed as a ratio between 0 and 1
            foreach (var pieceLocation in pieceLocations)
            {
                var legalMoves = GetValidMoves(position, new[] { pieceLocation });
                var maximumLegalMoves = GetMaximumLegalMoves(pieceLocation.Piece.Type);

                var degreeOfFreedom = (double)legalMoves.Count() / (double)maximumLegalMoves;
                score += degreeOfFreedom;
            }

            return score / pieceLocations.Count;
        }

        private int GetMaximumLegalMoves(ChessPieceType type)
        {
            switch (type)
            {
                case ChessPieceType.Pawn: return 5;
                case ChessPieceType.Knight: return 8;
                case ChessPieceType.Bishop: return 14;
                case ChessPieceType.Rook: return 14;
                case ChessPieceType.Queen: return 28;
                case ChessPieceType.King: return 8;
            }

            return 0;
        }

        private double CalculateMaterialAdvantage(ChessModel position)
        {
            var score = 0.0;
            var pieceLocations = position.GetPieceLocations().ToList();

            foreach(var piece in pieceLocations.Select(pl => pl.Piece))
            {
                var pieceValue = EvaluatePiece(piece);

                if (piece.Color == ChessPieceColor.Black)
                {
                    score += pieceValue;
                }
                else
                {
                    score -= pieceValue;
                }
            }

            return score;
        }

        private double EvaluatePiece(ChessPiece piece)
        {
            switch (piece.Type)
            {
                case ChessPieceType.Pawn: return 1.0;
                case ChessPieceType.Knight: return 3.0;
                case ChessPieceType.Bishop: return 3.0;
                case ChessPieceType.Rook: return 5.0;
                case ChessPieceType.Queen: return 9.0;
                case ChessPieceType.King: return 9999.0;
            }

            return 0.0;
        }

        private IEnumerable<ChessMove> GetValidMoves(ChessModel model, IEnumerable<ChessPieceLocation> pieces)
        {
            return pieces.SelectMany(
                p => p.ValidMoves(model).Select(
                    m => new ChessMove()
                    {
                        Source = p,
                        Destination = m
                    })
                );
        }

        private IList<ChessPieceLocation> FindMyPieces(ChessModel model)
        {
            return model.GetPieceLocations().Where(p => p.Piece.Color == color).ToList();
        }

        private IList<ChessPieceLocation> FindOpponentPieces(ChessModel model)
        {
            return model.GetPieceLocations().Where(p => p.Piece.Color != color).ToList();
        }
    }
}
