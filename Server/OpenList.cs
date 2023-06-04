using System;
using System.Collections.Generic;
using System.Linq;

/**
 * @author: Kevin Bauer
 * Project: ClientServerDijkstra
 * 
 **/

namespace Server
{
    internal class OpenList
    {
        private List<NodeEntry> sortedList;
        private Dictionary<Node, NodeEntry> dictionary;

        public OpenList()
        {
            sortedList = new List<NodeEntry>();
            dictionary = new Dictionary<Node, NodeEntry>();
        }

        public bool Add(NodeEntry entry)
        {
            if (dictionary.ContainsKey(entry.Node))
                return false;

            sortedList.Add(entry);
            Sort();
            dictionary.Add(entry.Node, entry);

            return true;
        }

        public bool IsEmpty()
        {
            return sortedList.Count == 0;
        }

        public NodeEntry GetBest()
        {
            if (IsEmpty())
                return null;

            Sort();
            NodeEntry best = sortedList[0];
            Remove(best);
            return best;
        }

        public NodeEntry Get(Node node)
        {
            if (dictionary.ContainsKey(node))
                return dictionary[node];
            else
                return null;
        }

        public void Remove(NodeEntry entry)
        {
            sortedList.Remove(entry);
            dictionary.Remove(entry.Node);
        }

        public void Print()
        {
            Console.WriteLine("Open List:");
            foreach (var entry in sortedList)
            {
                Console.WriteLine(entry.ToString());
            }
            Console.WriteLine();
        }

        private void Sort()
        {
            sortedList.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        }

        public bool IsInOpen(Node node)
        {
            return dictionary.ContainsKey(node);
        }
    }
}
