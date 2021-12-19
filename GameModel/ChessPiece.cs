using System;
using System.Collections.Generic;
using System.Text;

namespace GameModel
{
    public enum ChessPieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King,
    }

    public enum ChessPieceColor
    {
        White,
        Black,
    }

    public class ChessPiece
    {
        public ChessPieceType Type { get; private set; }
        public ChessPieceColor Color { get; private set; }

        public ChessPiece(ChessPieceType type, ChessPieceColor color)
        {
            Type = type;
            Color = color;
        }

        public ChessPiece(ChessPiece other)
        {
            this.Type = other.Type;
            this.Color = other.Color;
        }
    }
}
