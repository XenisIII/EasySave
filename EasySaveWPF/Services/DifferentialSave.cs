using System.Security.Cryptography;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace EasySaveWPF.Services
{
    /// <summary>
    /// Implements differential backup logic.
    /// </summary>
    public class DifferentialSave : CommonSaveCommand
    {
        /// <summary>
        /// Initializes differential backup with given settings.
        /// </summary>
        public DifferentialSave(CreateSave save)
        {
            Init(save);
        }

        /// <summary>
        /// Executes the differential backup.
        /// </summary>
        public void Execute(CreateSave save, string process)
        {
            // Prepare directory structure at target location.
            SetTree(save.SourcePath, save.TargetPath);
            int counter = 0;
            foreach (string element in SourcePathAllFiles)
            {
                if (process != null)
                {
                    CheckProcess(process);
                }
                SetInfosInStatsRTModel(save, element.Replace(save.SourcePath, ""));
                if (save.Ext == null || save.Ext == "")
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
                else if (save.Ext != null && save.Ext != "")
                {
                    string targetFile = element.Replace(save.SourcePath, save.TargetPath);
                    string fileExtension = Path.GetExtension(element);
                    string[] allowedExtensions = save.Ext.Split(';');
                    // Si le fichier existe et que les hashs sont les mêmes
                    if (File.Exists(targetFile) && CalculateFileHash(element) == CalculateFileHash(targetFile))
                    {
                        // Vérifie si l'extension du fichier fait partie des extensions à chiffrer, si oui : chiffrer
                        if (allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
                        {
                            string filename = Path.GetFileName(element);
                            string targetDirectory = element.Replace(save.SourcePath, save.TargetPath).Replace(filename, "");
                            string encryptedTargetFile = Path.Combine(targetDirectory, $".encrypted.{filename}");
                            CipherOrDecipher(element, encryptedTargetFile);
                            File.Delete(targetFile);
                        }
                    }
                    // Si fichier n'exite pas ou que le hash est différent
                    if (!File.Exists(targetFile) || CalculateFileHash(element) != CalculateFileHash(targetFile))
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
                        if (allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
                        {
                            string filename1 = Path.GetFileName(targetFile);
                            string encryptedFilename = $".encrypted.{filename1}";
                            string targetDirectory1 = targetFile.Substring(0, targetFile.Length - filename.Length);
                            targetFile = Path.Combine(targetDirectory1, encryptedFilename);
                            CipherOrDecipher(element, targetFile);
                            MessageBoxResult result = MessageBox.Show($"Le fichier {element.Replace(save.SourcePath, save.TargetPath)} existait dans le dossier de destination mais non chiffré et dans une version différente. Voulez-vous supprimer ce fichier?", "Fichier existant : version différente non chiffrée", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                File.Delete(element.Replace(save.SourcePath, save.TargetPath));
                            }
                        }
                        // Sinon copier uniquement
                        else
                        {
                            File.Copy(element, targetFile, true);
                        }
                    }
                }
                UpdateFinishedFileSave();
                counter++;
                if (SourcePathAllFiles.Count == counter)
                {
                    MessageBox.Show($"La sauvegarde {save.Name} est finie", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Calculates MD5 hash of a file for comparison.
        /// </summary>
        private string CalculateFileHash(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
