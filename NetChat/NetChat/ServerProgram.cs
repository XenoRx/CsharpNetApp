namespace NetChat
{
    internal class ServerProgram
    {
        static void Main(string[] args)
        {
            var server = new UDPServer();
            Task task = server.ServerListenerAsync();
        }
    }
}
