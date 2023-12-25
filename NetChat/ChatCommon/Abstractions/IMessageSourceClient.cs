using ChatCommon.Models;

namespace ChatCommon.Abstractions
{
    public interface IMessageSourceClient<T>
    {
        Task SendAsync(NetMessage message, T ep);

        NetMessage Receive(ref T ep);

        T CreateEndpoint();

        T GetServer();
    }
}
