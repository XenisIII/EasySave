using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Collections.Concurrent;


using EasySave.Models.StatsRTModelNameSpace;

namespace EasySave.Services
{
    public static class WriteStatsRT
    {
        private static readonly ConcurrentQueue<StatsRTModel> StatsQueue = new ConcurrentQueue<StatsRTModel>();
        private static readonly SemaphoreSlim WriteSemaphore = new SemaphoreSlim(1, 1);
        private static bool isWriting = false;

        // Méthode appelée pour ajouter des statistiques à la file d'attente
        public static void QueueStatsForWriting(StatsRTModel stats)
        {
            StatsQueue.Enqueue(stats);
            if (!isWriting)
            {
                isWriting = true;
                _ = WriteStatsFromQueueAsync(); // Lancez la tâche d'écriture sans attendre
            }
        }

        // Méthode d'arrière-plan pour l'écriture des statistiques
        private static async Task WriteStatsFromQueueAsync()
        {
            while (StatsQueue.TryDequeue(out var stats))
            {
                await WriteSemaphore.WaitAsync(); // Assurez-vous qu'une seule écriture a lieu à la fois
                try
                {
                    await WriteRealTimeStatsAsync(stats, "chemin vers votre répertoire de statistiques");
                }
                finally
                {
                    WriteSemaphore.Release(); // Permettre la prochaine écriture
                }
            }
            isWriting = false;
        }
        public static async Task WriteRealTimeStatsAsync(StatsRTModel stats, string statsDirectory)
        {
            // Ensure the stats directory exists
            Directory.CreateDirectory(statsDirectory);

            // Generate a stats file name based on the current date
            string statsFileName = $"stats_{DateTime.Now:yyyyMMdd}.json";
            string statsFilePath = Path.Combine(statsDirectory, statsFileName);

            List<StatsRTModel> statsList;

            // Check if the stats file already exists
            if (File.Exists(statsFilePath))
            {
                // Read the existing file and deserialize it into a list of stats
                string existingJson = await File.ReadAllTextAsync(statsFilePath);
                statsList = JsonSerializer.Deserialize<List<StatsRTModel>>(existingJson) ?? new List<StatsRTModel>();
            }
            else
            {
                // If the file doesn't exist, start with an empty list
                statsList = new List<StatsRTModel>();
            }

            // Add the new stats to the list
            statsList.Add(stats);

            // Serialize the updated list to JSON
            string updatedJson = JsonSerializer.Serialize(statsList, new JsonSerializerOptions { WriteIndented = true });

            // Asynchronously write the updated stats to the file
            await File.WriteAllTextAsync(statsFilePath, updatedJson);

            // Display real-time stats on the console
            Console.Clear();
            Console.WriteLine(updatedJson);
        }
    }
}
