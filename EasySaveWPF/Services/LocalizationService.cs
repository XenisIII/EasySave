using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Program
{
    static void Main()
    {
        // Ouvrir l'explorateur de fichiers sur C:\Windows
        LaunchProcess("explorer.exe", @"C:\Windows");

        // Attendre 2 secondes avant de continuer
        Thread.Sleep(2000);

        // Créer un fichier texte temporaire et l'ouvrir dans Notepad
        string tempFilePath = CreateTempFile();
        LaunchProcess("notepad.exe", tempFilePath);
    }

    static void LaunchProcess(string fileName, string arguments)
    {
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.Start();
            Console.WriteLine($"Processus {fileName} avec arguments '{arguments}' n° {process.Id} est lancé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du lancement de {fileName} avec arguments '{arguments}': {ex.Message}");
        }
    }

    static string CreateTempFile()
    {
        // Chemin du fichier temporaire
        string tempFile = Path.GetTempFileName();

        // Ajouter du contenu au fichier temporaire
        File.WriteAllText(tempFile, "Ceci est un fichier texte temporaire ouvert dans Notepad.");

        return tempFile;
    }
}
