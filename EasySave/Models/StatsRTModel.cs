using EasySave.Services.Common;

// Defines the namespace for the StatsRTModel, indicating it's part of the models related to real-time statistics.
namespace EasySave.Models;

/// <summary>
/// Represents the real-time statistics of an ongoing backup operation.
/// Inherits from ObservableObject to notify UI components of property changes.
/// </summary>
public class StatsRTModel : ObservableObject
{
    // Backing field for the State property.
    private string _state;

    // Name of the save operation.
    public string SaveName { get; set; }

    // The source directory path from which files are being backed up.
    public string SourceFilePath { get; set; }

    // The target directory path where files are being saved.
    public string TargetFilePath { get; set; }

    /// <summary>
    /// The current state of the backup operation (e.g., "In Progress", "Completed").
    /// Notifies observers when the value changes.
    /// </summary>
    public string State
    {
        get => this._state;
        set => this.SetProperty(ref this._state, value);
    }

    // The total number of files that need to be copied.
    public int TotalFilesToCopy { get; set; }

    // The total size of all files to be copied, in bytes.
    public long TotalFilesSize { get; set; }

    // The number of files left to copy to complete the backup operation.
    public int NbFilesLeftToDo { get; set; }

    // The progress of the backup operation, represented as a percentage.
    public int Progress { get; set; }
}
