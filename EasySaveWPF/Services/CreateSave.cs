using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace EasySaveWPF.Services
{
    public class CreateSave : INotifyPropertyChanged
    {
        private string _Name;
        private string _SourcePath;
        private string _TargetPath;
        private string _Type;
        private string _Status;
        private string? _Ext;
        public string Name 
        { 
            get => _Name; 
        }
        public string SourcePath 
        { 
            get => _SourcePath; 
        }
        public string TargetPath 
        { 
            get => _TargetPath; 
        }
        public string Type 
        { 
            get => _Type; 
        }
        public string? Ext 
        { 
            get => _Ext; 
        }
        public string Status
        {
            get => _Status;
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CreateSave(string name, string sourcePath, string targetPath, string type, string? ext, string status)
        {
            this._Name = name;
            this._SourcePath = sourcePath;
            this._TargetPath = targetPath;
            this._Type = type;
            this._Ext = ext;
            this._Status = status;
        }
    }
}