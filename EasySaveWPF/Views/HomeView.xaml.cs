using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using EasySaveWPF.Views;   

namespace EasySaveWPF.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        this.DataContext = this;
    }

    private static readonly string LogDirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PS-Logs");
    public void OpenLogsDirectory(object sender, RoutedEventArgs e)
    {
        // Open the logs directory in the file explorer
        System.Diagnostics.Process.Start("explorer.exe", LogDirPath);
    }   

    public void AddNewBackup(object sender, RoutedEventArgs e)
    {
        CreateSaveView createSaveWindow = new CreateSaveView();
        bool? result = createSaveWindow.ShowDialog();

        if (result == true)
        {
            // Handle the case where the user clicked "Save & Quit"
        }
        else
        {
            // Handle the case where the user clicked "Cancel"
        }
    }
}

