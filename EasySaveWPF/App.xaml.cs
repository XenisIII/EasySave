
﻿using System;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Windows;
using EasySaveWPF.Services;
using System.Diagnostics;
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

        private static Mutex _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "EasySave";

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

        ServerSocketService.Disconnect();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ServerSocketService.Disconnect();

        Debug.WriteLine(e.Exception.Message);

        MessageBox.Show("Internal error.");
    }

            base.OnStartup(e);

            LocalizationService.SetCulture(EasySaveWPF.Properties.Settings.Default.Language);
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