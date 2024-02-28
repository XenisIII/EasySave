using EasySaveWPF.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;


namespace EasySaveWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServerSocketService ServerSocketService { get; } = new ServerSocketService();

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private void OnStartup(object sender, StartupEventArgs e)
    {
        ServerSocketService.Connect();
        _ = ServerSocketService.AcceptConnectionAsync();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        ServerSocketService.Disconnect();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ServerSocketService.Disconnect();

        Debug.WriteLine(e.Exception.Message);

        MessageBox.Show("Internal error.");
    }

}
