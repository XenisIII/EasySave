using EasySave.Views;
using EasySave.Services;
using System;
using System.Globalization;

Console.WriteLine("-------------------------------------");
Console.WriteLine("-------------Easy Backup-------------");
Console.WriteLine("-------------------------------------");

string[] options = new string[]
{
    LocalizationService.GetString("HomeOption1CreateSave"),
    LocalizationService.GetString("HomeOption2Settings"),
    LocalizationService.GetString("HomeOption3OpenLog"),
    LocalizationService.GetString("HomeOption4Exit"),
};

int selected = 0;
bool done = false;

while (!done)
{
    Console.WriteLine("\nVeuillez choisir une des options : ");

    for (int i = 0; i < options.Length; i++)
    {
        if (selected == i)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("> ");
        }
        else
        {
            Console.Write("  ");
        }

        Console.WriteLine(options[i]);
        Console.ResetColor();
    }

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            selected = Math.Max(0, selected - 1);
            break;
        case ConsoleKey.DownArrow:
            selected = Math.Min(options.Length - 1, selected + 1);
            break;
        case ConsoleKey.Enter:
            Console.Clear();
            ExecuteSelectedOption(selected);
            break;
    }

    if (!done)
    {
        int newCursorTop = Console.CursorTop - options.Length - 2;
        Console.CursorTop = Math.Max(0, newCursorTop);
    }
}

    void ExecuteSelectedOption(int optionIndex)
{
    switch (optionIndex)
    {
        case 0:
            var backupJobView = new CreateBackupJobView();
            backupJobView.Display();
            break;
        case 1:
            var settingsView = new SettingsView();
            settingsView.Display();
            break;
        case 2:
            var logView = new LogView();
            logView.Display();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }
    Console.WriteLine("Appuyez sur une touche pour retourner au menu principal...");
    Console.ReadKey();
    Console.Clear();
}