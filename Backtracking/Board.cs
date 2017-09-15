using System;
using System.Collections.Generic;
using System.Linq;

namespace Backtracking
{
    public class Board
    {
        /// <summary>
        /// Default Solitaire board with one goal tile in the center
        /// </summary>
        public static readonly string[] Default =
        {
            "XXXXXXXXX",
            "XXXoooXXX",
            "XXXoooXXX",
            "XoooooooX",
            "XoooGoooX",
            "XoooooooX",
            "XXXoooXXX",
            "XXXoooXXX",
            "XXXXXXXXX"
        };
        public static readonly string[] French =
        {
            "XXXXXXXXX",
            "XXXOOOXXX",
            "XXOOOOOXX",
            "XOOO OOOX",
            "XOOOOOOOX",
            "XOOOOOOOX",
            "XXOOOOOXX",
            "XXXOOOXXX",
            "XXXXXXXXX"
        };
        const int nDimension = 0;
        const int mDimension = 1;
        static readonly Direction[] possibleDirections = (Direction[])Enum.GetValues(typeof(Direction));
        readonly Stack<Move> moveTrace = new Stack<Move>();
        public List<Move> MoveTrace
        {
            get
            {
                return moveTrace.Reverse().ToList();
            }
        }

        readonly Tile[,] tiles;

        public int PieceCount { get { return countOccupiedTiles(); } }

        public int Height { get { return tiles.GetLength(nDimension); } }
        public int Width { get { return tiles.GetLength(mDimension); } }

        public bool IsSolved
        {
            get { return sameNumberOfPiecesAsGoals && allGoalsOccupied; }
        }

        public Tile this[int row, int column]
        {
            get { return tiles[row, column]; }
        }

        public Tile AtPosition(Position position)
        {
            return tiles[position.Row, position.Column];
        }

        bool sameNumberOfPiecesAsGoals
        {
            get
            {
                return goals.Count == 0 && PieceCount == 1 || PieceCount == goals.Count;
            }
        }

        bool allGoalsOccupied
        {
            get
            {
                bool premise = true;
                foreach (var goal in goals)
                {
                    if (!this.AtPosition(goal).IsOccupied)
                    {
                        premise = false;
                    }
                }
                return premise;
            }
        }

        List<Position> goals
        {
            get
            {
                List<Position> goals = new List<Position>();
                for (int row = 0; row < Height; row++)
                {
                    for (int column = 0; column < Width; column++)
                    {
                        if (tiles[row, column].IsGoal)
                        {
                            goals.Add(new Position(row, column));
                        }
                    }
                }
                return goals;
            }
        }

        Board(Tile[,] tiles)
        {
            this.tiles = tiles;
        }

        public static Board Parse(string[] board)
        {
            var tiles = tilesFromStringArray(board);
            return new Board(tiles);
        }

        static Tile[,] tilesFromStringArray(string[] board)
        {
            var boardHeight = board.Length;
            var boardWidth = board[0].Length;
            var tiles = new Tile[boardHeight, boardWidth];
            for (int row = 0; row < boardHeight; row++)
            {
                for (int column = 0; column < boardWidth; column++)
                {
                    char tileCode = board[row][column].ToString().ToUpper()[0];
                    tiles[row, column] = Tile.Parse(tileCode);
                }
            }
            return tiles;
        }
        
        int countOccupiedTiles()
        {
            int pieceCount = 0;
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (tiles[row, column].IsOccupied)
                    {
                        pieceCount++;
                    }
                }
            }
            return pieceCount;
        }

        public bool Solve(Action<Board> moveCallback = null, Action<Board> backtrackCallback = null)
        {
            bool solved = IsSolved;
            int row = 0;
            while (row < Height && !solved)
            {
                int column = 0;
                while (column < Width && !solved)
                {
                    foreach (Direction direction in possibleDirections)
                    {
                        var move = new Move(this, new Position(row, column), direction);
                        if (move.IsValid)
                        {
                            playMove(move, moveCallback);
                            solved = Solve(moveCallback, backtrackCallback);
                            if (!solved)
                            {
                                undoLastMove(backtrackCallback);
                            }
                        }
                    }
                    column++;
                }
                row++;
            }
            return solved;
        }

        void playMove(Move move, Action<Board> moveCallback)
        {
            move.Play();
            moveTrace.Push(move);
            if (moveCallback != null)
            {
                moveCallback.Invoke(this);
            }
        }

        void undoLastMove(Action<Board> backtrackCallback)
        {
            var lastMove = moveTrace.Pop();
            lastMove.Undo();
            if (backtrackCallback != null)
            {
                backtrackCallback.Invoke(this);
            }
        }

        public override string ToString()
        {
            string output = String.Empty;
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    output += tiles[row, column];
                }
                output += Environment.NewLine;
            }
            return output;
        }

        int maxMovesCount { get { return PieceCount - 1; } }

        internal Position GetPosition(Tile tile)
        {
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (tile == tiles[row, column])
                    {
                        return new Position(row, column);
                    }
                }
            }
            throw new TileNotOnBoardException();
        }
    }
}
