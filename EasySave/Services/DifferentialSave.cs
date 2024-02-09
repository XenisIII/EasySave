using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;


// Classe principale pour la sauvegarde différentielle.
public class DifferentialSave
{
    public string SaveName { get; set; }
    public string SourceFilePath { get; set; }
    public string TargetFilePath { get; set; }
    public string MetaDataFilePath { get; set; }

    public void Copy(string SourceFilePath, string TargetFilePath)
    {
        // Récupérer la liste des fichiers dossier source
        string[] sourceFiles = Directory.GetFiles(SourceFilePath, "*", SearchOption.AllDirectories);

        foreach (string sourceFile in sourceFiles)
        {
            // Récupérer le chemin du fichier
            string relativePath = sourceFile.Substring(Source.Length + 1);

            // Construction du chemin cible 
            string targetFile = Path.Combine(TargetFilePath, relativePath);

            // Vérification si le fichier source existe dans le dossier cible
            if (File.Exists(targetFile))
            {
                // Calcul des hash pour les fichiers sources et cibles 
                string sourceHash = CalculateFileHash(sourceFile);
                string targetHash = CalculateFileHash(targetFile);

                //Si les hash sont différents, copier le fichier
                if (sourceHash != targetHash)
                {
                    //Créer le dossier si il n'existe pas
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                    // Copier le fichier
                    File.Copy(sourceFile, targetFile, true);
                }
            }
            else
            {
                // Créer le dossier si il n'existe pas
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                // Copie du fichier
                File.Copy(sourceFile, targetFile);
            }
        }
    }

    // On utilise cette fonction pour calculer le hash avec MD5
    private string CalculateFileHash(string filePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
}


