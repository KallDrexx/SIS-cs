using System;

namespace SisCsServer.Irc.Commands.Sent
{
    public static class SentCommandExtensions
    {
        public static void SendMessageToClient(this ISentCommand command, IrcClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (command == null)
                throw new ArgumentNullException("command");

            client.SendMessage(command.FormFullResponseString());
        }
    }
}
