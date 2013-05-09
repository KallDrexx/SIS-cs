using System;
using System.Collections.Generic;
using System.Linq;
using SisCsServer.Irc.Commands.Sent.Errors;
using SisCsServer.Irc.Commands.Sent.Replies;

namespace SisCsServer.Irc
{
    public class IrcController
    {
        private readonly List<IrcClient> _clients;
        private readonly List<Channel> _channels;

        public IrcController(List<IrcClient> clients)
        {
            _clients = clients;
            _channels = new List<Channel>();
        }

        public void SendActivationMessages(IrcClient client)
        {
            var message = new WelcomeReply
            {
                SenderAddress = Server.HostName,
                SenderNickName = client.NickName
            }.FormFullResponseString();

            client.SendMessage(message);
        }

        public void SendPrivateMessageToUser(IrcClient sender, string recipientNickName, string message)
        {
            // Find the client with the nickname
            var client = _clients.FirstOrDefault(x => x.NickName.Equals(recipientNickName));
            if (client == null || !client.ConnectionActive)
            {
                var errorMessage = new NoSuchNickError
                {
                    SenderAddress = Server.HostName,
                    SenderNickName = sender.NickName,
                    RecipientNickName = recipientNickName
                }.FormFullResponseString();

                sender.SendMessage(errorMessage);
                return;
            }

            client.SendMessage(
                string.Format(":{0} PRIVMSG {1} :{2}", sender.UserMask, recipientNickName, message));
        }

        public bool NickNameInUse(string nickname)
        {
            return _clients.Any(x => x.NickName.Equals(nickname, StringComparison.InvariantCultureIgnoreCase));
        }

        public void JoinChannel(IrcClient client, string channelName)
        {
            // If the channel doesn't exist yet, create it
            var channel =
                _channels.FirstOrDefault(x => x.Name.Equals(channelName, StringComparison.InvariantCultureIgnoreCase));

            if (channel == null)
            {
                channel = new Channel(channelName);
                _channels.Add(channel);
            }

            channel.JoinClient(client);
        }

        public void PartChannel(IrcClient client, string channelName)
        {
            var channel =
                _channels.FirstOrDefault(x => x.Name.Equals(channelName, StringComparison.InvariantCultureIgnoreCase));

            if (channel == null)
                return;

            channel.PartClient(client);
        }
    }
}
