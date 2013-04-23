using System.Net;

namespace SisCsServer
{
    class Program
    {
         static void Main(string[] args)
         {
             var server = new Server(IPAddress.Any, 9001);
             server.Run();
         }
    }
}