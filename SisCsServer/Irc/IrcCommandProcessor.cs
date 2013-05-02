using System;
using System.Collections.Generic;

namespace SisCsServer.Irc
{
    public class IrcCommandProcessor
    {
        private readonly IEnumerable<IrcClient> _clients;

        public IrcCommandProcessor(IEnumerable<IrcClient> clients)
        {
            _clients = clients;
        }

        public void ProcessCommand(IrcClient client, string command)
        {
            int spaceIndex = command.IndexOf(' ');
            if (spaceIndex <= 0)
                return;

            var commandName = command.Substring(0, spaceIndex);
            if (commandName.Equals("nick", StringComparison.InvariantCultureIgnoreCase))
                client.NickName = command.Substring(spaceIndex + 1);
        }
    }
}
