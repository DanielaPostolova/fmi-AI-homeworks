using System;
using System.Linq;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Who plays first? (Type '0' for computer or '1' for human)");
            var isHumanFirst = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            var algorithm = new MinimaxAlgorithm();

            if (isHumanFirst == 1)
            {
                ReadMove(algorithm);
                algorithm.PrintBoard();
            }

            while (true)
            {
                algorithm.ComputerPlay();
                algorithm.PrintBoard();
                if (algorithm.IsGameOver())
                {
                    algorithm.PrintResult();
                    break;
                }

                // Human move
                ReadMove(algorithm);
                algorithm.PrintBoard();

                if (algorithm.IsGameOver())
                {
                    algorithm.PrintResult();
                    break;
                }
            }
        }

        private static void ReadMove(MinimaxAlgorithm algorithm)
        {
            var move = Console.ReadLine();
            var indexes = move.Split(',').Select(int.Parse).ToArray();
            var isValid = algorithm.Mark(indexes[0], indexes[1]);
            while (!isValid)
            {
                Console.WriteLine("Invalid move!");
                move = Console.ReadLine();
                indexes = move.Split(',').Select(int.Parse).ToArray();
                isValid = algorithm.Mark(indexes[0], indexes[1]);
            }
        }
    }
}
