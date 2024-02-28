using EasySaveWPF.Common;
using EasySaveWPF.Services;

namespace EasySaveWPF.Models;

public class BackupJobModel : ObservableObject
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;

            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _sourcePath = "";
    public string SourcePath
    {
        get => _sourcePath;
        set
        {
            if (_sourcePath == value) return;

            _sourcePath = value;
            OnPropertyChanged(nameof(SourcePath));
        }
    }

    private string _targetPath = "";
    public string TargetPath
    {
        get => _targetPath;
        set
        {
            if (_targetPath == value) return;

            _targetPath = value;
            OnPropertyChanged(nameof(TargetPath));
        }
    }

    private string _type = "Complete";
    public string Type
    {
        get => _type;
        set
        {
            if (_type == value) return;

            _type = value;
            OnPropertyChanged(nameof(Type));
        }
    }

    private string _extensions = "";
    public string Extensions
    {
        get => _extensions;
        set
        {
            if (_extensions == value) return;

            _extensions = value;
            OnPropertyChanged(nameof(Extensions));
        }
    }
    private string _Status = LocalizationService.GetString("SaveCreatedStatus");
    public string Status
    {
        get => _Status;
        set
        {
            if (_Status == value) return;

            _Status = value;
            OnPropertyChanged(nameof(Status));
        }
    }

    private bool _PauseResume = false;
    public bool PauseResume
    {
        get => _PauseResume;
        set
        {
            if (_PauseResume == value) return;

            _PauseResume = value;
            OnPropertyChanged(nameof(PauseResume));
        }
    }
    private bool _Stop = false;
    public bool Stop
    {
        get => _Stop;
        set
        {
            if (_Stop == value) return;

            _Stop = value;
            OnPropertyChanged(nameof(Stop));
        }
    }
    private int _Progress;
    public int Progress
    {
        get => _Progress;
        set
        {
            if (_Progress == value) return;

            _Progress = value;
            OnPropertyChanged(nameof(Progress));

        }
    }
}