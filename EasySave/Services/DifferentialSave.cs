using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using EasySave.Services;



namespace EasySave.Services
{
    public class DifferentialSave : CommonSaveCommand
    {
        public DifferentialSave(CreateSave save)
        {
            init(save);
        }

        public void Execute(CreateSave save)
        {
            SetTree(save.SourcePath, save.TargetPath);
            foreach (string element in SourcePathAllFiles)
            {
                string TargetFile = element.Replace(save.SourcePath, save.TargetPath);
                SetInfosInStatsRTModel(save, element.Replace(save.SourcePath, ""));
                if (File.Exists(TargetFile))
                {
                    string sourceHash = CalculateFileHash(element);
                    string targetHash = CalculateFileHash(TargetFile);
                    if (sourceHash != targetHash)
                    {
                        Thread.Sleep(10);
                        File.Copy(element, TargetFile, true);
                    }
                }
                else
                {
                    Thread.Sleep(10);
                    File.Copy(element, TargetFile, true);
                }
                Thread.Sleep(10);
                UpdateFinishedFileSave();
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
