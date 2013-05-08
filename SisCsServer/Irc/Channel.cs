using System.Collections.Generic;

namespace SisCsServer.Irc
{
    public class Channel
    {
        private readonly List<IrcClient> _clients;

        public string ChannelName { get; private set; }

        public Channel(string name)
        {
            ChannelName = name;
        }


    }
}
