using EasySave.Services;
using System;

namespace EasySave.Views
{
    public class SettingsView
    {
        // Supported languages and their display names.
        private string[] _languages = new string[] { "en", "fr" };
        private string[] _languageDisplay = new string[] { "English", "Français" };

        // Supported log file types and their display names.
        private string[] _logTypes = new string[] { "xml", "json" };
        private string[] _logTypeDisplay = new string[] { "XML", "JSON" };

        // Current selection indexes for language and log type.
        private int _languageSelected = 0;
        private int _logTypeSelected = 0;

        // Indicates which setting is currently being selected.
        private bool _isSelectingLanguage = true;

        public Action OnViewFinished;

        public string Display()
        {
            bool done = false;
            string logFileType = "xml"; // Default to json

            while (!done)
            {
                Console.Clear();
                ConsoleHeader.Display();

                // Toggle between selecting language and log type.
                Console.WriteLine(_isSelectingLanguage ? LocalizationService.GetString("ChooseLanguageText") : LocalizationService.GetString("ChooseLogType"));

                if (_isSelectingLanguage)
                {
                    DisplayOptions(_languageDisplay, _languageSelected);
                }
                else
                {
                    DisplayOptions(_logTypeDisplay, _logTypeSelected);
                }

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (_isSelectingLanguage)
                        {
                            _languageSelected = Math.Max(0, _languageSelected - 1);
                        }
                        else
                        {
                            _logTypeSelected = Math.Max(0, _logTypeSelected - 1);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_isSelectingLanguage)
                        {
                            _languageSelected = Math.Min(_languages.Length - 1, _languageSelected + 1);
                        }
                        else
                        {
                            _logTypeSelected = Math.Min(_logTypes.Length - 1, _logTypeSelected + 1);
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (_isSelectingLanguage)
                        {
                            LocalizationService.SetCulture(_languages[_languageSelected]); // Apply the selected language.
                            _isSelectingLanguage = false; // Move to selecting log type.
                        }
                        else
                        {
                            logFileType = _logTypes[_logTypeSelected]; // Store the selected log file type.
                            done = true;
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        // Toggle between selecting language and log type without making a final selection.
                        _isSelectingLanguage = !_isSelectingLanguage;
                        break;
                }
            }

            OnViewFinished?.Invoke();
            return logFileType; // Return the selected log file type
        }

        private void DisplayOptions(string[] options, int selected)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.Write(i == selected ? "> " : "  "); // Highlight selected option.
                Console.WriteLine(options[i]);
            }
        }
    }
}
