using System.Text.Json;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using EasySaveWPF.Models;

namespace EasySaveWPF.Services
{
    public static class WriteStatsRT
    {

        public static void WriteLogsSync(LogVarModel logModel, string logDirPath, string format)
        {
            Directory.CreateDirectory(logDirPath);

            string fileName = format.ToLower() == "xml" ? "Log.xml" : "Log.json";
            string filePath = Path.Combine(logDirPath, $"{DateTime.Now:yyyyMMdd}_{fileName}");

            if (format.ToLower() == "xml")
            {
                List<LogVarModel> logs = ReadFromXmlFile<List<LogVarModel>>(filePath) ?? new List<LogVarModel>();
                logs.Add(logModel);
                WriteToXmlFile(filePath, logs);
            }
            else
            {
                List<LogVarModel> logs = ReadFromJsonFile<List<LogVarModel>>(filePath) ?? new List<LogVarModel>();
                logs.Add(logModel);
                WriteToJsonFile(filePath, logs);
            }
        }

        public static async Task WriteRealTimeStatsAsync(StatsRTModel stats, string statsDirectory, string format)
        {
            Directory.CreateDirectory(statsDirectory);

            string fileName = format.ToLower() == "xml" ? "Stats.xml" : "Stats.json";
            string filePath = Path.Combine(statsDirectory, $"{DateTime.Now:yyyyMMdd}_{fileName}");

            if (format.ToLower() == "xml")
            {
                List<StatsRTModel> statsList = ReadFromXmlFile<List<StatsRTModel>>(filePath) ?? new List<StatsRTModel>();
                statsList.Add(stats);
                await WriteToXmlFileAsync(filePath, statsList);
            }
            else
            {
                List<StatsRTModel> statsList = ReadFromJsonFile<List<StatsRTModel>>(filePath) ?? new List<StatsRTModel>();
                statsList.Add(stats);
                await WriteToJsonFileAsync(filePath, statsList);
            }
        }

        private static void WriteToXmlFile<T>(string filePath, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.Create(filePath))
            {
                serializer.Serialize(stream, data);
            }
        }

        private static async Task WriteToXmlFileAsync<T>(string filePath, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            await using (var stream = File.Create(filePath))
            {
                serializer.Serialize(stream, data);
            }
        }

        private static T? ReadFromXmlFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default(T);

            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.OpenRead(filePath))
            {
                return (T?)serializer.Deserialize(stream);
            }
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
    }
}