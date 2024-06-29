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

public abstract class SelectableItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, TCollectionType, TUpdateCollectionProperty> :
    ItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, IDataTemplate<SelectableItem<TSrc>, TTemplateElement>, TCollectionType, TUpdateCollectionProperty>
    where TRootElement : DependencyObject
    where TTemplateElement : UIElement
    where TCollectionType : IUpdateReadOnlyCollection<TSrc>
    where TUpdateCollectionProperty : UpdateCollectionPropertyBase<TSrc, TCollectionType>, IUpdateReadOnlyCollection<TSrc>, new()
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

    private void ItemsSourceProperty_ItemsChanged(IEnumerable<IUpdateAction<TSrc>> actions)
    {
        foreach (var action in actions)
        {

            switch (action)
            {
                case ItemsAddedUpdateAction<TSrc> added:
                    if (added.StartingIndex <= SelectedIndex)
                        SelectedIndex += added.Items.Count;
                    break;
                case ItemsRemovedUpdateAction<TSrc> removed:
                    if (removed.StartingIndex <= SelectedIndex)
                    {
                        if (SelectedIndex >= removed.StartingIndex + removed.Items.Count)
                            SelectedIndex -= removed.Items.Count;
                        else
                            // the item that we selected got removed
                            SelectedIndex = -1;
                    }
                    break;
                case ItemsMovedUpdateAction<TSrc> moved:
                    if (SelectedIndex == moved.OldIndex) SelectedIndex = moved.NewIndex;
                    else if (SelectedIndex == moved.NewIndex) SelectedIndex = moved.OldIndex;
                    break;
                case ItemsReplacedUpdateAction<TSrc> replaced:
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

    readonly Property<TSrc?> _SelectedValueProperty = new(default);

    public int SelectedIndex { get => SelectedIndexProperty.Value; set => SelectedIndexProperty.Value = value; }
    public bool PreferAlwaysSelectItem { get => PreferAlwaysSelectItemProperty.Value; set => PreferAlwaysSelectItemProperty.Value = value; }
    public ReadOnlyProperty<TSrc?> SelectedValueProperty { get; }
    public TSrc? SelectedValue => _SelectedValueProperty.Value;

    readonly UpdateCollection<SelectableItem<TSrc>> tempCollection = new();
    protected override IDisposable Bind(TUpdateCollectionProperty collection, IGDCollection<TTemplateElement> @out, IDataTemplate<SelectableItem<TSrc>, TTemplateElement> dataTemplate)
    {
        tempCollection.Clear();
        var a = collection.AsUpdateReadOnly().WithIndex().Bind(tempCollection,
            new DataTemplateWithIndex<TSrc, SelectableItem<TSrc>>(root => new(root, SelectedIndexProperty))
        );
        var b = tempCollection.Bind(@out, dataTemplate);
        return new Disposable(() =>
        {
            a.Dispose();
            b.Dispose();
            tempCollection.Clear();
        });
    }
    //partial void OnPrimarySelectedItemChanged(TSrc oldValue, TSrc newValue)
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


public abstract class OneWaySelectableItemsTemplatedControl<TRootElement, TSrc, TTemplateElement>
    : SelectableItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, IUpdateReadOnlyCollection<TSrc>, OneWayUpdateCollectionProperty<TSrc>>
    where TRootElement : DependencyObject
    where TTemplateElement : UIElement;
public abstract class TwoWaySelectableItemsTemplatedControl<TRootElement, TSrc, TTemplateElement>
    : SelectableItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, IUpdateCollection<TSrc>, TwoWayUpdateCollectionProperty<TSrc>>
    where TRootElement : DependencyObject
    where TTemplateElement : UIElement;