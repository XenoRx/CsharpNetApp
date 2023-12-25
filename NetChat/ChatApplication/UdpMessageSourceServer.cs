﻿using ChatCommon.Abstractions;
using ChatCommon.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace ChatApplication
{
    public class UdpMessageSouceServer : IMessageSourceServer<IPEndPoint>
    {
        private readonly UdpClient _udpClient;
        public UdpMessageSouceServer()
        {
            _udpClient = new UdpClient(12345);
        }

        public IPEndPoint CopyEndpoint(IPEndPoint ep)
        {
            return new IPEndPoint(ep.Address, ep.Port);
        }

        public IPEndPoint CreateEndpoint()
        {
            return new IPEndPoint(IPAddress.Any, 0);
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

        public Task SendAsyncChatApp(NetMessage message, IPEndPoint ep)
        {
            throw new NotImplementedException();
        }

        ChatCommon.Models.NetMessage IMessageSourceServer<IPEndPoint>.Receive(ref IPEndPoint ep)
        {
            throw new NotImplementedException();
        }
    }
}
