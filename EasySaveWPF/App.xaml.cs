using System;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Windows;

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "##||EasySaveWPFUniqueName||##";

            // Tente de créer un mutex global
            bool createdNew;
            _mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                // Si le mutex existe déjà, cela signifie qu'une instance de l'application est déjà en cours d'exécution.
                MessageBox.Show("Une instance de l'application EasySaveWPF est déjà en cours d'exécution.", "Instance déjà en cours", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); // Ferme l'application actuelle
                return;
            }

            base.OnStartup(e);

            // Continuez avec l'initialisation de votre application ici, si nécessaire
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex = null;
            }
            base.OnExit(e);
        }
    }
}