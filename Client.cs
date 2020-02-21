using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main()
        {
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting.....");

                client.Connect("185.148.145.209", 8888);
                // use the IP address as in the server program
                Console.WriteLine("Connected");

                string str = "*";


                while (str.Length > 0)
                {
                    Console.Write("Enter the string to be transmitted : ");
                    str = Console.ReadLine();
                    Stream tcpStream = client.GetStream();

                    byte[] ba = new ASCIIEncoding().GetBytes(str);

                    Console.WriteLine("Transmitting.....");

                    tcpStream.Write(ba, 0, ba.Length);

                    byte[] buff = new byte[100];
                    int k = tcpStream.Read(buff, 0, 100);

                    string s = "";
                    for (int i = 0; i < k; i++)
                        s += (char)(buff[i]);
                    Console.WriteLine(s);

                    if (s == "Termination string received.")
                    {
                        Console.WriteLine("Client shut down.");
                        client.Close();
                        return;
                    }
                }
                
            }
            catch (SocketException) { Console.WriteLine("Could not establish server connection. Shutting down..."); }
        }
    }
}
