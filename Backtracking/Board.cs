using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

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

        public readonly Tile[,] Tiles;

        public int PieceCount { get { return countOccupiedTiles(); } }


        public int Height { get { return Tiles.GetLength(nDimension); } }
        public int Width { get { return Tiles.GetLength(mDimension); } }

        public bool IsSolved
        {
            get { return sameNumberOfPiecesAsGoals && allGoalsOccupied; }
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
                    if (!Tiles[goal.Row, goal.Column].IsOccupied)
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
                        if (Tiles[row, column].IsGoal)
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
            Tiles = tiles;
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
                    if (Tiles[row, column].IsOccupied)
                    {
                        pieceCount++;
                    }
                }
            }
            return pieceCount;
        }

        public List<Move> Solve(Action<Board> moveCallback = null, Action<Board> backtrackCallback = null)
        {
            var moveTrace = new Stack<Move>(maxMovesCount);
            var solved = Solve(moveTrace, moveCallback, backtrackCallback);
            if (!solved)
            {
                throw new BoardNotSolvableException();
            }
            return moveTrace.Reverse().ToList();
        }

        bool Solve(Stack<Move> moveTrace, Action<Board> moveCallback, Action<Board> backtrackCallback)
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
                            playMove(move, moveTrace, moveCallback);
                            solved = Solve(moveTrace, moveCallback, backtrackCallback);
                            if (!solved)
                            {
                                undoLastMove(moveTrace, backtrackCallback);
                            }
                        }
                    }
                    column++;
                }
                row++;
            }
            return solved;
        }

        void playMove(Move move, Stack<Move> moveTrace, Action<Board> moveCallback)
        {
            move.Play();
            moveTrace.Push(move);
            if (moveCallback != null)
            {
                moveCallback.Invoke(this);
            }
        }

        void undoLastMove(Stack<Move> moveTrace, Action<Board> backtrackCallback)
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
                    output += Tiles[row, column];
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
                    if (tile == Tiles[row, column])
                    {
                        return new Position(row, column);
                    }
                }
            }
            throw new TileNotOnBoardException();
        }
    }
}
