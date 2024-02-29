using EasySaveWPF.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = null;
        public static ServerSocketService ServerSocketService { get; } = new ServerSocketService();

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            // Nom unique pour le mutex, basé sur quelque chose d'unique à l'application, par exemple le nom de l'application.
            const string mutexName = "EasySave";

            bool createdNew;
            mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                // Si le mutex existe déjà, cela signifie qu'une instance de l'application est déjà en cours d'exécution.
                MessageBox.Show("An instance of the EasySaveWPF application is already running.", "Instance already running", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); // Ferme l'application actuelle
                return;
            }

            ServerSocketService.Connect();
            _ = ServerSocketService.AcceptConnectionAsync();
            
            LocalizationService.SetCulture(EasySaveWPF.Properties.Settings.Default.Language);
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            ServerSocketService.Disconnect();

            if (mutex != null)
            {
                mutex.ReleaseMutex(); // Libère le mutex quand l'application se ferme
                mutex = null;
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ServerSocketService.Disconnect();

            Debug.WriteLine(e.Exception.Message);

            MessageBox.Show("Internal error.");
        }
    }
}
