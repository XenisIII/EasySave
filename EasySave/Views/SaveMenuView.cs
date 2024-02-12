using EasySave.Services;
using System;
using System.Globalization;
using System.Collections.Generic;
using EasySave.ViewModels;
using EasySave.Models;

namespace EasySave.Views;

/// <summary>
/// Manages the display and interaction of the save menu, allowing users to initiate, delete, or return from backup operations.
/// </summary>

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

    /// <summary>
    /// Displays the save menu options and handles user input to navigate and select options.
    /// </summary>
    /// <param name="SaveList">List of saves to be displayed and managed.</param>
    public void Display(List<CreateSave> SaveList)
    {
        foreach (CreateSave save in SaveList)
        {
            _configuredBackups.Add(save.Name);
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

    /// <summary>
    /// Displays the save menu options based on the current selection.
    /// </summary>
    /// <param name="selected">The index of the currently selected option.</param>
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

    /// <summary>
    /// Executes the action corresponding to the selected menu option.
    /// </summary>
    /// <param name="optionIndex">Index of the selected option.</param>
    /// <returns>True if returning to the main menu; otherwise, false.</returns>
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

    /// <summary>
    /// Allows the user to select backups for execution or deletion based on the specified action.
    /// </summary>
    /// <param name="Action">Determines whether backups are being executed or deleted.</param>
    /// <summary>
    /// Allows user to select backups for action (execute or delete) and triggers the corresponding process.
    /// </summary>
    /// <param name="Action">The action to be performed on selected backups ("Execute" or "Remove").</param>
    private void SelectBackups(string Action)
    {
        // Initialize selection flags to match the number of configured backups.
        _selectedBackups = new List<bool>(new bool[_configuredBackups.Count]);

        bool done = false;
        while (!done)
        {
            Console.Clear();
            ConsoleHeader.Display();
            Console.WriteLine(LocalizationService.GetString("SaveMenuViewSelectBackupMessageOption"));

            // Display backup options with selection status.
            for (int i = 0; i < _configuredBackups.Count; i++)
            {
                // Highlight the currently focused backup.
                if (i == _backupSelectionIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                Console.Write(_selectedBackups[i] ? "[x] " : "[ ] ");
                Console.WriteLine(_configuredBackups[i]);
                Console.ResetColor();
            }

            // Handle user input for navigation and selection.
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow: // Navigate up in the list.
                    _backupSelectionIndex = (_backupSelectionIndex > 0) ? _backupSelectionIndex - 1 : _configuredBackups.Count - 1;
                    break;

                case ConsoleKey.DownArrow: // Navigate down in the list.
                    _backupSelectionIndex = (_backupSelectionIndex + 1) % _configuredBackups.Count;
                    break;

                case ConsoleKey.Spacebar: // Toggle selection status of the current backup.
                    _selectedBackups[_backupSelectionIndex] = !_selectedBackups[_backupSelectionIndex];
                    break;

                case ConsoleKey.A: // Select all backups.
                    _selectedBackups = Enumerable.Repeat(true, _configuredBackups.Count).ToList();
                    break;

                case ConsoleKey.E: // Exit the selection process.
                    done = true;
                    break;

                case ConsoleKey.Enter: // Confirm selection and proceed with the action.
                    PerformSelectedBackups(Action);
                    Console.WriteLine("test1");
                    Console.WriteLine("blablabla");
                    done = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Initiates or deletes selected backups based on the user's choice and updates the backup selection status.
    /// </summary>
    /// <param name="Action">The action to perform on selected backups ("Execute" or "Remove").</param>

    private void PerformSelectedBackups(string Action)
    {
        // Iterates through each backup in the list to determine if it has been selected for action.
        for (int i = 0; i < _selectedBackups.Count; i++)
        {
            if (_selectedBackups[i])
            {
                Console.WriteLine(LocalizationService.GetString("SaveMenuViewInitiaeBackupPre") + _configuredBackups[i]);
                if (Action == "Remove")
                {
                    SaveToDelete.Add(i);
                    _selectedBackups[i] = false;
                }
                else if (Action == "Execute")
                {
                    SaveToExecute.Add(i);
                }
            }
        }

        Console.WriteLine(LocalizationService.GetString("SaveMenuViewBackupInitMsg"));
        Console.ReadKey(true);
    }
}