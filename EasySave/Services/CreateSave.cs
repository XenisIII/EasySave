using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave.ViewModels;

namespace EasySave.Services.CreateSaveNameSpace
{
    public class CreateSave
    {
        public string name;
        public string SourcePath;
        public string TargetPath;
        public string type;

        public CreateSave(string Name, string sourcePath, string targetPath, string Type)
        {
            this.name = Name;
            this.SourcePath = sourcePath;
            this.TargetPath = targetPath;
            this.type = Type;
        }
    }
}