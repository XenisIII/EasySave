using EasySaveWPF.Common;
using EasySaveWPF.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

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
            MessageBox.Show("Un ou plusieurs chemins spécifiés n'existent pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            _saveProcessViewModel.BackupJobs.Add(BackupJob);
        }

        ResetCreateBackupJobForm();
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
