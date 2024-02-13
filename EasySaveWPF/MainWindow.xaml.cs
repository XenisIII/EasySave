using System;
using System.Windows;

namespace EasySaveWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuPrincipal_Click(object sender, RoutedEventArgs e)
        {
            // Exemple de chargement du menu principal dans le Frame
            ContentFrame.Navigate(new Uri("Views/MenuPrincipal.xaml", UriKind.Relative));
        }

        private void Vue1_Click(object sender, RoutedEventArgs e)
        {
            // Charger la Vue 1 dans le Frame
            ContentFrame.Navigate(new Uri("Views/Vue1.xaml", UriKind.Relative));
        }

        private void Vue2_Click(object sender, RoutedEventArgs e)
        {
            // Charger la Vue 2 dans le Frame
            ContentFrame.Navigate(new Uri("Views/Vue2.xaml", UriKind.Relative));
        }

        private void LanguageSettingsPage(object sender, RoutedEventArgs e)
        {
            // Charger la Vue 2 dans le Frame
            ContentFrame.Navigate(new Uri("Views/LanguageSettingsView.xaml", UriKind.Relative));
        }
    }
}
