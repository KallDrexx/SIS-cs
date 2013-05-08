using SisCsServer.Irc.Commands.Sent.Errors;

namespace SisCsServer.Irc.Commands.Received
{
    [IrcCommand("USER", false)]
    public class UserCommand : IReceivedCommand
    {
        public void ProcessCommand(string[] args, IrcClient client, IrcController controller)
        {
            if (args.Length < 4)
            {
                var replyCommand = new ErrorNeedMoreParamsCommand
                {
                    Command = "USER",
                    SenderAddress = Server.HostName
                };

                client.SendMessage(replyCommand.FormFullResponseString());
                return;
            }

            client.FullName = args[3];

            // Attempt to activate the user
            client.AttemptUserActivation();
        }
    }
}
