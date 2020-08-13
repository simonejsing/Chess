using System;

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
        Fire = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7,
    }

    public class ChessModel
    {
        public ChessBoard Board { get; private set; } = new ChessBoard();

        public void Initialize()
        {
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
    }
}
