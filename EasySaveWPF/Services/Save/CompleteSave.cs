using System.IO; // Required for File operations
using System.Windows;
using EasySaveWPF.Models;

namespace EasySaveWPF.Services.Save;

/// <summary>
/// Implements a complete save operation by copying all files from source to target directory.
/// </summary>
public class CompleteSave : CommonSaveCommand
{
    /// <summary>
    /// Initializes a new complete save operation based on the provided save configuration.
    /// </summary>
    /// <param name="save">The save configuration.</param>
    public CompleteSave(BackupJobModel save)
    {
        Init(save);
    }

    /// <summary>
    /// Executes the complete save operation, copying all files and updating real-time statistics.
    /// </summary>
    /// <param name="save">The save configuration.</param>
    public void Execute(BackupJobModel save, string process)
    {
        // Prepares the target directory tree to mirror the source structure.
        SetTree(save.SourcePath, save.TargetPath);

        // Copies each file from the source to the target, updating stats for each file.
        foreach (string element in SourcePathAllFiles)
        {
            if(save.PauseResume)
            {
                while (save.PauseResume)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        save.Status = "Paused";
                    });
                    Thread.Sleep(1000);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    save.Status = LocalizationService.GetString("SaveInProgress");
                });
            }
            
            if (save.Stop)
            {
                save.Stop = false;
                return;
            }
            if (process != null)
            {
                CheckProcess(process);
            }
            if (save.Extensions != null && save.Extensions != "")
            {
                // Simulate stats update delay (replace with async/await in the future).
                //Thread.Sleep(10);
                SetInfosInStatsRTModel(save, element.Replace(save.SourcePath, ""));
                string fileExtension = Path.GetExtension(element);
                string[] allowedExtensions = save.Extensions.Split(';');
                if (allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) || save.Extensions == ".*")
                {
                    string target = element.Replace(save.SourcePath, save.TargetPath);
                    string filename = Path.GetFileName(target);
                    string encryptedFilename = $".encrypted.{filename}";
                    string targetDirectory = target.Substring(0, target.Length - filename.Length);
                    target = Path.Combine(targetDirectory, encryptedFilename);
                    CipherOrDecipher(element, target);
                }
                else
                {
                    File.Copy(element, element.Replace(save.SourcePath, save.TargetPath), true);
                }
            }
            else
            {
                SetInfosInStatsRTModel(save, element.Replace(save.SourcePath, ""));
                File.Copy(element, element.Replace(save.SourcePath, save.TargetPath), true);
            }

            //Thread.Sleep(10);
            UpdateFinishedFileSave();
        }
    }
}
