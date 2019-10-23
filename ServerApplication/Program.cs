using ServerApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace ServerApplication
{
    class Program
    {
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static List<Socket> _doctorSockets = new List<Socket>();
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string _stringprefix = "message//";
        private static string _fileprefix = "file//";
        static void Main(string[] args)
        {
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine();
        }
        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            _serverSocket.Listen(5); //Queue max = 5
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            _clientSockets.Add(socket);
            Console.WriteLine("Client Connected!!");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int received = socket.EndReceive(ar);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);
            string text = Encoding.ASCII.GetString(dataBuffer);
            string[] idandtext = text.Split(new string[1] { "##" },StringSplitOptions.None);
            string Id = idandtext[0];
            string message = idandtext[1];
            Console.WriteLine($"Received text: {message} from: {Id}");

            string response = string.Empty;
            if (Id.ToLower() == "doctor/message")
            {
                if (message.ToLower() == "connected")
                {
                    Console.WriteLine("Clients: " + _clientSockets.Count);
                    _clientSockets.Remove(socket);
                    Console.WriteLine("Clients: " + _clientSockets.Count);
                    Console.WriteLine("Doctors: " + _doctorSockets.Count);
                    _doctorSockets.Add(socket);
                    Console.WriteLine("Doctors: " + _doctorSockets.Count);
                }
                else if (message.ToLower() == "get time")
                {
                    response = _stringprefix + DateTime.Now.ToLongTimeString();
                }
                else if (message.ToLower() == "start test")
                {
                    string startTest = _stringprefix + "start test";
                    byte[] sendStart = Encoding.ASCII.GetBytes(startTest);

                    foreach (Socket client in _clientSockets)
                    {

                        client.BeginSend(sendStart, 0, sendStart.Length, SocketFlags.None, new AsyncCallback(SendCallback), client);
                    }
                    response = _stringprefix + "Command has been sent to client";
                }
                else if (message.ToLower() == "get logs")
                {
                    response = _stringprefix + getLogs();
                }
                else if (message.Contains("get file"))
                {
                    string[] filename = message.Split(new[] { "//" }, StringSplitOptions.None);
                    string realfilename = $"{getPath()}Logs/{filename[1]}";
                    string filetext = System.IO.File.ReadAllText(realfilename);
                    response = _fileprefix + filetext;

                }
                else
                {
                    Console.WriteLine("Invalid Request");
                    response = _stringprefix + "Invalid Request";
                }
            }
           
            if(Id.ToLower() == "client")
            {
                if(message.ToLower() == "connected")
                {
                    Console.WriteLine("Clients: " + _clientSockets.Count);
                }
                else
                {
                  
                    dynamic json = JsonConvert.DeserializeObject(message);
                    Console.WriteLine(json.timestart);
                    //Console.WriteLine($"Client sent: {message}");
                    string filepath = getPath() + $"Logs/{(json.timestart as JToken).Value<string>().Replace("/", "-").Replace(":", @".")}.txt";
                  
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Close();
                        
                        Thread.Sleep(500);
                        TextWriter tw = new StreamWriter(filepath);
                        tw.WriteLine(message);
                        tw.Close();
                    }
                    else if (File.Exists(filepath))
                    {
                        using (var tw = new StreamWriter(filepath, true)){
                            tw.WriteLine(message);
                            tw.Close();
                        }
                        
                       
                    }

                }
            }
            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);


        }
        private static void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }

        public static string getLogs()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string[] arraypaths = Directory.GetFiles(getPath() + "Logs/");
                foreach (string path in arraypaths)
                {
                    string filename = Path.GetFileName(path);

                    sb.AppendLine(filename);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }


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
