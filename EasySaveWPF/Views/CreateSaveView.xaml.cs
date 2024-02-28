using System.Windows;
using EasySaveWPF.ViewModels;

namespace EasySaveWPF.Views;

/// <summary>
/// Logique d'interaction pour CreateSaveView.xaml
/// </summary>
public partial class CreateSaveView : Window
{
    public CreateSaveView(CreateBackupJobViewModel createBackupJobViewModel)
    {
        this.DataContext = createBackupJobViewModel;

        InitializeComponent();
    }
    
    private void SaveQuitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
