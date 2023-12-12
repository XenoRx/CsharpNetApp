namespace NetChat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new UDPServer();
            Task task = server.ServerListenerAsync();
        }
    }
}
