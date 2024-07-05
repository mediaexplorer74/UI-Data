using Get.Data.DataTemplates;
using Get.Data.Properties;
using Get.Data.Collections.Conversion;
using Get.Data.Bundles;

namespace Get.UI.Data;

public class ItemsBundleControl<T> : TemplateControl<T> where T : Panel, new()
{
    public Property<IReadOnlyItemsBundle<UIElement>?> ItemsBundleProperty { get; } = new(default);
    IDisposable? disposable;
    protected override void Initialize(T rootElement)
    {
        disposable = ItemsBundleProperty.Value.OutputContent.Bind(rootElement.Children.AsGDCollection());
        ItemsBundleProperty.ValueChanged += (_, @new) =>
        {
            disposable?.Dispose();
            disposable = @new.OutputContent.Bind(rootElement.Children.AsGDCollection());
        };
    }
}