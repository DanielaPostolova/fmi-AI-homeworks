using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DecisionTreeClasiffier
{
    class Program
    {
        static void Main(string[] args)
        {
            const int sectionsCount = 10;

            var data = File.ReadAllLines(".\\data.csv")
                .Select(line => new Record(line))
                .ToList();

            Shuffle(data);

            var random = new Random();
            var dataCount = data.Count;
            var recordsInSection = dataCount / sectionsCount;
            var testDataStartIndex = random.Next(0, dataCount);

            var index = 0;
            var rates = new List<double>();
            do
            {
                testDataStartIndex += recordsInSection;
                testDataStartIndex %= dataCount;
                var testDataEndIndex = testDataStartIndex + recordsInSection;
                List<Record> testData;

                if (testDataEndIndex >= dataCount)
                {
                    testData = data.GetRange(testDataStartIndex, dataCount - testDataStartIndex);
                    testData.AddRange(data.GetRange(0, testDataEndIndex - dataCount));
                }
                else
                {
                    testData = data.GetRange(testDataStartIndex, testDataEndIndex - testDataStartIndex);
                }

                var traningData = data.FindAll(record => !testData.Contains(record));

                var classifier = new Classifier(traningData);

                var wrong = 0;
                var right = 0;
                foreach (var record in testData)
                {
                    var className = classifier.Classify(record);

                    if (className == record.ClassName)
                    {
                        right++;
                    }
                    else
                    {
                        wrong++;
                    }
                }

                var success = (right * 1.0) / (right + wrong);
                rates.Add(success);
                Console.WriteLine($"Index: {index + 1}, Right: {right}; Wrong: {wrong}; Accuracy: {success:P}");
                index++;
            } while (index < sectionsCount);

            Console.WriteLine($"Min: {rates.Min():P}, Max: {rates.Max():P}, Avg: {rates.Average():P}");
        }

        private static List<T> Shuffle<T>(List<T> list)
        {
            var random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
