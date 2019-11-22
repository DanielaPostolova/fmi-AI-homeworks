using System;
using System.Collections.Generic;

namespace NumberBoardGame
{
    public class ItterativeDeepeningAStarAlgorithm
    {
        private Tuple<int, int> _zeroPosition;

        public ItterativeDeepeningAStarAlgorithm(int[,] board, int size, int zeroIndex)
        {
            Board = board;
            Size = size;
            Path = new List<Direction>();
            ZeroIndex = zeroIndex == -1 ? size * size - 1 : zeroIndex;
        }

        private int[,] Board { get; set; }
        private int Size { get; set; }
        private int ZeroIndex { get; set; }

        private List<Direction> Path { get; set; }
        private Tuple<int, int> ZeroPosition
        {
            get => _zeroPosition ?? (_zeroPosition = FindZeroCoordinates(Board));
            set => _zeroPosition = value;
        }

        private Tuple<int, int> FindZeroCoordinates(int[,] board)
        {
            for (var r = 0; r < Size; r++)
            {
                for (var c = 0; c < Size; c++)
                {
                    if (board[r, c] == 0)
                    {
                        return new Tuple<int, int>(r, c);
                    }
                }
            }

            return null;
        }

        public bool Execute()
        {
            var treshold = CalculateDistanceToSolution(Board);

            while (true)
            {
                var temp = Search(0, treshold);
                if (temp == 0)
                {
                    PrintSolution();
                    return true;
                }

                treshold = temp;
            }
        }

        private int Search(int gScore, int treshold)
        {
            var movesToSolution = CalculateDistanceToSolution(Board);
            var fScore = gScore + movesToSolution;
            if (fScore > treshold)
            {
                return fScore;
            }

            if (movesToSolution == 0)
            {
                return 0;
            }

            var min = int.MaxValue;
            
            if (ZeroPosition.Item1 < Size - 1)
            {
                MoveUp();
                Path.Add(Direction.Up);
                var temp = Search(gScore + 1, treshold);
                if (temp == 0)
                {
                    return 0;
                }

                if (temp < min)
                {
                    min = temp;
                }
                MoveDown();
                Path.RemoveAt(Path.Count - 1);
            }

            if (ZeroPosition.Item2 > 0)
            {
                MoveRight();
                Path.Add(Direction.Right);
                var temp = Search(gScore + 1, treshold);
                if (temp == 0)
                {
                    return 0;
                }

                if (temp < min)
                {
                    min = temp;
                }
                MoveLeft();
                Path.RemoveAt(Path.Count - 1);
            }

            if (ZeroPosition.Item1 > 0)
            {
                MoveDown();
                Path.Add(Direction.Down);
                var temp = Search(gScore + 1, treshold);
                if (temp == 0)
                {
                    return 0;
                }

                if (temp < min)
                {
                    min = temp;
                }
                MoveUp();
                Path.RemoveAt(Path.Count - 1);
            }

            if (ZeroPosition.Item2 < Size - 1)
            {
                MoveLeft();
                Path.Add(Direction.Left);
                var temp = Search(gScore + 1, treshold);
                if (temp == 0)
                {
                    return 0;
                }

                if (temp < min)
                {
                    min = temp;
                }
                MoveRight();
                Path.RemoveAt(Path.Count - 1);
            }

            return min;
        }

        // Calculate board distance to the goal using Manhattan distance algorithm
        private int CalculateDistanceToSolution(int[,] board)
        {
            var distance = 0;

            for (var row = 0; row < Size; row++)
            {
                for (var col = 0; col < Size; col++)
                {
                    var elementToPlace = board[row, col];

                    if (elementToPlace > ZeroIndex)
                    {
                        elementToPlace++;
                    }

                    if (elementToPlace == 0) continue;
                    var rightElementPlace = new Tuple<int, int>((elementToPlace - 1) / Size, (elementToPlace - 1) % Size);
                    distance += Math.Abs(row - rightElementPlace.Item1) +
                                       Math.Abs(col - rightElementPlace.Item2);
                }
            }

            return distance;
        }

        // Functuins for moving a piece
        private void MoveDown()
        {
            var pieceUpZero = Board[ZeroPosition.Item1 - 1, ZeroPosition.Item2];
            Board[ZeroPosition.Item1, ZeroPosition.Item2] = pieceUpZero;
            Board[ZeroPosition.Item1 - 1, ZeroPosition.Item2] = 0;

            ZeroPosition = new Tuple<int, int>(ZeroPosition.Item1 - 1, ZeroPosition.Item2);
        }

        private void MoveUp()
        {
            var pieceDownZero = Board[ZeroPosition.Item1 + 1, ZeroPosition.Item2];
            Board[ZeroPosition.Item1, ZeroPosition.Item2] = pieceDownZero;
            Board[ZeroPosition.Item1 + 1, ZeroPosition.Item2] = 0;

            ZeroPosition = new Tuple<int, int>(ZeroPosition.Item1 + 1, ZeroPosition.Item2);
        }

        private void MoveRight()
        {
            var pieceLeftZero = Board[ZeroPosition.Item1, ZeroPosition.Item2 - 1];
            Board[ZeroPosition.Item1, ZeroPosition.Item2] = pieceLeftZero;
            Board[ZeroPosition.Item1, ZeroPosition.Item2 - 1] = 0;

            ZeroPosition = new Tuple<int, int>(ZeroPosition.Item1, ZeroPosition.Item2 - 1);

        }

        private void MoveLeft()
        {
            var pieceRightZero = Board[ZeroPosition.Item1, ZeroPosition.Item2 + 1];
            Board[ZeroPosition.Item1, ZeroPosition.Item2] = pieceRightZero;
            Board[ZeroPosition.Item1, ZeroPosition.Item2 + 1] = 0;

            ZeroPosition = new Tuple<int, int>(ZeroPosition.Item1, ZeroPosition.Item2 + 1);

        }

        private void PrintSolution()
        {
            Console.WriteLine(Path.Count);
            foreach (var step in Path)
            {
                Console.WriteLine(step.ToString().ToLower());
            }
        }
    }
}
