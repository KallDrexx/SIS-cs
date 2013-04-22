using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

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

        public async Task Start()
        {
            using (var reader = new StreamReader(_socket.GetStream()))
            {
                while (_server.IsRunning)
                {
                    try
                    {
                        var content = await reader.ReadLineAsync().ConfigureAwait(false);
                        if (content == null)
                        {
                            Console.WriteLine("Client {0} disconnected", _id);
                            return;
                        }

                        Console.WriteLine("Client {0} wrote: {1}", _id, content);
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("Client {0} disconnected", _id);
                        return;
                    }
                }
            }
        }
    }
}
