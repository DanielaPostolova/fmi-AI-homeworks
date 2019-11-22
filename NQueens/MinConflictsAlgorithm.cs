using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NQueens
{
    internal class MinConflictsAlgorithm
    {
        public MinConflictsAlgorithm(int size)
        {
            this.Size = size;
            this.QueensPositions = Enumerable.Repeat(0, size).ToArray();
            this.MainDiagonals = Enumerable.Repeat(0, size - 1).Concat(Enumerable.Repeat(1, size)).ToArray();
            this.SecondDiagoanls = Enumerable.Repeat(1, size).Concat(Enumerable.Repeat(0, size - 1)).ToArray();
            this.Rows = Enumerable.Repeat(0, size).ToArray();
            this.Rows[0] = size;
        }

        private int Size { get; }

        private int[] QueensPositions { get; }
        private int[] MainDiagonals { get; }
        private int[] SecondDiagoanls { get; }
        private int[] Rows { get; }

        public void ExecuteAndPrintSolution()
        {
            var findSolution = Execute();
            while (!findSolution)
            {
                findSolution = Execute();
                
            }

            PrintSolution();
        }

        private bool Execute()
        {
            var timer = new Stopwatch();
            timer.Start();

            var hasConflicts = true;
            while (hasConflicts)
            {
                hasConflicts = false;
                for (var col = 0; col < QueensPositions.Length; col++)
                {
                    var row = QueensPositions[col];
                    var currentConflicts = CalculatePositionConflicts(row, col) - 3;

                    if(currentConflicts == 0) continue;

                    hasConflicts = true;
                    var newPos = FindBestPosition(row, col, currentConflicts);
                    Rows[row]--;
                    Rows[newPos]++;
                    MainDiagonals[Math.Abs(row - col - Size + 1)]--;
                    MainDiagonals[Math.Abs(newPos - col - Size + 1)]++;
                    SecondDiagoanls[row + col]--;
                    SecondDiagoanls[newPos + col]++;
                    QueensPositions[col] = newPos;
                }

                if (timer.Elapsed > TimeSpan.FromMilliseconds(500))
                {
                    return false;
                }
            }

            timer.Stop();
            return true;
        }

        private int FindBestPosition(int currentRow, int col, int currentConflicts)
        {
            var minConflicts = currentConflicts;

            var allMinConflictPositions = new List<int>() { currentRow };
            for (var i = 1; i < Size; i++)
            {
                var row = (currentRow + i) % Size;
                var conflicts = CalculatePositionConflicts(row, col);
                if (conflicts < minConflicts)
                {
                    allMinConflictPositions = new List<int>() { row };
                    minConflicts = conflicts;

                    if (conflicts == 0) break;
                }

                if (conflicts == minConflicts && row != currentRow)
                {
                    allMinConflictPositions.Add(row);
                }
            }

            var random = new Random();
            return allMinConflictPositions[random.Next(0, allMinConflictPositions.Count)];
        }

        private int CalculatePositionConflicts(int row, int col)
        {
            return Rows[row] + MainDiagonals[Math.Abs(row - col - Size + 1)] + SecondDiagoanls[row + col];
        }

        private void PrintSolution()
        {
            for (var row = 0; row < Size; row++)
            {
                var line = new List<string>();
                for (var col = 0; col < Size; col++)
                {
                    line.Add(QueensPositions[col] == row ? "*" : "_");
                }
                Console.WriteLine(string.Join(" ", line));
            }
        }
    }
}
