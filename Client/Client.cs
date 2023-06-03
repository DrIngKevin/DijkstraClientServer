using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        public void Start()
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 11111);
                Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
                client.Connect(localEndPoint);

                Console.WriteLine("Client connected");

                // Clientnamen eingeben
                //Console.Write("Geben Sie eine Bezeichnung für den Client ein:");
                //string clientName = Console.ReadLine();
                //byte[] nameBytes = Encoding.ASCII.GetBytes(clientName);
                //client.Send(nameBytes);


                byte[] bytesSend;
                string msgSend = "";
                int numBytesSend;

                string msgReceived = null;
                byte[] bytesReceived = new Byte[1024];
                int numBytesReceived;

                Thread thread = new Thread(() => Input(client));
                thread.Start();

                do
                {

                    bytesReceived = new Byte[1024];
                    numBytesReceived = client.Receive(bytesReceived);
                    msgReceived = Encoding.ASCII.GetString(bytesReceived, 0, numBytesReceived);
                    Console.WriteLine(msgReceived);
                } while (msgSend != "bye" && thread.IsAlive);

                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (SocketException ex)
            {
                // Behandlung des Fehlers, wenn der Server nicht gestartet wurde oder nicht erreichbar ist
                Console.WriteLine("Der Server ist nicht gestartet oder nicht erreichbar. Der Client wird beendet.");
                return;
            }

        }

        public void Input(Socket client)
        {
            string msgSend = "";
            do
            {
                Console.Write(">> ");
                msgSend = Console.ReadLine();
                byte[] bytesSend = Encoding.ASCII.GetBytes(msgSend);
                int numBytesSend = client.Send(bytesSend);
                //Console.WriteLine("Message sent");
            } while (msgSend != "bye");
            
        }
    }
}
