using System.Net;
using System.Threading;

namespace SisCsServer
{
    class Program
    {
         static void Main(string[] args)
         {
             var ctx = new SingleThreadSynchronizationContext();
             SynchronizationContext.SetSynchronizationContext(ctx);

             var server = new Server(IPAddress.Any, 9001);
             server.Run();

             ctx.RunMessagePump();
         }
    }
}