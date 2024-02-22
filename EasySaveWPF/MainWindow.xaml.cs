using EasySaveWPF.Views;
using System.Windows;
using EasySaveWPF.ViewModels;
using EasySaveWPF.Services;

namespace EasySaveWPF;
public partial class MainWindow : Window
{
    public LogStatsRTViewModel? _logStatsRTVM { get; }
    public SaveProcess? _SaveProcessVM { get; }

    string langCode = Properties.Settings.Default.Language;
    public MainWindow()
    {
        _logStatsRTVM = new LogStatsRTViewModel();
        _SaveProcessVM = new SaveProcess(_logStatsRTVM);
        LocalizationService.SetCulture(langCode);
        InitializeComponent();
        ContentFrame.Content = new HomeView(_SaveProcessVM);

        
        LocalizationService.CultureChanged += OnCultureChanged;
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        // Load the Home view
        ContentFrame.Navigate(new HomeView(_SaveProcessVM));
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        // Load the Settings view into the content frame
        ContentFrame.Navigate(new SettingsView(_SaveProcessVM));
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        // Load the About view
        ContentFrame.Navigate(new AboutView());
    }
    private void Help_Click(object sender, RoutedEventArgs e)
    {
        // Load the About view
        ContentFrame.Navigate(new HelpView());
    }

    private void OnCultureChanged()
    {
        HomeButton.Content = LocalizationService.GetString("SideBarHome");
        SettingsButton.Content = LocalizationService.GetString("SideBarSettings");
        AboutButton.Content = LocalizationService.GetString("SideBarAbout");
        HelpButton.Content = LocalizationService.GetString("SideBarHelp");
    }
}
