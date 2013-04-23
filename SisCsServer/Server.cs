using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SisCsServer
{
    public class Server
    {
        private readonly TcpListener _listener;
        private readonly List<NetworkClient> _networkClients;
        private Task _clientListenTask;

        public bool IsRunning { get; private set; }

        public Server(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port); 
            _networkClients = new List<NetworkClient>();
        }

        public void Start()
        {
            _listener.Start();
            IsRunning = true;

            _clientListenTask = ListenForClients();
            
            while (IsRunning)
            {
                if (_clientListenTask.Exception != null)
                {
                    Console.WriteLine("Exception occurred listening for clients: {0}", _clientListenTask.Exception.Message);
                    IsRunning = false;
                }

                Thread.Sleep(100);
            }
        }

        public void ProcessClientCommand(NetworkClient client, string command)
        {
            foreach (var netClient in _networkClients)
                if (netClient.IsActive)
                    netClient.SendLine(command);
        }

        private void ClientConnected(TcpClient client, int clientNumber)
        {
            var netClient = new NetworkClient(this, client, clientNumber);
            netClient.ReceiveInputTask = netClient.Start();

            _networkClients.Add(netClient);
            Console.WriteLine("Client {0} Connected", clientNumber);
        }

        private async Task ListenForClients()
        {
            var numClients = 0;
            while (IsRunning)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                ClientConnected(tcpClient, numClients);
                numClients++;
            }

            _listener.Stop();
        }
    }
}
