using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SisCsServer.Irc
{
    public class IrcClient
    {
        private readonly NetworkClient _networkClient;
        private Task _receiveInputTask;

        public string NickName { get; set; }
        public bool IsActive { get { return _networkClient.IsActive; } }

        public event IrcCommandReceivedDelegate IrcCommandReceived;

        public IrcClient(TcpClient tcpClient, int clientId)
        {
            _networkClient = new NetworkClient(tcpClient, clientId);
            _networkClient.ClientDisconnected += ClientSocketDisconnected;
            _networkClient.MessageReceived += ProcessClientCommand;

            _receiveInputTask = _networkClient.ReceiveInput();
        }

        private void ClientSocketDisconnected(NetworkClient client)
        {
            if (client != _networkClient)
                return;

            client.IsActive = false;
            Console.WriteLine("User {0} disconnected", NickName);
        }

        private void ProcessClientCommand(NetworkClient client, string command)
        {
            if (client != _networkClient)
                return;

            Console.WriteLine("User {0}: {1}", NickName, command);

            if (IrcCommandReceived != null)
                IrcCommandReceived(this, command);
        }
    }
}
