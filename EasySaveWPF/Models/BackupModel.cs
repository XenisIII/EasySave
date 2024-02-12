using EasySave.Services.Common;

namespace EasySave.Models;

public class BackupModel : ObservableObject
{
  private string? _state;
  
  public string SaveName { get; set; }

  public string SourceFilePath { get; set; }

  public string TargetFilePath { get; set; }

  public string State
  {
    get => this._state;
    set => this.SetProperty(ref this._state, value);
  }

  public int TotalFilesToCopy { get; set; }

  public long TotalFilesSize { get; set; }

  public int NbFilesLeftToDo { get; set; }

  public int Progress { get; set; }
}