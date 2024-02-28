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

    private string _logType = Properties.Settings.Default.LogType;
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

    private string _processMetier = Properties.Settings.Default.masterProcess;
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

    private long _nKo = Properties.Settings.Default.fileSize;
    
    public long NKo
    {
        get => _nKo;
        set
        {
            if (_nKo == value) return;

            _nKo = value;
            OnPropertyChanged(nameof(NKo));
        }
    }

    /// <summary>
    /// Executes the save process.
    /// </summary>
    public void ExecuteSaveProcess()
    {
        var _ = ExecuteSaveProcessAsync();
    }

    public async Task ExecuteSaveProcessAsync()
    {
        long totalfilessize = 0;
        int timetoencrypt = 0;
        int errors = 0;
        var saveTasks = new List<Task>();

        foreach (var save in CheckedItems)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //save.Status = "In Progress";
                save.Status = LocalizationService.GetString("SaveInProgress");
            });

            // Créer une nouvelle tâche pour chaque sauvegarde
            var saveTask = Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                switch (save.Type)
                {
                    case "Complete":
                        var save1 = new CompleteSave(save);
                        //save.Status = "In Progress";
                        logStatsRTViewModel.NewWork(save1.StatsRTModel);
                        save1.Execute(save, _processMetier);
                        totalfilessize = save1.StatsRTModel.TotalFilesSize;
                        timetoencrypt = save1.ExitCode;
                        errors = save1.EncryptionErrors;
                        break;
                    case "Differential":
                        var save2 = new DifferentialSave(save);
                        //save.Status = "In Progress";
                        logStatsRTViewModel.NewWork(save2.StatsRTModel);
                        save2.Execute(save, _processMetier);
                        totalfilessize = save2.StatsRTModel.TotalFilesSize;
                        timetoencrypt = save2.ExitCode;
                        errors = save2.EncryptionErrors;
                        break;
                }

                stopwatch.Stop();
                var elapsedTime = stopwatch.Elapsed;
                var elapsedTimeFormatted = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds,
                    elapsedTime.Milliseconds / 10);

                this.SetLogModel(save.Name, save.SourcePath, save.TargetPath,
                    totalfilessize, elapsedTimeFormatted, timetoencrypt, errors);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    //save.Status = "Completed";
                    save.Status = LocalizationService.GetString("SaveFinished");
                });
                MessageBox.Show($"La sauvegarde {save.Name} est finie", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            saveTasks.Add(saveTask);
        }

        // Attendre que toutes les t�ches de sauvegarde soient termin�es
        await Task.WhenAll(saveTasks);

        MessageBox.Show($"Toutes les sauvegardes sont termin�es", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// Sets and updates the log model with backup process details.
    /// </summary>
    public void SetLogModel(string name, string sourcePath, string targetPath, long filesSize, string fileTransferTime, int timetoencrypt, int nbErrors)
    {
        var model = new LogVarModel()
        {
            Name = name,
            SourcePath = sourcePath,
            TargetPath = targetPath,
            FilesSize = filesSize,
            FileTransferTime = fileTransferTime,
            Time = DateTime.Now.ToString("yyyyMMdd_HHmmss"),
            Encryption = $"{timetoencrypt} ms",
            EncryptionErrors = nbErrors.ToString(),
        };

        // Update current state.
        this.CurrentLogModel = model;

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
        logStatsRTViewModel.Type = LogType;
        Properties.Settings.Default.LogType = LogType;
        Properties.Settings.Default.masterProcess = ProcessMetier;
        Properties.Settings.Default.fileSize = NKo;
        Properties.Settings.Default.Save();
        
        string language_code = "fr-FR";
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