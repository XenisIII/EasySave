using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave.ViewModels;
using EasySave.Services;

namespace EasySave.Models
{
    public class SavesModel
    {
        private List<CreateSave> _SavesList = new List<CreateSave>();
        public List<CreateSave> SavesList
        {
            get => _SavesList;
            set => _SavesList = value;
        }
    }
}