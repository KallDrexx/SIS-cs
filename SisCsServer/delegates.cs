namespace SisCsServer
{
    public delegate void MessageReceivedDelegate(NetworkClient client, string message);

    public delegate void ClientDisconnectedDelegate(NetworkClient client);
}
