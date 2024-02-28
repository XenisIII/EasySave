using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using EasySaveWPF.Models;

namespace EasySaveWPF.Services;

public class WriteStatsRT
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);
    public List<StatsRTModel> StatsList { get; set; } = new();

    public void WriteLogsSync(LogVarModel logModel, string logDirPath, string format)
    {
        CreateDirectoryIfNotExists(logDirPath);
        var dateNow = $"{DateTime.Now:yyyyMMdd}";
        var filePath = Path.Combine(logDirPath, $"{dateNow}_Log.txt");

        string serializedData;
        if (format.ToLower() == "xml")
        {
            var serializer = new XmlSerializer(typeof(LogVarModel));
            using var writer = new StringWriter();
            serializer.Serialize(writer, logModel);
            serializedData = writer.ToString();
        }
        else
        {
            serializedData = JsonSerializer.Serialize(logModel, new JsonSerializerOptions { WriteIndented = true });
        }

        AppendToFile(filePath, serializedData);
    }

    public async Task WriteRealTimeStatsAsync(StatsRTModel stats, string statsDirectory, string format)
    {
        CreateDirectoryIfNotExists(statsDirectory);
        var dateNow = $"{DateTime.Now:yyyyMMdd}";
        var filePath = Path.Combine(statsDirectory, $"{dateNow}_Stats.txt");

        await semaphore.WaitAsync();
        try
        {
            string serializedData;
            if (format.ToLower() == "xml")
            {
                var serializer = new XmlSerializer(typeof(StatsRTModel));
                using var writer = new StringWriter();
                serializer.Serialize(writer, stats);
                serializedData = writer.ToString();
            }
            else
            {
                serializedData = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
            }

            await AppendToFileAsync(filePath, serializedData);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private void AppendToFile(string filePath, string content)
    {
        using var streamWriter = File.AppendText(filePath);
        streamWriter.WriteLine(content);
    }

    private Task AppendToFileAsync(string filePath, string content)
    {
        return Task.Run(() => AppendToFile(filePath, content));
    }

    private static void CreateDirectoryIfNotExists(string logDirPath)
    {
        if (!Directory.Exists(logDirPath)) Directory.CreateDirectory(logDirPath);
    }
}