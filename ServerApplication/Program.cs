using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication
{
    class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        public static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    TcpListener myList = new TcpListener(IPAddress.Any, 8001);
                    myList.Start();
                    Console.WriteLine("The server is running at port 8001...");
                    Console.WriteLine("The local End point is  :" +
                    myList.LocalEndpoint);
                    Console.WriteLine("Waiting for a connection.....");
                    TcpClient s = myList.AcceptTcpClient();
                    Console.WriteLine("Connection accepted from " + s.Client.RemoteEndPoint);
                    byte[] b = new byte[100];
                    int k = s.Receive(b);
                    Console.WriteLine("Recieved...");
                    string Command = string.Empty;
                    for (int i = 0; i < k; i++)
                    {
                        Command = Command + Convert.ToChar(b[i]);
                    }
                    Console.WriteLine(Command);
                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes("The string was recieved by the server."));
                    Console.WriteLine("\nSent Acknowledgement");
                    s.Close();
                    myList.Stop();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}