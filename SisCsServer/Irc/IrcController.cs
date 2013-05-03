using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
