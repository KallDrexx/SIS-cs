using System.Collections.Generic;
using SisCsServer.Irc.Commands.Sent.Replies;

namespace SisCsServer.Irc
{
    public class IrcController
    {
        private readonly List<IrcClient> _clients;

        public IrcController(List<IrcClient> clients)
        {
            _clients = clients;
        }

        public void SendActivationMessages(IrcClient client)
        {
            var message = new WelcomeReply
            {
                SenderAddress = Server.HostName,
                NickName = client.NickName
            }.FormFullResponseString();

            client.SendMessage(message);
        }
    }
}
