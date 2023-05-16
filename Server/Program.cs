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

            nm.AddNode(nodeA.Desc);
            nm.AddNode(nodeB.Desc);
            nm.AddNode(nodeC.Desc);
            nm.AddNode(nodeD.Desc);
            nm.AddNode(nodeE.Desc);

            nm.AddConnection("A", "B", 2);
            nm.AddConnection("A", "C", 4);
            nm.AddConnection("B", "C", 1);
            nm.AddConnection("B", "D", 3);
            nm.AddConnection("C", "E", 2);
            nm.AddConnection("D", "E", 2);

            // run search algorithm
            List<Node> path = nm.Search(nodeA, nodeE);

            foreach (Node n in path)
            {
                Console.WriteLine(n.Desc);
            }


        }
    }
}
