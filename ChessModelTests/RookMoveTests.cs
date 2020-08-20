using System;
using FluentAssertions;
using GameModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessModelTests
{
    [TestClass]
    public class RookMoveTests
    {
        private ChessModel CreateSimplifiedBoard()
        {
            var model = new ChessModel();
            model.Board[Rank.Four, File.D] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.White);
            return model;
        }

        [TestMethod]
        public void RookCanMoveUpwards()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Eight, File.D).Should().BeTrue();
        }

        [TestMethod]
        public void RookCannotMoveOnRankAndFileAtTheSameTime()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Eight, File.E).Should().BeFalse();
        }

        [TestMethod]
        public void RookMovingUpCannotJumpOverPieces()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Six, File.D] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            model.MovePiece(Rank.Four, File.D, Rank.Eight, File.D).Should().BeFalse();
        }

        [TestMethod]
        public void RookMovingDownCannotJumpOverPieces()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Three, File.D] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            model.MovePiece(Rank.Four, File.D, Rank.Two, File.D).Should().BeFalse();
        }

        [TestMethod]
        public void RookMovingLeftCannotJumpOverPieces()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Four, File.B] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            model.MovePiece(Rank.Four, File.D, Rank.Four, File.A).Should().BeFalse();
        }

        [TestMethod]
        public void RookMovingRightCannotJumpOverPieces()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Four, File.E] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            model.MovePiece(Rank.Four, File.D, Rank.Four, File.F).Should().BeFalse();
        }
    }
}
