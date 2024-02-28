using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;

namespace EasySaveRemoteClient.Services;

public class ClientSocketService
{
    public Socket? ClientSocket { get; private set; }

    public void Connect()
    {
        IPEndPoint iPEndPoint = new(IPAddress.Parse("127.0.0.1"), 9050);

        Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            client.Connect(iPEndPoint);
        }
        catch (SocketException e)
        {
            Debug.WriteLine("Unable to connect to server.");
            Debug.WriteLine(e.ToString());
            throw;
        }

        ClientSocket = client;
    }

    public void Disconnect()
    {
        ClientSocket?.Shutdown(SocketShutdown.Both);
        ClientSocket?.Close();
        ClientSocket?.Dispose();
    }

    public async Task ListenAsync<T>(Action<T> action)
    {
        // Loop to listen to the data sent by the server 
        while (true)
        {
            // Retrieves the message from the server
            byte[] data = new byte[1024];
            int receiveData = await ClientSocket!.ReceiveAsync(data, SocketFlags.None);

            // Encodes all bytes to string
            string serializedMessage = Encoding.UTF8.GetString(data, 0, receiveData);
            var message = JsonSerializer.Deserialize<T>(serializedMessage);

            // Executes the delegate
            action(message!);
        }
    }
}
