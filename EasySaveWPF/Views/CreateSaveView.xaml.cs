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

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private string type;
    private void SaveQuitButton_Click(object sender, RoutedEventArgs e)
    {
        // Logic to save the backup and quit

        /*if ((bool)rbComplete.IsChecked)
        {
            type = "Complete";
        }
        else
        {
            type = "Differential";
        }*/

        // Save the backup
        //SaveProcess.CreateSaveFunc(txtName.Text, txtSourcePath.Text, txtDestinationPath.Text, type, txtExtCrypt.Text);

        Close();
    }

    private void SaveCreateButton_Click(object sender, RoutedEventArgs e)
    {
        //Close();
    }

    private void BackupType_Checked(object sender, RoutedEventArgs e)
    {
        //var radioButton = sender as RadioButton;
        //radioButton.IsChecked = true;
    }

}
