using NetChat;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    public class UDPClient
    {
        public static List<Message> Messages = new List<Message>();
        public static async Task RetrieveMessagesAsync()
        {
            // Get the UDP client instance
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

            // Retrieve messages from the server
            await RetrieveMessagesAsync(udpClient, iPEndPoint);
        }

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
        public static async Task RetrieveMessagesAsync(UdpClient udpClient, IPEndPoint iPEndPoint)
        {
            // Create a new MessageList object for the client
            var messageList = new MessageList();
            messageList.UserId = Environment.UserName;

            // Send a request to the server to get the messages
            string request = JsonSerializer.Serialize(messageList);
            byte[] data = Encoding.UTF8.GetBytes(request);
            udpClient.Send(data, data.Length, iPEndPoint);

            // Receive the response from the server
            byte[] responseBuffer = udpClient.Receive(ref iPEndPoint);
            string response = Encoding.UTF8.GetString(responseBuffer);

            // Deserialize the response from the server
            messageList = JsonSerializer.Deserialize<MessageList>(response);

            // Update the list of messages on the client
            UDPClient.Messages = messageList.Messages;
            // Update the list of messages on the client
            List<Message> unreadMessages = messageList.Messages.Where(message => !message.IsRead).ToList();
            UDPClient.Messages = unreadMessages;
            // Display the unread messages
            if (unreadMessages.Count > 0)
            {
                foreach (var message in unreadMessages)
                {
                    Console.WriteLine(message.Text);
                }
            }
            else
            {
                Console.WriteLine("No unread messages");
            }
        }

        public static void CheckForNewMessages()
        {
            while (true)
            {
                // Wait for a specified amount of time before checking for new messages
                Thread.Sleep(1000);

                try
                {
                    // Retrieve the messages from the server
                    List<Message> messages = UDPClient.Messages;

                    // Update the list of unread messages on the client
                    List<Message> unreadMessages = messages.Where(message => !message.IsRead).ToList();
                    UDPClient.Messages = unreadMessages;

                    // Display the unread messages
                    if (unreadMessages.Count > 0)
                    {
                        foreach (var message in unreadMessages)
                        {
                            Console.WriteLine(message.Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error checking for new messages: " + ex.Message);
                }
            }
        }

    }
}