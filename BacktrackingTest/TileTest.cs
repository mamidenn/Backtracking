using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backtracking;

namespace BacktrackingTest
{
    [TestClass]
    public class TileTest
    {
        [TestMethod]
        public void EdgeIsNotPlayable()
        {
            var tile = new Tile(TileType.Edge);
            Assert.IsFalse(tile.IsPlayable);
        }
        [TestMethod]
        public void NormalIsPlayable()
        {
            var tile = new Tile(TileType.Normal);
            Assert.IsTrue(tile.IsPlayable);
        }
        [TestMethod]
        public void GoalIsPlayable()
        {
            var tile = new Tile(TileType.Goal);
            Assert.IsTrue(tile.IsPlayable);
        }
        [TestMethod]
        public void NewGoalIsNotOccupied()
        {
            var tile = new Tile(TileType.Goal);
            Assert.IsFalse(tile.IsOccupied);
        }
        [TestMethod]
        public void NewNormalIsNotOccupied()
        {
            var tile = new Tile(TileType.Normal);
            Assert.IsFalse(tile.IsOccupied);
        }
        [TestMethod]
        public void AddPieceMakesOccupied()
        {
            var tile = new Tile(TileType.Normal);
            tile.AddPiece();
            Assert.IsTrue(tile.IsOccupied);
        }
        [TestMethod]
        [ExpectedException(typeof(TileIsNotPlayableException))]
        public void CannotAddPieceToEdge()
        {
            var tile = new Tile(TileType.Edge);
            tile.AddPiece();
        }
        [TestMethod]
        [ExpectedException(typeof(TileOccupationException))]
        public void CannotAddPieceToOccupied()
        {
            var tile = new Tile(TileType.Normal, TileState.Occupied);
            tile.AddPiece();
        }
        [TestMethod]
        [ExpectedException(typeof(TileOccupationException))]
        public void CannotRemovePieceFromFree()
        {
            var tile = new Tile(TileType.Normal);
            tile.RemovePiece();
        }
        [TestMethod]
        [ExpectedException(typeof(TileIsNotPlayableException))]
        public void CannotRemovePieceFromEdge()
        {
            var tile = new Tile(TileType.Edge);
            tile.RemovePiece();
        }
        [TestMethod]
        public void RemovePieceMakesNotOccupied()
        {
            var tile = new Tile(TileType.Normal, TileState.Occupied);
            tile.RemovePiece();
            Assert.IsFalse(tile.IsOccupied);
        }
        [TestMethod]
        public void ParseXMakesNotPlayable()
        {
            var tile = Tile.Parse('X');
            Assert.IsFalse(tile.IsPlayable);
        }
        [TestMethod]
        public void ParseGMakesGoal()
        {
            var tile = Tile.Parse('G');
            Assert.IsTrue(tile.IsGoal);
        }
        [TestMethod]
        public void ParseOMakesPlayableAndOccupied()
        {
            var tile = Tile.Parse('O');
            Assert.IsTrue(tile.IsPlayable);
            Assert.IsTrue(tile.IsOccupied);
        }
        [TestMethod]
        public void ParseSpaceMakesPlayableAndNotOccupied()
        {
            var tile = Tile.Parse(' ');
            Assert.IsTrue(tile.IsPlayable);
            Assert.IsFalse(tile.IsOccupied);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseAThrowsException()
        {
            Tile.Parse('A');
        }
    }
}
