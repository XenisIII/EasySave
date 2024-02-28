using EasySaveWPF.Models;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace EasySaveWPF.Services.Save;

/// <summary>
/// Handles backup operations and updates for real-time statistics.
/// </summary>
public abstract class CommonSaveCommand
{
    // Total size of files to be copied.
    private long Sizes;
    private int _exitCode = 0;
    public int ExitCode
    {
        get => _exitCode;
        set => _exitCode = value;
    }
    private int _encryptionErrors = 0;
    public int EncryptionErrors
    {
        get => _encryptionErrors;
        set => _encryptionErrors = value;
    }

    // Represents the current backup operation.
    private StatsRTModel _statsRTModel;

    /// <summary>
    /// Tracks all files from the source directory.
    /// </summary>
    protected List<string> SourcePathAllFiles { get; private set; }

    /// <summary>
    /// Exposes backup model details.
    /// </summary>
    public StatsRTModel StatsRTModel => _statsRTModel;

    /// <summary>
    /// Initializes backup preparation.
    /// </summary>
    public void Init(BackupJobModel save)
    {
        SourcePathAllFiles = [];

        VerifyFilesToCopy(save.SourcePath);

        _statsRTModel = new StatsRTModel();
    }

    /// <summary>
    /// Sets information in the real-time stats model.
    /// </summary>
    public void SetInfosInStatsRTModel(BackupJobModel save, string fileName)
    {
        _statsRTModel.SaveName = save.Name;
        _statsRTModel.TotalFilesToCopy = SourcePathAllFiles.Count;
        _statsRTModel.SourceFilePath = GetPathFile(fileName);
        _statsRTModel.TargetFilePath = GetPathFile(fileName).Replace(save.SourcePath, save.TargetPath);
        _statsRTModel.State = "Activated";
        _statsRTModel.TotalFilesSize = Sizes;
        _statsRTModel.NbFilesLeftToDo = SourcePathAllFiles.Count - SourcePathAllFiles.IndexOf(GetPathFile(fileName));
        _statsRTModel.Progress = (int)((_statsRTModel.TotalFilesToCopy - _statsRTModel.NbFilesLeftToDo) / (double)_statsRTModel.TotalFilesToCopy * 100);
    }

    /// <summary>
    /// Marks the backup as finished.
    /// </summary>
    public void UpdateFinishedFileSave()
    {
        _statsRTModel.State = "Finished";
    }

    /// <summary>
    /// Validates the source path and prepares the file list.
    /// </summary>
    public int VerifyFilesToCopy(string path)
    {
        if (!Directory.Exists(path))
        {
            return 0;
        }

        return CountAndSetListPathFiles(new DirectoryInfo(path), SourcePathAllFiles);
    }

    /// <summary>
    /// Finds the full path for a given file name.
    /// </summary>
    public string? GetPathFile(string name)
        => SourcePathAllFiles.FirstOrDefault(path => path.EndsWith(name));

    public void CheckProcess(string name_process)
    {
        Process[] localByName = Process.GetProcessesByName(name_process);

        if (localByName.Length == 0)
        {
            return;
        }

        foreach (var process in localByName)
        {
            MessageBox.Show(LocalizationService.GetString("CSCProcessRunning1") + $"{name_process}" + LocalizationService.GetString("CSCProcessRunning2"), LocalizationService.GetString("CSCProcessRunningCapt"), MessageBoxButton.OK, MessageBoxImage.Information);
            process.WaitForExit();
        }

        MessageBox.Show(LocalizationService.GetString("CSCAllProcessClosed1") + $"{name_process}" + LocalizationService.GetString("CSCAllProcessClosed2"), LocalizationService.GetString("CSCAllProcessClosedCapt"), MessageBoxButton.OK, MessageBoxImage.Information);
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

    public void CipherOrDecipher(string src, string target)
    {
        //string applicationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CryptoSoft.exe");
        Process CipherProcess = new Process();
        //CipherProcess.StartInfo.FileName = applicationPath;
        CipherProcess.StartInfo.FileName = "CryptoSoft.exe";
        CipherProcess.StartInfo.Arguments = $"\"{src}\" \"{target}\"";
        CipherProcess.StartInfo.UseShellExecute = false;
        CipherProcess.StartInfo.CreateNoWindow = true;
        CipherProcess.Start();
        CipherProcess.WaitForExit();
        if (CipherProcess.ExitCode == -1)
        {
            _encryptionErrors += 1;
        }
        else
        {
            _exitCode += CipherProcess.ExitCode;
        }
    }

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
}
