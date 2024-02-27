using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class RemoteServer
{
    private static readonly List<Socket> clients = new List<Socket>();

    private static async Task AccepterConnexionsAsync(Socket listener)
    {
        while (true)
        {
            Socket client = await listener.AcceptAsync();
            Console.WriteLine("Client connecté depuis : " + client.RemoteEndPoint);
            lock (clients)
            {
                clients.Add(client);
            }
            Task.Run(() => EcouterReseauAsync(client));
        }
    }

    private static async Task EcouterReseauAsync(Socket client)
    {
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                if (bytesRead == 0) break;
                if(Encoding.UTF8.GetString(buffer, 0, bytesRead) == "Start")
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        string message1 = i.ToString();
                        await EnvoyerMessageAsync(message1);
                        Thread.Sleep(100);
                    }
                }

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Le client a envoyé: " + receivedMessage);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors de la communication avec le client : " + ex.Message);
        }
        finally
        {
            lock (clients)
            {
                clients.Remove(client);
            }
            client.Close();
        }
    }

    private static async Task EnvoyerMessageAsync(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        List<Task> sendTasks = new List<Task>();

        lock (clients)
        {
            foreach (var client in clients)
            {
                sendTasks.Add(client.SendAsync(new ArraySegment<byte>(data), SocketFlags.None));
            }
        }

        await Task.WhenAll(sendTasks);
    }

    public static async Task StartServerAsync()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 53032);
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            Console.WriteLine("Serveur démarré. En attente de connexions...");

            _ = AccepterConnexionsAsync(listener);

            while (true)
            {
                /*string message = Console.ReadLine();
                await EnvoyerMessageAsync(message);*/
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors du démarrage du serveur : " + ex.Message);
        }
    }

    public static async Task Main(string[] args)
    {
        await StartServerAsync();
    }
}