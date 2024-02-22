using EasySaveWPF.Common;


// Defines the namespace for the StatsRTModel, indicating it's part of the models related to real-time statistics.
namespace EasySaveWPF.Models;

/// <summary>
/// Represents the real-time statistics of an ongoing backup operation.
/// Inherits from ObservableObject to notify UI components of property changes.
/// </summary>
public class StatsRTModel : ObservableObject
{
    /// <summary>
    /// Backing field for the State property.
    /// </summary>
    private string _state;

    /// <summary>
    /// Name of the save operation.
    /// </summary>
    public string SaveName { get; set; }

    /// <summary>
    /// The source directory path from which files are being backed up.
    /// </summary>
    public string SourceFilePath { get; set; }

    /// <summary>
    /// The target directory path where files are being saved.
    /// </summary>
    public string TargetFilePath { get; set; }

    /// <summary>
    /// The current state of the backup operation (e.g., "In Progress", "Completed").
    /// Notifies observers when the value changes.
    /// </summary>
    public string State
    {
        get => _state;
        set
        {
            if (_state == value) return;

            _state = value;
            OnPropertyChanged(nameof(State));
        }
    }

    /// <summary>
    /// The total number of files that need to be copied.
    /// </summary>
    public int TotalFilesToCopy { get; set; }

    /// <summary>
    /// The total size of all files to be copied, in bytes.
    /// </summary>
    public long TotalFilesSize { get; set; }

    /// <summary>
    /// The number of files left to copy to complete the backup operation.
    /// </summary>
    public int NbFilesLeftToDo { get; set; }

    /// <summary>
    /// The progress of the backup operation, represented as a percentage.
    /// </summary>
    public int Progress { get; set; }
}
