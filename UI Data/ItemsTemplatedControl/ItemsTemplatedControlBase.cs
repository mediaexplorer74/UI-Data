#nullable enable
using Get.Data.Collections.Update;
using Get.Data.Collections;
using Get.Data.Properties;
using Get.Data.Collections.Linq;
using Get.Data.Collections.Implementation;

namespace Get.UI.Data;
public abstract class ItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, TDataTemplate, TCollectionType, TUpdateCollectionProperty> : TemplateControl<TRootElement>
    where TDataTemplate : class
    where TTemplateElement : UIElement
    where TRootElement : UIElement, new()
    where TCollectionType : IUpdateReadOnlyCollection<TSrc>
    where TUpdateCollectionProperty : UpdateCollectionPropertyBase<TSrc, TCollectionType>, IUpdateReadOnlyCollection<TSrc>, new()
{
    public TUpdateCollectionProperty ItemsSourceProperty { get; } = new();
    public TCollectionType ItemsSource { get => ItemsSourceProperty.Value; set => ItemsSourceProperty.Value = value; }
    public Property<TDataTemplate?> ItemTemplateProperty { get; } = new(null);
    public TDataTemplate? ItemTemplate
    {
        get => ItemTemplateProperty.Value;
        set => ItemTemplateProperty.Value = value;
    }
    IDisposable? _collectionBinder;
    IGDCollection<TTemplateElement>? _targetChildren;
    public ItemsTemplatedControlBase()
    {
        ItemTemplateProperty.ValueChanged += (old, @new) => RefreshTemplate(@new);
    }
    void RefreshTemplate(TDataTemplate? @new)
    {
        _collectionBinder?.Dispose();
        _targetChildren?.Clear();
        if (ItemsSource is not null && @new is not null && _targetChildren is not null)
            _collectionBinder = Bind(ItemsSourceProperty, _targetChildren, @new);
    }

    protected sealed override void Initialize(TRootElement TemplatedParent)
    {
        _targetChildren = InitializeWithChildren(TemplatedParent);
        RefreshTemplate(ItemTemplate);
    }
    protected abstract IGDCollection<TTemplateElement> InitializeWithChildren(TRootElement TemplatedParent);
    protected abstract IDisposable Bind(TUpdateCollectionProperty collection, IGDCollection<TTemplateElement> @out, TDataTemplate dataTemplate);
    public IGDReadOnlyCollection<TTemplateElement?> ChildContainers => new Wrapper(_targetChildren);
    readonly struct Wrapper(IGDCollection<TTemplateElement>? children) :
        IGDReadOnlyCollection<TTemplateElement?>
    {
        public TTemplateElement? this[int index] {
            get
            {
                if (children is null) return null;
                if (index >= children.Count) return null;
                if (index < 0) return null;
                return children[index];
            }
        }
        public int Count => children?.Count ?? 0;
    }
}

public abstract class OneWayItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, TDataTemplate>
    : ItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, TDataTemplate, IUpdateReadOnlyCollection<TSrc>, OneWayUpdateCollectionProperty<TSrc>>
    where TDataTemplate : class
    where TTemplateElement : UIElement
    where TRootElement : UIElement, new();

public abstract class TwoWayItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, TDataTemplate>
    : ItemsTemplatedControlBase<TRootElement, TSrc, TTemplateElement, TDataTemplate, IUpdateCollection<TSrc>, TwoWayUpdateCollectionProperty<TSrc>>
    where TDataTemplate : class
    where TTemplateElement : UIElement
    where TRootElement : UIElement, new();