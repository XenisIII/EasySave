using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace EasySave.Services
{
    public static class WriteStatsRT
    {
        public static async Task WriteRealTimeStatsAsync(StatsRTModel stats, string statsDirectory)
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
