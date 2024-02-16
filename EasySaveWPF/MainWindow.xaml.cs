using EasySaveWPF.Views;
using System.Windows;

namespace EasySaveWPF;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ContentFrame.Content = new HomeView();
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        // Load the Home view
        ContentFrame.Navigate(new HomeView());
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        // Load the Settings view into the content frame
        ContentFrame.Navigate(new SettingsView());
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
