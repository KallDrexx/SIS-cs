using System;
using System.IO;
using System.Net.Sockets;

namespace SisCsServer
{
    public class NetworkClient
    {
        private readonly TcpClient _socket;
        private readonly Server _server;
        private readonly int _id;

        public NetworkClient(Server server, TcpClient socket, int id)
        {
            _socket = socket;
            _id = id;
            _server = server;
        }

        public async void Start()
        {
            using (var reader = new StreamReader(_socket.GetStream()))
            {
                while (_server.IsRunning)
                {
                    var line = await reader.ReadLineAsync();
                    if (!_socket.Connected || line == null)
                    {
                        Console.WriteLine("Client {0} disconnected", _id);
                        return;
                    }

                    Console.WriteLine("Client {0} wrote: {1}", _id, line);
                }
            }
        }
    }
}
