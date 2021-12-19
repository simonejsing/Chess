using System;
using FluentAssertions;
using GameModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessModelTests
{
    [TestClass]
    public class CheckTests
    {
        private ChessModel CreateBoardWhereWhiteKingIsInCheck()
        {
            var model = new ChessModel();
            model.Board[Rank.Six, File.A] = new ChessPiece(ChessPieceType.King, ChessPieceColor.Black);
            model.Board[Rank.Three, File.A] = new ChessPiece(ChessPieceType.King, ChessPieceColor.White);
            model.Board[Rank.Four, File.B] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.Black);
            model.Board[Rank.Six, File.D] = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.White);
            return model;
        }

        [TestMethod]
        public void WhiteKingIsInCheck()
        {
            var model = CreateBoardWhereWhiteKingIsInCheck();
            model.WhiteIsInCheck().Should().BeTrue();
        }

        [TestMethod]
        public void WhenKingIsInCheckNormalMovesAreInvalid()
        {
            var model = CreateBoardWhereWhiteKingIsInCheck();
            model.MovePiece(Rank.Six, File.D, Rank.Five, File.F).Should().BeFalse();
        }

        [TestMethod]
        public void WhenKingIsInCheckMovesOutOfCheck()
        {
            var model = CreateBoardWhereWhiteKingIsInCheck();
            model.MovePiece(Rank.Three, File.A, Rank.Four, File.A).Should().BeTrue();
        }

        [TestMethod]
        public void WhenKingIsInCheckCaptureAttackingPiece()
        {
            var model = CreateBoardWhereWhiteKingIsInCheck();
            model.MovePiece(Rank.Three, File.A, Rank.Four, File.B).Should().BeTrue();
        }

        [TestMethod]
        public void WhenKingIsInCheckBlockAttackingPiece()
        {
            var model = new ChessModel();
            model.Board[Rank.Six, File.A] = new ChessPiece(ChessPieceType.King, ChessPieceColor.Black);
            model.Board[Rank.Three, File.A] = new ChessPiece(ChessPieceType.King, ChessPieceColor.White);
            model.Board[Rank.Seven, File.E] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.Black);
            model.Board[Rank.Three, File.D] = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.White);
            model.MovePiece(Rank.Three, File.D, Rank.Four, File.B).Should().BeTrue();
        }
    }
}
