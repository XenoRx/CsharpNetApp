﻿
using ChatCommon.Models;

namespace EntityFramework
{
    public class User
    {
        public virtual List<Message>? MessagesTo { get; set; } = new();
        public virtual List<Message>? MessagesFrom { get; set; } = new();
        public int Id { get; set; }
        public string? FullName { get; set; }

        //public static implicit operator User(global::ChatCommon.Models.User v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
