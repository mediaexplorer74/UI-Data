using Get.Data.Properties;
namespace Get.UI.Data;
public class ThemeResources
{
    public static IReadOnlyProperty<T> Create<T>(string resourcesName, FrameworkElement ele)
    {
        if (!ele.Resources.TryGetValue(resourcesName, out var val)) val = Application.Current.Resources[resourcesName];
        var prop = new Property<T>((T)val);
        ele.ActualThemeChanged += delegate
        {
            if (!ele.Resources.TryGetValue(resourcesName, out var val)) val = Application.Current.Resources[resourcesName];
            prop.Value = (T)val;
        };
        return prop;
    }
}