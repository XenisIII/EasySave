using System.Globalization;
using System.Resources;

namespace EasySave.Services
{
    public static class LocalizationService
    {
        private static ResourceManager resourceManager = new ResourceManager("EasySave.Resources.Strings", typeof(LocalizationService).Assembly);

        public static void SetCulture(string cultureCode)
        {
            CultureInfo culture = new CultureInfo(cultureCode);
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
        }

        public static string GetString(string name)
        {
            return resourceManager.GetString(name, CultureInfo.CurrentUICulture);
        }
    }
}
