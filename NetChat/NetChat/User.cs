namespace NetChat
{
    public class User
    {
        private UDPServer _udpServer;
        public string Nickname { get; set; }
        public User(UDPServer udpServer)
        {
            _udpServer = udpServer;
        }

        public User(string nickname)
        {
            Nickname = nickname;
        }
        public void Send(Message message)
        {
            List<User> users = _udpServer.Users;
            User? user = users.FirstOrDefault(u => u.Nickname == message.Recipient);
            if (user == null)
            {
                user?.Send(message);
            }
        }
    }
}
