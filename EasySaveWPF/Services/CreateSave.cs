namespace EasySaveWPF.Services
{
  public class CreateSave(string name, string sourcePath, string targetPath, string type, string? ext)
  {
    public string Name { get; } = name;
    public string SourcePath { get; } = sourcePath;
    public string TargetPath { get; } = targetPath;
    public string Type { get; } = type;

    public string? Ext { get;  } = ext;
  }
}