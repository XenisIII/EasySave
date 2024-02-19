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
using System.Security.Cryptography;

namespace EasySaveWPF.Views;

/// <summary>
/// Logique d'interaction pour CreateSaveView.xaml
/// </summary>
public partial class CreateSaveView : Window, INotifyPropertyChanged
{
    private bool _CreateNewSave = false;
    public bool CreateNewSave
    {
        get => _CreateNewSave;
        set => _CreateNewSave = value;
    }
    public bool DialogResult { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public CreateSaveView(SaveProcess saveProcess)
    {
        this.DataContext = saveProcess;
        InitializeComponent();
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

        this.DialogResult = true;
        this.Close();
    }

    private void SaveCreateButton_Click(object sender, RoutedEventArgs e)
    {
        CreateNewSave = true;
        this.Close();
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


    private void BackupType_Checked(object sender, RoutedEventArgs e)
    {
        //var radioButton = sender as RadioButton;
        //radioButton.IsChecked = true;
    }

}
