using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;


namespace Server
{
    class Program
    {
        public static TcpListener tcpListener;
        public static List<TcpClient> tcpClientsList = new List<TcpClient>();

        static void Main(string[] args)
        {
            tcpListener = new TcpListener(IPAddress.Any, 11000);
            tcpListener.Start();

            Console.WriteLine("Server Connected");

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                tcpClientsList.Add(tcpClient);

                Thread thread = new Thread(ClientListener);
                thread.Start(tcpClient);
            }
        }

        public static void ClientListener(object obj)
        {
            TcpClient tcpClient = (TcpClient)obj;
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            Console.WriteLine("Client connected");

            while (true)
            {
                string message = reader.ReadLine();
                BroadCast(message, tcpClient);
                Console.WriteLine(message);
            }
        }

        public static void BroadCast(string msg, TcpClient excludeClient)
        {
            foreach (TcpClient client in tcpClientsList)
            {
                if (client != excludeClient)
                {
                    StreamWriter sWriter = new StreamWriter(client.GetStream());
                    sWriter.WriteLine(msg);
                    sWriter.Flush();
                }
            }
        }
    }
}
