using System;
using System.Collections.Generic;
using System.Threading;

namespace Backtracking
{
    class Program
    {
        const int DisplayNthMove = 1;

        static void Main(string[] args)
        {
            var boardString = Board.Default;
            var board = Board.Parse(boardString);
            int movesCount = 0;
            Action<Board> moveCallback = (b) =>
            {
                movesCount++;
                if (movesCount % DisplayNthMove != 0) { return; }
                Console.SetCursorPosition(0, 0);
                Console.Write(b);
                Console.SetCursorPosition(11, 0);
                Console.Write("Pieces: {0}{1}", new string('=', b.PieceCount), new string(' ', 37 - b.PieceCount));
                Console.SetCursorPosition(11, 1);
                Console.Write("Moves tried: {0}", movesCount);
            };
            try
            {
                List<Move> moves = board.Solve(moveCallback);
                board = Board.Parse(boardString);
                Console.Clear();
                Console.Write(board);
                Console.SetCursorPosition(11, 1);
                Console.Write("Moves tried: {0}", movesCount++);
                foreach (var move in moves)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.25));
                    move.Replay(board);
                    Console.SetCursorPosition(0, 0);
                    Console.Write(board);
                }
            }
            catch (BoardNotSolvableException)
            {
                Console.WriteLine("Board can't be solved.");
            }
            Console.ReadLine();
        }
    }
}
