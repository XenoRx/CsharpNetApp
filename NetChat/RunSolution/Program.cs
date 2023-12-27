using ChatApplication;
using System.Net;
namespace RunSolution
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                var s = new Server<IPEndPoint>(new UdpMessageSouceServer());
                await s.Start();
            }
            else
            if (args.Length == 1)
            {
                var c = new Client<IPEndPoint>(new UdpMessageSourceClient(), args[0]);
                await c.Start();
            }
            else
            {

                Console.WriteLine("Enter nickname to run the server");
                Console.WriteLine("Enter nickname and ip address to run the client");
            }

            Console.ReadKey(true);
        }
    }
}
