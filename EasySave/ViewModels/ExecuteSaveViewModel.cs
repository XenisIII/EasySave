using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave.Models;
using EasySave.Services;
using EasySave.Services.CreateSaveNameSpace;
using EasySave.Models.SavesListNameSpace;
using EasySave.Services.CompleteSaveNameSpace;

namespace EasySave.ViewModels.SaveProcess
{
    public class SaveProcess
    {
        private SavesModel _SavesList = new SavesModel();
        public SavesModel SavesList
        {
            get => _SavesList;
        }
        public SaveProcess(string Input)
        {
            string[] WhichSaveToExecute = Input.Split(';');
            foreach (string save in WhichSaveToExecute)
            {
                CreateSave SaveToExecute = _SavesList.SavesList[int.Parse(save)];
                switch (SaveToExecute.type)
                {
                    case "Differential":
                        CompleteSave save1 = new CompleteSave(SaveToExecute);
                        break;
                    case "Complete":
                        break;
                }
            }
        }
        public void CreateSaveFunc(string Name, string SourcePath, string TargetPath, string Type)
        {
            _SavesList.SavesList.Add(new CreateSave(Name, SourcePath, TargetPath, Type));
        }
        public void DeleteSaveFunc(string Name)
        {
            _SavesList.SavesList.RemoveAll(save => save.name == Name);
        }
    }
}