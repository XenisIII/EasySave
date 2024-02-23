using System.Text.Json;
using System.Xml.Serialization;
using System.IO;
using EasySaveWPF.Models;
using System.Threading;

namespace EasySaveWPF.Services;

public class WriteStatsRT
{
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    public List<StatsRTModel> StatsList { get; set; } = new List<StatsRTModel>();

    public void WriteLogsSync(LogVarModel logModel, string logDirPath, string format)
    {
        CreateDirectoryIfNotExists(logDirPath);
        var dateNow = $"{DateTime.Now:yyyyMMdd}";
        string filePath = Path.Combine(logDirPath, $"{dateNow}_Log.txt");

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

        AppendToFileAsync(filePath, serializedData).Wait();
    }

    public async Task WriteRealTimeStatsAsync(StatsRTModel stats, string statsDirectory, string format)
    {
        CreateDirectoryIfNotExists(statsDirectory);
        var dateNow = $"{DateTime.Now:yyyyMMdd}";
        string filePath = Path.Combine(statsDirectory, $"{dateNow}_Stats.txt");

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

    /*private void AppendToFile(string filePath, string content)
    {
        using var streamWriter = File.AppendText(filePath);
        streamWriter.WriteLine(content);
    }*/

    private async Task AppendToFileAsync(string filePath, string content)
    {
        await semaphore.WaitAsync();
        try
        {
            using (var streamWriter = File.AppendText(filePath))
            {
                await streamWriter.WriteLineAsync(content);
            }
        }
        finally
        {
            semaphore.Release();
        }
    }

    /*private async Task _AppendToFileAsync(string filePath, string content)
    {
        await semaphore.WaitAsync();
        try
        {
            using var streamWriter = File.AppendText(filePath);
            streamWriter.WriteLine(content);
        }
        finally
        {
            semaphore.Release();
        }
    }*/


/*private Task AppendToFileAsync(string filePath, string content)
{
    return Task.Run(() => AppendToFile(filePath, content));
}*/

/*private static void WriteToXmlFile<T>(string filePath, T data)
{
    using var stream = File.Create(filePath);

    var serializer = new XmlSerializer(typeof(T));

    serializer.Serialize(stream, data);
}*/

/*private Task WriteToXmlFileAsync<T>(string filePath, T data)
    => Task.Run(() => WriteToXmlFile(filePath, data));

private static T? ReadFromXmlFile<T>(string filePath)
{
    if (!File.Exists(filePath)) return default(T);

    using var stream = File.OpenRead(filePath);

    var serializer = new XmlSerializer(typeof(T));

    return (T?)serializer.Deserialize(stream);
}*/

/*private static void WriteToJsonFile<T>(string filePath, T data)
{
    string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

    File.WriteAllText(filePath, json);
}*/

/*private static async Task WriteToJsonFileAsync<T>(string filePath, T data)
{
    await using var stream = File.Create(filePath);

    await JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions { WriteIndented = true });
}*/

/*private static T? ReadFromJsonFile<T>(string filePath)
{
    if (!File.Exists(filePath)) return default(T);

    string json = File.ReadAllText(filePath);

    return JsonSerializer.Deserialize<T>(json);
}*/

private static void CreateDirectoryIfNotExists(string logDirPath)
{
    if (!Directory.Exists(logDirPath))
    {
        Directory.CreateDirectory(logDirPath);
    }
}
}
