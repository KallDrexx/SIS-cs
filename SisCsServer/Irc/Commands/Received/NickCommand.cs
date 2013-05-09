using SisCsServer.Irc.Commands.Sent;
using SisCsServer.Irc.Commands.Sent.Errors;

namespace SisCsServer.Irc.Commands.Received
{
    [IrcCommand("NICK", false)]
    public class NickCommand : IReceivedCommand
    {
        public void ProcessCommand(string[] args, IrcClient client, IrcController controller)
        {
            if (args.Length == 0 || args[0].Trim() == string.Empty)
                return;

            if (controller.NickNameInUse(args[0]))
            {
                new NicknameInUseError
                {
                    SenderAddress = Server.HostName,
                    SenderNickName = client.NickName,
                    AttemptedNickName = args[0]
                }.SendMessageToClient(client);

                return;
            }

            client.NickName = args[0];

            // Attempt to activate the user
            client.AttemptUserActivation();
            client.SetUserMask();
        }
    }
}
