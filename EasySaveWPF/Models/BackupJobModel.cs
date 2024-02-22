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
}