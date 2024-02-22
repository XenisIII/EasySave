using System.Diagnostics;
using System.Windows.Input;
using EasySaveWPF.Models;
using EasySaveWPF.Services;
using System.Collections.ObjectModel;
using System.Windows;
using EasySaveWPF.Services.Save;
using EasySaveWPF.Common;

namespace EasySaveWPF.ViewModels;

/// <summary>
/// Manages execution of backup processes and logging.
/// </summary>
public class SaveProcessViewModel : ObservableObject
{
    private LogStatsRTViewModel logStatsRTViewModel = new LogStatsRTViewModel();

    public SaveProcessViewModel()
    {
        ApplyChangesCommand = new RelayCommand(ApplySettingsChanges);
        DeleteSaveCommand = new RelayCommand(DeleteSaveFunc);
        ExecuteSaveCommand = new RelayCommand(ExecuteSaveProcess);
        CheckBoxChangedCommand = new RelayCommand<BackupJobModel>(HandleCheckBoxChanged);
    }

    // Holds all configured save tasks.
    public ICommand? DeleteSaveCommand { get; set; }
    public ICommand? ExecuteSaveCommand { get; set; }
    public ICommand? ApplyChangesCommand { get; set; }
    public ICommand CheckBoxChangedCommand { get; }

    /// <summary>
    /// A list containing instances of BackupJobModel, each representing a unique backup job configuration.
    /// This property is initialized as an empty list, ready to store backup configurations.
    /// </summary>
    public ObservableCollection<BackupJobModel> BackupJobs { get; set; } = new ObservableCollection<BackupJobModel>();

    //public ObservableCollection<CreateSave> SelectedSaves { get; set; } = new ObservableCollection<CreateSave>();
    public ObservableCollection<BackupJobModel> CheckedItems { get; set; } = new ObservableCollection<BackupJobModel>();

    // Represents the current log for ongoing save task.
    public LogVarModel CurrentLogModel { get; set; }

    private string _logType;
    public string LogType
    {
        get => _logType;
        set
        {
            if (_logType == value) return;

            _logType = value;
            OnPropertyChanged(nameof(LogType));
        }
    }

    private string _processMetier;
    public string ProcessMetier
    {
        get => _processMetier;
        set
        {
            if (_processMetier == value) return;

            _processMetier = value;
            OnPropertyChanged(nameof(ProcessMetier));
        }
    }

    private string _language;
    public string Language
    {
        get => _language;
        set
        {
            if (_language == value) return;

            _language = value;
            OnPropertyChanged(nameof(Language));
        }
    }

    /*private bool _Complete;
    public bool Complete
    {
        get => _Complete;
        set => _Complete = value;
    }
    private bool _Differential;
    public bool Differential
    {
        get => _Differential;
        set => _Differential = value;
    }*/

    /// <summary>
    /// Executes the save process.
    /// </summary>
    public void ExecuteSaveProcess()
    {
        foreach (var createSave in CheckedItems)
        {
            //Must be replace in oother version
            //Thread.Sleep(100);
            var stopwatch = new Stopwatch();
            //var save = this.SaveList.SaveList[saveIndex];
            switch (createSave.Type)
            {
                case "Complete":
                    var completeSave = new CompleteSave(createSave);

                    logStatsRTViewModel.NewWork(completeSave.StatsRTModel);

                    stopwatch.Start();
                    completeSave.Execute(createSave, ProcessMetier);
                    stopwatch.Stop();

                    var timeElapsed = stopwatch.Elapsed;

                    var elapsedTimeFormatted = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                      timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds,
                      timeElapsed.Milliseconds / 10);

                    SetLogModel(createSave.Name, createSave.SourcePath, createSave.TargetPath,
                      completeSave.StatsRTModel.TotalFilesSize, elapsedTimeFormatted);
                    break;
                case "Differential":
                    var differentialSave = new DifferentialSave(createSave);

                    logStatsRTViewModel.NewWork(differentialSave.StatsRTModel);

                    stopwatch.Start();
                    differentialSave.Execute(createSave, ProcessMetier);
                    stopwatch.Stop();

                    var timeElapsed1 = stopwatch.Elapsed;
                    var elapsedTimeFormatted1 =
                      $"{timeElapsed1.Hours:00}:{timeElapsed1.Minutes:00}:{timeElapsed1.Seconds:00}.{timeElapsed1.Milliseconds / 10:00}";
                    SetLogModel(createSave.Name, createSave.SourcePath, createSave.TargetPath,
                      differentialSave.StatsRTModel.TotalFilesSize, elapsedTimeFormatted1);
                    break;
            }
        }
    }

    /// <summary>
    /// Sets and updates the log model with backup process details.
    /// </summary>
    public void SetLogModel(string name, string sourcePath, string targetPath, long filesSize, string fileTransferTime)
    {
        var model = new LogVarModel()
        {
            Name = name,
            SourcePath = sourcePath,
            TargetPath = targetPath,
            FilesSize = filesSize,
            FileTransferTime = fileTransferTime,
            Time = DateTime.Now.ToString("yyyyMMdd_HHmmss")
        };

        // Update current state.
        CurrentLogModel = model;

        logStatsRTViewModel.WriteLog(this.CurrentLogModel);
    }

    /// <summary>
    /// Deletes specified save tasks based on their indices.
    /// </summary>
    public void DeleteSaveFunc()
    {
        // Créer une copie de la liste CheckedItems pour éviter les modifications pendant l'itération
        var itemsToRemove = CheckedItems.ToList();

        foreach (var item in itemsToRemove)
        {
            // Supprimer chaque élément coché de la liste principale
            BackupJobs.Remove(item);
        }

        // Vider CheckedItems après la suppression des éléments de la liste principale
        CheckedItems.Clear();

        // Notifier que la liste principale a changé, si nécessaire
        //OnPropertyChanged(nameof(SaveList.SaveList));
        /*whichSaveToDelete.Sort();
        whichSaveToDelete.Reverse();

        foreach (var index in whichSaveToDelete.Where(index => index >= 0 && index < this.SaveList.SaveList.Count))
            this.SaveList.SaveList.RemoveAt(index);*/
    }

    public void ApplySettingsChanges()
    {
        string language_code = "fr-FR";
        logStatsRTViewModel.Type = LogType;
        switch (Language)
        {
            case "English":
                language_code = "en-US";
                break;
            case "Français":
                language_code = "fr-FR";
                break;
        }
        LocalizationService.SetCulture(language_code);
        MessageBox.Show($"Changements appliqués avec succès!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void HandleCheckBoxChanged(BackupJobModel save)
    {
        if (!CheckedItems.Contains(save))
        {
            CheckedItems.Add(save);
        }
        else
        {
            CheckedItems.Remove(save);
        }
    }
}