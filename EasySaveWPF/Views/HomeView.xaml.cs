using System.Windows;
using System.Windows.Controls;
using System.IO;
using EasySaveWPF.ViewModels;
using System.Diagnostics;

namespace EasySaveWPF.Views;

public partial class HomeView : UserControl
{
    private static readonly string LogDirPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PS-Logs");

    readonly SaveProcessViewModel _saveProcessViewModel;

    public HomeView(SaveProcessViewModel saveProcess)
    {
        _saveProcessViewModel = saveProcess;

        DataContext = saveProcess;

        InitializeComponent();
    }

    public void OpenLogsDirectory(object sender, RoutedEventArgs e)
    {
        if (!Directory.Exists(LogDirPath))
        {
            Directory.CreateDirectory(LogDirPath);
        }

        // Open the logs directory in the file explorer
        Process.Start("explorer.exe", LogDirPath);
    }   

    public void AddNewBackup(object sender, RoutedEventArgs e)
    {
        CreateBackupJobViewModel createBackupJobViewModel = new(_saveProcessViewModel);

        CreateSaveView createSaveWindow = new(createBackupJobViewModel)
        {
            Owner = Window.GetWindow(this)
        };

        createSaveWindow.ShowDialog();
    }
}

