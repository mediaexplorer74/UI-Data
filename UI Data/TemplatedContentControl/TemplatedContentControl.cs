using Get.Data.DataTemplates;
using Get.Data.Properties;

namespace Get.UI.Data;

public abstract class TypedTemplateContentControl<TContent, TTargetElement, TRootElement>
    : TemplateControl<TRootElement>
    where TRootElement : DependencyObject
{
    public Property<IDataTemplate<TContent, TTargetElement>?> ContentTemplateProperty { get; } = new(null);
    public IDataTemplate<TContent, TTargetElement>? ContentTemplate
    {
        get => ContentTemplateProperty.Value;
        set => ContentTemplateProperty.Value = value;
    }
    public Property<TContent?> ContentProperty { get; } = new(default);
    public TContent? Content
    {
        get => ContentProperty.Value;
        set => ContentProperty.Value = value;
    }

}
public abstract class TypedTemplateContentControl<TContent, TRootElement> :
    TypedTemplateContentControl<TContent, UIElement, TRootElement>
    where TRootElement : DependencyObject;