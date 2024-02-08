using EasySave.Services;
using System;
using System.Globalization;

namespace EasySave.Views
{
    public class SettingsView
    {
        private string[] _languages = new string[]
        {
            "en", // English
            "fr"  // Fran�ais
        };

        private string[] _languageDisplay = new string[]
        {
            "English",
            "Fran�ais"
        };

        // Ajoutez un d�l�gu� pour notifier lorsque la vue est termin�e.
        public Action OnViewFinished;

        public void Display()
        {
            int selected = 0;
            bool done = false;

            while (!done)
            {
                Console.Clear();
                Console.WriteLine(LocalizationService.GetString("ChooseLanguageText")); // "Choisissez la langue :"

                for (int i = 0; i < _languages.Length; i++)
                {
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write("  ");
                    }
                    Console.WriteLine(_languageDisplay[i]);
                }

                Console.ResetColor();
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selected = (selected + _languages.Length - 1) % _languages.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selected = (selected + 1) % _languages.Length;
                        break;
                    case ConsoleKey.Enter:
                        LocalizationService.SetCulture(_languages[selected]);
                        done = true; // Sortie imm�diate apr�s le choix de la langue
                        // Pas besoin d'afficher de message ou de demander une autre touche
                        break;
                }
            }

            // Notification que la vue est termin�e.
            OnViewFinished?.Invoke();
        }
    }
}
