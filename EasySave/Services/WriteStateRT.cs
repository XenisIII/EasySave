using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Collections.Concurrent;
using EasySave.Models;


using EasySave.Models;

namespace EasySave.Services
{
    public static class WriteStatsRT
    {
        public static async Task WriteLogsAsync(LogModel logModel, string LogDirectory)
        {
            Directory.CreateDirectory(LogDirectory);
            string LogFileName = $"Log_{DateTime.Now:yyyyMMdd}.json";
            string LogFilePath = Path.Combine(LogDirectory, LogFileName);
            List<LogModel> LogList;
            if (File.Exists(LogFilePath))
            {
                string existingJson = await File.ReadAllTextAsync(LogFilePath);
                LogList = JsonSerializer.Deserialize<List<LogModel>>(existingJson) ?? new List<LogModel>();
            }
            else
            {
                LogList = new List<LogModel>();
            }
            LogList.Add(logModel);
            await WriteToJsonFileAsync(LogList, LogFilePath);
        }

        public static async Task WriteRealTimeStatsAsync(StatsRTModel stats, string statsDirectory)
        {
            Directory.CreateDirectory(statsDirectory);

            // G�n�rez un nom de fichier de statistiques bas� sur la date actuelle
            string statsFileName = $"stats_{DateTime.Now:yyyyMMdd}.json";
            string statsFilePath = Path.Combine(statsDirectory, statsFileName);

            List<StatsRTModel> statsList;

            // V�rifiez si le fichier de statistiques existe d�j�
            if (File.Exists(statsFilePath))
            {
                // Lisez le fichier existant et d�s�rialisez-le en une liste de statistiques
                string existingJson = await File.ReadAllTextAsync(statsFilePath);
                statsList = JsonSerializer.Deserialize<List<StatsRTModel>>(existingJson) ?? new List<StatsRTModel>();
            }
            else
            {
                // Si le fichier n'existe pas, commencez par une liste vide
                statsList = new List<StatsRTModel>();
            }

            // Ajoutez les nouvelles statistiques � la liste
            statsList.Add(stats);

            // Utilisez la fonction g�n�rique pour �crire dans le fichier JSON
            await WriteToJsonFileAsync(statsList, statsFilePath);
        }
        public static async Task WriteToJsonFileAsync<T>(List<T> items, string filePath)
        {
            // S�rialisez la liste d'objets en JSON
            string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });

            // �crivez de mani�re asynchrone le JSON dans le fichier
            await File.WriteAllTextAsync(filePath, json);
        }

    }
}
