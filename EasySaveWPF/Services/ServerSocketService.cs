using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        string tempFilePath = CreateTempFile();
        Process process = LaunchProcess("notepad.exe", tempFilePath);

        // Ajouter un EventHandler pour gérer l'événement de sortie du processus
        process.EnableRaisingEvents = true; // Nécessaire pour que l'événement Exited soit déclenché
        process.Exited += new EventHandler(Process_Exited);

        // Attendre 2 secondes avant de continuer (simplement pour l'exemple)
        Console.WriteLine("Attente de 2 secondes avant de vérifier l'état du processus...");
        Thread.Sleep(2000);

        // Vérifier si le processus est toujours actif
        if (!process.HasExited)
        {
            Console.WriteLine("Le processus fils est toujours actif.");
        }
        else
        {
            Console.WriteLine("Le processus fils a déjà quitté.");
        }

        // Attendre que l'utilisateur ferme Notepad (pour l'exemple)
        Console.WriteLine("Appuyez sur une touche pour quitter une fois que Notepad est fermé...");
        Console.ReadKey();
    }

    static Process LaunchProcess(string fileName, string arguments)
    {
        Process process = new Process();
        process.StartInfo.FileName = fileName;
        process.StartInfo.Arguments = arguments;
        process.Start();
        Console.WriteLine($"Processus {fileName} avec arguments '{arguments}' n° {process.Id} est lancé.");
        return process;
    }

    static string CreateTempFile()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Ceci est un fichier texte temporaire ouvert dans Notepad.");
        return tempFile;
    }

    // Gestionnaire d'événements pour la sortie du processus
    private static void Process_Exited(object sender, EventArgs e)
    {
        Console.WriteLine("Le processus fils a terminé son exécution.");
    }
}

