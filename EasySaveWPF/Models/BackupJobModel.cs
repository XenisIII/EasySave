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

        // Ajouter un EventHandler pour g�rer l'�v�nement de sortie du processus
        process.EnableRaisingEvents = true; // N�cessaire pour que l'�v�nement Exited soit d�clench�
        process.Exited += new EventHandler(Process_Exited);

        // Attendre 2 secondes avant de continuer (simplement pour l'exemple)
        Console.WriteLine("Attente de 2 secondes avant de v�rifier l'�tat du processus...");
        Thread.Sleep(2000);

        // V�rifier si le processus est toujours actif
        if (!process.HasExited)
        {
            Console.WriteLine("Le processus fils est toujours actif.");
        }
        else
        {
            Console.WriteLine("Le processus fils a d�j� quitt�.");
        }

        // Attendre que l'utilisateur ferme Notepad (pour l'exemple)
        Console.WriteLine("Appuyez sur une touche pour quitter une fois que Notepad est ferm�...");
        Console.ReadKey();
    }

    static Process LaunchProcess(string fileName, string arguments)
    {
        Process process = new Process();
        process.StartInfo.FileName = fileName;
        process.StartInfo.Arguments = arguments;
        process.Start();
        Console.WriteLine($"Processus {fileName} avec arguments '{arguments}' n� {process.Id} est lanc�.");
        return process;
    }

    static string CreateTempFile()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Ceci est un fichier texte temporaire ouvert dans Notepad.");
        return tempFile;
    }

    // Gestionnaire d'�v�nements pour la sortie du processus
    private static void Process_Exited(object sender, EventArgs e)
    {
        Console.WriteLine("Le processus fils a termin� son ex�cution.");
    }
}
