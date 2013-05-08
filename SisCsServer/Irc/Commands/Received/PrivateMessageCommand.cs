using System;

namespace SisCsServer.Irc.Commands.Received
{
    [IrcCommand("PRIVMSG")]
    public class PrivateMessageCommand : IReceivedCommand
    {
        public void ProcessCommand(string[] args, IrcClient client, IrcController controller)
        {
            if (args.Length < 2)
                return;

            controller.SendPrivateMessageToUser(client, args[0], args[1]);
        }
    }
}
