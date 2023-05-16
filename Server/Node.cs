using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Node
    {
        private string desc;
        private Dictionary<Node, int> neighs;

        public string Desc 
        {
            get { return desc; }
            private set { desc = value; }
        }

        public Node(string desc)
        {
            Desc = desc;
            neighs = new Dictionary<Node, int>();
        }

        public bool AddNeigh(Node node, int weight)
        {
            if (neighs.ContainsKey(node)) { return false; }
            
            neighs.Add(node, weight);
            return true;
        }

        public List<Node> GetNeighborNodes()
        {
            return neighs.Keys.ToList();
        }

        public Dictionary<Node, int> getNeighs()
        {
            return neighs;
        }
    }
}
