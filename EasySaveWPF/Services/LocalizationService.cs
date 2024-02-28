using System.Globalization;
using System.Resources;

namespace EasySaveWPF.Services;

/// <summary>
/// Provides localization services for the application, allowing for dynamic language changes.
/// </summary>
public static class LocalizationService
{
    /// <summary>
    /// Manages resource strings for localization.
    /// </summary>
    private static ResourceManager resourceManager =
        new ResourceManager("EasySaveWPF.Resources.Strings", typeof(LocalizationService).Assembly);

    public delegate void CultureChangedHandler();
    public static event CultureChangedHandler? CultureChanged;

    /// <summary>
    /// Sets the application's culture to the specified culture code.
    /// </summary>
    /// <param name="cultureCode">The culture code to set the application to (e.g., "en-US", "fr-FR").</param>
    public static void SetCulture(string cultureCode)
    {
        CultureInfo culture = new CultureInfo(cultureCode);
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;

        Properties.Settings.Default.Language = cultureCode;
        Properties.Settings.Default.Save();

        CultureChanged?.Invoke();
    }

    /// <summary>
    /// Retrieves a localized string by its name.
    /// </summary>
    /// <param name="name">The name of the string resource.</param>
    /// <returns>The localized string.</returns>
    public static string GetString(string name) 
        => resourceManager.GetString(name, CultureInfo.CurrentUICulture);
}
