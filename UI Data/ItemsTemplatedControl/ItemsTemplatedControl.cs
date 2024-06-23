using Get.Data.Collections.Update;
using Get.Data.Collections;
using Get.Data.DataTemplates;
using Get.Data.Properties;

namespace Get.UI.Data;
public abstract class ItemsTemplatedControl<T, TElement> : ItemsTemplatedControlBase<T, DataTemplate<T, UIElement>, TElement> where TElement : DependencyObject
{
    public static PropertyDefinition<ItemsControl<T>, IUpdateReadOnlyCollection<T>> ItemsSourcePropertyDefinition { get; } = new(x => x.ItemsSourceProperty);

    protected sealed override IDisposable Bind(OneWayUpdateCollectionProperty<T> collection, IGDCollection<UIElement> @out, DataTemplate<T, UIElement> dataTemplate)
        => collection.Bind(@out, dataTemplate);
}
