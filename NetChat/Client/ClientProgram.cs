namespace Client
{
    internal class ClientProgram
    {
        static async Task Main(string[] args)
        {
            Task task = UDPClient.SendMessageAsync();
            await UDPClient.RetrieveMessagesAsync();

            // Create a thread to check for new messages
            Thread messageCheckerThread = new Thread(UDPClient.CheckForNewMessages);

            messageCheckerThread.Start();

            await UDPClient.SendMessageAsync();

            messageCheckerThread.Join();
        }
    }
}
