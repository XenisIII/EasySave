using System.Text.Json;

namespace EasySave.Services
{
  public class LogEntry
  {
    public string SaveName { get; set; } = string.Empty;
    public string SourceFilePath { get; set; } = string.Empty;
    public string TargetFilePath { get; set; } = string.Empty;
    public long TotalFileSize { get; set; } = 0;
    public double FileTransferTime { get; set; } = 0;
    public string DateTimeStamp { get; set; } = string.Empty;
  }

  public static class WriteLog
  {
    // Method to write a single log entry to a JSON file.
    public static async Task WriteLogEntryAsync(LogEntry logEntry, string logDirectory)
    {
      try
      {
        // Ensure the log directory exists
        Directory.CreateDirectory(logDirectory);

        // Generate a log file name based on the current date
        string logFileName = $"log_{DateTime.Now:yyyyMMdd}.json";
        string logFilePath = Path.Combine(logDirectory, logFileName);

        string jsonLogEntry = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });

        // Asynchronously append the log entry to the log file, creating the file if it doesn't exist.
        await File.AppendAllTextAsync(logFilePath, jsonLogEntry + Environment.NewLine);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred while writing the log: {ex.Message}");
        // Handle exceptions (e.g., log to a console or another log file)
      }
    }
  }
}