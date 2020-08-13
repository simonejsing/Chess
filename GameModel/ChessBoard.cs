using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GameModel
{
    public class ChessBoard
    {
        private readonly ChessPiece[,] board = new ChessPiece[8, 8];

        public ChessPiece this[Rank r, File f]
        {
            get => GetPiece(r, f);
            set => SetPiece(r, f, value);
        }

        private ChessPiece GetPiece(Rank r, File f)
        {
            return board[RankToIndex(r), FileToIndex(f)];
        }

        private void SetPiece(Rank r, File f, ChessPiece p)
        {
            board[RankToIndex(r), FileToIndex(f)] = p;
        }

        private int FileToIndex(File f)
        {
            return (int)f;
        }

        private int RankToIndex(Rank r)
        {
            return (int)r;
        }
    }
}
