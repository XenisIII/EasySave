using System.Windows.Controls;
using System.Windows;
using EasySaveWPF.Services;
using System.ComponentModel;
using System.Windows.Input;
using EasySaveWPF.ViewModels; // Required for INotifyPropertyChanged

namespace EasySaveWPF.Views;

public partial class SettingsView : UserControl, INotifyPropertyChanged
{
    private LogStatsRTViewModel _logStatsRTVM;
    public event PropertyChangedEventHandler PropertyChanged;
    
    private readonly string _langCode = Properties.Settings.Default.Language; 
    private readonly string _logType = Properties.Settings.Default.LogType;

    public SettingsView(SaveProcessViewModel saveProcess)
    {
        this.DataContext = saveProcess;
        InitializeComponent();
        
        if (_langCode == "en-US")
        {
            languageComboBox1.SelectedIndex = 0;
        }
        else
        {
            languageComboBox1.SelectedIndex = 1;
        }

        if (_logType == "xml")
        {
            logTypeChange.SelectedIndex = 0;
        }
        else
        {
            logTypeChange.SelectedIndex = 1;
        }
    }

    private void ApplyChanges_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = Application.Current.MainWindow as MainWindow;
        mainWindow?.Home_Click(this, new RoutedEventArgs());  
    }

    
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !e.Text.All(char.IsDigit);
    }
}