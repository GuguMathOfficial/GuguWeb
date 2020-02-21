using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace CsTests
{
    static class Start
    {
	static void Receive(Socket s)
        {
            try
            {
                byte[] b = new byte[1024];
                int k = s.Receive(b);

                while (k > 0)
                {
                    string str = "";

                    Console.WriteLine("Recieved...");
                    for (int i = 0; i < k; i++)
                        str += (char)(b[i]);

                    if (str == "!!!EXIT_SERVER!!!")
                    {
                        s.Send(new ASCIIEncoding().GetBytes("Termination string received."));
                        return;
                    }
                    Console.WriteLine(str); // the received string

                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes("The string was recieved by the server."));
                    Console.WriteLine("\nSent Acknowledgement");
                    Receive(s);
                }
            }
            catch (SocketException) // connection closed by the client
            {
                return;
            }
            
        }

        static void Main()
        {
            System.Net.IPAddress ipAd = System.Net.IPAddress.Parse("127.0.0.1");
            // use local m/c IP address, and 
            // use the same in the client

            /* Initializes the Listener */
            TcpListener myList = new TcpListener(ipAd, 8888);

            /* Start Listeneting at the specified port */
            myList.Start();

            Console.WriteLine("The server is running at port 8888...");
            Console.WriteLine("The local End point is  :" +
                                myList.LocalEndpoint);
            Console.WriteLine("Waiting for a connection.....");

            Socket s = myList.AcceptSocket();
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            Receive(s);
                    
            /* clean up */
            s.Close();
                
                
            myList.Stop();
		}
    }
}