using ChatCommon.Models;
using System.Net;


namespace UnitTests.Abstracts
{
    public interface IMessageSource
    {
        Task SendAsync(NetMessage message, IPEndPoint ep);

        NetMessage Receive(ref IPEndPoint ep);
    }
}