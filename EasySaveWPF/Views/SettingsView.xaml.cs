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
    // Implement INotifyPropertyChanged to update the UI when the language changes
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
    private readonly string _langCode = Properties.Settings.Default.Language; 
    private readonly string _logType = Properties.Settings.Default.LogType;

    public SettingsView(SaveProcessViewModel saveProcess)
    {
        this.DataContext = saveProcess;
        InitializeComponent();
        
        if (_langCode == "en-US")
        {
            languageComboBox1.SelectedItem = 0;
        }
        else
        {
            languageComboBox1.SelectedIndex = 1;
        }

        if (_logType == "xml")
        {
            logTypeChange.SelectedItem = 0;
        }
        else
        {
            logTypeChange.SelectedIndex = 1;
        }
    }
    
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !e.Text.All(char.IsDigit);
    }
}