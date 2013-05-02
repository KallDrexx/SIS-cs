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
        private readonly List<IrcClient> _ircClients;
        private Task _clientListenTask;

        public bool IsRunning { get; private set; }

        public Exception ClientListenTaskException
        {
            get { return _clientListenTask.Exception; }
        }

        public Server(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port); 
            _ircClients = new List<IrcClient>();
        }

        public void Run()
        {
            _listener.Start();
            IsRunning = true;

            _clientListenTask = ListenForClients();
        }

        private void ClientConnected(TcpClient client, int clientNumber)
        {
            var user = new IrcClient(client, clientNumber) {NickName = clientNumber.ToString()};
            _ircClients.Add(user);

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
