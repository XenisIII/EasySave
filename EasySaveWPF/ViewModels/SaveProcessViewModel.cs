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
        private string _LogType;
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
        private string _Language;
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

            foreach (var save in CheckedItems)
            {
                //Must be replace in oother version
                Thread.Sleep(100);
                var stopwatch = new Stopwatch();
                //var save = this.SaveList.SaveList[saveIndex];
                switch (save.Type)
                {
                    case "Complete":
                        var save1 = new CompleteSave(save);
                        logStatsRTViewModel.NewWork(save1.StatsRTModel);
                        stopwatch.Start();
                        save1.Execute(save, _ProcessMetier);
                        stopwatch.Stop();
                        var timeElapsed = stopwatch.Elapsed;
                        var elapsedTimeFormatted = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                          timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds,
                          timeElapsed.Milliseconds / 10);
                        this.SetLogModel(save.Name, save.SourcePath, save.TargetPath,
                          save1.StatsRTModel.TotalFilesSize, elapsedTimeFormatted);
                        break;
                    case "Differential":
                        var save2 = new DifferentialSave(save);
                        logStatsRTViewModel.NewWork(save2.StatsRTModel);
                        stopwatch.Start();
                        save2.Execute(save, _ProcessMetier);
                        stopwatch.Stop();
                        var timeElapsed1 = stopwatch.Elapsed;
                        var elapsedTimeFormatted1 =
                          $"{timeElapsed1.Hours:00}:{timeElapsed1.Minutes:00}:{timeElapsed1.Seconds:00}.{timeElapsed1.Milliseconds / 10:00}";
                        this.SetLogModel(save.Name, save.SourcePath, save.TargetPath,
                          save2.StatsRTModel.TotalFilesSize, elapsedTimeFormatted1);
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
                this.SaveList.SaveList.Add(new(_Name, _SrcPath, _TargetPath, _Type, _Extensions));
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