#nullable enable
using Get.Data.Collections.Update;
using Get.Data.Collections;
using Get.Data.Properties;

namespace Get.UI.Data;
public abstract class ItemsTemplatedControlBase<T, TTemplate, TElement> : TemplateControl<TElement>
    where TTemplate : class
    where TElement : DependencyObject
{
    public OneWayUpdateCollectionProperty<T> ItemsSourceProperty { get; } = new();
    public IUpdateReadOnlyCollection<T> ItemsSource { get => ItemsSourceProperty.Value; set => ItemsSourceProperty.Value = value; }
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
    protected abstract IDisposable Bind(OneWayUpdateCollectionProperty<T> collection, IGDCollection<UIElement> @out, TTemplate dataTemplate);
}