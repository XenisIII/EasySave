using System;
using System.IO;
using EasySave.Services;
using EasySave.Models.StatsRTModelNameSpace;
using EasySave.Services.CreateSaveNameSpace;

namespace EasySave.Services.CommonSaveCommandNameSpace
{
    public class CommonSaveCommand
    {
        public List<string> SourcePathAllFiles;
        private long Sizes;
        private StatsRTModel _StatsRTModel;
        public StatsRTModel statsRTModel
        {
            get => _StatsRTModel;
        }
        public void init(CreateSave save)
        {
            VerifyFilesToCopy(save.SourcePath)
        }
        public void SetInfosInStatsRTModel(CreateSave save, string fileName)
        {
            _StatsRTModel = new StatsRTModel();
            _StatsRTModel.SaveName = save.name;
            _StatsRTModel.TotalFilesToCopy = VerifyFilesToCopy(save.SourcePath);
            _StatsRTModel.SourceFilePath = GetPathFile(fileName);
            _StatsRTModel.TargetFilePath = GetPathFile(fileName).Replace(save.SourcePath, save.TargetPath);
            _StatsRTModel.State = "Activated";
            _StatsRTModel.TotalFilesSize = this.Sizes;
            _StatsRTModel.NbFilesLeftToDo = this.SourcePathAllFiles.Count - this.SourcePathAllFiles.IndexOf(GetPathFile(fileName));
            _StatsRTModel.Progress = (int)Math.Round(((double)(_StatsRTModel.TotalFilesToCopy - _StatsRTModel.NbFilesLeftToDo) / _StatsRTModel.TotalFilesToCopy) * 100);
        }

        public void UpdateFinishedFileSave()
        {
            _StatsRTModel.State = "Finished";
        }

        public int VerifyFilesToCopy(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Le chemin spécifié n'existe pas.");
                return 0;
            }
            return CountAndSetListPathFiles(new DirectoryInfo(path), this.SourcePathAllFiles);
        }

        public string GetPathFile(string name)
        {
            foreach(string path in this.SourcePathAllFiles)
            {
                if (path.EndsWith(name))
                {
                    return path;
                }
            }
            return null;
        }

        private int CountAndSetListPathFiles(DirectoryInfo directory, List<string> SourcePathAllFiles)
        {
            int count = 0;

            // Obtenir tous les fichiers dans le dossier actuel et les ajouter à la liste
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                this.SourcePathAllFiles.Add(file.FullName); // Ajouter le chemin complet du fichier à la liste
                this.Sizes += file.Length;
            }
            count += files.Length;

            // Parcourir tous les sous-dossiers et compter les fichiers de manière récursive
            DirectoryInfo[] subDirectories = directory.GetDirectories();
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                count += CountAndSetListPathFiles(subDirectory, this.SourcePathAllFiles); // Passer la même liste à l'appel récursif
            }

            return count;
        }
        public void SetTree(string sourceDir, string destDir)
        {
            // Créer le dossier de destination s'il n'existe pas
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Obtenir tous les sous-dossiers du dossier source
            DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
            DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

            foreach (DirectoryInfo subDir in subDirectories)
            {
                // Construire le chemin du sous-dossier de destination correspondant
                string destSubDirPath = Path.Combine(destDir, subDir.Name);

                // Créer le sous-dossier dans le dossier de destination
                Directory.CreateDirectory(destSubDirPath);

                // Appel récursif pour traiter les sous-dossiers du sous-dossier actuel
                SetTree(subDir.FullName, destSubDirPath);
            }
        }
}
