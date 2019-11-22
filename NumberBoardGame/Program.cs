using System;

namespace NumberBoardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbersCount = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            var boardSize = (int)Math.Sqrt(numbersCount + 1);
            var zeroIndex = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            var board = new int[boardSize, boardSize];

            for (var r = 0; r < boardSize; r++)
            {
                var line = Console.ReadLine()?.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                for (var c = 0; c < line.Length; c++)
                {
                    board[r, c] = int.Parse(line[c]);
                }
            }

            var algorithm = new ItterativeDeepeningAStarAlgorithm(board, boardSize, zeroIndex);
            algorithm.Execute();

            Console.ReadLine();
        }
    }
}
