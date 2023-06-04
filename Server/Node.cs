using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * @author: Kevin Bauer
 * Project: ClientServerDijkstra
 * 
 **/


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

        public Dictionary<Node, int> GetNeighs()
        {
            return neighs;
        }

        public override string ToString()
        {
            return desc;
        }

        public void print()
        {
            Console.WriteLine("**" + Desc);
            if (neighs.Count == 0)
                Console.WriteLine("no neighbours");
            else
                foreach (Node n in neighs.Keys)
                    Console.WriteLine("  " + n.Desc + " (" + neighs[n] + ")");
        }

    }
}
