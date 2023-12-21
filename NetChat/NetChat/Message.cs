using System.Text.Json;

namespace NetChat
{

    public class Message
    {
        public enum Commands
        {
            Register,
            Message,
            Delete,
            Confirm
        }
        public bool IsRead = false;
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; }
        public string NicknameFrom { get; set; }
        public string NicknameTo { get; set; }
        public string ListId { get; set; }
        public Commands Command { get; set; }
        public DateTime DateTime { get; set; }
        public string Recipient { get; set; }
        public string FormattedDateTime => DateTime.ToString("yyyy-MM-dd HH:mm:ss");


        private void ValidateMessageFields(string text, string nicknameFrom, string nicknameTo)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Message text cannot be empty");
            }

            if (string.IsNullOrEmpty(nicknameFrom))
            {
                throw new ArgumentException("NicknameFrom cannot be empty");
            }

            if (string.IsNullOrEmpty(nicknameTo))
            {
                throw new ArgumentException("NicknameTo cannot be empty");
            }
        }





        public string SerializeMessageToJson() => JsonSerializer.Serialize(this);
        public static Message? DeserializeFromJson(string message) => JsonSerializer.Deserialize<Message>(message);

        public void Print()
        {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            return $"{FormattedDateTime} получено сообщение {Text} от {NicknameFrom} (ID: {Id})";
            //return $"{DateTime} получено сообщение {Text} от {NicknameFrom}";
        }
    }
}