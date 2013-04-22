using System;
using System.Net;
using System.Net.Sockets;

namespace SisCsServer
{
    public class Server
    {
        private readonly TcpListener _listener;

        public bool IsRunning { get; set; }

        public Server(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port); 
        }

        public void Start()
        {
            _listener.Start();
            IsRunning = true;
            ListenForClients();
        }

        private async void ListenForClients()
        {
            int numClients = 0;
            while (IsRunning)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                var netClient = new NetworkClient(this, tcpClient, numClients);
                netClient.Start();
                Console.WriteLine("Client Connected");

                numClients++;
            }
        }
    }
}
