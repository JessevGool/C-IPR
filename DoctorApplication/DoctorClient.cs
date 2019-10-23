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
    class DoctorClient
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string _prefixmessage = "doctor/message##";
        private static string _prefixfile = "doctor/file##";
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
                string req = _prefixmessage+Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(req);
                _clientSocket.Send(buffer);

                byte[] receivedBuffer = new byte[1073741824];
                int rec = _clientSocket.Receive(receivedBuffer);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuffer, data, rec);
                string serverresponse = Encoding.ASCII.GetString(data);
                string[] prefixwithmessage = serverresponse.Split(new[] { "//" }, StringSplitOptions.None);
                if (prefixwithmessage[0] == "message")
                {
                    Console.WriteLine($"Received String: {prefixwithmessage[1]}");
                }
                else if (prefixwithmessage[0] == "file")
                {
                    Console.WriteLine($"Received filedata: {prefixwithmessage[1]}");
                    StreamWriter streamWriter = new StreamWriter(getPath() + "Logs/log.txt");
                    streamWriter.Write(prefixwithmessage[1]);
                    streamWriter.Close();

                    
                }
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
            string connected = "doctor/message##connected";
            byte[] buffer = Encoding.ASCII.GetBytes(connected);
            _clientSocket.Send(buffer);
        }
        public static string getPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Startsplit = startupPath.Substring(0, startupPath.LastIndexOf("bin"));
            string split = Startsplit.Replace(@"\", "/");
            return split;
        }

    }

}
