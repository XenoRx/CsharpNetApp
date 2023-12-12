namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task = UDPClient.SendMessageAsync();
        }
    }
}
