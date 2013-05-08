using System;

namespace SisCsServer.Irc.Commands
{
    public class IrcCommandAttribute : Attribute
    {
        public string CommandName { get; private set; }
        public bool RequiresActivatedUser { get; set; }

        public IrcCommandAttribute(string commandName, bool requiresActivatedUser = true)
        {
            CommandName = commandName;
            RequiresActivatedUser = requiresActivatedUser;
        }
    }
}
