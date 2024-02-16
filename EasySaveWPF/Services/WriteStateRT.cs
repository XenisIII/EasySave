using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using EasySaveWPF.Models;

namespace EasySaveWPF.Services;

public static class WriteStatsRT
{
    /// <summary>
    /// Writes log entries synchronously to a JSON file.
    /// </summary>
    /// <param name="logModel">The log entry to write.</param>
    /// <param name="logDirPath">The directory path to store the log file.</param>
    public static void WriteLogsSync(LogVarModel logModel, string logDirPath)
    {
        // Ensure the directory exists (if not, it will be created. If yes, it will continue)
        Directory.CreateDirectory(logDirPath);

        // Determine the log file path
        var logFilePath = Path.Combine(logDirPath, $"Log_{DateTime.Now:yyyyMMdd}.json");
        List<LogVarModel> list;

        // Deserialize existing log entries if the file exists
        if (File.Exists(logFilePath))
        {
            using var stream = File.OpenRead(logFilePath);
            list = JsonSerializer.Deserialize<List<LogVarModel>>(stream) ?? new List<LogVarModel>();
        }
        else
        {
            list = new List<LogVarModel>();
        }

        // Add new log entry and write to file
        list.Add(logModel);
        WriteToJsonFileAsync(list, logFilePath); // This method should ideally be asynchronous
    }

    /// <summary>
    /// Writes real-time backup stats to a JSON file asynchronously.
    /// </summary>
    /// <param name="backup">The backup model to log.</param>
    /// <param name="statsDirectory">The directory path to store the stats file.</param>
    public static async Task WriteRealTimeStatsAsync(StatsRTModel backup, string statsDirectory)
    {
        // Ensure the directory exists
        Directory.CreateDirectory(statsDirectory);

        // Determine the stats file path
        var statsFileName = $"stats_{DateTime.Now:yyyyMMdd}.json";
        var statsFilePath = Path.Combine(statsDirectory, statsFileName);

        List<StatsRTModel> statsList;

        // Deserialize existing stats if the file exists
        if (File.Exists(statsFilePath))
        {
            using var stream = File.OpenRead(statsFilePath);
            statsList = JsonSerializer.Deserialize<List<StatsRTModel>>(stream) ?? new List<StatsRTModel>();
        }
        else
        {
            statsList = new List<StatsRTModel>();
        }

        // Add new stats and write to file
        statsList.Add(backup);
        await WriteToJsonFileAsync(statsList, statsFilePath);
    }

    /// <summary>
    /// Generic method to write a collection of items to a JSON file asynchronously.
    /// </summary>
    /// <param name="items">The collection of items to write.</param>
    /// <param name="filePath">The file path to write the items to.</param>
    public static async Task WriteToJsonFileAsync<T>(IEnumerable<T> items, string filePath)
    {
        // Overwrite the file with the updated collection
        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, items, new JsonSerializerOptions { WriteIndented = true });
    }
}
