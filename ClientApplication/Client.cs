using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    class Client
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string _prefix = "client##";
        
        private string name;
        private int age;
        private double weight;
        private int gender;
        public Client(string name,int age,double weight,int gender)
        {
            this.name = name;
            this.age = age;
            this.weight = weight;
            this.gender = gender;
        }

        static void Main(string[] args)
        {
           
            Console.Title = "Client";
            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            while (true)
            {
                ReceiveMessage();
                Console.Write("Enter a request: ");
                string req = _prefix + Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(req);
                _clientSocket.Send(buffer);

                
            }
        }
        private static void ReceiveMessage()
        {
            byte[] receivedBuffer = new byte[1024];
            int rec = _clientSocket.Receive(receivedBuffer);
            byte[] data = new byte[rec];
            Array.Copy(receivedBuffer, data, rec);
            string serverresponse = Encoding.ASCII.GetString(data);
            string[] prefixwithmessage = serverresponse.Split(new[] { "//" }, StringSplitOptions.None);
            if (prefixwithmessage[0] == "message")
            {
                Console.WriteLine($"Received String: {prefixwithmessage[1]}");
            }
            else if (prefixwithmessage[1] == "file")
            {

            }
        }

        private static void LoopConnect()
        {
            int attempts = 0;
            while (!_clientSocket.Connected)
            {


                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException e)
                {
                    Console.Clear();
                    Console.WriteLine($"Connection attempts: {attempts}");
                }
            }
            Console.Clear();
            Console.WriteLine("Connected");
            string connected = "client##connected";
            byte[] buffer = Encoding.ASCII.GetBytes(connected);
            _clientSocket.Send(buffer);

        }
    }
}
