namespace SisCsServer.Irc.Commands.Received
{
    [IrcCommand("NICK", false)]
    public class NickCommand : IReceivedCommand
    {
        public void ProcessCommand(string[] args, IrcClient client, IrcController controller)
        {
            if (args.Length == 0 || args[0].Trim() == string.Empty)
                return;

            client.NickName = args[0].Trim();

            // Attempt to activate the user
            client.AttemptUserActivation();
            client.SetUserMask();
        }
    }
}
