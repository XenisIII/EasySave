using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;


// Classe principale pour la sauvegarde diff�rentielle.
public class DifferentialSave
{
    public string SaveName { get; set; }
    public string SourceFilePath { get; set; }
    public string TargetFilePath { get; set; }
    public string MetaDataFilePath { get; set; }

    public void Copy(string SourceFilePath, string TargetFilePath)
    {
        // R�cup�rer la liste des fichiers dossier source
        string[] sourceFiles = Directory.GetFiles(SourceFilePath, "*", SearchOption.AllDirectories);

        foreach (string sourceFile in sourceFiles)
        {
            // R�cup�rer le chemin du fichier
            string relativePath = sourceFile.Substring(Source.Length + 1);

            // Construction du chemin cible 
            string targetFile = Path.Combine(TargetFilePath, relativePath);

            // V�rification si le fichier source existe dans le dossier cible
            if (File.Exists(targetFile))
            {
                // Calcul des hash pour les fichiers sources et cibles 
                string sourceHash = CalculateFileHash(sourceFile);
                string targetHash = CalculateFileHash(targetFile);

                //Si les hash sont diff�rents, copier le fichier
                if (sourceHash != targetHash)
                {
                    //Cr�er le dossier si il n'existe pas
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                    // Copier le fichier
                    File.Copy(sourceFile, targetFile, true);
                }
            }
            else
            {
                // Cr�er le dossier si il n'existe pas
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


