using System.Collections.Generic;
using System.Linq;

namespace DecisionTreeClasiffier
{
    internal class Record
    {
        public string ClassName { get; set; }
        public Dictionary<string, string> AttributesDict { get; set; }

        public Record(string csvLine)
        {
            var values = csvLine.Split(',');
            AttributesDict = new Dictionary<string, string>();

            ClassName = values[0];
            var keys = Attributes.Values.Keys.ToArray();

            for (var i = 1; i < values.Length; i++)
            {
                var value = Attributes.Values[keys[i- 1]].Contains(values[i])
                    ? values[i]
                    : "?";

                AttributesDict.Add(keys[i - 1], value);
            }
        }
    }
}
