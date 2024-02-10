using System;

namespace EasySave.Models
{
    public class LogModel
    {
        private string _name;
        private string _sourcePath;
        private string _targetPath;
        private long _filessize;
        private string _fileTransfertTime;
        private string _time;

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string SourcePath
        {
            get => _sourcePath;
            set => _sourcePath = value;
        }
        public string TargetPath
        {
            get => _targetPath;
            set => _targetPath = value;
        }
        public long FilesSize
        {
            get => _filessize;
            set => _filessize = value;
        }
        public string FileTransfertTime
        {
            get => _fileTransfertTime;
            set => _fileTransfertTime = value;
        }
        public string Time
        {
            get => _time;
            set => _time = value;
        }
    }
}