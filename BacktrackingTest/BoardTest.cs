using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backtracking;

namespace BacktrackingTest
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void ParseString()
        {
            string[] boardCast = new[] {
                "XG",
                " O"
            };
            var board = Board.Parse(boardCast);
            Assert.IsFalse(board.Tiles[0, 0].IsPlayable);
            Assert.IsTrue(board.Tiles[0, 1].IsPlayable);
            Assert.IsTrue(board.Tiles[0, 1].IsGoal);
            Assert.IsFalse(board.Tiles[0, 1].IsOccupied);
            Assert.IsTrue(board.Tiles[1, 0].IsPlayable);
            Assert.IsFalse(board.Tiles[1, 0].IsOccupied);
            Assert.IsTrue(board.Tiles[1, 1].IsPlayable);
            Assert.IsTrue(board.Tiles[1, 1].IsOccupied);
        }
        [TestMethod]
        public void SolveSimpleBoard()
        {
            string[] boardCast = new[] { "OOG" };
            var board = Board.Parse(boardCast);
            board.Solve();
            Assert.IsTrue(board.IsSolved);
        }
        [TestMethod]
        [ExpectedException(typeof(BoardNotSolvableException))]
        public void BoardNotSolvable()
        {
            string[] boardCast = new[] { "OOO " };
            var board = Board.Parse(boardCast);
            board.Solve();
        }
    }
}
