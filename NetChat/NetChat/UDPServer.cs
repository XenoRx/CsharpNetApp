using System.Net;
using System.Net.Sockets;
using System.Text;
using static NetChat.Message;

namespace NetChat
{
    public class UDPServer
    {
        public List<User> Users = new List<User>();
        // Список сообщений
        public Dictionary<string, MessageList> MessageLists { get; set; } = new Dictionary<string, MessageList>();
        public async Task ServerListenerAsync()
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Dictionary<string, Message> messages = new Dictionary<string, Message>();

            Console.WriteLine("Waiting for client's message: ");

            CancellationTokenSource cts = new CancellationTokenSource();

            while (!cts.IsCancellationRequested)
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
                    Thread.Sleep(1000);
                    string confirmationMessage = "Message received successfully";
                    byte[] confirmationData = Encoding.UTF8.GetBytes(confirmationMessage);
                    udpClient.Send(confirmationData, confirmationData.Length, iPEndPoint);
                    //
                    int bytes = await udpClient.SendAsync(confirmationData, iPEndPoint);
                }
                if (message.Text.ToLower().Equals("exit")) cts.Cancel();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public void AddClient(Message message)
        {
            if (message.Command == Commands.Register)
            {
                if (message.Command == Commands.Register)
                {
                    //adds user to the list
                    User user = new User(message.NicknameFrom);
                    Users.Add(user);
                }
            }
        }
        public void RemoveClient(Message message)
        {
            if (message.Command == Commands.Delete)
            {
                //delete user from the list
                foreach (User users in Users)
                {
                    if (users.Nickname == message.NicknameFrom)
                    {
                        Users.Remove(users);
                        break;
                    }
                }
            }
        }
        public void SendToAll(Message message)
        {
            //send message to all users in list
            foreach (User users in Users)
            {
                users.Send(message);
            }
        }
        public void SendTo(Message message)
        {
            // search user by nickname
            User? user = Users.FirstOrDefault(u => u.Nickname == message.Recipient);

            // sent message to the user
            if (user != null)
            {
                user.Send(message);
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