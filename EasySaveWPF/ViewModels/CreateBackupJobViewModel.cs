using System.Collections.ObjectModel;
using EasySaveWPF.Common;
using EasySaveWPF.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using EasySaveWPF.Services;
using System.Text.Json;


namespace EasySaveWPF.ViewModels;

public class CreateBackupJobViewModel : ObservableObject
{
    private readonly SaveProcessViewModel _saveProcessViewModel;

    public CreateBackupJobViewModel(SaveProcessViewModel saveProcessViewModel)
    {
        _saveProcessViewModel = saveProcessViewModel;

        BrowsePathCommand = new RelayCommand<string>(Browse);
        CreateSaveCommand = new RelayCommand(Create, CanCreate);
        ResetValuesCommand = new RelayCommand(ResetTempValues);
    }

    private BackupJobModel _backupJob = new();
    public BackupJobModel BackupJob
    {
        get => _backupJob;
        set
        {
            if (_backupJob == value) return;

            _backupJob = value;
            OnPropertyChanged(nameof(BackupJob));
        }
    }

    public ICommand? BrowsePathCommand { get; set; }
    public ICommand? CreateSaveCommand { get; set; }
    public ICommand? ResetValuesCommand { get; set; }

    public void Browse(string propertyName)
    {
        OpenFolderDialog folderDialog = new()
        {
            Multiselect = false
        };

        if (folderDialog.ShowDialog() is not true
            || folderDialog.FolderNames.Length == 0)
        {
            return;
        }

        if (propertyName == nameof(BackupJob.SourcePath))
        {
            BackupJob.SourcePath = folderDialog.FolderNames[0]; // Set the source path text box
        }
        else if (propertyName == nameof(BackupJob.TargetPath))
        {
            BackupJob.TargetPath = folderDialog.FolderNames[0]; // Set the destination path text box
        }
    }

    /// <summary>
    /// Adds a new save task configuration to the list.
    /// </summary>
    public void Create()
    {
        if (!ArePathsValid())
        {
            MessageBox.Show(LocalizationService.GetString("CBJVMUnknownPath"), LocalizationService.GetString("CBJVMUnknownPathCapt"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            _saveProcessViewModel.BackupJobs.Add(BackupJob);

            // Serialize the BackupJobs list to a JSON string
            string jsonString = JsonSerializer.Serialize(_saveProcessViewModel.BackupJobs, new JsonSerializerOptions { WriteIndented = true });

            // Define the file name
            string fileName = "backupJobs.json";

            // Construct the full file path in the application's root directory
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            // Write the JSON string to the file, overwriting any existing content
            File.WriteAllText(filePath, jsonString);

            // Optionally, send the BackupJobs list asynchronously over the server socket
            _ = App.ServerSocketService.SendAsync(_saveProcessViewModel.BackupJobs);
        }

        ResetCreateBackupJobForm();
    }
    
    public void LoadBackupJobsFromFile()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backupJobs.json");
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            var backupJobs = JsonSerializer.Deserialize<List<BackupJobModel>>(jsonString);
            if (backupJobs != null)
            {
                foreach (var job in backupJobs)
                {
                    _saveProcessViewModel.BackupJobs.Add(job);
                }
            }
        }
    }

    public bool CanCreate() => !string.IsNullOrWhiteSpace(BackupJob.Name) &&
                  !string.IsNullOrWhiteSpace(BackupJob.SourcePath) &&
                  !string.IsNullOrWhiteSpace(BackupJob.TargetPath);

    public void ResetTempValues()
    {
        ResetCreateBackupJobForm();
    }

    private void ResetCreateBackupJobForm()
    {
        BackupJob = new BackupJobModel();
    }

    private bool ArePathsValid()
        => Directory.Exists(BackupJob.SourcePath) && Directory.Exists(BackupJob.TargetPath);
}
