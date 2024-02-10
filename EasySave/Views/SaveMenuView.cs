using EasySave.Services;
using System;
using System.Globalization;
using System.Collections.Generic;
using EasySave.ViewModels;
using EasySave.Models;

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

        private List<string> _configuredBackups = new List<string>();
        public List<int> SaveToExecute = new List<int>();
        public List<int> SaveToDelete = new List<int>();

        private List<bool> _selectedBackups = new List<bool>();
        private int _backupSelectionIndex = 0; // Index for navigating the backup list

        public void Display(List<CreateSave> SaveList)
        {
            foreach(CreateSave save in SaveList)
            {
                _configuredBackups.Add(save.name);
            }
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
                            backToMain = PerformAction(selected); // Perform the selected action
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

        private bool PerformAction(int optionIndex)
        {
            switch (optionIndex)
            {
                case 0:
                    SelectBackups("Execute");
                    return true;
                case 1:
                    SelectBackups("Remove");
                    return true;
                case 2:
                    // Return to main menu
                    return true;
            }
            return false;
        }

        private void SelectBackups(string Action)
        {
            // Initialise la liste pour s'assurer qu'elle a la même taille que _configuredBackups
            _selectedBackups = new List<bool>(new bool[_configuredBackups.Count]);

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
                    // Utilisez Indexer pour accéder à l'élément de la liste
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
                        // Inverse la valeur à l'index sélectionné dans la liste
                        _selectedBackups[_backupSelectionIndex] = !_selectedBackups[_backupSelectionIndex];
                        break;
                    case ConsoleKey.A:
                        // Définit tous les éléments de la liste sur true
                        _selectedBackups = Enumerable.Repeat(true, _configuredBackups.Count).ToList();
                        break;
                    case ConsoleKey.Enter:
                        PerformSelectedBackups(Action);
                        done = true;
                        break;
                }
            }
        }

        private void PerformSelectedBackups(string Action)
        {
            // Assume this method initiates the backups
            for (int i = 0; i < _selectedBackups.Count; i++)
            {
                if (_selectedBackups[i])
                {
                    Console.WriteLine($"Initiating backup: {_configuredBackups[i]}");
                    if(Action == "Remove")
                    {
                        SaveToDelete.Add(i);
                        _selectedBackups[i] = false;
                    }
                    else if(Action == "Execute")
                    {
                        SaveToExecute.Add(i);
                    }
                }
            }

            Console.WriteLine("Backups initiated. \nPress any key to return...");
            Console.ReadKey(true);
        }
    }
}
