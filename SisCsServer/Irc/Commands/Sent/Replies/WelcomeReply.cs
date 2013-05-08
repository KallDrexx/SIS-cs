using System;

namespace SisCsServer.Irc.Commands.Sent.Replies
{
    public class WelcomeReply : ISentCommands
    {
        private const int ResponseCode = 001;
        private const string Message = "Welcome to the SIS server";

        public string SenderAddress { get; set; }
        public string NickName { get; set; }

        public string FormFullResponseString()
        {
            return string.Format(":{0} {1} {2} {3}",
                                 SenderAddress, ResponseCode.ToString("000"), NickName, Message);
        }
    }
}
