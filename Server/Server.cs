using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    internal class Server
    {
        public void Start()
        {
            IPEndPoint localEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 11111);
            Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Waiting connection ... ");

            Socket server;
            while (true)
            {
                server = listener.Accept();
                Thread t = new Thread(new ParameterizedThreadStart(Run));
                t.Start(server);
            }
        }

        public void Run(object o)
        {
            ClientConnection conn = new ClientConnection(o);
            conn.Run();

            /*Console.WriteLine("Client connected");

            Socket server = (Socket)o;

            string msgReceived = null;
            byte[] bytesReceived = new Byte[1024];
            int numBytesReceived;

            byte[] bytesSend;
            string msgSend = "";
            int numBytesSend;

            do
            {
                numBytesReceived = server.Receive(bytesReceived);
                msgReceived = Encoding.ASCII.GetString(bytesReceived, 0, numBytesReceived);
                Console.WriteLine("Text received -> {0} ", msgReceived);

                msgSend = msgReceived + "::" + msgReceived;
                bytesSend = Encoding.ASCII.GetBytes(msgSend);
                numBytesSend = server.Send(bytesSend);
                Console.WriteLine("Message sent");
            } while (msgReceived != "bye");

            server.Shutdown(SocketShutdown.Both);
            server.Close();*/
        }
    }
}
