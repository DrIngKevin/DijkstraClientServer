using System;
using System.Collections.Generic;
using System.IO;
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
    internal class NodeManagement
    {
        private Dictionary<string, Node> _nodes;

        public NodeManagement()
        {
            _nodes = new Dictionary<string, Node>();
        }

        public bool AddNode(string desc)
        {
            if (_nodes.ContainsKey(desc)) { return false; }

            _nodes.Add(desc, new Node(desc));
            return true;
        }

        public bool AddNode(Node n)
        {
            if (_nodes.ContainsKey(n.Desc)) { return false; }

            _nodes.Add(n.Desc, n);
            return true;
        }

        public bool AddConnection(string from, string to, int weigth)
        {
            if (!_nodes.ContainsKey(to) || !_nodes.ContainsKey(from)) { return false; }

            return _nodes[from].AddNeigh(_nodes[to], weigth) && _nodes[to].AddNeigh(_nodes[from], weigth);
        }

        public List<Node> Search(Node start, Node end)
        {
            if (start == null || end == null) { return new List<Node>(); }

            //Lege jeweils ein Objekt vom Typ Open List und ClosedList an
            OpenList openList = new OpenList();
            ClosedList closedList = new ClosedList();

            //Füge das erste Element in die OpenList ein
            openList.Add(new NodeEntry(start, 0, null));
            //openList.Print(); 

            //Hole den besten Eintrag aus der OpenList
            NodeEntry bestEntry = openList.GetBest();
            //openList.Print(); 
            
            //Fahre solange fort bis die OpenList leer ist
            while (bestEntry != null)
            {
                //Verwenden den Knoten aus dem Eintrag
                Node curNode = bestEntry.Node;

                //Untersuche jeden Nachbarn dieses Knotens
                foreach (Node neighbor in curNode.GetNeighborNodes())
                {
                    //Ist der Nachbar bereits in der ClosedList: keine Aktion erforderlich
                    if (closedList.IsInClosed(neighbor))
                    {
                        //closedList.Print(); 
                        continue;
                    }

                    //Berechne die Entfernung von Start bis zum Nachbarn durch Addition der Entfernung von Start zum aktuellen Knoten
                    int tentativeDistance = bestEntry.Distance + curNode.GetNeighs()[neighbor];

                    //Ist der Nachbar bereits in der OpenList: ist der aktuell gefundene Weg besser?
                    NodeEntry neighborEntry = openList.Get(neighbor);
                    if (neighborEntry != null)
                    {
                        if (tentativeDistance < neighborEntry.Distance)
                        {
                            //Dann aktualisiere den Eintrag in der OpenList(Knoten bleibt gleich, bisheriger Weg und Vorgänger werden aktualisiert)
                            neighborEntry.Distance = tentativeDistance;
                            neighborEntry.PreNode = curNode;
                        }
                    }
                    //Der Nachbar ist weder in OpenList noch in der ClosedList: Füge einen neuen Eintrag in die OpenList hinzu
                    else
                    {
                        NodeEntry newEntry = new NodeEntry(neighbor, tentativeDistance, curNode);
                        openList.Add(newEntry);
                        //openList.Print(); 
                    }
                }

                //Füge den Knoten in die ClosedList hinzu
                closedList.AddEntry(bestEntry);
                //closedList.Print(); 

                //Hole den besten Eintrag aus der OpenList(siehe Methode GetBest der Klasse OpenList)
                bestEntry = openList.GetBest();

                //Gibt es keinen Eintrag, wird die Schleife abgebrochen(in diesem Fall wird kein Lösungspfad gefunden)
                //Beinhaltet der Eintrag den Zielknoten, wird der Eintrag in die ClosedList aufgenommen und die Schleife abgebrochen
                if (bestEntry != null && bestEntry.Node.Equals(end))
                {
                    closedList.AddEntry(bestEntry);
                    //closedList.Print(); Console.WriteLine("Target Entry was added in CL!");
                    break;
                }
            }

            //closedList.Print(); Console.WriteLine("CL at the end!");
            //openList.Print(); Console.WriteLine("OL at the end!");
            //Retourniere das Ergebnis
            List<Node> path = closedList.GetResult(end);
            path.Reverse();
            return path;

        }



        public Dictionary<string, Node> GetNodes()
        {
            return _nodes;
        }

        public void print()
        {
            foreach (Node n in _nodes.Values)
                n.print();
        }

        public bool LoadFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    //Console.WriteLine(line);
                    if (line.Contains("-"))
                    {
                        string[] parts = line.Split('-');
                        string fromNode = parts[0];
                        string toNode = parts[1];
                        int weight = int.Parse(parts[2]);

                        AddConnection(fromNode, toNode, weight);
                    }
                    else
                    {
                        AddNode(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Die angegebene Datei wurde nicht gefunden.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ein Fehler ist aufgetreten: " + ex.Message);
                return false;
            }
            return true;
        }
       
        public Node GetNodeByDesc(string desc)
        {
            if(_nodes.ContainsKey(desc))
                return _nodes[desc];
            return null;
        }

        public void PrintPath(List<Node> path)
        {
            if (path != null)
            {
                Console.WriteLine("Shortest path from Start to End:");
                foreach (Node node in path)
                {
                    Console.WriteLine(node.Desc);
                }
            }
            else
            {
                Console.WriteLine("No path found from Start to End.");
            }
        }
    }
}
