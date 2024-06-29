using Get.Data.Bindings;
using Get.Data.DataTemplates;

class DataTemplateConverter<TIn1, TIn2, TOut>(DataTemplate<TIn2, TOut> dataTemplate, Func<TIn1, TIn2> func) : IDataTemplate<TIn1, TOut>
{
    public IDataTemplateGeneratedValue<TIn1, TOut> Generate(IReadOnlyBinding<TIn1> source)
    {
        throw new NotImplementedException();
    }

    public void NotifyRecycle(IDataTemplateGeneratedValue<TIn1, TOut> recycledItem)
    {
        throw new NotImplementedException();
    }
}