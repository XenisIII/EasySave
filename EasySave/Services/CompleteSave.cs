using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave.ViewModels;
using EasySave.Services;

namespace EasySave.Services
{
    public class CompleteSave : CommonSaveCommand
    {
        public CompleteSave(CreateSave save) 
        {
            init(save);
        }
        public void Execute(CreateSave save)
        {
            SetTree(save.SourcePath, save.TargetPath);
            foreach (string element in SourcePathAllFiles)
            {
                //Laisser le temps d'écrire les stats -> remplacer par un await par la suite
                Thread.Sleep(10);
                SetInfosInStatsRTModel(save, element.Replace(save.SourcePath, ""));
                File.Copy(element, element.Replace(save.SourcePath, save.TargetPath), true);
                //Laisser le temps d'écrire les stats -> remplacer par un await par la suite
                Thread.Sleep(10);
                UpdateFinishedFileSave();
            }
        }
    }
}