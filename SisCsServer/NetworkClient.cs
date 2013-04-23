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
        public int Id { get { return _id; } }
        public TcpClient Socket { get { return _socket; } }

        public NetworkClient(Server server, TcpClient socket, int id)
        {
            _socket = socket;
            _id = id;
            _server = server;
        }

        public async Task ReceiveInput()
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
                            _server.ClientDisconnected(this);
                            return;
                        }

                        await _server.ProcessClientCommand(this, content);
                    }
                    catch (IOException)
                    {
                        _server.ClientDisconnected(this);
                        return;
                    }
                }
            }
        }

        public async Task SendLine(string line)
        {
            if (!IsActive)
                return;

            try
            {
                var writer = new StreamWriter(_networkStream);
                await writer.WriteLineAsync(line);
                writer.Flush();
            }
            catch (IOException)
            {
                // socket closed
                _server.ClientDisconnected(this);
            }
        }
    }
}
