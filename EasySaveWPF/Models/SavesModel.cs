using EasySaveWPF.Services;
using System.Collections.Generic; // Ensure to include this for List<T>

namespace EasySaveWPF.Models;

/// <summary>
/// Holds a collection of backup jobs created by the user.
/// This model acts as a container for all backup configurations,
/// allowing for easy management and retrieval of individual backup settings.
/// </summary>
public class SavesModel
{
    /// <summary>
    /// A list containing instances of CreateSave, each representing a unique backup job configuration.
    /// This property is initialized as an empty list, ready to store backup configurations.
    /// </summary>
    public List<CreateSave> SaveList { get; set; } = new List<CreateSave>();
}
