using NetChat;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SentMessage(args[0], args[1]);
        }



        public static void SentMessage(string from, string ip)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

            while (true)
            {
                string messageText;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Введите сообщение: ");
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
                //Message message = new Message() { Text = messageText, NicknameFrom = from, NicknameTo = "Server", DateTime = DateTime.Now };
                string json = message.SerializeMessageToJson();
                byte[] data = Encoding.UTF8.GetBytes(json);

                // Send the message to the server
                udpClient.Send(data, data.Length, iPEndPoint);

                // Wait for the confirmation message from the server
                byte[] confirmationBuffer = udpClient.Receive(ref iPEndPoint);
                string confirmationMessage = Encoding.UTF8.GetString(confirmationBuffer);

                Console.WriteLine(confirmationMessage);
                Console.ReadLine();
            }
        }

    }
}