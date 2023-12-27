
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnitTests.Abstracts;
using static EntityFramework.NetMessages;

namespace UnitTests.Services
{
    public class UdpMessageSouce : IMessageSource
    {
        private readonly UdpClient _udpClient;
        public UdpMessageSouce()
        {
            _udpClient = new UdpClient(12345);
        }
        public NetMessage Receive(ref IPEndPoint ep)
        {
            byte[] data = _udpClient.Receive(ref ep);
            string str = Encoding.UTF8.GetString(data);
            return NetMessage.DeserializeMessgeFromJSON(str) ?? new NetMessage();
        }

        public async Task SendAsync(NetMessage message, IPEndPoint ep)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message.SerialazeMessageToJSON());

            await _udpClient.SendAsync(buffer, buffer.Length, ep);
        }

        public Task SendAsync(NetMessage message, IPEndPoint ep)
        {
            throw new NotImplementedException();
        }

        NetMessage IMessageSource.Receive(ref IPEndPoint ep)
        {
            throw new NotImplementedException();
        }

        ChatCommon.Models.NetMessage IMessageSource.Receive(ref IPEndPoint ep)
        {
            throw new NotImplementedException();
        }

        Task IMessageSource.SendAsync(ChatCommon.Models.NetMessage message, IPEndPoint ep)
        {
            throw new NotImplementedException();
        }
    }
}