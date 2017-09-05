using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Backtracking
{
    class Board
    {
        const int defaultBoardSize = 9;

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
        /// <summary>
        /// The tiles of the board in a 2D array
        /// </summary>
        public Tile[,] Tiles { get; private set; }

        public int PieceCount
        {
            get
            {
                int count = 0;
                for (int n = 0; n < Tiles.GetLength(0); n++)
                {
                    for (int m = 0; m < Tiles.GetLength(1); m++)
                    {
                        if (Tiles[n, m].IsOccupied)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }
        public bool IsSolved
        {
            get
            {
                bool solved = false;
                if (_goals.Count == 0 && PieceCount == 1 || PieceCount == _goals.Count)
                {
                    solved = true;
                    foreach (var goal in _goals)
                    {
                        if (!Tiles[goal.Item1, goal.Item2].IsOccupied)
                        {
                            solved = false;
                        }
                    }
                }
                return solved;
            }
        }

        List<Tuple<int, int>> _goals
        {
            get
            {
                List<Tuple<int, int>> goals = new List<Tuple<int, int>>();
                for (int n = 0; n < Tiles.GetLength(0); n++)
                {
                    for (int m = 0; m < Tiles.GetLength(1); m++)
                    {
                        if (Tiles[n, m].IsGoal)
                        {
                            goals.Add(new Tuple<int, int>(n, m));
                        }
                    }
                }
                return goals;
            }
        }

        public Board()
        {
            Tiles = new Tile[defaultBoardSize, defaultBoardSize];
            for (int n = 0; n < defaultBoardSize; n++)
            {
                for (int m = 0; m < defaultBoardSize; m++)
                {
                    if (2 < n && n < defaultBoardSize - 3 && 0 < m && m < defaultBoardSize - 1 || 2 < m && m < defaultBoardSize - 3 && 0 < n && n < defaultBoardSize - 1)
                    {
                        TileState state;
                        TileType type = TileType.Normal;
                        if (n == defaultBoardSize / 2 && m == defaultBoardSize / 2)
                        {
                            state = TileState.Free;
                            type = TileType.Goal;
                        }
                        else
                        {
                            state = TileState.Occupied;
                        }
                        Tiles[n, m] = new Tile(type, state);
                    }
                    else
                    {
                        Tiles[n, m] = new Tile(TileType.Edge);
                    }
                }
            }
        }

        private Board(Tile[,] tiles)
        {
            Tiles = tiles;
        }

        public static Board Parse(string[] board)
        {
            var tiles = new Tile[board.Length, board[0].Length];
            for (int n = 0; n < board.Length; n++)
            {
                for (int m = 0; m < board[n].Length; m++)
                {
                    switch (board[n][m].ToString().ToUpper())
                    {
                        case "X":
                            tiles[n, m] = new Tile(TileType.Edge);
                            break;
                        case "O":
                            tiles[n, m] = new Tile(TileType.Normal, TileState.Occupied);
                            break;
                        case "G":
                            tiles[n, m] = new Tile(TileType.Goal, TileState.Free);
                            break;
                        case " ":
                            tiles[n, m] = new Tile(TileType.Normal, TileState.Free);

                            break;
                        default:
                            throw new ArgumentException("Tiles can only be X, O, G or space character.");
                    }
                }
            }
            return new Board(tiles);
        }

        public List<Move> Solve(Action<Board> moveCallback = null, Action<Board> backtrackCallback = null)
        {
            var moves = new Stack<Move>(PieceCount - 1);
            Solve(moves, moveCallback);
            if (!IsSolved)
            {
                throw new BoardNotSolvableException();
            }
            return moves.Reverse().ToList();
        }

        void Solve(Stack<Move> moves, Action<Board> moveCallback = null, Action<Board> backtrackCallback = null)
        {
            bool solved = IsSolved;
            var directions = Enum.GetValues(typeof(Direction));
            for (int i = 0; i < Tiles.GetLength(0) && !solved; i++)
            {
                for (int j = 0; j < Tiles.GetLength(1) && !solved; j++)
                {
                    foreach (Direction direction in directions)
                    {
                        var move = new Move(this, new Tuple<int, int>(i, j), direction);
                        if (move.IsValid)
                        {
                            move.Play();
                            moves.Push(move);
                            if (moveCallback != null)
                            {
                                moveCallback.Invoke(this);
                            }
                            Solve(moves, moveCallback, backtrackCallback);
                            solved = IsSolved;
                            if (!solved)
                            {
                                move.Undo();
                                moves.Pop();
                                if (backtrackCallback != null) 
                                {
                                    backtrackCallback.Invoke(this);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            string output = String.Empty;
            for (int n = 0; n < Tiles.GetLength(0); n++)
            {
                for (int m = 0; m < Tiles.GetLength(1); m++)
                {
                    output += Tiles[n, m];
                }
                output += Environment.NewLine;
            }
            return output;
        }
    }
}
