using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;
using EasySaveRemoteClient.ViewModels;

namespace EasySaveRemoteClient;

public partial class MainWindow : Window
{
    public ClientViewModel clientViewModelMain { get; set; } = new ClientViewModel();
    public MainWindow()
    {
        this.DataContext = clientViewModelMain;
        InitializeComponent();
        
    }

    private void Execute_Click(object sender, RoutedEventArgs e)
    {
    }
}

