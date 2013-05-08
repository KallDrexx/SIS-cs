namespace SisCsServer.Irc.Commands.Sent.Errors
{
    public class ErrorNeedMoreParamsCommand : ISentCommands
    {
        private const int ResponseCode = 461;
        private const string ResponseText = ":Not enough parameters";

        public string SenderAddress { get; set; }
        public string Command { get; set; }
        public string NickName { get; set; }

        public string FormFullResponseString()
        {
            return string.Format(":{0} {1} {2} {3} {4}", 
                SenderAddress, ResponseCode, NickName, Command, ResponseText);
        }
    }
}
