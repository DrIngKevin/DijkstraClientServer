using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();

            NodeManagement nm = new NodeManagement();

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");
            Node nodeD = new Node("D");
            Node nodeE = new Node("E");

            /*
            nm.AddNode(nodeA.Desc);
            nm.AddNode(nodeB.Desc);
            nm.AddNode(nodeC.Desc);
            nm.AddNode(nodeD.Desc);
            nm.AddNode(nodeE.Desc);
            */
            nm.AddNode(nodeA);
            nm.AddNode(nodeB);
            nm.AddNode(nodeC);
            nm.AddNode(nodeD);
            nm.AddNode(nodeE);

            nm.AddConnection("A", "B", 2);
            nm.AddConnection("A", "C", 4);
            nm.AddConnection("B", "C", 1);
            nm.AddConnection("B", "D", 3);
            nm.AddConnection("C", "E", 2);
            nm.AddConnection("D", "E", 2);

            // Debug output
            Console.WriteLine("Nodes:");
            foreach (var node in nm.GetNodes().Values)
            {
                Console.WriteLine(node.ToString());
            }

            Console.WriteLine("Connections:");
            foreach (var nodeEntry in nm.GetNodes())
            {
                var node = nodeEntry.Value;
                foreach (var neighborEntry in node.GetNeighs())
                {
                    var neighbor = neighborEntry.Key;
                    var weight = neighborEntry.Value;
                    Console.WriteLine($"{node.Desc} -> {neighbor.Desc} (Weight: {weight})");
                }
            }

            // run search algorithm
            List<Node> path = nm.Search(nodeE, nodeA);

            // Print the path
            if (path != null)
            {
                Console.WriteLine("Shortest path from E to A:");
                foreach (Node node in path)
                {
                    Console.WriteLine(node.Desc);
                }
            }
            else
            {
                Console.WriteLine("No path found from E to A.");
            }


        }
    }
}
