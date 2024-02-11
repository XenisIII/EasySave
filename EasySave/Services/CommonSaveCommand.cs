using EasySave.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EasySave.Services;

/// <summary>
/// Handles backup operations and updates for real-time statistics.
/// </summary>
public class CommonSaveCommand
{
    // Tracks all files from the source directory.
    public List<string> SourcePathAllFiles;

    // Total size of files to be copied.
    private long Sizes;

    // Represents the current backup operation.
    private BackupModel _backupModel;

    // Exposes backup model details.
    public BackupModel BackupModel => this._backupModel;

    /// <summary>
    /// Initializes backup preparation.
    /// </summary>
    public void Init(CreateSave save)
    {
        this.SourcePathAllFiles = new List<string>();
        VerifyFilesToCopy(save.SourcePath);
        this._backupModel = new BackupModel();
    }

    /// <summary>
    /// Sets information in the real-time stats model.
    /// </summary>
    public void SetInfosInStatsRTModel(CreateSave save, string fileName)
    {
        _backupModel.SaveName = save.Name;
        _backupModel.TotalFilesToCopy = SourcePathAllFiles.Count;
        _backupModel.SourceFilePath = GetPathFile(fileName);
        _backupModel.TargetFilePath = GetPathFile(fileName).Replace(save.SourcePath, save.TargetPath);
        _backupModel.State = "Activated";
        _backupModel.TotalFilesSize = Sizes;
        _backupModel.NbFilesLeftToDo = SourcePathAllFiles.Count - SourcePathAllFiles.IndexOf(GetPathFile(fileName));
        _backupModel.Progress = (int)((_backupModel.TotalFilesToCopy - _backupModel.NbFilesLeftToDo) / (double)_backupModel.TotalFilesToCopy * 100);
    }

    /// <summary>
    /// Marks the backup as finished.
    /// </summary>
    public void UpdateFinishedFileSave()
    {
        _backupModel.State = "Finished";
    }

    /// <summary>
    /// Validates the source path and prepares the file list.
    /// </summary>
    public int VerifyFilesToCopy(string path)
    {
        if (!Directory.Exists(path))
        {
            Console.WriteLine("Specified path does not exist.");
            return 0;
        }

        return CountAndSetListPathFiles(new DirectoryInfo(path), SourcePathAllFiles);
    }

    /// <summary>
    /// Finds the full path for a given file name.
    /// </summary>
    public string? GetPathFile(string name) =>
        SourcePathAllFiles.FirstOrDefault(path => path.EndsWith(name));

    /// <summary>
    /// Recursively counts files and updates the file list and total size.
    /// </summary>
    private int CountAndSetListPathFiles(DirectoryInfo directory, List<string> sourcePathAllFiles)
    {
        var count = 0;
        foreach (var file in directory.GetFiles())
        {
            sourcePathAllFiles.Add(file.FullName);
            Sizes += file.Length;
            count++;
        }

        foreach (var subDirectory in directory.GetDirectories())
            count += CountAndSetListPathFiles(subDirectory, sourcePathAllFiles);

        return count;
    }

    /// <summary>
    /// Creates the directory structure in the target directory.
    /// </summary>
    public void SetTree(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        var directoryInfo = new DirectoryInfo(sourceDir);
        foreach (DirectoryInfo subDir in directoryInfo.GetDirectories())
        {
            var destSubDirPath = Path.Combine(destDir, subDir.Name);
            Directory.CreateDirectory(destSubDirPath);
            SetTree(subDir.FullName, destSubDirPath);
        }
    }
}
