using EasySave.Services;
using System;
using System.Globalization;
using System.Collections.Generic;
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

        private List<string> _configuredBackups = new List<string>()
        {
            "Backup 1",
            "Backup 2",
            "Backup 3",
            "Backup 4",
            "Backup 5"
        };

        private bool[] _selectedBackups = new bool[5];
        private int _backupSelectionIndex = 0; // Index for navigating the backup list

        public void Display()
        {
            int selected = 0;
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                ConsoleHeader.Display();
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
            Console.SetCursorPosition(0, Console.CursorTop); // Ensure the cursor is at the correct position after the header
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
            switch (optionIndex)
            {
                case 0:
                    SelectBackups();
                    break;
                case 1:
                    // Delete backup logic
                    break;
                case 2:
                    // Return to main menu
                    break;
            }
        }

        private void SelectBackups()
        {
            bool done = false;
            while (!done)
            {
                Console.Clear();
                ConsoleHeader.Display();
                Console.WriteLine(LocalizationService.GetString("SaveMenuViewSelectBackupMessageOption"));
                for (int i = 0; i < _configuredBackups.Count; i++)
                {
                    if (i == _backupSelectionIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    Console.Write(_selectedBackups[i] ? "[x] " : "[ ] ");
                    Console.WriteLine(_configuredBackups[i]);
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        _backupSelectionIndex = (_backupSelectionIndex > 0) ? _backupSelectionIndex - 1 : _configuredBackups.Count - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        _backupSelectionIndex = (_backupSelectionIndex + 1) % _configuredBackups.Count;
                        break;
                    case ConsoleKey.Spacebar:
                        _selectedBackups[_backupSelectionIndex] = !_selectedBackups[_backupSelectionIndex];
                        break;
                    case ConsoleKey.A:
                        Array.Fill(_selectedBackups, true);
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                }
            }

            PerformSelectedBackups();
        }

        private void PerformSelectedBackups()
        {
            // Assume this method initiates the backups
            for (int i = 0; i < _selectedBackups.Length; i++)
            {
                if (_selectedBackups[i])
                {
                    Console.WriteLine($"Initiating backup: {_configuredBackups[i]}");
                    // Here you would call the backup logic for _configuredBackups[i]
                }
            }

            Console.WriteLine("Backups initiated. \nPress any key to return...");
            Console.ReadKey(true);
        }
    }
}
