﻿using System.Text.Json;
namespace EntityFramework;

public class NetMessages
{
    public enum Commands
    {
        Register,
        Message,
        Confirmation
    }
    public class NetMessage
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string? NickNameFrom { get; set; }
        public string? NickNameTo { get; set; }

        public Commands Commands { get; set; }

        public string SerialazeMessageToJSON() => JsonSerializer.Serialize(this);

        public static NetMessage? DeserializeMessgeFromJSON(string message) => JsonSerializer.Deserialize<NetMessage>(message);

        public void PrintGetMessageFrom()
        {
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"{DateTime} \n Revieved a message {Text} \n from {NickNameFrom}  ";
        }
    }
}
