using System.Collections.Generic;
using System.Linq;
using SisCsServer.Irc.Commands.Sent;
using SisCsServer.Irc.Commands.Sent.Announcements;
using SisCsServer.Irc.Commands.Sent.Replies;

namespace SisCsServer.Irc
{
    public class Channel
    {
        private readonly List<IrcClient> _clients;

        public string Name { get; private set; }
        public string Topic { get; set; }

        public Channel(string name)
        {
            _clients = new List<IrcClient>();
            Name = name;
            Topic = "No Topic Set";
        }

        public void JoinClient(IrcClient client)
        {
            if (_clients.Contains(client))
                return; // already part of this channel

            _clients.Add(client);

            // Announce the join to all clients in the channel
            foreach (var channelClient in _clients)
            {
                new UserJoinedChannelAnnouncement
                {
                    UserMask = client.UserMask,
                    ChannelName = Name
                }.SendMessageToClient(channelClient);
            }

            // Send response messages
            new ChannelTopicReply
            {
                SenderAddress = Server.HostName,
                RecipientNickName = client.NickName,
                ChannelName = Name,
                Topic = Topic
            }.SendMessageToClient(client);

            var usersToList = _clients.Where(x => x != client)
                                      .Select(x => x.NickName)
                                      .ToArray();

            new ChannelUserListReply
            {
                SenderAddress = Server.HostName,
                RecipientNickName = client.NickName,
                ChannelName = Name,
                Users = usersToList
            }.SendMessageToClient(client);

            new ChanneluserListEndReply()
            {
                SenderAddress = Server.HostName,
                ClientNick = client.NickName,
                ChannelName = Name
            }.SendMessageToClient(client);
        }

        public void PartClient(IrcClient client)
        {
            if (!_clients.Contains(client))
                return;

            // Relay the client parting to all clients
            foreach (var ircClient in _clients)
            {
                new UserPartedChannelAnnouncement
                {
                    SenderMask = client.UserMask,
                    ChannelName = Name
                }.SendMessageToClient(ircClient);
            }

            _clients.Remove(client);
        }
    }
}
