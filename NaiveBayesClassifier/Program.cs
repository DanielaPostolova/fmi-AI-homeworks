using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NaiveBayesClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            const int attributesCount = 16;
            const int sectionsCount = 10;

            var data = File.ReadAllLines(".\\HW_05_data.csv")
                .Skip(1)
                .Select(line => new Record(line, attributesCount))
                .ToList();

            Shuffle(data);

            var random = new Random();
            var dataCount = data.Count;
            var recordsInSection = dataCount / sectionsCount;
            var testDataStartIndex = random.Next(0, dataCount);

            var index = 0;
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

                var classifier = new Classifier(traningData, attributesCount);

                var wrong = 0;
                var right = 0;
                foreach (var record in testData)
                {
                    var isDemocrat = classifier.Classify(record);

                    if ((isDemocrat && record.ClassName == "democrat")
                        || (!isDemocrat && record.ClassName == "republican"))
                    {
                        right++;
                    }
                    else
                    {
                        wrong++;
                    }
                }

                Console.WriteLine($"Right: {right}; Wrong: {wrong}; Accuracy: {(right * 1.0) / (right + wrong)}%");
                index++;
            } while (index < sectionsCount);
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
