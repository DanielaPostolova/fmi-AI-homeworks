using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelingSalesman
{
    class Program
    {
        static void Main(string[] args)
        {
            var pointsCount = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            var points = new List<Tuple<int, int>>();
            var randomGenerator = new Random();

            while (points.Count == 0 && !ValidatePoints(points))
            {
                for (var i = 0; i < pointsCount; i++)
                {
                    points.Add(new Tuple<int, int>(randomGenerator.Next(0, 1000), randomGenerator.Next(0, 1000)));
                }
            }

            var algorithm = new GeneticAlgorithm(points);
            algorithm.Execute();
        }

        private static bool ValidatePoints(IReadOnlyCollection<Tuple<int, int>> points)
        {
            return points.GroupBy(p => p.Item1).Any(gr => gr.Count() > 1)
                || points.GroupBy(p => p.Item2).Any(gr => gr.Count() > 1);
        }
    }
}
