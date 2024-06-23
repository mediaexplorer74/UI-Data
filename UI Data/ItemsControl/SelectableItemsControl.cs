using Get.Data.Collections.Conversion;
using Get.Data.Collections;

namespace Get.UI.Data;

public class SelectableItemsControl<T>(UIElement element, IGDCollection<UIElement> children) : OneWaySelectableItemsTemplatedControl<T, UserControl>
{
    public SelectableItemsControl(UIElement element, IList<UIElement> children)
        : this(element, children.AsGDCollection()) { }
    
    protected override IGDCollection<UIElement> InitializeWithChildren(UserControl TemplatedParent)
    {
        TemplatedParent.Content = element;
        return children;
    }
}