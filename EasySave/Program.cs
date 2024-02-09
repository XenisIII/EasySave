using EasySave.Views;
using EasySave.Services;
using System;
using System.Globalization;
using EasySave.ViewModels.SaveProcess;

SaveProcess _SaveProcess = new SaveProcess();

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
            var backupJobView = new CreateBackupJobView();
            var(backupName, @sourceDirectory, @targetDirectory, backupType) = backupJobView.Display();
            _SaveProcess.CreateSaveFunc(backupName, @sourceDirectory, @targetDirectory, backupType);
            break;
        case 1:
            var saveMenuView = new SaveMenuView();
            saveMenuView.Display();
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
