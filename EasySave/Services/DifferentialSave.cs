using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace EasySave.Services
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
        public void Execute(CreateSave save)
        {
            // Prepare directory structure at target location.
            SetTree(save.SourcePath, save.TargetPath);

            foreach (string element in SourcePathAllFiles)
            {
                string targetFile = element.Replace(save.SourcePath, save.TargetPath);

                // Update real-time stats model before copying the file.
                SetInfosInStatsRTModel(save, element.Replace(save.SourcePath, ""));

                // Copy file if it doesn't exist at target or if it's different from the source.
                if (!File.Exists(targetFile) || CalculateFileHash(element) != CalculateFileHash(targetFile))
                {
                    Thread.Sleep(10); // Simulated delay for stats update.
                    File.Copy(element, targetFile, true);
                }

                // Mark the backup step as complete.
                UpdateFinishedFileSave();
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
