using EasySave.Services;
using System;
using System.Globalization;
using System.IO;
using EasySave.ViewModels.SaveProcess;

namespace EasySave.Views
{
    public class CreateBackupJobView
    {
//       public SaveProcess _saveProcess;
//        public CreateBackupJobView(SaveProcess _SaveProcess) 
//        {
//            this._saveProcess = _SaveProcess;
//        }
        public Action OnReturnToMainMenu;
        
        public (string backupName, string @sourceDirectory, string @targetDirectory, string backupType) Display()
        {
            ConsoleHeader.Display();
            Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewTitle"));
            string backupName = AskForInput(LocalizationService.GetString("CreateBackupJobViewAskForInputName"));
            if (string.IsNullOrWhiteSpace(backupName) || backupName.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null);
            }

            string sourceDirectory = AskForDirectory(LocalizationService.GetString("CreateBackupJobViewAskForDirectorySource"));
            if (string.IsNullOrWhiteSpace(sourceDirectory) || sourceDirectory.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null);
            }

            string targetDirectory = AskForDirectory(LocalizationService.GetString("CreateBackupJobViewAskForDirectoryTarget"));
            if (string.IsNullOrWhiteSpace(targetDirectory) || targetDirectory.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null); 
            }

            string backupType = AskForBackupType();
            if (backupType.ToLower() == "exit")
            {
                OnReturnToMainMenu?.Invoke();
                return (null, null, null, null); 
            }
            return (backupName, @sourceDirectory, @targetDirectory, backupType);
        }

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

        private string AskForDirectory(string prompt)
        {
            string path;
            do
            {
                Console.WriteLine(prompt);
                path = Console.ReadLine();

                if (path?.ToLower() == "exit") // Check if the user wants to exit to main menu
                {
                    return path;
                }

                if (string.IsNullOrWhiteSpace(path)) // Check if the input is not empty or just whitespace
                {
                    Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewNoEntryPath"));
                }
                else if (!Directory.Exists(path)) // Check if the directory exists
                {
                    Console.WriteLine(LocalizationService.GetString("CreateBackupJobViewPathNoExist"));
                    path = null; // Reset path to prompt again
                }
            }
            while (string.IsNullOrEmpty(path)); // Continue to prompt as long as the path is empty or invalid

            return path;
        }

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
