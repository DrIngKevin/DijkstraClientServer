using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
