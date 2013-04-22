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

        private Task ReceiveInputTask { get; set; }

        public NetworkClient(Server server, TcpClient socket, int id)
        {
            _socket = socket;
            _id = id;
            _server = server;
        }

        public void Start()
        {
            ReceiveInputTask = new Task(ReceiveInput);
            ReceiveInputTask.Start();
        }

        public void SendLine(string line)
        {
            try
            {
                var writer = new StreamWriter(_networkStream);
                writer.WriteLine(line);
                writer.Flush();
            }
            catch (IOException)
            {
                // socket closed
                return;
            }
        }

        private void ReceiveInput()
        {
            _networkStream = _socket.GetStream();

            using (var reader = new StreamReader(_networkStream))
            {
                while (_server.IsRunning)
                {
                    try
                    {
                        var content = reader.ReadLine();
                        if (content == null)
                        {
                            Console.WriteLine("Client {0} disconnected", _id);
                            return;
                        }

                        _server.ProcessClientCommand(this, content);
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
