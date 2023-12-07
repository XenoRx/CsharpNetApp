﻿using System.Text.Json;

namespace NetChat
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string NicknameFrom { get; set; }
        public string NicknameTo { get; set; }

        public string SerializeMessageToJson() => JsonSerializer.Serialize(this);
        public static Message? DeserializeFromJson(string message) => JsonSerializer.Deserialize<Message>(message);
        public void Print()
        {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            return $"{DateTime} получено сообщение {Text} от {NicknameFrom}";
        }
    }
}