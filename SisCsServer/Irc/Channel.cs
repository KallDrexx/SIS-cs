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

            new ChannelUserListReply
            {
                SenderAddress = Server.HostName,
                RecipientNickName = client.NickName,
                ChannelName = Name,
                Users = _clients.Select(x => x.NickName).ToArray()
            }.SendMessageToClient(client);

            new ChanneluserListEndReply()
            {
                SenderAddress = Server.HostName,
                ClientNick = client.NickName,
                ChannelName = Name
            }.SendMessageToClient(client);
        }
    }
}
