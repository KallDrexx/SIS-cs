using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SisCsServer
{
    public class NetworkClient
    {
        private readonly TcpClient _socket;
        private NetworkStream _networkStream;
        private readonly Server _server;
        private readonly int _id;

        public Task ReceiveInputTask { get; set; }
        public bool IsActive { get; set; }

        public NetworkClient(Server server, TcpClient socket, int id)
        {
            _socket = socket;
            _id = id;
            _server = server;
        }

        public async Task Start()
        {
            IsActive = true;
            _networkStream = _socket.GetStream();

            using (var reader = new StreamReader(_networkStream))
            {
                while (_server.IsRunning && IsActive)
                {
                    try
                    {
                        var content = await reader.ReadLineAsync();
                        if (content == null)
                        {
                            IsActive = false;
                            Console.WriteLine("Client {0} disconnected", _id);
                            return;
                        }

                        _server.ProcessClientCommand(this, content);
                        Console.WriteLine("Client {0} wrote: {1}", _id, content);
                    }
                    catch (IOException)
                    {
                        IsActive = false;
                        Console.WriteLine("Client {0} disconnected", _id);
                        return;
                    }
                }
            }
        }

        public void SendLine(string line)
        {
            if (!IsActive)
                return;

            try
            {
                var writer = new StreamWriter(_networkStream);
                writer.WriteLine(line);
                writer.Flush();
            }
            catch (IOException)
            {
                // socket closed
                IsActive = false;
            }
        }
    }
}
