using System.Diagnostics;
using EasySave.Models;
using EasySave.Services;

namespace EasySave.ViewModels;

/// <summary>
/// Manages execution of backup processes and logging.
/// </summary>
public class SaveProcess(LogStatsRTViewModel logStatsRTViewModel)
{
    // Holds all configured save tasks.
    public SavesModel SaveList { get; } = new();

    // Represents the current log for ongoing save task.
    public LogVarModel CurrentLogModel { get; set; }

    /// <summary>
    /// Executes the save process for each selected save task.
    /// </summary>
    /// <param name="whichSaveToExecute">Enumerable of save task indices to execute.</param>
    public void ExecuteSaveProcess(IEnumerable<int> whichSaveToExecute)
  {

    Console.Clear();

    foreach (var saveIndex in whichSaveToExecute)
    {
      //Must be replace in oother version
      Thread.Sleep(100);
      var stopwatch = new Stopwatch();
      var save = this.SaveList.SaveList[saveIndex];
      switch (save.Type)
      {
        case "Complete":
          var save1 = new CompleteSave(save);
          logStatsRTViewModel.NewWork(save1.StatsRTModel);
          stopwatch.Start();
          save1.Execute(save);
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
          save2.Execute(save);
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
    public void CreateSaveFunc(string name, string sourcePath, string targetPath, string type) =>
    this.SaveList.SaveList.Add(new(name, sourcePath, targetPath, type));

    /// <summary>
    /// Deletes specified save tasks based on their indices.
    /// </summary>
    public void DeleteSaveFunc(List<int> whichSaveToDelete)
  {
    whichSaveToDelete.Sort();
    whichSaveToDelete.Reverse();

    foreach (var index in whichSaveToDelete.Where(index => index >= 0 && index < this.SaveList.SaveList.Count))
      this.SaveList.SaveList.RemoveAt(index);
  }
}