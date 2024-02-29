using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Process processExplorer;
        Process processNotepad;

        // Lancer explorer.exe et afficher son ID
        try
        {
            processExplorer = Process.Start("explorer.exe");
            Console.WriteLine($"Processus explorer n° {processExplorer.Id} est lancé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du lancement d'Explorer.exe : {ex.Message}");
        }

        // Lancer notepad.exe et afficher son ID
        try
        {
            processNotepad = Process.Start("notepad.exe");
            Console.WriteLine($"Processus notepad n° {processNotepad.Id} est lancé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du lancement de Notepad.exe : {ex.Message}");
        }
    }
}
