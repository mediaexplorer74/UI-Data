using Get.Data.Bindings;
using Get.Data.Collections.Conversion;
using Get.Data.Collections.Update;
using Get.Data.Collections;

namespace Get.UI.Data;
public class ItemsControl<T>(UIElement element, IGDCollection<UIElement> children) : OneWayItemsTemplatedControl<UserControl, T, UIElement>
{
    public ItemsControl(UIElement element, IList<UIElement> children)
        : this(element, children.AsGDCollection()) { }


    protected override IGDCollection<UIElement> InitializeWithChildren(UserControl TemplatedParent)
    {
        TemplatedParent.Content = element;
        return children;
    }
}
