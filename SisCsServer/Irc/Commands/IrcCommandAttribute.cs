using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCsServer.Irc.Commands
{
    public class IrcCommandAttribute : Attribute
    {
        public string CommandName { get; private set; }

        public IrcCommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}
