using System;

namespace SisCsServer.Irc.Commands
{
    [IrcCommand("NICK")]
    public class NickCommand : IReceivedCommand
    {
        public void ProcessCommand(string[] args, IrcClient client, IrcController controller)
        {
            if (args.Length == 0 || args[0].Trim() == string.Empty)
                return;

            client.NickName = args[0].Trim();
        }
    }
}
