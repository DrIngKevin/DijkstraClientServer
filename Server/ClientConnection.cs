using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            //Console.WriteLine("Client connected");
            Console.WriteLine($"Client {ClientName} connected");

            string msgReceived = null;
            byte[] bytesReceived = new Byte[1024];
            int numBytesReceived;

            

            do
            {
                numBytesReceived = socket.Receive(bytesReceived);
                msgReceived = Encoding.ASCII.GetString(bytesReceived, 0, numBytesReceived);
                Console.WriteLine("Text received -> {0} ", msgReceived);
                CheckCommand(msgReceived);
                
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
            Console.WriteLine("Message sent");
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
                    nm.AddNode(args[0]);
                    Send(args[0]);
                    break;
                case "print":
                    SendNodeInfo();
                    break;
                case "addconnection":
                    nm.AddConnection(args[0], args[1], int.Parse(args[2]));
                    break;
                case "load":
                    nm.LoadFromFile(filepath);
                    break;
                case "search":
                    SendPathInfo(nm.Search(nm.GetNodeByDesc(args[0]), nm.GetNodeByDesc(args[1])));
                    break;
                default:
                    Send("Invalid Command!");
                    break;
            }

        }

        private void SendNodeInfo()
        {
            foreach(var node in nm.GetNodes().Values)
            {
                Send("**" + node.Desc);
                if (node.GetNeighborNodes().Count == 0)
                    Send("no neighbours");
                else
                    foreach (Node n in node.GetNeighs().Keys)
                        Send("  " + n.Desc + " (" + node.GetNeighs()[n] + ")");
            }

            nm.print();
        }

        private void SendPathInfo(List<Node> path)
        {
            if (path != null)
            {
                Send(string.Format("Shortest path from {0} to {1}:", path[0].Desc, path[path.Count-1].Desc));
                //Send("Shortest path from Start to End");
                foreach (Node node in path)
                {
                    Send(node.Desc);
                }
            }
            else
            {
                Send("No path found from Start to End.");
            }
        }
    }
}
