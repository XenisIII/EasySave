using EasySave.Services;
using System;
using System.Globalization;
//using EasySave.ViewModels;

namespace EasySave.Views
{
    public class SaveMenuView
    {
        private string[] _options = new string[]
        {
        "1-"+LocalizationService.GetString("SaveMenuInitiateBackup"),
        "2-"+LocalizationService.GetString("SaveMenuDeleteBackup"),
        "3-"+LocalizationService.GetString("SaveMenuReturnHome"),
        };

        public void Display()
        {
            int selected = 0;
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                DisplayOptions(selected); // Display the options

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selected = selected > 0 ? selected - 1 : _options.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selected = selected < _options.Length - 1 ? selected + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        if (selected == _options.Length - 1)
                        {
                            backToMain = true; // This will exit the loop and return to the main menu
                        }
                        else
                        {
                            PerformAction(selected); // Perform the selected action
                        }
                        break;
                }
            }
        }

        private void DisplayOptions(int selected)
        {
            Console.SetCursorPosition(0, 5); // Set the cursor below the main menu
            for (int i = 0; i < _options.Length; i++)
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
                Console.WriteLine(_options[i]);
            }
            Console.ResetColor();
        }

        private void PerformAction(int optionIndex)
        {
            // Clear the lines below the menu before performing the action
            for (int i = 0; i < _options.Length; i++)
            {
                Console.SetCursorPosition(0, 5 + i);
                ClearCurrentConsoleLine();
            }

            // Dummy action handler - replace with actual logic
            Console.WriteLine("Performing action for " + _options[optionIndex]);
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }

}
