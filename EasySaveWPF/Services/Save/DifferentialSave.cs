using System.Security.Cryptography;
using System.IO;
using System.Windows;
using EasySaveWPF.Models;
using static EasySaveWPF.ViewModels.SaveProcessViewModel;
using System.Collections.ObjectModel;

namespace EasySaveWPF.Services.Save;

/// <summary>
/// Implements differential backup logic.
/// </summary>
public class DifferentialSave : CommonSaveCommand
{
    /// <summary>
    /// Initializes differential backup with given settings.
    /// </summary>
    public DifferentialSave(BackupJobModel save, ObservableCollection<FileExtension> ExtensionsPriority)
    {
        Init(save);
        var selectedExtensions = ExtensionsPriority.Where(extension => extension.IsSelected == true)
                                           .Select(extension => extension.Extension)
                                           .ToList();
        if (selectedExtensions is not null)
        {
            Sort(selectedExtensions);
        }
    }

    /// <summary>
    /// Executes the differential backup.
    /// </summary>
    public void Execute(BackupJobModel save, string process)
    {
        // Prepare directory structure at target location.
        SetTree(save.SourcePath, save.TargetPath);

        foreach (string file in SourcePathAllFiles)
        {
            bool stop = CheckPlayPauseStop(save);
            if (stop)
            {
                return;
            }
            if (process is not null)
            {
                CheckProcess(process, save);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //save.Status = "In Progress";
                    save.Status = LocalizationService.GetString("SaveInProgress");
                });
            }

            SetInfosInStatsRTModel(save, file.Replace(save.SourcePath, ""));

            if (save.Extensions is null || string.IsNullOrEmpty(save.Extensions))
            {
                ExecuteSave(save, file);
            }
            else
            {
                ExecuteSaveByExtension(save, file);
            }

            UpdateFinishedFileSave();
            save.Progress = StatsRTModel.Progress;
        }
        StatsRTModel.Progress = 100;
        save.Progress = StatsRTModel.Progress;
    }

    private void ExecuteSave(BackupJobModel save, string element)
    {
        string targetFile = element.Replace(save.SourcePath, save.TargetPath);
        string filename = Path.GetFileName(element);
        string targetDirectory = element.Replace(save.SourcePath, save.TargetPath).Replace(filename, "");
        string encryptedTargetFile = Path.Combine(targetDirectory, $".encrypted.{filename}");

        if (File.Exists(encryptedTargetFile))
        {
            MessageBoxResult result = MessageBox.Show("Le fichier que vous souhaitez sauvegarder existe déjà en fichier chiffré. Voulez-vous le sauvegarder tel quel ou effectuer le processus de sauvegarde différentielle sur le fichier chiffré? Le fichier sera sauvegardé chiffré.", "Fichier chiffré existant", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Déchiffrer le fichier cible pour comparaison
                string tempDecryptedFile = Path.GetTempFileName();
                CipherOrDecipher(encryptedTargetFile, tempDecryptedFile);

                // Comparer les hashes
                if (CalculateFileHash(element) != CalculateFileHash(tempDecryptedFile))
                {
                    // Les fichiers sont différents, chiffrer et remplacer le fichier cible
                    File.Copy(element, tempDecryptedFile, true);
                    CipherOrDecipher(tempDecryptedFile, encryptedTargetFile);
                }

                // Supprimer le fichier temporaire déchiffré
                File.Delete(tempDecryptedFile);
            }
            else if (result == MessageBoxResult.No)
            {
                if (!File.Exists(targetFile) || CalculateFileHash(element) != CalculateFileHash(targetFile))
                {
                    Thread.Sleep(10); // Simulated delay for stats update.
                    File.Copy(element, targetFile, true);
                }
            }
        }
        else
        {
            if (!File.Exists(targetFile) || CalculateFileHash(element) != CalculateFileHash(targetFile))
            {
                Thread.Sleep(10); // Simulated delay for stats update.
                File.Copy(element, targetFile, true);
            }
        }
    }

    private void ExecuteSaveByExtension(BackupJobModel save, string element)
    {
        string targetFile = element.Replace(save.SourcePath, save.TargetPath);
        string fileExtension = Path.GetExtension(element);
        string[] allowedExtensions = save.Extensions.Split(';');

        // Si le fichier existe et que les hashs sont les mêmes
        if (File.Exists(targetFile) && CalculateFileHash(element) == CalculateFileHash(targetFile))
        {
            // Vérifie si l'extension du fichier fait partie des extensions à chiffrer, si oui : chiffrer
            if (allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) || save.Extensions == ".*")
            {
                string filename = Path.GetFileName(element);
                string targetDirectory = element.Replace(save.SourcePath, save.TargetPath).Replace(filename, "");
                string encryptedTargetFile = Path.Combine(targetDirectory, $".encrypted.{filename}");
                CipherOrDecipher(element, encryptedTargetFile);
                File.Delete(targetFile);
            }
        }
        // Si fichier n'exite pas ou que le hash est différent
        else
        {
            string filename = Path.GetFileName(element);
            string targetDirectory = element.Replace(save.SourcePath, save.TargetPath).Replace(filename, "");
            string encryptedTargetFile = Path.Combine(targetDirectory, $".encrypted.{filename}");

            if (File.Exists(encryptedTargetFile))
            {
                // Déchiffrer le fichier cible pour comparaison
                string tempDecryptedFile = Path.GetTempFileName();

                CipherOrDecipher(encryptedTargetFile, tempDecryptedFile);

                // Comparer les hashes
                if (CalculateFileHash(element) != CalculateFileHash(tempDecryptedFile))
                {
                    // Les fichiers sont différents, chiffrer et remplacer le fichier cible
                    File.Copy(element, tempDecryptedFile, true);
                    CipherOrDecipher(tempDecryptedFile, encryptedTargetFile);
                }
            }
            // Vérifie si l'extension du fichier fait partie des extensions à chiffrer, si oui : copier et chiffrer
            if (allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) || save.Extensions == ".*")
            {
                string filename1 = Path.GetFileName(targetFile);
                string encryptedFilename = $".encrypted.{filename1}";
                string targetDirectory1 = targetFile.Substring(0, targetFile.Length - filename.Length);

                targetFile = Path.Combine(targetDirectory1, encryptedFilename);

                CipherOrDecipher(element, targetFile);

                /*MessageBoxResult result = MessageBox.Show($"Le fichier {element.Replace(save.SourcePath, save.TargetPath)} existait dans le dossier de destination mais non chiffré et dans une version différente. Voulez-vous supprimer ce fichier?", "Fichier existant : version différente non chiffrée", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(element.Replace(save.SourcePath, save.TargetPath));
                }*/
            }
            // Sinon copier uniquement
            else
            {
                File.Copy(element, targetFile, true);
            }
        }
    }

    /// <summary>
    /// Calculates MD5 hash of a file for comparison.
    /// </summary>
    private static string CalculateFileHash(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);

        byte[] hash = md5.ComputeHash(stream);

        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}
