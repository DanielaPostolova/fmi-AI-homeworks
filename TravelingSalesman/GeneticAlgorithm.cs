using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Point = System.Tuple<int, int>;

namespace TravelingSalesman
{
    class GeneticAlgorithm
    {
        public List<Point> Points { get; set; }
        private int PopulationSize { get; }
        private int EliteIndividualsCount { get; }
        private int GenerationsCount { get; }
        private int IndividualSize { get; }
        private double MutationRate { get; }
        private int MaxGenerationsWithoutImprovement { get; }

        private Random Random { get; }

        public GeneticAlgorithm(List<Point> points, int populationSize = 100, int eliteIndividualsCount = 2,
            int generationsCount = 100, double mutationRate = 0.01, int maxGenerationsWithoutImprovement = 800)
        {
            Points = points;
            PopulationSize = populationSize;
            EliteIndividualsCount = eliteIndividualsCount;
            GenerationsCount = generationsCount;
            IndividualSize = points.Count;
            MutationRate = mutationRate;
            MaxGenerationsWithoutImprovement = maxGenerationsWithoutImprovement;
            Random = new Random();
        }

        public void Execute()
        {
            var currentPopulation = GenerateFirstPopulation();

            var noImprovementCounter = 0;
            var generation = 0;
            var best = currentPopulation.OrderBy(ind => ind.Distance).Take(1).Single();
            while (noImprovementCounter < MaxGenerationsWithoutImprovement)
            {
                currentPopulation = GenerateNextPopulation(currentPopulation);
                generation++;
                var newBest = currentPopulation.OrderBy(ind => ind.Distance).Take(1).Single();
                if (best.Distance > newBest.Distance)
                {
                    noImprovementCounter = 0;
                    best = newBest;

                    Console.Write($"Shortest distance after generation {generation}: ");
                    Console.WriteLine(best.Distance);
                }
                else
                {
                    noImprovementCounter++;
                }
            }

            Console.Write($"Shortest distance after {GenerationsCount} generations: ");
            Console.WriteLine(best.Distance);
        }

        private List<Individual> GenerateNextPopulation(List<Individual> population)
        {
            var nextPopulation = new List<Individual>();

            nextPopulation
                .AddRange(population.OrderBy(i => i.Distance).Take(EliteIndividualsCount));

            var totalFitnessScore = population.Sum(individual => 1 / individual.Distance);
            while (nextPopulation.Count < PopulationSize)
            {
                var firstParent = GetParent(population, totalFitnessScore);
                var secondParent = GetParent(population, totalFitnessScore);

                nextPopulation.AddRange(Crossover(firstParent, secondParent));
            }
            return nextPopulation;
        }

        private List<Individual> Crossover(Individual firstParent, Individual secondParent)
        {
            var firstChildRoute = new List<int>(firstParent.Route.Count);
            var secondChildRoute = new List<int>(firstParent.Route.Count);
            var startIndex = Random.Next(0, IndividualSize);
            var endIndex = Random.Next(0, IndividualSize);

            if (endIndex < startIndex)
            {
                var temp = startIndex;
                startIndex = endIndex;
                endIndex = temp;
            }

            firstChildRoute.AddRange(new List<int>(firstParent.Route.GetRange(startIndex, endIndex - startIndex)));
            firstChildRoute.AddRange(new List<int>(secondParent.Route.FindAll(point => !firstChildRoute.Contains(point))));

            secondChildRoute.AddRange(new List<int>(secondParent.Route.GetRange(startIndex, endIndex - startIndex)));
            secondChildRoute.AddRange(new List<int>(firstParent.Route.FindAll(point => !secondChildRoute.Contains(point))));

            var firstChild = new Individual(firstChildRoute, CalculateDistance(firstChildRoute));
            var secondChild = new Individual(secondChildRoute, CalculateDistance(secondChildRoute));

            Mutate(firstChild);
            Mutate(secondChild);

            return new List<Individual>() { firstChild, secondChild };
        }

        private void Mutate(Individual individual)
        {
            for (var i = 0; i < IndividualSize; i++)
            {
                if (Random.NextDouble() >= MutationRate) continue;
                var swapWith = Random.Next(0, IndividualSize);

                if (swapWith == i) continue;
                var temp = individual.Route[i];
                individual.Route[i] = individual.Route[swapWith];
                individual.Route[swapWith] = temp;
                individual.Distance = CalculateDistance(individual.Route);
            }
        }

        // Select parents using roulette method
        private Individual GetParent(List<Individual> population, double totalFitnessScore)
        {
            var randomNumber = Random.NextDouble();
            var sum = 0.0;
            for (var i = 0; i < PopulationSize; i++)
            {
                var fitness = 1 / population[i].Distance;
                var currentRouteRightBorder = sum + fitness / totalFitnessScore;
                if (randomNumber < currentRouteRightBorder)
                {
                    return population[i];
                }

                sum += currentRouteRightBorder;
            }

            return null;
        }

        private double CalculateDistance(List<int> route)
        {
            var routeSize = route.Count;
            var distance = 0.0;
            for (var i = 0; i < routeSize - 1; i++)
            {
                distance += CalculateDistance(Points[route[i]], Points[route[i + 1]]);
            }

            return distance;
        }

        private double CalculateDistance(Point x, Point y)
        {
            var xDistance = Math.Abs(x.Item1 - y.Item1);
            var yDistance = Math.Abs(x.Item2 - y.Item2);

            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }

        private List<Individual> GenerateFirstPopulation()
        {
            var population = new List<Individual>();

            for (var i = 0; i < PopulationSize; i++)
            {
                var route = GenerateRoute();
                population.Add(new Individual(route, CalculateDistance(route)));
            }

            return population;
        }

        private List<int> GenerateRoute()
        {
            var size = Points.Count;
            var route = Enumerable.Range(0, size).ToList();

            for (var i = 0; i < size; i++)
            {
                var randomIndex = Random.Next(0, size);
                var value = route[randomIndex];
                route[randomIndex] = route[i];
                route[i] = value;
            }

            return route;
        }

        private void PrintIndividual(Individual individual)
        {
            var builder = new StringBuilder();
            foreach (var point in individual.Route)
            {
                builder.Append(point + " ");
            }

            builder.Append($" - {individual.Distance}");

            Console.WriteLine(builder.ToString());
        }

        private void PrintCoordinates(List<Point> route)
        {
            var builder = new StringBuilder();
            builder.Append("Cities coordinates: ");
            foreach (var point in route)
            {
                builder.Append($"({point.Item1}; {point.Item2}) ");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}
