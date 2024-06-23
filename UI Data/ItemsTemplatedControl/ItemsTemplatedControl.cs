using Get.Data.Collections.Update;
using Get.Data.Collections;
using Get.Data.DataTemplates;
using Get.Data.Properties;

namespace Get.UI.Data;
public abstract class TwoWayItemsTemplatedControl<T, TElement> : TwoWayItemsTemplatedControlBase<T, DataTemplate<T, UIElement>, TElement> where TElement : DependencyObject
{
    protected sealed override IDisposable Bind(TwoWayUpdateCollectionProperty<T> collection, IGDCollection<UIElement> @out, DataTemplate<T, UIElement> dataTemplate)
        => collection.Bind(@out, dataTemplate);
}

public abstract class OneWayItemsTemplatedControl<T, TElement> : OneWayItemsTemplatedControlBase<T, DataTemplate<T, UIElement>, TElement> where TElement : DependencyObject
{
    protected sealed override IDisposable Bind(OneWayUpdateCollectionProperty<T> collection, IGDCollection<UIElement> @out, DataTemplate<T, UIElement> dataTemplate)
        => collection.Bind(@out, dataTemplate);
}
