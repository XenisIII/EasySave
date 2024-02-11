using EasySave.Services;
using System;
using System.Globalization;
using System.IO;
using EasySave.ViewModels;

namespace EasySave.Views
{
    /// <summary>
    /// Provides the UI for creating a new backup job.
    /// </summary>
    public class CreateBackupJobView
    {
        // Action delegate to trigger return to the main menu.
        public Action OnReturnToMainMenu;

        /// <summary>
        /// Displays the UI for creating a backup job and collects user input.
        /// </summary>
        /// <returns>A tuple containing backup name, source directory, target directory, and backup type.</returns>
        public (string backupName, string sourceDirectory, string targetDirectory, string backupType) Display()
        {
            ConsoleHeader.Display(); // Displays the console header.
            Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewTitle")); // Displays the title.

            // Asks for and validates the backup name.
            string backupName = AskForInput(LocalizationService.GetString("CreateBackupJobViewAskForInputName"));
            if (string.IsNullOrWhiteSpace(backupName) || backupName.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null);
            }

            // Asks for and validates the source directory.
            string sourceDirectory = AskForDirectory(LocalizationService.GetString("CreateBackupJobViewAskForDirectorySource"));
            if (string.IsNullOrWhiteSpace(sourceDirectory) || sourceDirectory.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null);
            }

            // Asks for and validates the target directory.
            string targetDirectory = AskForDirectory(LocalizationService.GetString("CreateBackupJobViewAskForDirectoryTarget"));
            if (string.IsNullOrWhiteSpace(targetDirectory) || targetDirectory.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null);
            }

            // Asks for and sets the backup type.
            string backupType = AskForBackupType();
            if (backupType.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null);
            }

            // Indicates successful creation and prompts user to press Enter to continue.
            Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewSuccessMessage"));
            Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewPressEnter"));
            Console.ReadLine();

            OnReturnToMainMenu?.Invoke();

            return (backupName, sourceDirectory, targetDirectory, backupType);
        }

        /// <summary>
        /// Prompts the user for input and returns the string entered.
        /// </summary>
        private string AskForInput(string prompt)
        {
            string input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        /// <summary>
        /// Prompts the user for a directory path and validates its existence.
        /// </summary>
        private string AskForDirectory(string prompt)
        {
            string path;
            do
            {
                Console.WriteLine(prompt);
                path = Console.ReadLine();

                if (path?.ToLower() == "exit")
                {
                    return path;
                }

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewNoEntryPath"));
                }
                else if (!Directory.Exists(path))
                {
                    Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewPathNoExist"));
                    path = null; // Reset path to prompt again
                }
            }
            while (string.IsNullOrEmpty(path)); // Loop until a valid path is entered or the user exits.

            return path;
        }

        /// <summary>
        /// Prompts the user to select the type of backup: Complete or Differential.
        /// </summary>
        private string AskForBackupType()
        {
            string input;
            do
            {
                Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewAskForBackupTypeChoice"));
                Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewAskForBackupTypeComplete"));
                Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewAskForBackupTypeIncremential"));
                Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewAskForBackupTypeExit"));
                input = Console.ReadLine();
                if (input == "1" || input == "2" || input.ToLower() == "exit")
                {
                    break;
                }
                Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewAskForBackupTypeError"));
            }
            while (true);

            return input == "1" ? "Complete" : input == "2" ? "Differential" : input;
        }
    }
}
