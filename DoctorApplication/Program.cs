using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApplication
{
    class Program
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string _prefix = "doctor##";
        static void Main(string[] args)
        {
            Console.Title = "Doctor_Client";
            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            while (true)
            {
                Console.Write("Enter a request: ");
                string req = _prefix+Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(req);
                _clientSocket.Send(buffer);
                byte[] receivedBuffer = new byte[1024];
                int rec = _clientSocket.Receive(receivedBuffer);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuffer, data, rec);
                Console.WriteLine($"Received: {Encoding.ASCII.GetString(data)}");
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
            string connected = "doctor##connected";
            byte[] buffer = Encoding.ASCII.GetBytes(connected);
            _clientSocket.Send(buffer);
        }
            
            
    }
}
