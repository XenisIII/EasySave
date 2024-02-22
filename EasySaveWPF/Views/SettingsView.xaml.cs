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

    public SettingsView(SaveProcessViewModel saveProcess)
    {
        this.DataContext = saveProcess;
        InitializeComponent();
        // Set DataContext for data binding
        //this.DataContext = this;
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
            // Refresh the UI strings by raising the PropertyChanged event for each localized property
            RefreshLocalizedStrings();
    }

    private void RefreshLocalizedStrings()
    {
        // Refresh all properties that are bound to UI elements that use localized strings
        // This could be done by raising PropertyChanged for each property or by a more global UI refresh method
        // Example:
        OnPropertyChanged("LocalizedProperty1");
        OnPropertyChanged("LocalizedProperty2");
        // Continue for all properties that need refreshing...
    }

    private void logTypeChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void languageComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    // Implement LogFormatChanged and LanguageSelectionChanged as needed, potentially using the properties above
    // ...
}