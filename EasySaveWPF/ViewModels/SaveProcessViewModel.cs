using System.Diagnostics;
using System.Net.Http.Headers;
using System.Windows.Input;
using EasySaveWPF.Models;
using EasySaveWPF.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;

namespace EasySaveWPF.ViewModels
{

    /// <summary>
    /// Manages execution of backup processes and logging.
    /// </summary>
    public class SaveProcess
    {
        // Holds all configured save tasks.
        public ICommand? createSave { get; set; }
        public ICommand? _DeleteSave { get; set; }
        public ICommand? ExecuteSave { get; set; }
        public ICommand? ResetValues { get; set; }
        public ICommand? _ApplyChanges { get; set; }
        public ICommand CheckBoxChangedCommand { get; }
        public SavesModel SaveList { get; } = new();
        private LogStatsRTViewModel logStatsRTViewModel = new LogStatsRTViewModel();
        //public ObservableCollection<CreateSave> SelectedSaves { get; set; } = new ObservableCollection<CreateSave>();
        public ObservableCollection<CreateSave> CheckedItems { get; set; } = new ObservableCollection<CreateSave>();

        // Represents the current log for ongoing save task.
        public LogVarModel CurrentLogModel { get; set; }
        private string _Name = "";
        public string Name
        {
            get => _Name;
            set => _Name = value;
        }
        private string _SrcPath = "";
        public string SrcPath
        {
            get => _SrcPath;
            set => _SrcPath = value;
        }
        private string _TargetPath = "";
        public string TargetPath
        {
            get => _TargetPath;
            set => _TargetPath = value;
        }
        private string _Type = "Complete";
        public string Type
        {
            get => _Type;
            set => _Type = value;
        }
        private string _Extensions = "";
        public string Extensions
        {
            get => _Extensions;
            set => _Extensions = value;
        }
        private string _LogType = "xml";
        public string LogType
        {
            get => _LogType;
            set => _LogType = value;
        }
        private string _ProcessMetier;
        public string ProcessMetier
        {
            get => _ProcessMetier;
            set => _ProcessMetier = value;
        }
        private string _Language = "Français";
        public string Language
        {
            get => _Language;
            set => _Language = value;
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
        /// Executes the save process for each selected save task.
        /// </summary>
        /// <param name="whichSaveToExecute">Enumerable of save task indices to execute.</param>

        public SaveProcess(LogStatsRTViewModel logStatsRTViewModel)
        {
            createSave = new RelayCommand(CreateSaveFunc);
            _ApplyChanges = new RelayCommand(ApplySettingsChanges);
            _DeleteSave = new RelayCommand(DeleteSaveFunc);
            ExecuteSave = new RelayCommand(ExecuteSaveProcess);
            ResetValues = new RelayCommand(ResetTempValues);
            CheckBoxChangedCommand = new RelayCommand<CreateSave>(HandleCheckBoxChanged);
        }
        public void ExecuteSaveProcess(object parameter)
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
                            save1.Execute(save, _ProcessMetier);
                            totalfilessize = save1.StatsRTModel.TotalFilesSize;
                            timetoencrypt = save1.ExitCode;
                            errors = save1.EncryptionErrors;
                            break;
                        case "Differential":
                            var save2 = new DifferentialSave(save);
                            //save.Status = "In Progress";
                            logStatsRTViewModel.NewWork(save2.StatsRTModel);
                            save2.Execute(save, _ProcessMetier);
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

            // Attendre que toutes les tâches de sauvegarde soient terminées
            await Task.WhenAll(saveTasks);

            MessageBox.Show($"Toutes les sauvegardes sont terminées", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Adds a new save task configuration to the list.
        /// </summary>
        public void CreateSaveFunc(object parameter)
        {
            if (!ArePathsValid())
            {
                MessageBox.Show("Un ou plusieurs chemins spécifiés n'existent pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {   
                this.SaveList.SaveList.Add(new(_Name, _SrcPath, _TargetPath, _Type, _Extensions, LocalizationService.GetString("SaveReady")));
            }
            _Type = "Complete";
            _Name = "";
            _SrcPath = "";
            _TargetPath = "";
            _Extensions = "";
        }

        public void ResetTempValues(object parameter)
        {
            _Type = "Complete";
            _Name = "";
            _SrcPath = "";
            _TargetPath = "";
            _Extensions = "";
        }

        /// <summary>
        /// Deletes specified save tasks based on their indices.
        /// </summary>
        public void DeleteSaveFunc(object parameter)
        {
            // Créer une copie de la liste CheckedItems pour éviter les modifications pendant l'itération
            var itemsToRemove = CheckedItems.ToList();

            foreach (var item in itemsToRemove)
            {
                // Supprimer chaque élément coché de la liste principale
                SaveList.SaveList.Remove(item);
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

        public void ApplySettingsChanges(object parameter)
        {
            string language_code = "fr-FR";
            logStatsRTViewModel.Type = _LogType;
            switch (_Language)
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

        private void HandleCheckBoxChanged(CreateSave save)
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
        private bool ArePathsValid()
        {
            return Directory.Exists(_SrcPath) && Directory.Exists(_TargetPath);
        }

    }
}