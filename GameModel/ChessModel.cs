﻿using System;

namespace GameModel
{
    public enum File
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6,
        H = 7,
    }

    public enum Rank
    {
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7,
    }

    public class ChessModel
    {
        public ChessBoard Board { get; private set; } = new ChessBoard();
        public ChessPieceColor ActivePlayer { get; private set; }

        public void Initialize()
        {
            ActivePlayer = ChessPieceColor.White;

            foreach (File file in Enum.GetValues(typeof(File)))
            {
                Board[Rank.Two, file] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
                Board[Rank.Seven, file] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            }

            Board[Rank.One, File.A] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.White);
            Board[Rank.One, File.B] = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.White);
            Board[Rank.One, File.C] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            Board[Rank.One, File.D] = new ChessPiece(ChessPieceType.Queen, ChessPieceColor.White);
            Board[Rank.One, File.E] = new ChessPiece(ChessPieceType.King, ChessPieceColor.White);
            Board[Rank.One, File.F] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            Board[Rank.One, File.G] = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.White);
            Board[Rank.One, File.H] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.White);

            Board[Rank.Eight, File.A] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.Black);
            Board[Rank.Eight, File.B] = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.Black);
            Board[Rank.Eight, File.C] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.Black);
            Board[Rank.Eight, File.D] = new ChessPiece(ChessPieceType.Queen, ChessPieceColor.Black);
            Board[Rank.Eight, File.E] = new ChessPiece(ChessPieceType.King, ChessPieceColor.Black);
            Board[Rank.Eight, File.F] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.Black);
            Board[Rank.Eight, File.G] = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.Black);
            Board[Rank.Eight, File.H] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.Black);
        }

        public bool MovePiece(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var pieceToMove = Board[fromRank, fromFile];

            if (pieceToMove.Color != ActivePlayer)
                return false;

            if (pieceToMove.Type == ChessPieceType.Rook)
            {
                if (!IsValidRookMove(fromRank, fromFile, toRank, toFile))
                    return false;
            }

            var pieceToCapture = Board[toRank, toFile];
            if (pieceToCapture != null && pieceToCapture.Color == ActivePlayer)
                return false;

            Board[toRank, toFile] = pieceToMove;
            Board[fromRank, fromFile] = null;

            AlternatePlayerTurn();
            return true;
        }

        private bool IsValidRookMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            if (fromRank != toRank && fromFile != toFile)
                return false;

            if (fromRank != toRank)
            {
                var lowerRank = fromRank < toRank ? fromRank : toRank;
                var upperRank = fromRank < toRank ? toRank : fromRank;
                for (var r = lowerRank + 1; r < upperRank; r++)
                {
                    if (Board[r, fromFile] != null)
                        return false;
                }
            }

            if (fromFile != toFile)
            {
                var lowerFile = fromFile < toFile ? fromFile : toFile;
                var upperFile = fromFile < toFile ? toFile : fromFile;
                for (var f = lowerFile + 1; f < upperFile; f++)
                {
                    if (Board[fromRank, f] != null)
                        return false;
                }
            }

            return true;
        }

        private void AlternatePlayerTurn()
        {
            ActivePlayer = ActivePlayer == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White;
        }
    }
}
