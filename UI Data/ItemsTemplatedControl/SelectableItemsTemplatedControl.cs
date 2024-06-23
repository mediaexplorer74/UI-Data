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

public abstract class SelectableItemsTemplatedControlBase<T, TElement, TCollectionType, TUpdateCollectionProperty> :
    ItemsTemplatedControlBase<T, IDataTemplate<SelectableItem<T>, UIElement>, TElement, TCollectionType, TUpdateCollectionProperty>
    where TElement : DependencyObject
    where TCollectionType : IUpdateReadOnlyCollection<T>
    where TUpdateCollectionProperty : UpdateCollectionPropertyBase<T, TCollectionType>, IUpdateReadOnlyCollection<T>, new()
{
    public SelectableItemsTemplatedControlBase()
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
    public Property<int> SelectedIndexProperty { get; } = new(-1);

    readonly Property<T?> _SelectedValueProperty = new(default);

    public int SelectedIndex { get => SelectedIndexProperty.Value; set => SelectedIndexProperty.Value = value; }
    public ReadOnlyProperty<T?> SelectedValueProperty { get; }
    public T? SelectedValue => _SelectedValueProperty.Value;

    readonly UpdateCollection<SelectableItem<T>> tempCollection = new();
    protected override IDisposable Bind(TUpdateCollectionProperty collection, IGDCollection<UIElement> @out, IDataTemplate<SelectableItem<T>, UIElement> dataTemplate)
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


public abstract class OneWaySelectableItemsTemplatedControl<T, TElement>
    : SelectableItemsTemplatedControlBase<T, TElement, IUpdateReadOnlyCollection<T>, OneWayUpdateCollectionProperty<T>>
    where TElement : DependencyObject;
public abstract class TwoWaySelectableItemsTemplatedControl<T, TElement>
    : SelectableItemsTemplatedControlBase<T, TElement, IUpdateReadOnlyCollection<T>, OneWayUpdateCollectionProperty<T>>
    where TElement : DependencyObject;