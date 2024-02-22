using System.Text.Json;
using System.Xml.Serialization;
using System.IO;
using EasySaveWPF.Models;

namespace EasySaveWPF.Services;

public class WriteStatsRT
{
    public List<StatsRTModel> StatsList { get; set; } = new List<StatsRTModel>();

    public void WriteLogsSync(LogVarModel logModel, string logDirPath, string format)
    {
        CreateDirectoryIfNotExists(logDirPath);

        var dateNow = $"{DateTime.Now:yyyyMMdd}";

        if (format.ToLower() == "xml")
        {
            string filePath = Path.Combine(logDirPath, $"{dateNow}_Log.xml");

            List<LogVarModel> logs = ReadFromXmlFile<List<LogVarModel>>(filePath) ?? [];

            logs.Add(logModel);

            WriteToXmlFile(filePath, logs);
        }
        else
        {
            string filePath = Path.Combine(logDirPath, $"{dateNow}_Log.json");

            List<LogVarModel> logs = ReadFromJsonFile<List<LogVarModel>>(filePath) ?? [];

            logs.Add(logModel);

            WriteToJsonFile(filePath, logs);
        }
    }

    public async Task WriteRealTimeStatsAsync(StatsRTModel stats, string statsDirectory, string format)
    {
        CreateDirectoryIfNotExists(statsDirectory);

        var dateNow = $"{DateTime.Now:yyyyMMdd}";
        StatsRTModel data = new(stats);

        if (format.ToLower() == "xml")
        {
            //List<StatsRTModel> statsList = ReadFromXmlFile<List<StatsRTModel>>(filePath) ?? new List<StatsRTModel>();
            StatsList.Add(data);

            string filePath = Path.Combine(statsDirectory, $"{dateNow}_Stats.xml");

            await WriteToXmlFileAsync(filePath, StatsList);
        }
        else
        {
            //List<StatsRTModel> statsList = ReadFromJsonFile<List<StatsRTModel>>(filePath) ?? new List<StatsRTModel>();
            StatsList.Add(data);

            string filePath = Path.Combine(statsDirectory, $"{dateNow}_Stats.json");

            await WriteToJsonFileAsync(filePath, StatsList);
        }
    }

    private static void WriteToXmlFile<T>(string filePath, T data)
    {
        using var stream = File.Create(filePath);

        var serializer = new XmlSerializer(typeof(T));

        serializer.Serialize(stream, data);
    }

    private Task WriteToXmlFileAsync<T>(string filePath, T data)
        => Task.Run(() => WriteToXmlFile(filePath, data));

    private static T? ReadFromXmlFile<T>(string filePath)
    {
        if (!File.Exists(filePath)) return default(T);

        using var stream = File.OpenRead(filePath);

        var serializer = new XmlSerializer(typeof(T));

        return (T?)serializer.Deserialize(stream);
    }

    private static void WriteToJsonFile<T>(string filePath, T data)
    {
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filePath, json);
    }

    private static async Task WriteToJsonFileAsync<T>(string filePath, T data)
    {
        await using var stream = File.Create(filePath);

        await JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions { WriteIndented = true });
    }

    private static T? ReadFromJsonFile<T>(string filePath)
    {
        if (!File.Exists(filePath)) return default(T);

        string json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<T>(json);
    }

    private static void CreateDirectoryIfNotExists(string logDirPath)
    {
        if (!Directory.Exists(logDirPath))
        {
            Directory.CreateDirectory(logDirPath);
        }
    }
}