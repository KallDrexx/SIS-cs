using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SisCsServer.Irc;

namespace SisCsServer
{
    public class Server
    {
        private readonly TcpListener _listener;
        private readonly List<IrcClient> _ircClients;
        private readonly IrcCommandProcessor _commandProcessor;
        private readonly IrcController _controller;
        private Task _clientListenTask;

        public bool IsRunning { get; private set; }

        public Server(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port); 
            _ircClients = new List<IrcClient>();
            _controller = new IrcController(_ircClients);
            _commandProcessor = new IrcCommandProcessor(_controller);
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
            user.IrcCommandReceived += _commandProcessor.ProcessCommand;
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
