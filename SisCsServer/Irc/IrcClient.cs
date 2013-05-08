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
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string UserMask { get; private set; }
        public bool ConnectionActive { get { return _networkClient.IsActive; } }
        public bool UserActivated { get; private set; }

        public event IrcCommandReceivedDelegate IrcCommandReceived;
        public event UserActivatedDelegate IrcUserActivated;

        public IrcClient(TcpClient tcpClient, int clientId)
        {
            _networkClient = new NetworkClient(tcpClient, clientId);
            _networkClient.ClientDisconnected += ClientSocketDisconnected;
            _networkClient.MessageReceived += ProcessClientCommand;

            _receiveInputTask = _networkClient.ReceiveInput();
        }

        public async void SendMessage(string message)
        {
            await _networkClient.SendLine(message);
        }

        public void AttemptUserActivation()
        {
            // For now, a user is activated if he has a valid nickname and full name set
            if (!string.IsNullOrWhiteSpace(NickName) && !string.IsNullOrWhiteSpace(FullName))
            {
                UserActivated = true;
                if (IrcUserActivated != null)
                    IrcUserActivated(this);
            }
        }

        public void SetUserMask()
        {
            UserMask = string.Format("{0}!~{1}@{2}", NickName, UserName, _networkClient.RemoteAddress);
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
