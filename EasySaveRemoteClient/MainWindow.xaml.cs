using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySaveRemoteClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Socket clientSocket;
    private const string ServerIP = "127.0.0.1";
    private const int ServerPort = 53032;
    public MainWindow()
    {
        InitializeComponent();
        ConnectToServer();
    }
    private void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort), new AsyncCallback(ConnectCallback), null);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cannot connect to server: {ex.Message}");
        }
    }

    private void ConnectCallback(IAsyncResult AR)
    {
        try
        {
            clientSocket.EndConnect(AR);
            Dispatcher.Invoke(() =>
            {
                //startButton.IsEnabled = true;
            });
            Task.Run(() => ReceiveData());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cannot connect to server: {ex.Message}");
        }
    }

    private void ReceiveData()
    {
        byte[] buffer = new byte[1024];
        try
        {
            while (true)
            {
                int received = clientSocket.Receive(buffer);
                if (received == 0) return;
                int value = int.Parse(Encoding.UTF8.GetString(buffer, 0, received));
                Dispatcher.Invoke(() =>
                {
                    //progressBar.Value = value;
                    //Pourcentage.Content = $"{value}%";
                });
                if (value == 100)
                {
                    Dispatcher.Invoke(() =>
                    {
                        //Finished.Content = "Finished";
                    });
                }
            }
        }
        catch
        {

        }
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        byte[] data = Encoding.UTF8.GetBytes("Start");
        await clientSocket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
    }

    private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }
}
