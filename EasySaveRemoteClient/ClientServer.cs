// Ajoutez ces usings en haut de votre fichier
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveRemoteClient;

public class ClientServer
{
    private TcpClient client;
    private NetworkStream stream;
    
    public async Task ConnectToServer(string host, int port)
    {
        client = new TcpClient();
        await client.ConnectAsync(host, port);
        stream = client.GetStream();
        StartListening();
    }

    private async void StartListening()
    {
        var buffer = new byte[1024];
        int bytesRead;
        while (client.Connected)
        {
            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            // Update the UI based on the message
        }
    }

    public async Task SendCommand(string command)
    {
        var buffer = Encoding.UTF8.GetBytes(command);
        await stream.WriteAsync(buffer, 0, buffer.Length);
    }
}
