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
using EasySave.Services.CommonSaveCommandNameSpace;
using EasySave.Services.CreateSaveNameSpace;

namespace EasySave.Services.CompleteSaveNameSpace
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
                SetInfosInStatsRTModel(save, element.Replace(save.SourcePath, ""));
                File.Copy(element, element.Replace(save.SourcePath, save.TargetPath), true);
                UpdateFinishedFileSave();
            }
        }
    }
}