using System;
using System.Collections.Generic;

/**
 * @author: Kevin Bauer
 * Project: ClientServerDijkstra
 * 
 **/

namespace Server
{
    internal class ClosedList
    {
        private Dictionary<Node, NodeEntry> entries;

        public ClosedList()
        {
            entries = new Dictionary<Node, NodeEntry>();
        }

        public bool AddEntry(NodeEntry entry)
        {
            if (entries.ContainsKey(entry.Node))
            {
                return false;
            }

            entries.Add(entry.Node, entry);
            return true;
        }

        public bool IsInClosed(Node node)
        {
            return entries.ContainsKey(node);
        }

        public List<Node> GetResult(Node endNode)
        {
            if (!entries.TryGetValue(endNode, out NodeEntry entry))
            {
                return new List<Node>(); // Return an empty list if the endNode is not found in entries
            }

            List<Node> result = new List<Node>();
            while (entry != null)
            {
                result.Add(entry.Node);
                if ((entry.PreNode==null) ||(!entries.TryGetValue(entry.PreNode, out entry)))
                {
                    break; // Break the loop if the PreNode is not found in entries
                }
            }

            return result;
        }



        public void Print()
        {
            foreach (var entry in entries.Values)
            {
                Console.WriteLine(entry.ToString());
            }
        }
    }
}
