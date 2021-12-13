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
            if (myPieces.Count == 0)
                return;

            // var opponentPieces = FindOpponentPieces(model);
            ChessPieceLocation pieceToMove = new ChessPieceLocation();
            var validMoves = new List<Cell>();

            while(myPieces.Count > 0)
            {
                var randomPieceNumber = random.Next(0, myPieces.Count);
                pieceToMove = myPieces[randomPieceNumber];

                validMoves = pieceToMove.ValidMoves(model).ToList();
                if (validMoves.Count != 0)
                {
                    break;
                }

                myPieces.Remove(pieceToMove);
            }

            if (validMoves.Count == 0)
                // This is stalemate
                return;

            var randomMoveNumber = random.Next(0, validMoves.Count);

            var moveToMake = validMoves[randomMoveNumber];
            model.MovePiece(pieceToMove.Cell, moveToMake);
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
