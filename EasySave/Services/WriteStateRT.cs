using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace EasySave.Services
{
    public class RealTimeStats
    {
        public string Name { get; set; } = string.Empty;
        public string SourceFilePath { get; set; } = string.Empty;
        public string TargetFilePath { get; set; } = string.Empty;
        public string State { get; set; } = "START";
        public int TotalFilesToCopy { get; set; } = 0;
        public long TotalFilesSize { get; set; } = 0;
        public int NbFilesLeftToDo { get; set; } = 0;
        public double Progression { get; set; } = 0;
    }

    public static class WriteStatsRT
    {
        public static async Task WriteRealTimeStatsAsync(RealTimeStats stats, string statsDirectory)
        {
            // Ensure the stats directory exists
            Directory.CreateDirectory(statsDirectory);

            // Generate a stats file name based on the current date
            string statsFileName = $"stats_{DateTime.Now:yyyyMMdd}.json";
            string statsFilePath = Path.Combine(statsDirectory, statsFileName);

            // Serialize the stats to JSON
            string jsonStats = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });

            // Asynchronously write the stats to the file
            await File.WriteAllTextAsync(statsFilePath, jsonStats);

            // Display real-time stats on the console
            Console.Clear();
            Console.WriteLine(jsonStats);
        }
    }
}
