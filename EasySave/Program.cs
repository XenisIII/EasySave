using EasySave.Views;
using EasySave.Services;
using System;
using System.Globalization;
using EasySave.ViewModels;
using EasySave.Services.Common;

LogStatsRTViewModel _LogStatsRTViewModel = new LogStatsRTViewModel();
SaveProcess _SaveProcess = new SaveProcess(_LogStatsRTViewModel);
int NbMaxSave = 5;
int NbSave = 0;

while (true) // Boucle infinie pour le menu principal
{

    Console.Clear();
    ConsoleHeader.Display();

    // Rechargez les options localisées à chaque itération pour refléter la langue actuelle
    string[] options = new string[]
    {
        "1-"+LocalizationService.GetString("HomeOptionCreateSave"),
        "2-"+LocalizationService.GetString("HomeOptionSaveMenu"),
        "3-"+LocalizationService.GetString("HomeOptionSettings"),
        "4-"+LocalizationService.GetString("HomeOptionOpenLog"),
        "5-"+LocalizationService.GetString("HomeOptionExit"),
    };

    Console.WriteLine(LocalizationService.GetString("HomeChooseOptionMessage")); // Message pour choisir une option

    int selected = 0;
    bool done = false;

    while (!done)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (selected == i)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("> ");
            }
            else
            {
                Console.ResetColor();
                Console.Write("  ");
            }

            Console.WriteLine(options[i]);
        };

        Console.ResetColor();
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                selected = Math.Max(0, selected - 1);
                break;
            case ConsoleKey.DownArrow:
                selected = Math.Min(options.Length - 1, selected + 1);
                break;
            case ConsoleKey.Enter:
                done = true; // Marquez que vous avez terminé avec le menu actuel
                break;
        }

        // Réinitialisez la position du curseur pour empêcher le défilement
        Console.SetCursorPosition(0, Console.CursorTop - options.Length);
    }

    // Exécutez l'option sélectionnée et revenez au menu principal
    ExecuteSelectedOption(selected);
}

void ExecuteSelectedOption(int optionIndex)
{
    Console.Clear();
    switch (optionIndex)
    {
        case 0:
            if(NbSave == NbMaxSave)
            {
                Console.WriteLine("Vous avez atteint le nombre maximum de sauvegardes (5)");
            }
            else
            {
                var backupJobView = new CreateBackupJobView();
                var (backupName, @sourceDirectory, @targetDirectory, backupType) = backupJobView.Display();
                if(backupName !=null && sourceDirectory !=null && @targetDirectory !=null && backupType !=null)
                {
                    _SaveProcess.CreateSaveFunc(backupName, @sourceDirectory, @targetDirectory, backupType);
                    NbSave += 1;
                }
            }
            break;
        case 1:
            var saveMenuView = new SaveMenuView();
            saveMenuView.Display(_SaveProcess.SavesList.SavesList);
            List<int> SaveToExecute = new List<int>();
            List<int> SaveToDelete = new List<int>();
            _SaveProcess.DeleteSaveFunc(saveMenuView.SaveToDelete);
            _SaveProcess.ExecuteSaveProcess(saveMenuView.SaveToExecute);
            break;
        case 2:
            var settingsView = new SettingsView();
            settingsView.Display();
            break;
        case 3:
            var logView = new LogView();
            logView.Display();
            break;
        case 4:
            Environment.Exit(0);
            break;
    }
    //Console.WriteLine(LocalizationService.GetString("HomePressAKeyToReturnHome")); // Prompt user with a KeyPress (optional)
    //Console.ReadKey();
};
