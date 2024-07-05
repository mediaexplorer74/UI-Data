using Get.Data.Bindings;
using Get.Data.Bindings.Linq;
using Get.Data.Bundles;
using Get.Data.Properties;

namespace Get.UI.Data;

public class ContentBundleControl : TemplateControl<Border>
{
    public Property<IContentBundle<UIElement>?> ContentBundleProperty { get; } = new(default);
    public IContentBundle<UIElement> ContentBundle
    {
        get => ContentBundleProperty.Value;
        set => ContentBundleProperty.Value = value;
    }
    protected override void Initialize(Border rootElement)
    {
        if (ContentBundleProperty.Value is null)
            ContentBundleProperty.ValueChanged += (_, _) => Debugger.Break();
        var prop = ContentBundleProperty.SelectPath(x => x.OutputContent);
        prop.ValueChanged += (_, child) => rootElement.Child = child;
        rootElement.Child = prop.CurrentValue;
    }
    public ContentBundleControl()
    {
    }
}