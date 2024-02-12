using EasySave.Services;
using System;

namespace EasySave.Views
{
    /// <summary>
    /// Allows users to select the application's language.
    /// </summary>
    public class SettingsView
    {
        // Supported languages and their display names.
        private string[] _languages = new string[] { "en", "fr" };
        private string[] _languageDisplay = new string[] { "English", "Français" };

        /// <summary>
        /// Delegate for notifying when the view process is finished.
        /// </summary>
        public Action OnViewFinished;

        /// <summary>
        /// Displays language selection options and handles user input.
        /// </summary>
        public void Display()
        {
            int selected = 0;
            bool done = false;

            while (!done)
            {
                Console.Clear();
                ConsoleHeader.Display();
                Console.WriteLine(LocalizationService.GetString("ChooseLanguageText"));

                // Display language options.
                for (int i = 0; i < _languages.Length; i++)
                {
                    Console.Write(i == selected ? "> " : "  "); // Highlight selected language.
                    Console.WriteLine(_languageDisplay[i]);
                }

                // Handle keyboard input for navigation and selection.
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selected = Math.Max(0, selected - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selected = Math.Min(_languages.Length - 1, selected + 1);
                        break;
                    case ConsoleKey.Enter:
                        LocalizationService.SetCulture(_languages[selected]); // Apply the selected language.
                        done = true;
                        break;
                }
            }

            // Notify that the view process is finished.
            OnViewFinished?.Invoke(); 
        }
    }
}
