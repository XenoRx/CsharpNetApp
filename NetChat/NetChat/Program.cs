using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetChat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server("Hello");

        }

        public static void Server(string name)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Dictionary<string, Message> messages = new Dictionary<string, Message>();

            Console.WriteLine("Сервер ждет сообщение от клиента");

            while (true)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                if (buffer == null) break;

                var messageText = Encoding.UTF8.GetString(buffer);
                Message? message = Message.DeserializeFromJson(messageText);

                // Check if the message has already been processed
                if (!messages.ContainsKey(message.Id))
                {
                    message.Print();
                    messages.Add(message.Id, message);

                    // Process the message and send a confirmation message back to the client
                    Thread.Sleep(1000); // Add a 1-second delay
                    string confirmationMessage = "Message received successfully";
                    byte[] confirmationData = Encoding.UTF8.GetBytes(confirmationMessage);
                    udpClient.Send(confirmationData, confirmationData.Length, iPEndPoint);
                }
            }
        }
        static Message DeserializeMessage(byte[] buffer)
        {
            var json = Encoding.UTF8.GetString(buffer);
            return Message.DeserializeFromJson(json);

        }
        byte[] SerializeToJson(Message message)
        {
            var json = message.SerializeMessageToJson();
            return Encoding.UTF8.GetBytes(json);
        }
    }
}