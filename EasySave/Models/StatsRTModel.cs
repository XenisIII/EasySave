using System;
using EasySave.Services.Common;

namespace EasySave.Models.StatsRTModelNameSpace
{
    public class StatsRTModel : ObservableObject
    {
        private string _SaveName;
        private string _SourceFilePath;
        private string _TargetFilePath;
        private string _State;
        private int _TotalFilesToCopy;
        private long _TotalFilesSize;
        private int _NbFilesLeftToDo;
        private int _Progress;

        public string SaveName 
        { 
            get => _SaveName;
            set => _SaveName = value;
        }
        public string SourceFilePath
        {
            get => _SourceFilePath;
            set => _SourceFilePath = value;
        }
        public string TargetFilePath
        {
            get => _TargetFilePath;
            set => _TargetFilePath = value;
        }
        public string State
        {
            get => _State;
            set => SetProperty(ref _State, value);
        }
        public int TotalFilesToCopy
        {
            get => _TotalFilesToCopy;
            set => _TotalFilesToCopy = value;
        }
        public long TotalFilesSize
        {
            get => _TotalFilesSize;
            set => _TotalFilesSize = value;
        }
        public int NbFilesLeftToDo
        {
            get => _NbFilesLeftToDo;
            set => _NbFilesLeftToDo = value;
        }
        public int Progress
        {
            get => _Progress;
            set => _Progress = value;
        }
    }
}
