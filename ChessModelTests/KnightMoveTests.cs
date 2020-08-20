using System;
using FluentAssertions;
using GameModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessModelTests
{
    [TestClass]
    public class KnightMoveTests
    {
        private ChessModel CreateSimplifiedBoard()
        {
            var model = new ChessModel();
            model.Board[Rank.Four, File.D] = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.White);
            return model;
        }

        [TestMethod]
        public void KnightCanMoveTwoOnRankAndOneOnFile()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.C).Should().BeTrue();
        }

        [TestMethod]
        public void KnightMustMoveTwoOnRankAndOneOnFile()
        {
            var model = CreateSimplifiedBoard();
            model.MovePiece(Rank.Four, File.D, Rank.Six, File.B).Should().BeFalse();
            model.MovePiece(Rank.Four, File.D, Rank.Five, File.C).Should().BeFalse();
        }
    }
}
