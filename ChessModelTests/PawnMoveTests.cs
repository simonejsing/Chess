using System;
using FluentAssertions;
using GameModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessModelTests
{
    [TestClass]
    public class PawnMoveTests
    {
        private ChessModel CreateSimplifiedBoard(Rank rank = Rank.Two, File file = File.D)
        {
            var model = new ChessModel();
            model.Board[rank, File.D] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            return model;
        }

        private bool PawnMove(Rank fromRank, File fromFile, Rank toRank, File toFile)
        {
            var model = CreateSimplifiedBoard(fromRank, fromFile);
            return model.MovePiece(fromRank, fromFile, toRank, toFile);
        }

        [TestMethod]
        public void WhitePawnCanMoveOneFileAtATime()
        {
            PawnMove(Rank.Two, File.D, Rank.Three, File.D).Should().BeTrue();
        }

        [TestMethod]
        public void WhitePawnCanMoveTwoFilesFromStartingPosition()
        {
            PawnMove(Rank.Two, File.D, Rank.Four, File.D).Should().BeTrue();
        }

        [TestMethod]
        public void WhitePawnCannotMoveTwoFilesFromNoneStartingPosition()
        {
            PawnMove(Rank.Three, File.D, Rank.Five, File.D).Should().BeFalse();
        }

        [TestMethod]
        public void WhitePawnCannotMoveToAnotherFileWhenNotCapturing()
        {
            PawnMove(Rank.Three, File.D, Rank.Four, File.C).Should().BeFalse();
            PawnMove(Rank.Three, File.D, Rank.Four, File.E).Should().BeFalse();
        }

        private bool PawnCapture(Rank fromRank, File fromFile, Rank toRank, File toFile, ChessPieceColor color)
        {
            var model = CreateSimplifiedBoard(fromRank, fromFile);
            model.Board[toRank, toFile] = new ChessPiece(ChessPieceType.Bishop, color);
            return model.MovePiece(fromRank, fromFile, toRank, toFile);
        }

        [TestMethod]
        public void WhitePawnCanCaptureDiagonally()
        {
            PawnCapture(Rank.Three, File.D, Rank.Four, File.E, ChessPieceColor.Black).Should().BeTrue();
            PawnCapture(Rank.Three, File.D, Rank.Four, File.C, ChessPieceColor.Black).Should().BeTrue();
            PawnCapture(Rank.Three, File.D, Rank.Four, File.E, ChessPieceColor.White).Should().BeFalse();
            PawnCapture(Rank.Three, File.D, Rank.Four, File.C, ChessPieceColor.White).Should().BeFalse();
        }

        [TestMethod]
        public void WhitePawnCannotCaptureDiagonallyWhenMovingTwoSquaresForward()
        {
            PawnCapture(Rank.Two, File.D, Rank.Four, File.C, ChessPieceColor.Black).Should().BeFalse();
        }
    }
}
