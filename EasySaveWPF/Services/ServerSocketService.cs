using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows;

namespace EasySaveWPF.Services
{
    public class ServerSocketService
    {
        public Socket? Listener { get; private set; }
        public Socket? Handler { get; private set; }
        public IPEndPoint? HandlerIPEndPoint { get; private set; }

        public void Connect()
        {
            if (Listener is not null) throw new InvalidOperationException(
                message: $"The server is already connected.");

            // Define the server endpoint
            IPEndPoint iPEndPoint = new(
                address: IPAddress.Parse("127.0.0.1"),
                port: 9050);

            // Create a new TCP socket
            Listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the server endpoint
            Listener.Bind(iPEndPoint);

            // Listens for incoming connections.
            Listener.Listen(10);
        }

        public async Task AcceptConnectionAsync()
        {
            // Accept incoming connection from the client
            if (Listener is null) throw new InvalidOperationException(
                message: $"Cannot accept client connection. You should connect to the server before.");

            Handler = await Listener.AcceptAsync();
            MessageBox.Show($"Client connecté depuis :  {Handler.RemoteEndPoint}", "Connected", MessageBoxButton.OK, MessageBoxImage.Information);

            // Retrieves the client's IP address (and port)
            HandlerIPEndPoint = (IPEndPoint?)Handler.RemoteEndPoint;
        }

        public async Task SendAsync<T>(T message)
        {
            if (Handler is null) throw new InvalidOperationException(
                message: $"Cannot send message to {nameof(Handler)}. The client is not connected to the server");

            // Encodes the message in byte[]
            var serializedMessage = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(serializedMessage);

            // Send message to the client
            await Handler.SendAsync(data, SocketFlags.None);
        }

        public void Disconnect()
        {
            Listener?.Close();
            Listener?.Dispose();
        }
    }

}
