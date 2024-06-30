using Get.Data.Bindings;
using Get.Data.Bindings.Linq;
using Get.Data.DataTemplates;
using Get.Data.Properties;
using Get.Data.Collections.Conversion;

namespace Get.UI.Data;

public class ItemsBundleControl<T> : TemplateControl<T> where T : Panel, new()
{
    public Property<ReadOnlyItemsBundle<UIElement>?> ItemsBundleProperty { get; } = new(default);
    IDisposable? disposable;
    protected override void Initialize(T rootElement)
    {
        disposable = ItemsBundleProperty.Value.ReadOnlyOutputContent.Bind(rootElement.Children.AsGDCollection());
        ItemsBundleProperty.ValueChanged += (_, @new) =>
        {
            disposable?.Dispose();
            disposable = @new.ReadOnlyOutputContent.Bind(rootElement.Children.AsGDCollection());
        };
    }
    readonly struct NullProperty : IReadOnlyProperty<UIElement?>
    {
        UIElement? IReadOnlyDataBinding<UIElement?>.CurrentValue => null;

        event Action INotifyBinding<UIElement?>.RootChanged
        {
            add { }
            remove { }
        }

        event ValueChangingHandler<UIElement> INotifyBinding<UIElement?>.ValueChanging
        {
            add { }
            remove { }
        }

        event ValueChangedHandler<UIElement> INotifyBinding<UIElement?>.ValueChanged
        {
            add { }
            remove { }
        }

        void IReadOnlyProperty<UIElement>.BindOneWayToSource(IBinding<UIElement> binding) { }
    }
}