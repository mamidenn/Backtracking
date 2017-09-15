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
            Assert.IsFalse(board[0, 0].IsPlayable);
            Assert.IsTrue(board[0, 1].IsPlayable);
            Assert.IsTrue(board[0, 1].IsGoal);
            Assert.IsFalse(board[0, 1].IsOccupied);
            Assert.IsTrue(board[1, 0].IsPlayable);
            Assert.IsFalse(board[1, 0].IsOccupied);
            Assert.IsTrue(board[1, 1].IsPlayable);
            Assert.IsTrue(board[1, 1].IsOccupied);
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
        public void BoardNotSolvable()
        {
            string[] boardCast = new[] { "OOO " };
            var board = Board.Parse(boardCast);
            board.Solve();
            Assert.IsFalse(board.IsSolved);
        }
    }
}
