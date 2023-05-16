using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class ClosedList
    {
        Dictionary<Node, NodeEntry> entries;

        public ClosedList()
        {
            entries = new Dictionary<Node, NodeEntry>();
        }

        public void AddEntry(NodeEntry entry)
        {
            if(!entries.ContainsKey(entry.Node))
            entries.Add(entry.Node, entry);
        }

        public bool IsInClosed(Node node)
        {
            return entries.TryGetValue(node, out _ /*NodeEntry entry*/);
        }

        public List<Node> GetResult(Node endNode)
        {
            List<Node> result = new List<Node>();
            Node current = endNode;

            while (current != null)
            {
                result.Add(current);
                //eingefügt wegen NullRefernceError
                if(GetEntry(current) == null || GetEntry(current).PreNode == null) { break; }

                current = GetEntry(current).PreNode;
            }
            return result;
        }

        public NodeEntry GetEntry(Node node)
        {
            foreach (NodeEntry entry in entries.Values)
            {
                if (entry.Node.Equals(node)) { return entry; }
            }
            return null;
        }
    }
}
