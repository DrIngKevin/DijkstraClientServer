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
    internal class NodeEntry
    {
        public Node Node { private set; get; }
        public int Distance {  set; get; }
        public Node PreNode {  set; get; }

        public NodeEntry(Node node, int distance, Node preNode)
        {
            Node = node;
            Distance = distance;
            PreNode = preNode;
        }

        public override string ToString()
        {
            string nodeString = Node != null ? Node.ToString() : "null";
            string preNodeString = PreNode != null ? PreNode.ToString() : "null";
            return string.Format("{0}-{1}-{2}", nodeString, Distance, preNodeString);
        }

    }
}
