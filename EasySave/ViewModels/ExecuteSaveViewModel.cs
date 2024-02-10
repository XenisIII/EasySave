using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave.Services;
using EasySave.Models;
using EasySave.ViewModels;
using System.Diagnostics;

namespace EasySave.ViewModels
{
    public class SaveProcess
    {
        private LogStatsRTViewModel _LogStatsRTViewModel;
        private SavesModel _SavesList = new SavesModel();
        private LogModel _logModel = new LogModel();
        public SavesModel SavesList
        {
            get => _SavesList;
        }
        public LogModel logModel
        {
            get => _logModel;
        }
        public SaveProcess(LogStatsRTViewModel logStatsRTViewModel)
        {
            this._LogStatsRTViewModel = logStatsRTViewModel;
        }
        public void ExecuteSaveProcess(List<int> WhichSaveToExecute)
        {
            //            List<int> WhichSaveToExecute = new List<int>();
            //            WhichSaveToExecute.Add(Input);
            //            string[] WhichSaveToExecute = Input.Split(';');
            //Attendre le réafficheage de la console
            Console.Clear();
            foreach (int save in WhichSaveToExecute)
            {
                //Laisser le temps d'écrire les logs.Fait parti des "Temp Utiles" ou une appli console. A remplacer par await par la suite
                Thread.Sleep(100);
                Stopwatch stopwatch = new Stopwatch();
                //CreateSave SaveToExecute = _SavesList.SavesList[int.Parse(save)];
                CreateSave SaveToExecute = _SavesList.SavesList[save];
                switch (SaveToExecute.type)
                {
                    case "Complete":
                        CompleteSave save1 = new CompleteSave(SaveToExecute);
                        this._LogStatsRTViewModel.NewWork(save1.statsRTModel);
                        stopwatch.Start();
                        save1.Execute(SaveToExecute);
                        stopwatch.Stop();
                        TimeSpan timeElapsed = stopwatch.Elapsed;

                        // Pour obtenir le temps en formats spécifiques :
                        string elapsedTimeFormatted = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds,
                            timeElapsed.Milliseconds / 10);
                        setLogModel(SaveToExecute.name, SaveToExecute.SourcePath, SaveToExecute.TargetPath, save1.statsRTModel.TotalFilesSize, elapsedTimeFormatted);
                        break;
                    case "Differential":
                        DifferentialSave save2 = new DifferentialSave(SaveToExecute);
                        this._LogStatsRTViewModel.NewWork(save2.statsRTModel);
                        stopwatch.Start();
                        save2.Execute(SaveToExecute);
                        stopwatch.Stop();
                        TimeSpan timeElapsed1 = stopwatch.Elapsed;

                        // Pour obtenir le temps en formats spécifiques :
                        string elapsedTimeFormatted1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            timeElapsed1.Hours, timeElapsed1.Minutes, timeElapsed1.Seconds,
                            timeElapsed1.Milliseconds / 10);
                        setLogModel(SaveToExecute.name, SaveToExecute.SourcePath, SaveToExecute.TargetPath, save2.statsRTModel.TotalFilesSize, elapsedTimeFormatted1);
                        break;
                }
            }
        }
        public void setLogModel(string Name, string SourcePath, string TargetPath, long filesSize, string fileTransfertTime)
        {
            _logModel.Name = Name;
            _logModel.SourcePath = SourcePath;
            _logModel.TargetPath = TargetPath;
            _logModel.FilesSize = filesSize;
            _logModel.FileTransfertTime = fileTransfertTime;
            _logModel.Time = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            this._LogStatsRTViewModel.WriteLog(_logModel);
        }
        public void CreateSaveFunc(string Name, string SourcePath, string TargetPath, string Type)
        {
            _SavesList.SavesList.Add(new CreateSave(Name, SourcePath, TargetPath, Type));
        }
        public void DeleteSaveFunc(List<int> WhichSaveToDelete)
        {
            // Il est important de supprimer les éléments de la fin vers le début pour éviter de décaler les indices pendant que nous supprimons des éléments.
            WhichSaveToDelete.Sort();
            WhichSaveToDelete.Reverse();

            foreach (int index in WhichSaveToDelete)
            {
                if (index >= 0 && index < _SavesList.SavesList.Count)
                {
                    _SavesList.SavesList.RemoveAt(index);
                }
                else
                {
                    // Gérer l'erreur ou ignorer l'indice invalide.
                }
            }
        }
    }
}