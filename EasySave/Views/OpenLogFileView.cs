using EasySave.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

/*namespace EasySave.Views
{
    public class LogView
    {
        public async Task Display()
        {
            LogEntry logEntry = new LogEntry
            {
                SaveName = "Save1",
                SourceFilePath = @"D:\projet_2\TEST\source1234\fichiertxt_1.txt",
                TargetFilePath = @"E:\SAVE\projet_2\TEST\source1234\fichiertxt_1.txt",
                TotalFileSize = 174592,
                FileTransferTime = 38.029,
                DateTimeStamp = "17/12/2020 17:06:49"
            };

            string logDirectory = @"C:\Program Files\EasySave\log";

            await WriteLog.WriteLogEntryAsync(logEntry, logDirectory);
        }
    }
}*/

namespace EasySave.Views
{
    public class LogView
    {
        public async Task Display()
        {
            ConsoleHeader.Display();
            RealTimeStats realTimeStats = new RealTimeStats
            {
                Name = "Save1",
                // Initialize other properties as needed
            };

            string statsDirectory = @"C:\Users\Arenz\Desktop\log"; // Replace with your actual stats directory path

            // Simulate a backup process
            for (int i = 0; i < 100; i++)
            {
                realTimeStats.Progression = i;
                realTimeStats.State = i < 99 ? "IN PROGRESS" : "END";
                realTimeStats.NbFilesLeftToDo = 100 - i;

                // Update the stats in real-time
                await WriteStatsRT.WriteRealTimeStatsAsync(realTimeStats, statsDirectory);

                // Wait for a while to simulate time passing
                Thread.Sleep(1000);
            }
        }
    }
}