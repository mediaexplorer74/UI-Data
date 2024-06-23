using Get.Data.DataTemplates;

namespace Get.UI.Data;

public class TypedContentControl<TContent> : TypedTemplateContentControl<TContent, UserControl>
{
    UserControl? TemplatedParent;
    protected override void Initialize(UserControl rootElement)
    {
        TemplatedParent = rootElement;
        if (ContentTemplate is not null)
            TemplatedParent.Content =
                (_generatedValue = ContentTemplate.Generate(ContentProperty))
                .GeneratedValue;
    }
    IDataTemplateGeneratedValue<TContent, UIElement>? _generatedValue;
    public TypedContentControl()
    {
        ContentTemplateProperty.ValueChanged += (old, @new) =>
        {
            if (old is not null && _generatedValue is not null)
                old.NotifyRecycle(_generatedValue);
            if (@new is not null && TemplatedParent is not null)
                TemplatedParent.Content =
                    (_generatedValue = @new.Generate(ContentProperty))
                    .GeneratedValue;
        };
    }
}