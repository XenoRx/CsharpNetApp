using ChatCommon.Abstractions;
using ChatCommon.Models;
using System.Net;
using System.Net.Sockets;

namespace ChatApplication
{
    public class Client<T>
    {
        public readonly string _name;

        public IMessageSourceClient<T> _messageSource;
        public T remoteEndPoint;
        public Client(IMessageSourceClient<T> messageSourceClient, string name)
        {
            this._name = name;

            _messageSource = messageSourceClient;
            remoteEndPoint = _messageSource.CreateEndpoint();
        }

        public UdpClient udpClientClient = new UdpClient();
        public async Task ClientListener()
        {
            while (true)
            {
                try
                {
                    var messageReceived = _messageSource.Receive(ref remoteEndPoint);

                    Console.WriteLine($"Recieved message from {messageReceived.NickNameFrom}:");
                    Console.WriteLine(messageReceived.Text);

                    await Confirm(messageReceived, remoteEndPoint);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error with the message recieving: " + ex.Message);
                }
            }
        }

        public async Task Confirm(NetMessage message, T remoteEndPoint)
        {
            message.Command = Command.Confirmation;
            await _messageSource.SendAsync(message, remoteEndPoint);
        }


        public void Register(T remoteEndPoint)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            var message = new NetMessage() { NickNameFrom = _name, NickNameTo = null, Text = null, Command = Command.Register, EndPoint = ep };
            _messageSource.SendAsync(message, remoteEndPoint);
            Console.WriteLine("We are here");
        }

        public async Task ClientSender()
        {


            Register(remoteEndPoint);

            while (true)
            {
                try
                {
                    Console.Write("Enter reciever name: ");
                    var nameTo = Console.ReadLine();

                    Console.Write("spell your message and press Enter: ");
                    var messageText = Console.ReadLine();

                    var message = new NetMessage() { Command = Command.Message, NickNameFrom = _name, NickNameTo = nameTo, Text = messageText };

                    await _messageSource.SendAsync(message, remoteEndPoint);

                    Console.WriteLine("Message sent.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public async Task Start()
        {
            new Thread(async () => await ClientListener()).Start();

            await ClientSender();

        }
    }
}
