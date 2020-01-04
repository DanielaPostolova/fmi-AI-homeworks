using System;
using System.Collections.Generic;

namespace NaiveBayesClassifier
{
    class Classifier
    {
        public List<Record> Data { get; set; }
        public int AttributesCount { get; set; }

        private int[,,] AttributeValueCounts { get; set; }
        private int TotalRepublicans { get; set; }
        private int TotalDemocrats { get; set; }

        public Classifier(List<Record> data, int attributesCount)
        {
            Data = data;
            AttributesCount = attributesCount;
            TotalDemocrats = 0;
            TotalRepublicans = 0;

            AttributeValueCounts = GetAttributeValueCounts();
            AddMissingValues();
        }

        public bool Classify(Record record)
        {
            var republicanPartialProbability = CalcPartialProbability("republican", record);
            var democratPartialProbability = CalcPartialProbability("democrat", record);
            var evidence = republicanPartialProbability + democratPartialProbability;
            var republicanProb = republicanPartialProbability / evidence;
            var democratProb = democratPartialProbability / evidence;

            //Console.WriteLine($"Republican probability: {republicanProb}");
            //Console.WriteLine($"Democrat probability: {democratProb}");

            return democratProb > republicanProb;
        }

        private void AddMissingValues()
        {
            for (var i = 0; i < AttributesCount; i++)
            {
                foreach (var record in Data)
                {
                    if (record.Attributes[i] != null) continue;
                    var trueValuesCount = AttributeValueCounts[i, 0, 0] + AttributeValueCounts[i, 0, 1];
                    var falseValuesCount = AttributeValueCounts[i, 1, 0] + AttributeValueCounts[i, 1, 1];
                    record.Attributes[i] = trueValuesCount >= falseValuesCount;
                }
            }
        }


        private double CalcPartialProbability(string className, Record record)
        {
            var classNameIndex = className == "democrat" ? 1 : 0;
            var total = className == "democrat" ? TotalDemocrats : TotalRepublicans;

            var result = 1.0;
            for (var i = 0; i < AttributesCount; i++)
            {
                result *= (AttributeValueCounts[i, Convert.ToInt32(record.Attributes[i]), classNameIndex] + 1) / ((total + 2) * 1.0);
            }

            return result;
        }

        private int[,,] GetAttributeValueCounts()
        {
            // three-dementional array - attribute, attribute value (true/false), classname (republican/democrat)
            var result = new int[AttributesCount, 2, 2];

            foreach (var record in Data)
            {
                for (var j = 0; j < AttributesCount; j++)
                {
                    var classNameIndex = 0;
                    TotalRepublicans++;

                    if (record.ClassName == "democrat")
                    {
                        classNameIndex = 1;
                        TotalDemocrats++;
                        TotalRepublicans--;
                    }

                    result[j, Convert.ToInt32(record.Attributes[j]), classNameIndex]++;
                }
            }

            return result;
        }
    }
}
