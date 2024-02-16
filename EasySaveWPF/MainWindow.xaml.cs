using EasySaveWPF.Views;
using System.Windows;
using EasySaveWPF.ViewModels;

namespace EasySaveWPF;
public partial class MainWindow : Window
{
    public LogStatsRTViewModel? _logStatsRTVM { get; }
    public SaveProcess? _SaveProcessVM { get; }
    public MainWindow()
    {
        _logStatsRTVM = new LogStatsRTViewModel();
        _SaveProcessVM = new SaveProcess(_logStatsRTVM);
        InitializeComponent();
        ContentFrame.Content = new HomeView(_SaveProcessVM);
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
}
