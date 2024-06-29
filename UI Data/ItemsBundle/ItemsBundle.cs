using Get.Data.Bindings;
using Get.Data.Bindings.Linq;
using Get.Data.Collections;
using Get.Data.Collections.Implementation;
using Get.Data.Collections.Update;
using Get.Data.DataTemplates;
using Get.Data.ObservableCollection.Bindings;
using Get.Data.Properties;
using Windows.UI.Xaml;

namespace Get.UI.Data;


public abstract class ItemsBundle<TOut> : ReadOnlyItemsBundle<TOut>
{
    internal abstract IUpdateItemsBundleOutputCollection<TOut?> OutputContent { get; }
    internal sealed override IUpdateReadOnlyCollection<TOut?> ReadOnlyOutputContent => OutputContent;
}
public abstract class ItemsBundleBase<TIn, TOut> : ItemsBundle<TOut>
{
    protected abstract IUpdateItemsBundleOutputCollection<TIn> ItemsSourceBase { get; }
    public Property<IDataTemplate<TIn, TOut>?> ItemTemplateProperty { get; } = new(default);
    public IDataTemplate<TIn, TOut>? ItemTemplate
    {
        get => ItemTemplateProperty.Value;
        set => ItemTemplateProperty.Value = value;
    }
    internal override IUpdateItemsBundleOutputCollection<TOut> OutputContent => new Wrapper<TIn, TOut>(ItemsSourceBase, dest);
    readonly UpdateCollection<TOut> dest = new();
    IDataTemplateGeneratedValue<TIn, TOut>? _generatedValue;
    public ItemsBundleBase()
    {
        ItemTemplateProperty.ValueChanged += (old, @new) => RefreshTemplate(@new);
    }
    IDisposable? _collectionBinder;
    void RefreshTemplate(IDataTemplate<TIn, TOut>? @new)
    {
        _collectionBinder?.Dispose();
        dest.Clear();
        if (@new is not null)
            _collectionBinder = ItemsSourceBase.Bind(dest, @new);
    }
}
public class ItemsBundle<TIn, TOut> : ItemsBundleBase<TIn, TOut>
{
    public TwoWayUpdateCollectionProperty<TIn> ItemsSourceProperty { get; } = [];
    public IUpdateCollection<TIn> ItemsSource
    {
        get => ItemsSourceProperty.Value;
        set => ItemsSourceProperty.Value = value;
    }
    protected override IUpdateItemsBundleOutputCollection<TIn> ItemsSourceBase => new Wrapper<TIn>(ItemsSourceProperty);
}
//internal class ItemsBundleInternal<TIn, TOut>(IUpdateItemsBundleOutputCollection<TIn> ItemsSourceBase) : ItemsBundleBase<TIn, TOut>
//{
//    protected override IUpdateItemsBundleOutputCollection<TIn> ItemsSourceBase => ItemsSourceBase;
//}
//public abstract class MiddleItemsBundle<TIn2, TOut>
//{
//    public Property<IDataTemplate<TIn2, TOut>?> ItemTemplateProperty { get; } = new(default);
//    public IDataTemplate<TIn2, TOut>? ItemTemplate
//    {
//        get => ItemTemplateProperty.Value;
//        set => ItemTemplateProperty.Value = value;
//    }
//    protected internal abstract MiddleItemsBundleProcessor<TIn2, TOut> Create(IAdaptableDataTemplate<TIn2> adaptableDataTemplate);
//}
//public interface IAdaptableDataTemplate<TIn2>
//{
//    DataTemplateDefinition<TIn1, TIn2> Generate<TIn1>();
//}
//public class ItemsBundle<TIn1, TIn2, TOut> : MiddleItemsBundle<TIn2, TOut>
//{
//    public Property<IUpdateCollection<TIn1>?> ItemsSourceProperty { get; } = new(default);
//    public IUpdateCollection<TIn1>? ItemsSource
//    {
//        get => ItemsSourceProperty.Value;
//        set => ItemsSourceProperty.Value = value;
//    }
//}
//public class MiddleItemsBundleProcessor<TIn2, TOut>
//{
//    public IUpdateItemsBundleOutputCollection<TIn2> ConvertedItemsProperty => a.ItemsBundleProperty;
//    ItemsBundlePropertyWrapper a = new();
//    internal ItemsBundle<TOut> FinalTemplate => a;
//    public MiddleItemsBundleProcessor(IReadOnlyBinding<MiddleItemsBundle<TIn2, TOut>> prop, IAdaptableDataTemplate<TIn2> innerTemplate)
//    {
//        void A(MiddleItemsBundle<TIn2, TOut>? bundle)
//        {
//            a.ItemsBundleProperty.Value = bundle.Create(innerTemplate);
//        }
//        prop.ValueChanged += (_, @new) => A(@new);
//        A(prop.CurrentValue);
//    }
//    class ItemsBundlePropertyWrapper : ItemsBundle<TOut>
//    {
//        public Property<ItemsBundle<TOut>?> ItemsBundleProperty { get; } = new(default);
//        ItemsBundleUpdateCollectionProperty<TOut> ItemsBundleUpdateCollectionProperty1 { get; }
//        internal override IUpdateItemsBundleOutputCollection<TOut?> OutputContent => ItemsBundleUpdateCollectionProperty1;
//        class ItemsBundleUpdateCollectionProperty<T> : UpdateCollectionPropertyBase<T, IUpdateItemsBundleOutputCollection<T>>, IUpdateItemsBundleOutputCollection<T>, IUpdateFixedSizeCollection<T>, IUpdateReadOnlyCollection<T>, IGDReadOnlyCollection<T>, ICollectionUpdateEvent<T>, IGDFixedSizeCollection<T>, IGDCollection<T>, IClearImplGDCollection<T>, IMoveImplGDCollection<T>
//        {
//            public T this[int index]
//            {
//                get => CurrentValue[index];
//                set => CurrentValue[index] = value;
//            }

//            public int Count => CurrentValue.Count;

//            public ItemsBundleUpdateCollectionProperty() : base(new Wrapper<T>(new UpdateCollection<T>())) { }
//            public void Clear() => CurrentValue.Clear();

//            public void Insert(int index, T item) => CurrentValue.Insert(index, item);

//            public void RemoveAt(int index) => CurrentValue.RemoveAt(index);

//            public void Move(int index1, int index2) => CurrentValue.Move(index1, index2);

//            public void Remove(T item) => DefaultImplementations.Remove(CurrentValue, item);
//        }
//    }
//}
//public abstract class MiddleItemsBundleProcessorInternal<TIn2, TOut>
//{
//    public abstract IUpdateItemsBundleOutputCollection<TIn2> ConvertedItemsProperty { get; }
//    internal abstract ItemsBundleInternal<TIn2, TOut> FinalTemplate { get; }
//}
//public class ItemsBundleProcessor<TIn1, TIn2, TOut> : MiddleItemsBundleProcessorInternal<TIn2, TOut>
//{
//    ItemsBundle<TIn1, TIn2> ItemsBundle2 { get; }
//    public ItemsBundleProcessor(ItemsBundle<TIn1, TIn2, TOut> prop, DataTemplateDefinition<TIn1, TIn2> innerTemplate)
//    {
//        ItemsBundle2 = new()
//        {
//            ItemTemplate = new DataTemplate<TIn1, TIn2>(innerTemplate)
//        };
//        ItemsBundle2.ItemsSourceProperty.Bind(prop.ItemsSourceProperty, ReadOnlyBindingModes.OneWay);
//        FinalTemplate = new(ItemsBundle2.OutputContent);
//        FinalTemplate.ItemTemplateProperty.Bind(prop.ItemTemplateProperty, ReadOnlyBindingModes.OneWay);
//    }

//    public sealed override IUpdateItemsBundleOutputCollection<TIn2> ConvertedItemsProperty => ItemsBundle2.OutputContent;
//    internal sealed override ItemsBundleInternal<TIn2, TOut> FinalTemplate { get; }
//}
readonly struct Wrapper<TIn, TOut>(IUpdateItemsBundleOutputCollection<TIn> ItemsSourceBase, IUpdateReadOnlyCollection<TOut> dest) : IUpdateItemsBundleOutputCollection<TOut>
{
    public TOut this[int index] { get => dest[index]; }

    public int Count => dest.Count;

    public event UpdateCollectionItemsChanged<TOut> ItemsChanged
    {
        add => dest.ItemsChanged += value;
        remove => dest.ItemsChanged -= value;
    }

    public void Move(int index1, int index2)
        => ItemsSourceBase.Move(index1, index2);

    public void Remove(TOut item)
        => ItemsSourceBase.RemoveAt(dest.IndexOf(item));

    public void RemoveAt(int index)
        => ItemsSourceBase.RemoveAt(index);
}
readonly struct Wrapper<TIn>(IUpdateCollection<TIn> c) : IUpdateItemsBundleOutputCollection<TIn>
{
    public TIn this[int index] => c[index];

    public int Count => c.Count;

    public event UpdateCollectionItemsChanged<TIn> ItemsChanged
    {
        add => c.ItemsChanged += value;
        remove => c.ItemsChanged -= value;
    }

    public void Move(int index1, int index2)
        => c.Move(index1, index2);

    public void Remove(TIn item)
        => c.Remove(item);

    public void RemoveAt(int index)
        => c.RemoveAt(index);
}