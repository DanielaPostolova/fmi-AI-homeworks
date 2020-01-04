using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTreeClasiffier
{
    class Classifier
    {
        public IList<Record> Data { get; set; }
        private Node Root { get; set; }

        public Classifier(IList<Record> data)
        {
            Data = data;
            Root = new Node();

            BuildTree(Root, Attributes.Values.Keys.ToList(), Data);
        }

        public string Classify(Record record)
        {
            return Classify(Root, record);
        }

        private string Classify(Node node, Record record)
        {
            if (!string.IsNullOrEmpty(node.Decision))
            {
                return node.Decision;
            }

            var child = node.Children.First(c => c.Value == record.AttributesDict[node.Attribute]);
            return Classify(child, record);
        }

        private void BuildTree(Node node, IList<string> availableAttributes, IList<Record> data)
        {
            if (!data.Any())
            {
                node.Decision = "?";
                return;
            }

            if (data.All(r => r.ClassName == Attributes.Positive))
            {
                node.Decision = Attributes.Positive;
                return;
            }

            if (data.All(r => r.ClassName == Attributes.Negative))
            {
                node.Decision = Attributes.Negative;
                return;
            }

            if (data.Count <= Attributes.OverfittingThreshold || !availableAttributes.Any())
            {
                node.Decision = data.Count(r => r.ClassName == Attributes.Positive) > data.Count / 2
                    ? Attributes.Positive
                    : Attributes.Negative;

                return;
            }

            var maxGain = double.MinValue;
            foreach (var attribute in availableAttributes)
            {
                var attributeGain = CalculateGain(attribute, data);
                if (attributeGain > maxGain)
                {
                    maxGain = attributeGain;
                    node.Attribute = attribute;
                }
            }

            var newAvailableAttributes = availableAttributes.Where(attr => attr != node.Attribute).ToList();
            foreach (var value in Attributes.Values[node.Attribute])
            {
                var child = new Node() { Value = value };
                node.Children.Add(child);
                BuildTree(child, newAvailableAttributes,
                    data.Where(r => r.AttributesDict[node.Attribute] == value).ToList());
            }
        }

        private double CalculateGain(string attribute, IList<Record> data)
        {
            var classEntropy = CalculateEntropy(data.Count(r => r.ClassName == Attributes.Positive),
                data.Count(r => r.ClassName == Attributes.Negative));
            return classEntropy - CalculateEntropy(attribute, data);
        }

        private double CalculateEntropy(int positive, int negative)
        {
            if (positive == 0 && negative == 0) return 1;
            if (positive == 0 || negative == 0) return 0;

            var positivePart = (positive * 1.0) / (positive + negative);
            var negativePart = (negative * 1.0) / (positive + negative);

            return -(positivePart * Math.Log(positivePart, 2)) - (negativePart * Math.Log(negativePart, 2));
        }

        private double CalculateEntropy(string attribute, IList<Record> data)
        {
            var result = 0.0;
            foreach (var attributeValue in Attributes.Values[attribute])
            {
                var filteredData = data.Where(record => record.AttributesDict[attribute] == attributeValue).ToList();

                var positive = filteredData.Count(record => record.ClassName == Attributes.Positive);
                var negative = filteredData.Count(record => record.ClassName == Attributes.Negative);

                var attributeValueProbability = filteredData.Count / (data.Count * 1.0);
                result += attributeValueProbability * CalculateEntropy(positive, negative);
            }

            return result;
        }
    }
}
