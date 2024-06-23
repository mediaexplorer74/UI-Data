#nullable enable
using Get.Data.Collections.Update;
using Get.Data.Collections;
using Get.Data.Properties;

namespace Get.UI.Data;
public abstract class ItemsTemplatedControlBase<T, TTemplate, TElement, TCollectionType, TUpdateCollectionProperty> : TemplateControl<TElement>
    where TTemplate : class
    where TElement : DependencyObject
    where TCollectionType : IUpdateReadOnlyCollection<T>
    where TUpdateCollectionProperty : UpdateCollectionPropertyBase<T, TCollectionType>, IUpdateReadOnlyCollection<T>, new()
{
    public TUpdateCollectionProperty ItemsSourceProperty { get; } = new();
    public TCollectionType ItemsSource { get => ItemsSourceProperty.Value; set => ItemsSourceProperty.Value = value; }
    public Property<TTemplate?> ItemTemplateProperty { get; } = new(null);
    public TTemplate? ItemTemplate
    {
        get => ItemTemplateProperty.Value;
        set => ItemTemplateProperty.Value = value;
    }
    IDisposable? _collectionBinder;
    IGDCollection<UIElement>? _targetChildren;
    public ItemsTemplatedControlBase()
    {
        ItemTemplateProperty.ValueChanged += (old, @new) => RefreshTemplate(@new);
    }
    void RefreshTemplate(TTemplate? @new)
    {
        _collectionBinder?.Dispose();
        _targetChildren?.Clear();
        if (ItemsSource is not null && @new is not null && _targetChildren is not null)
            _collectionBinder = Bind(ItemsSourceProperty, _targetChildren, @new);
    }

    protected sealed override void Initialize(TElement TemplatedParent)
    {
        _targetChildren = InitializeWithChildren(TemplatedParent);
        RefreshTemplate(ItemTemplate);
    }
    protected abstract IGDCollection<UIElement> InitializeWithChildren(TElement TemplatedParent);
    protected abstract IDisposable Bind(TUpdateCollectionProperty collection, IGDCollection<UIElement> @out, TTemplate dataTemplate);
}

public abstract class OneWayItemsTemplatedControlBase<T, TTemplate, TElement>
    : ItemsTemplatedControlBase<T, TTemplate, TElement, IUpdateReadOnlyCollection<T>, OneWayUpdateCollectionProperty<T>>
    where TTemplate : class
    where TElement : DependencyObject;

public abstract class TwoWayItemsTemplatedControlBase<T, TTemplate, TElement>
    : ItemsTemplatedControlBase<T, TTemplate, TElement, IUpdateReadOnlyCollection<T>, TwoWayUpdateCollectionProperty<T>>
    where TTemplate : class
    where TElement : DependencyObject;