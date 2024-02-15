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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace EasySaveWPF.Views;

/// <summary>
/// Logique d'interaction pour CreateSaveView.xaml
/// </summary>
public partial class CreateSaveView : Window
{
    public bool DialogResult { get; private set; }

    public CreateSaveView()
    {
        InitializeComponent();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
    }

    private void SaveQuitButton_Click(object sender, RoutedEventArgs e)
    {
        // Logic to save the backup and quit
        // ...
        this.DialogResult = true;
    }

    private void SaveCreateButton_Click(object sender, RoutedEventArgs e)
    {
        // Logic to save the backup and reset fields for new input
        // ...
        txtName.Clear();
        txtSourcePath.Clear();
        txtDestinationPath.Clear();
        txtType.Clear();
    }
}
