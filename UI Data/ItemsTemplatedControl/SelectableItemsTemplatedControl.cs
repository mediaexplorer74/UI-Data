using Get.Data.Bindings;
using Get.Data.Collections.Conversion;
using Get.Data.Collections.Update;
using Get.Data.Collections;
using Get.Data.DataTemplates;
using Get.Data.Properties;
using Get.Data;
using Get.Data.Collections.Linq;
using Get.Data.Bindings.Linq;

namespace Get.UI.Data;

public abstract class SelectableItemsTemplatedControl<T, TElement> :
    ItemsTemplatedControlBase<T, DataTemplate<SelectableItem<T>, UIElement>, TElement>
    where TElement : DependencyObject
{
    public SelectableItemsTemplatedControl()
    {
        SelectedValueProperty = new(_SelectedValueProperty);
        SelectedIndexProperty.ValueChanged += SelectedIndexProperty_ValueChanged;
        ItemsSourceProperty.ItemsChanged += ItemsSourceProperty_ItemsChanged;
        tempCollection.ItemsChanged += delegate
        {
            var a = this;
        };
    }

    private void ItemsSourceProperty_ItemsChanged(IEnumerable<IUpdateAction<T>> actions)
    {
        foreach (var action in actions)
        {

            switch (action)
            {
                case ItemsAddedUpdateAction<T> added:
                    if (added.StartingIndex <= SelectedIndex)
                        SelectedIndex += added.Items.Count;
                    break;
                case ItemsRemovedUpdateAction<T> removed:
                    if (removed.StartingIndex <= SelectedIndex)
                    {
                        if (SelectedIndex >= removed.StartingIndex + removed.Items.Count)
                            SelectedIndex -= removed.Items.Count;
                        else
                            // the item that we selected got removed
                            SelectedIndex = -1;
                    }
                    break;
                case ItemsMovedUpdateAction<T> moved:
                    if (SelectedIndex == moved.OldIndex) SelectedIndex = moved.NewIndex;
                    if (SelectedIndex == moved.NewIndex) SelectedIndex = moved.OldIndex;
                    break;
                case ItemsReplacedUpdateAction<T> replaced:
                    if (SelectedIndex == replaced.Index) SelectedIndex = -1;
                    break;
            }
        }
    }

    private void SelectedIndexProperty_ValueChanged(int oldValue, int newValue)
    {
        if (newValue >= 0)
            _SelectedValueProperty.Value = ItemsSourceProperty[newValue];
        else
            _SelectedValueProperty.Value = default;
    }

    public static PropertyDefinition<SelectableItemsControl<T>, int> SelectedIndexPropertyDefnition { get; } = new(x => x.SelectedIndexProperty);
    public static ReadOnlyPropertyDefinition<SelectableItemsControl<T>, T?> SelectedValuePropertyDefnition { get; } = new(x => x.SelectedValueProperty);
    public Property<int> SelectedIndexProperty { get; } = new(-1);

    readonly Property<T?> _SelectedValueProperty = new(default);

    public int SelectedIndex { get => SelectedIndexProperty.Value; set => SelectedIndexProperty.Value = value; }
    public ReadOnlyProperty<T?> SelectedValueProperty { get; }
    public T? SelectedValue => _SelectedValueProperty.Value;

    readonly UpdateCollection<SelectableItem<T>> tempCollection = new();
    public static PropertyDefinition<SelectableItemsControl<T>, IUpdateReadOnlyCollection<T>> ItemsSourcePropertyDefinition { get; } = new(x => x.ItemsSourceProperty);
    protected override IDisposable Bind(OneWayUpdateCollectionProperty<T> collection, IGDCollection<UIElement> @out, DataTemplate<SelectableItem<T>, UIElement> dataTemplate)
    {
        tempCollection.Clear();
        var a = collection.AsUpdateReadOnly().WithIndex().Bind(tempCollection,
            new DataTemplateWithIndex<T, SelectableItem<T>>(root => new(root, SelectedIndexProperty))
        );
        var b = tempCollection.Bind(@out, dataTemplate);
        return new Disposable(() =>
        {
            a.Dispose();
            b.Dispose();
            tempCollection.Clear();
        });
    }
}

