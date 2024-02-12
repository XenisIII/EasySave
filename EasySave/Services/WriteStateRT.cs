using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using EasySave.Models;

namespace EasySave.Services;

public static class WriteStatsRT
{
    /// <summary>
    /// Writes log entries synchronously to a JSON file.
    /// </summary>
    /// <param name="logModel">The log entry to write.</param>
    /// <param name="logDirPath">The directory path to store the log file.</param>
    public static void WriteLogsSync(LogModel logModel, string logDirPath)
    {
        // Ensure the directory exists
        // JB: Que se passe-t-il si le dossier existe déjà?
        Directory.CreateDirectory(logDirPath);

        // Determine the log file path
        var logFilePath = Path.Combine(logDirPath, $"Log_{DateTime.Now:yyyyMMdd}.json");
        List<LogModel> list;

        // Deserialize existing log entries if the file exists
        if (File.Exists(logFilePath))
        {
            using var stream = File.OpenRead(logFilePath);
            list = JsonSerializer.Deserialize<List<LogModel>>(stream) ?? new List<LogModel>();
        }
        else
        {
            list = new List<LogModel>();
        }

        // Add new log entry and write to file
        list.Add(logModel);
        // JB: Ici on a potentiellement un problème, si on veut executer une méthode asynchrone de manière synchrone
        // on peut faire WriteToJsonFileAsync(list, logFilePath).Wait();
        WriteToJsonFileAsync(list, logFilePath); // This method should ideally be asynchronous
    }

    /// <summary>
    /// Writes real-time backup stats to a JSON file asynchronously.
    /// </summary>
    /// <param name="backup">The backup model to log.</param>
    /// <param name="statsDirectory">The directory path to store the stats file.</param>
    public static async Task WriteRealTimeStatsAsync(BackupModel backup, string statsDirectory)
    {
        // Ensure the directory exists
        Directory.CreateDirectory(statsDirectory);

        // Determine the stats file path
        var statsFileName = $"stats_{DateTime.Now:yyyyMMdd}.json";
        var statsFilePath = Path.Combine(statsDirectory, statsFileName);

        List<BackupModel> statsList;

        // Deserialize existing stats if the file exists
        if (File.Exists(statsFilePath))
        {
            using var stream = File.OpenRead(statsFilePath);
            statsList = JsonSerializer.Deserialize<List<BackupModel>>(stream) ?? new List<BackupModel>();
        }
        else
        {
            statsList = new List<BackupModel>();
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
