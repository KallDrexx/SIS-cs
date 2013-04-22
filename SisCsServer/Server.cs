using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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

        public Task Start()
        {
            _listener.Start();
            IsRunning = true;

            var task = new Task(ListenForClients);
            task.Start();
            return task;
        }

        private void ListenForClients()
        {
            var numClients = 0;
            while (IsRunning)
            {
                var tcpClient = _listener.AcceptTcpClient();
                var netClient = new NetworkClient(this, tcpClient, numClients);
                netClient.Start();

                Console.WriteLine("Client {0} Connected", numClients);
                numClients++;
            }
        }
    }
}
