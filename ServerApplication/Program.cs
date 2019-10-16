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
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static List<Socket> _doctorSockets = new List<Socket>();
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
            string[] idandtext = text.Split(new[] { "##" },StringSplitOptions.None);
            string Id = idandtext[0];
            string message = idandtext[1];
            Console.WriteLine($"Received text: {message} from: {Id}");

            string response = string.Empty;
            if (Id.ToLower() == "doctor")
            {
                if(message.ToLower() == "connected")
                {
                    Console.WriteLine("Clients: " + _clientSockets.Count);
                    _clientSockets.Remove(socket);
                    Console.WriteLine("Clients: " + _clientSockets.Count);
                    Console.WriteLine("Doctors: " + _doctorSockets.Count);
                    _doctorSockets.Add(socket);
                    Console.WriteLine("Doctors: " + _doctorSockets.Count);
                }
                else if(message.ToLower() == "get time")
                {
                    response = DateTime.Now.ToLongTimeString();
                }
            }
            if(Id.ToLower() == "client")
            {
                if(message.ToLower() == "connected")
                {
                    Console.WriteLine("Clients: " + _clientSockets.Count);
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
    }
}
