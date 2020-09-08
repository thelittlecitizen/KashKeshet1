using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 11000);
            Console.WriteLine("Client Connected to server.");
            try
            {
                Thread thread = new Thread(Read);
                thread.Start(tcpClient);

                
                StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());
                StreamWriter sWriterclient = new StreamWriter(tcpClient.GetStream());

                sWriterclient.WriteLine($"Client connected");
                sWriterclient.Flush();



                while (true)
                {
                    if (tcpClient.Connected)
                    {
                       

                        Console.WriteLine("please enter your message");
                        string input = Console.ReadLine();
                        sWriter.WriteLine($"Client write: {input}");
                        sWriter.Flush();
                        if (input == "bye")
                        {
                            sWriterclient.WriteLine($"Client disconnected");
                            sWriterclient.Flush();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            Console.ReadKey();
        }

        static void Read(object obj)
        {
            TcpClient tcpClient = (TcpClient)obj;
            StreamReader sReader = new StreamReader(tcpClient.GetStream());

            while (true)
            {
                try
                {
                    string message = sReader.ReadLine();
                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }
    }
}