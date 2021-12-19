using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

    public struct Cell
    {
        public Rank rank;
        public File file;
    }

    public class ChessModel
    {
        public ChessBoard Board { get; private set; } = new ChessBoard();
        public ChessPieceColor ActivePlayer { get; private set; }

        public ChessModel()
        {
        }

        public ChessModel(ChessModel other)
        {
            this.Board = new ChessBoard(other.Board);
            this.ActivePlayer = other.ActivePlayer;
        }

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

        public bool MovePiece(ChessMove move)
        {
            return MovePiece(move.Source.Cell, move.Destination);
        }

        public bool MovePiece(Cell from, Cell to)
        {
            return MovePiece(from.rank, from.file, to.rank, to.file);
        }

        public bool MovePiece(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            if (!IsValidMove(fromRank, fromFile, toRank, toFile))
                return false;

            CommitMove(fromRank, fromFile, toRank, toFile);
            return true;
        }

        private void CommitMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            Board[toRank, toFile] = Board[fromRank, fromFile];
            Board[fromRank, fromFile] = null;
            AlternatePlayerTurn();
        }

        public bool WhiteIsInCheck()
        {
            return KingIsInCheck(ChessPieceColor.White);
        }

        public bool BlackIsInCheck()
        {
            return KingIsInCheck(ChessPieceColor.Black);
        }

        public bool KingIsInCheck(ChessPieceColor color)
        {
            var myKingPosition = GetPieceLocations().Where(pl => pl.Piece.Color == color && pl.Piece.Type == ChessPieceType.King).FirstOrDefault();
            if (myKingPosition.Piece == null)
            {
                return false;
            }

            var opponentPieces = GetPieceLocations().Where(pl => pl.Piece.Color != color);

            foreach(var p in opponentPieces)
            {
                if (IsValidPhysicalMove(p.Cell, myKingPosition.Cell))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsValidMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var pieceToMove = Board[fromRank, fromFile];

            if (pieceToMove == null || pieceToMove.Color != ActivePlayer)
                return false;

            if(!IsValidPhysicalMove(fromRank, fromFile, toRank, toFile))
            {
                return false;
            }

            // Check if the resulting position is valid (i.e. current player's king is not in check)
            var simulatedModel = new ChessModel(this);
            simulatedModel.CommitMove(fromRank, fromFile, toRank, toFile);
            return !simulatedModel.KingIsInCheck(ActivePlayer);
        }

        private bool IsValidPhysicalMove(Cell from, Cell to)
        {
            return IsValidPhysicalMove(from.rank, from.file, to.rank, to.file);
        }

        /**
         * This method calculates if the specified move is physically possible to make with
         * the piece. It does not consider whether the resulting position is a valid position
         * with regards to whether or not the King is in check.
         */
        private bool IsValidPhysicalMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var pieceToMove = Board[fromRank, fromFile];

            if (pieceToMove == null)
                return false;

            var pieceWiseRules = new Dictionary<ChessPieceType, Func<bool>>() {
                { ChessPieceType.Pawn, () => IsValidPawnMove(fromRank, fromFile, toRank, toFile) },
                { ChessPieceType.Bishop, () => IsValidBishopMove(fromRank, fromFile, toRank, toFile) },
                { ChessPieceType.Queen, () => IsValidBishopMove(fromRank, fromFile, toRank, toFile) || IsValidRookMove(fromRank, fromFile, toRank, toFile) },
                { ChessPieceType.King, () => IsValidKingMove(fromRank, fromFile, toRank, toFile) },
                { ChessPieceType.Rook, () => IsValidRookMove(fromRank, fromFile, toRank, toFile) },
                { ChessPieceType.Knight, () => IsValidKnightMove(fromRank, fromFile, toRank, toFile) },
            };

            if (!pieceWiseRules[pieceToMove.Type]())
                return false;

            var pieceToCapture = Board[toRank, toFile];
            if (pieceToCapture != null && pieceToCapture.Color == pieceToMove.Color)
                return false;

            return true;
        }

        public IEnumerable<ChessPieceLocation> GetPieceLocations()
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var r = (Rank)i;
                    var f = (File)j;
                    var p = Board[r, f];

                    if(p != null)
                    {
                        yield return new ChessPieceLocation { Piece = p, Cell = new Cell { rank = r, file = f } };
                    }
                }
            }
        }

        private bool IsValidKingMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var changeInRank = Math.Abs(fromRank - toRank);
            var changeInFile = Math.Abs(fromFile - toFile);

            return changeInRank <= 1 && changeInFile <= 1 && changeInFile + changeInRank > 0;
        }

        private bool IsValidPawnMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var pieceColor = Board[fromRank, fromFile].Color;
            var toPiece = Board[toRank, toFile];
            var isCapture = toPiece != null && toPiece.Color != pieceColor;

            var sign = pieceColor == ChessPieceColor.White ? 1 : -1;
            var startingRank = pieceColor == ChessPieceColor.White ? Rank.Two : Rank.Seven;
            var rankMaxMove = fromRank == startingRank && !isCapture ? 2 : 1;

            var changeInRank = toRank - fromRank;
            var changeInFile = toFile - fromFile;

            if (changeInRank * sign < 1 || changeInRank * sign > rankMaxMove)
                return false;

            if (!isCapture && changeInFile != 0)
                return false;

            if (isCapture && Math.Abs(changeInFile) != 1)
                return false;

            return true;
        }

        private bool IsValidBishopMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var changeInRank = Math.Abs(fromRank - toRank);
            var changeInFile = Math.Abs(fromFile - toFile);

            if (changeInFile != changeInRank)
                return false;

            var fileSign = fromFile < toFile ? 1 : -1;
            var rankSign = fromRank < toRank ? 1 : -1;

            for (var i = 1; i < changeInFile; i++)
            {
                var r = fromRank + i * rankSign;
                var f = fromFile + i * fileSign;
                if (Board[r, f] != null)
                    return false;
            }

            return true;
        }

        private bool IsValidKnightMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var changeInRank = Math.Abs(fromRank - toRank);
            var changeInFile = Math.Abs(fromFile - toFile);

            return (changeInRank == 2 && changeInFile == 1) || (changeInRank == 1 && changeInFile == 2);
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
