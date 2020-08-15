using System;
using FluentAssertions;
using GameModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessModelTests
{
    [TestClass]
    public class ChessModelTests
    {
        private ChessModel CreateModel()
        {
            var model = new ChessModel();
            model.Initialize();
            return model;
        }

        [TestMethod]
        public void WhiteIsStartingPlayer()
        {
            var model = CreateModel();
            model.ActivePlayer.Should().Be(ChessPieceColor.White);
        }

        [TestMethod]
        public void AfterLegalMoveCurrentPlayerChangesToBlack()
        {
            var model = CreateModel();
            model.MovePiece(Rank.Two, File.A, Rank.Four, File.A).Should().BeTrue();
            model.ActivePlayer.Should().Be(ChessPieceColor.Black);
        }

        [TestMethod]
        public void WhiteCannotMoveBlackPieces()
        {
            var model = CreateModel();
            model.MovePiece(Rank.Seven, File.A, Rank.Six, File.A).Should().BeFalse();
            model.ActivePlayer.Should().Be(ChessPieceColor.White);
        }
    }
}
