using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using EasySaveRemoteClient.Services;
using EasySaveRemoteClient.ViewModels;

namespace EasySaveRemoteClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ClientViewModel clientViewModel { get; set; }
    public static ClientSocketService ClientSocketService { get; } = new ClientSocketService();

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private void OnStartup(object sender, StartupEventArgs e)
    {
        ClientSocketService.Connect();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        ClientSocketService.Disconnect();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Debug.WriteLine(e.Exception.Message);

        MessageBox.Show("Internal error.");

        ClientSocketService.Disconnect();
    }
}
