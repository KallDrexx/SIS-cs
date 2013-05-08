namespace SisCsServer.Irc.Commands.Sent
{
    public interface ISentCommands
    {
        string SenderAddress { get; set; }
        string SenderNickName { get; set; }
        string FormFullResponseString();
    }
}
