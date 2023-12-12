using NetChat;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class UDPClient
    {
        //static void Main(string[] args)
        //{
        //    SentMessage(args[0], args[1]);
        //}



        public static async Task SendMessageAsync()
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

            while (true)
            {
                Console.WriteLine("Message from?");
                string? from = Console.ReadLine();
                string? messageText;

                do
                {
                    //Console.Clear();
                    Console.WriteLine("Enter your message: ");
                    messageText = Console.ReadLine();
                } while (string.IsNullOrEmpty(messageText));

                Message message = new Message()
                {
                    Text = messageText,
                    NicknameFrom = from,
                    NicknameTo = "Server",
                    DateTime = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                };
                //if (messageText == "Exit")
                //{
                //    // Exit the chat
                //    Console.WriteLine("Shutting down the application...");
                //    break;
                //}

                string json = message.SerializeMessageToJson();
                byte[] data = Encoding.UTF8.GetBytes(json);

                // Send the message to the server
                udpClient.Send(data, data.Length, iPEndPoint);

                // Wait for the confirmation message from the server
                byte[] confirmationBuffer = udpClient.Receive(ref iPEndPoint);
                string confirmationMessage = Encoding.UTF8.GetString(confirmationBuffer);

                var JSONmsg = message.SerializeMessageToJson();
                byte[] buffer = Encoding.UTF8.GetBytes(JSONmsg);
                int bytes = await udpClient.SendAsync(buffer, buffer.Length, iPEndPoint);

                Console.WriteLine(confirmationMessage);
            }

        }

    }
}