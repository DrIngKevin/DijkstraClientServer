using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


/**
 * @author: Kevin Bauer
 * Project: ClientServerDijkstra
 * 
 **/

namespace Client
{
    internal class Client
    {
        string clientName = "";

        public void Start()
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 11111);
                Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
                client.Connect(localEndPoint);

                Console.WriteLine("Client connected");

                Thread thread = new Thread(() => Input(client));
                thread.Start();

                // Warten, bis der Input-Thread gestartet wurde und den Namen gesendet hat
                thread.Join();

                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Der Server ist nicht gestartet oder nicht erreichbar. Der Client wird beendet.");
                return;
            }
        }

        public void Input(Socket client)
        {
            int i = 0;
            string msgSend = "";

            do
            {
                if (i == 0)
                {
                    Console.Write("Geben Sie einen Namen für den Client ein: ");
                    clientName = Console.ReadLine();
                    byte[] bytesSend = Encoding.ASCII.GetBytes(clientName);
                    int numBytesSend = client.Send(bytesSend);
                    Console.WriteLine($"{clientName} connected");
                    i++;
                }
                else
                {
                    Console.Write($"{clientName}>> ");
                    msgSend = Console.ReadLine();
                    byte[] bytesSend = Encoding.ASCII.GetBytes(msgSend);
                    int numBytesSend = client.Send(bytesSend);

                    // Warten, bis eine Antwort vom Server empfangen wird
                    Console.WriteLine(Recieve(client));
                }
            } while (msgSend != "bye");
        }

        public string Recieve(Socket client)
        {
            var bytesReceived = new Byte[1024];
            var numBytesReceived = client.Receive(bytesReceived);
            var msgReceived = Encoding.ASCII.GetString(bytesReceived, 0, numBytesReceived);
            return msgReceived;
        }
    }
}




