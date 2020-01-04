using System;

namespace NaiveBayesClassifier
{
    internal class Record
    {
        public string ClassName { get; set; }
        public bool?[] Attributes { get; set; }
        private int AttributesCount { get; set; }

        public Record(string csvLine, int attributesCount)
        {
            AttributesCount = attributesCount;
            Attributes = new bool?[AttributesCount];

            var values = csvLine.Split(',');
            ClassName = values[0];

            for (var i = 1; i < AttributesCount; i++)
            {
                Attributes[i] = null;
                if (values[i + 1] != "?")
                {
                    Attributes[i] = Convert.ToBoolean(values[i + 1]);
                }
            }
        }
    }
}
