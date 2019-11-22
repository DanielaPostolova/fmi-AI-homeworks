using System;

namespace NQueens
{
    class Program
    {
        static void Main(string[] args)
        {
            var queensCount = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            if (queensCount < 4)
            {
                throw new InvalidOperationException();
            }

            var algorithm = new MinConflictsAlgorithm(queensCount);
            algorithm.ExecuteAndPrintSolution();
        }
    }
}
