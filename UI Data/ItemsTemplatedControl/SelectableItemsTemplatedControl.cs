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
        PreferAlwaysSelectItemProperty.ValueChanged += (old, @new) =>
        {
            if (@new && SelectedIndex < 0)
                // let's trigger SelectedIndex auto selection logic
                SelectedIndex = -1;
        };
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
                    else if (SelectedIndex == moved.NewIndex) SelectedIndex = moved.OldIndex;
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

        if (newValue is < 0 && PreferAlwaysSelectItemProperty.Value && ItemsSourceProperty.Count > 0)
        {
            var guessNewIndex = Math.Clamp(oldValue - 1, 0, ItemsSourceProperty.Count - 1);
            SelectedIndex = guessNewIndex;
        }
    }
    public Property<bool> PreferAlwaysSelectItemProperty { get; } = new(false);
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
    //partial void OnPrimarySelectedItemChanged(T oldValue, T newValue)
    //{
    //    if (oldValue == newValue) return;
    //    if (IsLoaded)
    //    {
    //        var container = SafeContainerFromIndex(PrimarySelectedIndex);
    //        if ((container is null && newValue is null) ||
    //            (container is not null && ItemFromContainer(container) == newValue))
    //        {
    //            // Everything is already taken care by index setter
    //            return;
    //        }
    //        if (newValue == null)
    //        {
    //            PrimarySelectedIndex = -1;
    //            return;
    //        }
    //        PrimarySelectedIndex = SafeIndexFromContainer(ContainerFromItem(newValue));
    //        return;
    //    }
    //    else
    //    {
    //        if (newValue is null)
    //        {
    //            if (PrimarySelectedIndex is < 0)
    //                // this is already correct
    //                return;
    //            PrimarySelectedIndex = -1;
    //        }
    //        if (ItemsSource is IList list)
    //        {
    //            if (PrimarySelectedIndex >= 0 && PrimarySelectedIndex < list.Count
    //                && list[PrimarySelectedIndex] == newValue)
    //                // this is already correct
    //                return;
    //            var i = list.IndexOf(newValue);
    //            if (i > 0) throw new KeyNotFoundException("The item you requested to select is not found in the list");
    //            PrimarySelectedIndex = i;
    //        }
    //        else
    //        {
    //            if (PrimarySelectedIndex >= 0 && PrimarySelectedIndex < Items.Count
    //                && Items[PrimarySelectedIndex] == newValue)
    //                // this is already correct
    //                return;
    //            var i = Items.IndexOf(newValue);
    //            if (i > 0) throw new KeyNotFoundException("The item you requested to select is not found in the list");
    //            PrimarySelectedIndex = i;
    //        }

    //    }
    //}
}


public abstract class OneWaySelectableItemsTemplatedControl<T, TElement>
    : SelectableItemsTemplatedControlBase<T, TElement, IUpdateReadOnlyCollection<T>, OneWayUpdateCollectionProperty<T>>
    where TElement : DependencyObject;
public abstract class TwoWaySelectableItemsTemplatedControl<T, TElement>
    : SelectableItemsTemplatedControlBase<T, TElement, IUpdateCollection<T>, TwoWayUpdateCollectionProperty<T>>
    where TElement : DependencyObject;