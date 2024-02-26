using System.ComponentModel;
using EasySaveWPF.Models;
using EasySaveWPF.Services;
using System.IO;
using System.Windows;

namespace EasySaveWPF.ViewModels;

/// <summary>
/// ViewModel responsible for handling real-time logging and state changes of backup operations.
/// </summary>
public class LogStatsRTViewModel
{
    // Directory path where logs are stored.
    private static readonly string LogDirPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PS-Logs");

    public WriteStatsRT WriteStatsRT { get; } = new WriteStatsRT();
    public string Type { get; set; } = "xml";

    /// <summary>
    /// Sets a new backup model for monitoring and logs state changes.
    /// </summary>
    /// <param name="statsRTModel">The backup model to monitor.</param>
    public void NewWork(StatsRTModel statsRTModel)
    {
        // Detach event handler from the current model if it exists.
        if (statsRTModel is not null)
            statsRTModel.PropertyChanged -= OnStatsRTModelPropertyChanged;

        // Attach the new backup model and set up property change notification.
        statsRTModel!.PropertyChanged += OnStatsRTModelPropertyChanged;
    }

    /// <summary>
    /// Responds to property changes on the current backup model.
    /// </summary>
    private void OnStatsRTModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var statsRTModel = sender as StatsRTModel;

        // Handle state changes specifically.
        if (e.PropertyName == nameof(StatsRTModel.State))
            HandleStateChangeAsync(statsRTModel!);
    }

    /// <summary>
    /// Handles state changes by writing real-time stats.
    /// </summary>
    /// <param name="state">The backup model with updated state.</param>
    /*private void HandleStateChangeAsync(StatsRTModel state)
    {
        var task = Task.Run(async () => await WriteStatsRT.WriteRealTimeStatsAsync(state, LogDirPath, Type));
        task.Wait();
    }*/
    private async void HandleStateChangeAsync(StatsRTModel state)
    {
        await WriteStatsRT.WriteRealTimeStatsAsync(state, LogDirPath, Type);
    }

    /// <summary>
    /// Writes log information synchronously.
    /// </summary>
    /// <param name="log">The log information to write.</param>
    public void WriteLog(LogVarModel log) =>
        WriteStatsRT.WriteLogsSync(log, LogDirPath, Type);
}