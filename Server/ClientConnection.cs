using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/**
 * @author: Kevin Bauer
 * Project: ClientServerDijkstra
 * 
 **/

namespace Server
{
    internal class ClientConnection
    {
        private Socket socket;
        private NodeManagement nm;
        public string ClientName { get; set; }


        public ClientConnection(object o)
        {
            socket = o as Socket;
            nm = new NodeManagement();
        }

        public void Run()
        {
            Console.WriteLine("Client connected");
            //Console.WriteLine($"Client {ClientName} connected");

            string msgReceived = null;
            byte[] bytesReceived = new Byte[1024];
            int numBytesReceived;


            int i = 0;
            do
            {
                if (i == 0)
                {
                    numBytesReceived = socket.Receive(bytesReceived);
                    msgReceived = Encoding.ASCII.GetString(bytesReceived, 0, numBytesReceived);
                    Console.WriteLine("Text received -> {0} ", msgReceived);
                    ClientName = msgReceived;
                }
                else
                {
                    numBytesReceived = socket.Receive(bytesReceived);
                    msgReceived = Encoding.ASCII.GetString(bytesReceived, 0, numBytesReceived);
                    Console.WriteLine("Text from {1} received -> {0} ", msgReceived, ClientName);
                    CheckCommand(msgReceived);
                }
                i++;
                
            } while (msgReceived != "bye");

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public void Send(string messageSend)
        {
            byte[] bytesSend;
            int numBytesSend;
            bytesSend = Encoding.ASCII.GetBytes(messageSend);
            numBytesSend = socket.Send(bytesSend);
            //Console.WriteLine("Message sent");
            Thread.Sleep(1);
        }

        public void CheckCommand(string message)
        {
            string command = "";
            string[] args = {};
            string filepath = "";
            if (message.Contains('('))
            {
                command = message.Substring(0, message.IndexOf('('));
                args = message.Substring(command.Length + 1, message.Length - command.Length - 2).Split(',');
            }
            else if (message.Contains(':'))
            {
                int colonIndex = message.IndexOf(':');
                command = message.Substring(0, colonIndex);
                filepath = message.Substring(colonIndex + 1);
            }
            else { command = message; }

            switch (command.ToLower())
            {
                case "addnode":
                    if(nm.AddNode(args[0]) == false) 
                    {
                        Send("Knoten mit dieser Bezeichnung existiert bereits");
                    }
                    else { Send("Knoten wurde hinzugefuegt"); };
                    break;
                case "print":
                    SendNodeInfo();
                    break;
                case "addconnection":
                    if(nm.AddConnection(args[0], args[1], int.Parse(args[2])))
                    {
                        Send($"Die Connection zwischen {args[0]} und {args[1]} wurde hinzugefuegt");
                    }
                    else { Send($"Eine Connection zwischen {args[0]} und {args[1]} konnte nicht gebildet werden"); }
                    break;
                case "load":
                    if (nm.LoadFromFile(filepath)) 
                    { 
                        Send($"Informationen wurden erfolgreich aus {filepath} ausgelesen"); 
                    }
                    else { Send($"Die angegebene Datei: {filepath} konnte nicht gefunden werden"); }
                    break;
                case "search":
                    SendPathInfo(nm.Search(nm.GetNodeByDesc(args[0]), nm.GetNodeByDesc(args[1])));
                    break;
                case "bye":
                    break;
                default:
                    Send("Invalid Command!");
                    break;
            }

        }

        private void SendNodeInfo()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var node in nm.GetNodes().Values)
            {
                sb.AppendLine("**" + node.Desc);
                if (node.GetNeighborNodes().Count == 0)
                    sb.AppendLine("no neighbours");
                else
                    foreach (Node n in node.GetNeighs().Keys)
                        sb.AppendLine("  " + n.Desc + " (" + node.GetNeighs()[n] + ")");
            }

            Send(sb.ToString());
        }

        private void SendPathInfo(List<Node> path)
        {
            StringBuilder sb = new StringBuilder();

            if (path != null && path.Count > 1)
            {
                sb.AppendFormat("Shortest path from {0} to {1}:", path[0].Desc, path[path.Count - 1].Desc);
                sb.AppendLine();
                foreach (Node node in path)
                {
                    sb.AppendLine(node.Desc);
                }
            }
            else
            {
                sb.AppendLine("No path found from Start to End.");
            }

            Send(sb.ToString());
        }

    }
}
