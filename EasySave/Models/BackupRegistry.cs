using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using EasySave.Services;

namespace EasySave.Models;

/// <summary>
///   Index each created backup and allow you to save them to file.
/// </summary>
public class BackupRegistry
{
  [JsonPropertyName("backups")]
  public Collection<CreateSave> Backups { get; init; }

  public void Save(string path)
  {
    using var stream = File.OpenWrite(path);
    JsonSerializer.Serialize(stream, this);
  }
}