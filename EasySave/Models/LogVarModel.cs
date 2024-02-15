namespace EasySave.Models;

/// <summary>
/// Represents a single log entry for a backup operation, capturing essential details about the operation.
/// </summary>
public class LogVarModel
{
    // Name of the backup operation.
    public required string Name { get; set; }

    // Path of the source directory from which files were backed up.
    public required string SourcePath { get; set; }

    // Path of the target directory to which files were backed up.
    public required string TargetPath { get; set; }

    // Total size of all files transferred during the backup, in bytes.
    public required long FilesSize { get; set; }

    // Time taken to transfer files as a string. Could be in any time format (e.g., "2 minutes", "30 seconds").
    public required string FileTransferTime { get; set; }

    // Timestamp of when the backup operation was completed.
    public required string Time { get; set; }
}
