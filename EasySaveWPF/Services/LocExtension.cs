using System.Globalization;
using System.Resources;
using System.Windows.Markup;

namespace EasySaveWPF.Services;

public class LocExtension : MarkupExtension
{
    public string Key { get; set; }

    private static ResourceManager resourceManager = new ResourceManager("EasySaveWPF.Resources.Strings", typeof(LocExtension).Assembly);

    public LocExtension() { }
    public LocExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return resourceManager.GetString(Key, CultureInfo.CurrentUICulture);
    }
}
