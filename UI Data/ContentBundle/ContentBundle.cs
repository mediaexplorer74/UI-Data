using Get.Data.Bindings;
using Get.Data.Bindings.Linq;
using Get.Data.DataTemplates;
using Get.Data.Properties;
using Windows.UI.Xaml;

namespace Get.UI.Data;

public abstract class ContentBundle
{
    internal abstract IReadOnlyProperty<UIElement?> OutputContent { get; }
}

public abstract class ContentBundle<TOut> : ContentBundle where TOut : UIElement
{
    internal abstract IReadOnlyProperty<TOut?> TypedOutputContent { get; }
}
public class ContentBundle<TIn, TOut> : ContentBundle<TOut>
    where TOut : UIElement
{
    public Property<TIn?> ContentProperty { get; } = new(default);
    public Property<DataTemplate<TIn,TOut>?> ContentTemplateProperty { get; } = new(default);
    Property<TOut?> OutElement { get; } = new(null);

    internal override IReadOnlyProperty<TOut> TypedOutputContent => OutElement;
    internal override IReadOnlyProperty<UIElement?> OutputContent { get; }

    IDataTemplateGeneratedValue<TIn, TOut>? _generatedValue;
    public ContentBundle()
    {
        var prop = new Property<UIElement?>(default);
        prop.Bind(OutElement.Select<TOut, UIElement>(x => x), ReadOnlyBindingModes.OneTime);
        OutputContent = prop;
        ContentTemplateProperty.ValueChanged += (old, @new) =>
        {
            if (old is not null && _generatedValue is not null)
                old.NotifyRecycle(_generatedValue);
            if (@new is not null && OutElement is not null)
                OutElement.Value =
                    (_generatedValue = @new.Generate(ContentProperty))
                    .GeneratedValue;
        };
    }
}
public class ConstantContentBundle<T>(T ele) : ContentBundle<T> where T : UIElement
{
    internal override IReadOnlyProperty<T> TypedOutputContent { get; } = new ReadOnlyProperty<T>(ele);
    internal override IReadOnlyProperty<UIElement> OutputContent { get; } = new ReadOnlyProperty<UIElement>(ele);
}
public class ConstantContentBundle(UIElement ele) : ConstantContentBundle<UIElement>(ele);