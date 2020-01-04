using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    class MinimaxAlgorithm
    {
        public char Human { get; }
        public char Computer { get; }
        public char Neutral { get; }

        public List<char> Board { get; set; }
        public int Depth { get; set; }

        public MinimaxAlgorithm(char human = 'X', char computer = 'O', char neutral = '-')
        {
            Human = human;
            Computer = computer;
            Neutral = neutral;
            Board = GetEmptyBoard();
            Depth = 0;
        }

        public List<char> GetEmptyBoard()
        {
            return Enumerable.Repeat(Neutral, 9).ToList();
        }

        public int Evaluate()
        {
            // Check rows for victory
            for (var i = 0; i < 9; i += 3)
            {
                if (Board[i] == Board[i + 1] && Board[i + 1] == Board[i + 2])
                {
                    if (Board[i] == Human) return -1;
                    if (Board[i] == Computer) return 1;
                }
            }

            // Check columns for victory
            for (var i = 0; i < 3; i++)
            {
                if (Board[i] == Board[i + 3] && Board[i + 3] == Board[i + 6])
                {
                    if (Board[i] == Human) return -1;
                    if (Board[i] == Computer) return 1;
                }
            }

            // Check diagonals for victory
            if ((Board[0] == Board[4] && Board[4] == Board[8])
                || (Board[2] == Board[4] && Board[4] == Board[6]))
            {
                if (Board[4] == Human) return -1;
                if (Board[4] == Computer) return 1;
            }

            return 0;
        }

        public bool Mark(int row, int col)
        {
            var index = row * 3 + col;
            if (Board[index] != Neutral) return false;

            Board[index] = Human;
            Depth++;
            return true;
        }

        public bool IsDraw()
        {
            return Depth == 9;
        }

        public bool IsGameOver()
        {
            return IsDraw() || Evaluate() != 0;
        }

        public void ComputerPlay()
        {
            var index = GetNextMove();
            Board[index] = Computer;
            Depth++;
        }

        public int GetNextMove()
        {
            var indexToMove = -1;
            var bestEvaluation = int.MinValue;

            for (var i = 0; i < 9; i++)
            {
                if (Board[i] != Neutral) continue;

                Board[i] = Computer;
                Depth++;

                var evaluation = Minimax(int.MinValue, int.MaxValue, false);

                Board[i] = Neutral;
                Depth--;

                if (evaluation > bestEvaluation)
                {
                    bestEvaluation = evaluation;
                    indexToMove = i;
                }
            }

            return indexToMove;
        }

        public int Minimax(int alpha, int beta, bool isMax)
        {
            var score = 9 - Depth;
            var eval = Evaluate();
            if (eval > 0) return eval + score;
            if (eval < 0) return eval - score;
            if (IsDraw()) return 0;

            if (isMax)
            {
                var max = int.MinValue;

                for (var i = 0; i < 9; i++)
                {
                    if (Board[i] != Neutral) continue;

                    Board[i] = Computer;
                    Depth++;

                    var evaluation = Minimax(alpha, beta, false);

                    Board[i] = Neutral;
                    Depth--;

                    max = Math.Max(max, evaluation);
                    alpha = Math.Max(alpha, evaluation);

                    if (beta <= alpha) break;
                }
                return max;
            }
            else
            {
                var min = int.MaxValue;
                for (var i = 0; i < 9; i++)
                {
                    if (Board[i] != Neutral) continue;

                    Board[i] = Human;
                    Depth++;

                    var evaluation = Minimax(alpha, beta, true);

                    Board[i] = Neutral;
                    Depth--;

                    min = Math.Min(min, evaluation);
                    beta = Math.Min(beta, evaluation);

                    if (beta <= alpha) break;
                }
                return min;
            }
        }

        public void PrintBoard()
        {
            Console.WriteLine("Depth: " + Depth);
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Console.Write(Board[i * 3 + j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void PrintResult()
        {
            var evaluation = Evaluate();
            if (evaluation > 0)
            {
                Console.WriteLine("You lost");
                return;
            }

            Console.WriteLine("Draw");
        }
    }
}
