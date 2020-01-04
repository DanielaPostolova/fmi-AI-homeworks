using System.Collections.Generic;

namespace DecisionTreeClasiffier
{
    class Node
    {
        public string Attribute { get; set; }
        public string Decision { get; set; }

        public string Value { get; set; }
        public IList<Node> Children { get; set; }

        public Node()
        {
            Children = new List<Node>();
        }
    }
}
