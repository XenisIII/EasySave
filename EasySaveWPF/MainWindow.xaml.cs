using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace ProcessManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadProcesses();
        }

        private void LoadProcesses()
        {
            var processList = Process.GetProcesses().Select(p => new
            {
                Id = p.Id,
                Name = p.ProcessName,
                Priority = p.BasePriority.ToString(),
                VirtualMemory = p.VirtualMemorySize64.ToString()
            }).ToList();

            lvProcessus.ItemsSource = processList;
        }

        private void btnStopProcess_Click(object sender, RoutedEventArgs e)
        {
            if (lvProcessus.SelectedItem != null)
            {
                var process = (dynamic)lvProcessus.SelectedItem;
                try
                {
                    Process.GetProcessById(process.Id).Kill();
                    MessageBox.Show($"Processus {process.Name} arrêté.");
                    LoadProcesses(); // Recharger la liste après l'arrêt d'un processus
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Impossible d'arrêter le processus sélectionné : {ex.Message}");
                }
            }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
