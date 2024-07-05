using Get.Data.Bundles;
using Get.Data.DataTemplates;
using Get.Data.Properties;

namespace Get.UI.Data;

public abstract class TypedTemplateContentControl<TContent, TTargetElement, TRootElement>
    : TemplateControl<TRootElement>
    where TRootElement : UIElement, new()
    where TTargetElement : UIElement
{
    public Property<ContentBundle<TContent, TTargetElement>?> ContentBundleProperty { get; } = new(null);
    public ContentBundle<TContent, TTargetElement>? ContentBundle
    {
        get => ContentBundleProperty.Value;
        set => ContentBundleProperty.Value = value;
    }

}
public abstract class TypedTemplateContentControl<TContent, TRootElement> :
    TypedTemplateContentControl<TContent, UIElement, TRootElement>
    where TRootElement : UIElement, new();