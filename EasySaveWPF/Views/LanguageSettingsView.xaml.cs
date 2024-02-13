using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace EasySaveWPF.Views
{
    public partial class LanguageSettingsPage : Page
    {
        public LanguageSettingsPage()
        {
            InitializeComponent();
        }

        private void ApplyLanguage_Click(object sender, RoutedEventArgs e)
        {
            var selectedLanguage = ((ComboBoxItem)LanguageComboBox.SelectedItem).Content.ToString();
            switch (selectedLanguage)
            {
                case "Français":
                    ChangeCulture("fr-FR");
                    break;
                case "English":
                    ChangeCulture("en-US");
                    break;
                    // Ajouter d'autres cas pour d'autres langues
            }
            MessageBox.Show("La langue a été changée. Veuillez redémarrer l'application pour appliquer les changements.", "Changement de langue", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ChangeCulture(string cultureCode)
        {
            CultureInfo culture = new CultureInfo(cultureCode);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            // Vous devrez ajouter la logique pour actualiser les vues si nécessaire
        }
    }
}
