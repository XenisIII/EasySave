using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System.IO;
using EasySaveWPF.ViewModels;

namespace EasySaveWPF.Views;

/// <summary>
/// Logique d'interaction pour CreateSaveView.xaml
/// </summary>
public partial class CreateSaveView : Window, INotifyPropertyChanged
{
    public bool DialogResult { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public CreateSaveView()
    {
        InitializeComponent();
        this.DataContext = this;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private string type;
    private void SaveQuitButton_Click(object sender, RoutedEventArgs e)
    {
        // Logic to save the backup and quit

        if (!ArePathsValid())
        {
            MessageBox.Show("Un ou plusieurs chemins spécifiés n'existent pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if ((bool)rbComplete.IsChecked)
        {
            type = "Complete";
        }
        else
        {
            type = "Differential";
        }

        // Save the backup
        //SaveProcess.CreateSaveFunc(txtName.Text, txtSourcePath.Text, txtDestinationPath.Text, type, txtExtCrypt.Text);

        this.DialogResult = true;
        this.Close();
    }

    private void SaveCreateButton_Click(object sender, RoutedEventArgs e)
    {
        // Logic to save the backup and reset fields for new input
        if (!ArePathsValid())
        {
            MessageBox.Show("Un ou plusieurs chemins spécifiés n'existent pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        txtName.Clear();
        txtSourcePath.Clear();
        txtDestinationPath.Clear();
        txtExtCrypt.Clear();
    }

    private void BrowsePath_Click(object sender, RoutedEventArgs e)
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog
        {
            IsFolderPicker = true // Ensures the dialog is set to select folders
        };

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            // Determine which button was clicked and set the appropriate text box
            if (sender == btnBrowseSourcePath) // Assuming btnBrowseSourcePath is your button for browsing source path
            {
                txtSourcePath.Text = dialog.FileName; // Set the source path text box
            }
            else if (sender == btnBrowseDestinationPath) // Assuming btnBrowseDestinationPath is your button for browsing destination path
            {
                txtDestinationPath.Text = dialog.FileName; // Set the destination path text box
            }
        }
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public bool CanSave
    {
        get
        {
            return !string.IsNullOrWhiteSpace(txtName.Text) &&
                   !string.IsNullOrWhiteSpace(txtSourcePath.Text) &&
                   !string.IsNullOrWhiteSpace(txtDestinationPath.Text);
        }
    }
    private void RequiredField_TextChanged(object sender, TextChangedEventArgs e)
    {
        NotifyPropertyChanged(nameof(CanSave));
    }

    private bool ArePathsValid()
    {
        return Directory.Exists(txtSourcePath.Text) && Directory.Exists(txtDestinationPath.Text);
    }

    private void BackupType_Checked(object sender, RoutedEventArgs e)
    {
        var radioButton = sender as RadioButton;
        if (radioButton != null)
        {
            // Vous pouvez utiliser la propriété Content du RadioButton sélectionné pour déterminer le type choisi
            // Exemple : Sauvegarder le choix dans une variable ou ajuster l'interface utilisateur en conséquence
            // string selectedBackupType = radioButton.Content.ToString();
        }
    }

}
