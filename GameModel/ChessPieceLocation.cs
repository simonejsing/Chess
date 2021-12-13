using System;
using System.Collections.Generic;
using System.Text;

namespace GameModel
{
    public struct ChessPieceLocation
    {
        public ChessPiece Piece { get; set; }
        public Cell Cell { get; set; }

        public IEnumerable<Cell> ValidMoves(ChessModel model)
        {
            for (var i = Rank.One; i <= Rank.Eight; i++)
            {
                for (var j = File.A; j <= File.H; j++)
                {
                    var validMove = model.IsValidMove(Cell.rank, Cell.file, i, j);
                    if (validMove)
                    {
                        yield return new Cell { rank = i, file = j };
                    }
                }
            }
        }
    }
}
