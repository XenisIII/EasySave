using System.Windows.Controls;
using System.Windows;
using EasySaveWPF.Services;
using System.ComponentModel;
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

    public SettingsView(SaveProcess saveProcess)
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

    // Add properties to bind with your UI elements, these should notify the UI when changed
    private string _selectedLanguage = "en-US"; // Default language
    public string SelectedLanguage
    {
        get { return _selectedLanguage; }
        set
        {
            if (_selectedLanguage != value)
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
                // Optionally, change the language as soon as a new one is selected
                ChangeLanguage(_selectedLanguage);
            }
        }
    }

    // Add properties for other settings as needed...

    private void ApplyChanges_Click(object sender, RoutedEventArgs e)
    {
        // Apply the selected settings
        ChangeLanguage(SelectedLanguage);
        // Apply other settings...
    }

    private void LogFormatChanged(object sender, RoutedEventArgs e)
    {
        // Handle log format changes...
    }   

    private void ChangeLanguage(string cultureCode)
    {
            LocalizationService.SetCulture(cultureCode);
    }

    private void logTypeChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void languageComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}