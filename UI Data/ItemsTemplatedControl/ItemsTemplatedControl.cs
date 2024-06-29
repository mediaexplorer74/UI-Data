using Get.Data.Collections.Update;
using Get.Data.Collections;
using Get.Data.DataTemplates;
using Get.Data.Properties;

namespace Get.UI.Data;
public abstract class TwoWayItemsTemplatedControl<TRootElement, TSrc, TTemplateElement> :
    TwoWayItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, DataTemplate<TSrc, TTemplateElement>>
    where TRootElement : DependencyObject
    where TTemplateElement : UIElement
{
    protected sealed override IDisposable Bind(TwoWayUpdateCollectionProperty<TSrc> collection, IGDCollection<TTemplateElement> @out, DataTemplate<TSrc, TTemplateElement> dataTemplate)
        => collection.Bind(@out, dataTemplate);
}

public abstract class OneWayItemsTemplatedControl<TRootElement, TSrc, TTemplateElement> :
    OneWayItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, DataTemplate<TSrc, TTemplateElement>>
    where TRootElement : DependencyObject
    where TTemplateElement : UIElement
{
    protected sealed override IDisposable Bind(OneWayUpdateCollectionProperty<TSrc> collection, IGDCollection<TTemplateElement> @out, DataTemplate<TSrc, TTemplateElement> dataTemplate)
        => collection.Bind(@out, dataTemplate);
}
