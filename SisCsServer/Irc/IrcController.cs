using System.Collections.Generic;

namespace SisCsServer.Irc
{
    public class IrcController
    {
        private readonly List<IrcClient> _clients;

        public IrcController(List<IrcClient> clients)
        {
            _clients = clients;
        }
    }
}
