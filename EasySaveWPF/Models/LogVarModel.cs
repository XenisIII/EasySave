namespace EasySaveWPF.Models;

/// <summary>
/// Represents a single log entry for a backup operation, capturing essential details about the operation.
/// </summary>
public class LogVarModel
{
    /// <summary>
    /// Name of the backup operation.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Path of the source directory from which files were backed up.
    /// </summary>
    public required string SourcePath { get; set; }

    /// <summary>
    /// Path of the target directory to which files were backed up.
    /// </summary>
    public required string TargetPath { get; set; }

    /// <summary>
    /// Total size of all files transferred during the backup, in bytes.
    /// </summary>
    public required long FilesSize { get; set; }

    /// <summary>
    /// Time taken to transfer files as a string. Could be in any time format (e.g., "2 minutes", "30 seconds").
    /// </summary>
    public required string FileTransferTime { get; set; }

    /// <summary>
    /// Timestamp of when the backup operation was completed.
    /// </summary>
    public required string Time { get; set; }
}
