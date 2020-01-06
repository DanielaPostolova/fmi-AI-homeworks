using System;
using System.IO;
using System.Linq;

namespace KMeans
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataFile = Console.ReadLine();
            int.TryParse(Console.ReadLine(), out var clustersCount);

            var data = File.ReadAllLines($".\\{dataFile}")
                .Skip(1)
                .Select(line => line.Split(' ', '\t').Select(double.Parse).ToList())
                .ToList();

            var algorithm = new KMeansAlgorithm(data, clustersCount);
            algorithm.Execute();
        }
    }
}
