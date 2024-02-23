using EasySaveWPF.Models;
using EasySaveWPF.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic; // Ensure this is included for List<>
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace EasySaveWPF.Views
{
    public partial class LogView : Page
    {
        private ObservableCollection<string> _logs = new ObservableCollection<string>();
        private FileSystemWatcher _watcher = new FileSystemWatcher();
        private string _logType;

        private static readonly string LogDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PS-Logs");

        public LogView(string logType)
        {
            InitializeComponent();
            _logType = logType;
            LogsListBox.ItemsSource = _logs;

            string fileName = logType == "xml" ? "Stats.xml" : "Stats.json";
            _watcher.Path = LogDirPath;
            _watcher.Filter = fileName;
            _watcher.Changed += OnLogFileChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnLogFileChanged(object sender, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(async () => // Use async here to allow await inside
            {
                try
                {
                    await Task.Delay(500); // Simple retry mechanism
                    if (_logType == "xml")
                    {
                        await DeserializeXmlLog(e.FullPath);
                    }
                    else if (_logType == "json")
                    {
                        await DeserializeJsonLog(e.FullPath);
                    }
                }
                catch (IOException ex)
                {
                    _logs.Add($"Error accessing log file: {ex.Message}");
                }
                catch (Exception ex) // Catch other exceptions, such as deserialization issues
                {
                    _logs.Add($"Error processing log file: {ex.Message}");
                }
            });
        }

        private async Task DeserializeXmlLog(string filePath)
        {
            await Task.Run(() => // Perform IO-bound operation on a background thread
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var serializer = new XmlSerializer(typeof(List<StatsRTModel>));
                    var logEntries = (List<StatsRTModel>)serializer.Deserialize(stream);
                    UpdateUI(logEntries);
                }
            });
        }
    
        private async Task DeserializeJsonLog(string filePath)
        {
            string jsonContent = await File.ReadAllTextAsync(filePath); // Async file read
            var logEntries = JsonConvert.DeserializeObject<List<StatsRTModel>>(jsonContent);
            UpdateUI(logEntries);
        }

        private void UpdateUI(List<StatsRTModel> logEntries)
        {
            _logs.Clear();
            foreach (var entry in logEntries)
            {
                _logs.Add($"{entry.SaveName} - Progress: {entry.Progress}%");
            }
            // Ensure there are entries before attempting to access them
            Progress.Value = logEntries?.LastOrDefault()?.Progress ?? 0;
        }
    }
}
