using System;
using FluentAssertions;
using GameModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessModelTests
{
    [TestClass]
    public class BishopMoveTests
    {
        private ChessModel CreateSimplifiedBoard()
        {
            var model = new ChessModel();
            model.Board[Rank.Four, File.D] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            return model;
        }

        [TestMethod]
        public void BishopCanMoveUpAndRight()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.F).Should().BeTrue();
        }

        [TestMethod]
        public void BishopCanMoveUpAndLeft()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.B).Should().BeTrue();
        }

        [TestMethod]
        public void BishopCanMoveDownAndRight()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Two, File.F).Should().BeTrue();
        }

        [TestMethod]
        public void BishopCanMoveDownAndLeft()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Two, File.B).Should().BeTrue();
        }

        [TestMethod]
        public void BishopCannotJumpOverMoveUpAndRight()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Five, File.E] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.F).Should().BeFalse();
        }

        [TestMethod]
        public void BishopCannotJumpOverMoveUpAndLeft()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Five, File.C] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.B).Should().BeFalse();
        }

        [TestMethod]
        public void BishopCannotJumpOverMoveDownAndRight()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Three, File.E] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            model.MovePiece(Rank.Four, File.D, Rank.Two, File.F).Should().BeFalse();
        }

        [TestMethod]
        public void BishopCannotJumpOverMoveDownAndLeft()
        {
            var model = CreateSimplifiedBoard();
            model.Board[Rank.Three, File.C] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            model.MovePiece(Rank.Four, File.D, Rank.Two, File.B).Should().BeFalse();
        }

        [TestMethod]
        public void BishopCannotMoveUpOnly()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.D).Should().BeFalse();
        }

        [TestMethod]
        public void BishopCannotMoveDownOnly()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Two, File.D).Should().BeFalse();
        }

        [TestMethod]
        public void BishopCannotMoveLeftOnly()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Four, File.B).Should().BeFalse();
        }

        [TestMethod]
        public void BishopCannotMoveRightOnly()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Four, File.F).Should().BeFalse();
        }

        [TestMethod]
        public void BishopMustMoveDiagonally()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.E).Should().BeFalse();
        }
    }
}
