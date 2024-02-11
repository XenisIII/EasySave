using EasySave.Services;
using System;
using System.Diagnostics; // Required for Process
using System.IO; // Required for Path
using System.Threading.Tasks;

namespace EasySave.Views
{
    /// <summary>
    /// Provides functionality to display logs and open the directory containing log files.
    /// </summary>
    public class LogView
    {
        private static readonly string LogDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PS-Logs");

        public async Task Display()
        {
            ConsoleHeader.Display();
            OpenLogDirectory();
        }

        /// <summary>
        /// Opens the log directory in the file explorer.
        /// </summary>
        private void OpenLogDirectory()
        {
            // Ensure the directory exists before trying to open it
            if (!Directory.Exists(LogDirPath))
            {
                Console.WriteLine("The log directory does not exist. Creating directory...");
                Directory.CreateDirectory(LogDirPath);
            }

            try
            {
                Console.WriteLine($"Opening log directory: {LogDirPath}");
                Process.Start("explorer.exe", LogDirPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to open the log directory: {ex.Message}");
            }
        }
    }
}
