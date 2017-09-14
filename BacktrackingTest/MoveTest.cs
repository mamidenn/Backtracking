using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backtracking;

namespace BacktrackingTest
{
    [TestClass]
    public class MoveTest
    {
        static readonly string[] boardCast = new []{
            "XXXXXX",
            "XOO  X",
            "XXXXXX"
        };
        readonly Board board = Board.Parse(boardCast);

        [TestMethod]
        public void OverPieceToEmptyIsValid()
        {
            var move = new Move(board, new Position(1, 1), Direction.Right);
            Assert.IsTrue(move.IsValid);
        }
        [TestMethod]
        public void OverEmptyToEmptyIsNotValid()
        {
            var move = new Move(board, new Position(1, 2), Direction.Right);
            Assert.IsFalse(move.IsValid);
        }
        [TestMethod]
        public void OverPieceToEdgeIsNotValid()
        {
            var move = new Move(board, new Position(1, 2), Direction.Left);
            Assert.IsFalse(move.IsValid);
        }
        [TestMethod]
        public void OverEdgeIsNotValid()
        {
            var move = new Move(board, new Position(1, 1), Direction.Up);
            Assert.IsFalse(move.IsValid);
        }
        [TestMethod]
        public void MoveRemovesPiece()
        {
            var move = new Move(board, new Position(1, 1), Direction.Right);
            move.Play();
            Assert.IsFalse(board.Tiles[1, 1].IsOccupied);
        }
        [TestMethod]
        public void MoveRemovesObstacle()
        {
            var move = new Move(board, new Position(1, 1), Direction.Right);
            move.Play();
            Assert.IsFalse(board.Tiles[1, 2].IsOccupied);
        }
        [TestMethod]
        public void MoveAddsPiece()
        {
            var move = new Move(board, new Position(1, 1), Direction.Right);
            move.Play();
            Assert.IsTrue(board.Tiles[1, 3].IsOccupied);
        }
    }
}
