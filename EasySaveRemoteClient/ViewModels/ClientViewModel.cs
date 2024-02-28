using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveRemoteClient.Models;
using EasySaveRemoteClient.Common;
using System.Collections.ObjectModel;
using EasySaveRemoteClient.Services;
using System.Windows;

namespace EasySaveRemoteClient.ViewModels
{
    public class ClientViewModel : ObservableObject
    {
        public ObservableCollection<BackupJobModel> BackupJobs { get; set; } = new ObservableCollection<BackupJobModel>();

        public ClientViewModel() 
        { 
             _ = App.ClientSocketService.ListenAsync<ObservableCollection<BackupJobModel>>(
            action: UpdateTransfertProgress);
        }

        private void UpdateTransfertProgress(ObservableCollection<BackupJobModel> SaveList)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                BackupJobs.Clear();
                foreach (var job in SaveList)
                {
                    BackupJobs.Add(job);
                }
            });
        }

    }
}
