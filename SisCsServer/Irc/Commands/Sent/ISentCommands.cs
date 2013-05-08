namespace SisCsServer.Irc.Commands.Sent
{
    public interface ISentCommands
    {
        string SenderAddress { get; set; }
        string NickName { get; set; }
        string FormFullResponseString();
    }
}
