using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            {
                return false;
            }

            sortedList.Add(entry);
            Sort();
            dictionary.Add(entry.Node, entry);

            return true;
        }

        public bool IsInOpen(Node node)
        {
            return dictionary.ContainsKey(node);
        }

        public void Sort()
        {
            sortedList.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        }

        //Sortiert zuerst die Liste.
        //Entfernt das erste Element aus der sortierten Liste und liefert dieses zurück.
        public NodeEntry GetBest()
        {
            if (sortedList.Count == 0)
            {
                return null;
            }

            Sort();
            NodeEntry best = sortedList[0];
            sortedList.RemoveAt(0);
            dictionary.Remove(best.Node);
            return best;
        }

        public NodeEntry Get(Node node)
        {
            if (dictionary.ContainsKey(node))
            {
                return dictionary[node];
            }
            else
            {
                return null;
            }
        }
    }
}
