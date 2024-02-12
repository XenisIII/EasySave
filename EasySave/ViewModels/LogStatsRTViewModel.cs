using System.ComponentModel;
using EasySave.Models;
using EasySave.Services;
using System;

namespace EasySave.ViewModels
{
    /// <summary>
    /// ViewModel responsible for handling real-time logging and state changes of backup operations.
    /// </summary>
    public class LogStatsRTViewModel
    {
        // Directory path where logs are stored.
        private static readonly string LogDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PS-Logs");

        // Current backup model being monitored.
        private BackupModel _currentBackupModel;

        /// <summary>
        /// Sets a new backup model for monitoring and logs state changes.
        /// </summary>
        /// <param name="backupModel">The backup model to monitor.</param>
        public void NewWork(BackupModel backupModel)
        {
            // Detach event handler from the current model if it exists.
            if (this._currentBackupModel is not null)
                this._currentBackupModel.PropertyChanged -= this.OnBackupModelPropertyChanged;

            // Attach the new backup model and set up property change notification.
            this._currentBackupModel = backupModel ?? throw new ArgumentNullException(nameof(backupModel));
            this._currentBackupModel.PropertyChanged += this.OnBackupModelPropertyChanged;
        }

        /// <summary>
        /// Responds to property changes on the current backup model.
        /// </summary>
        private void OnBackupModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Handle state changes specifically.
            if (e.PropertyName == nameof(BackupModel.State))
                this.HandleStateChange(this._currentBackupModel);
        }

        /// <summary>
        /// Handles state changes by writing real-time stats.
        /// </summary>
        /// <param name="state">The backup model with updated state.</param>
        private void HandleStateChange(BackupModel state) =>
            WriteStatsRT.WriteRealTimeStatsAsync(state, LogDirPath);

        // JB: ici on un méthode synchrone qui appelle une méthode asynchrone?

        /// <summary>
        /// Writes log information synchronously.
        /// </summary>
        /// <param name="log">The log information to write.</param>
        public void WriteLog(LogModel log) =>
            WriteStatsRT.WriteLogsSync(log, LogDirPath);
    }
}
