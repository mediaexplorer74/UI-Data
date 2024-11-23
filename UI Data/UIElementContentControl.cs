using Get.Data.Properties;

namespace Get.UI.Data;
[AutoProperty]
public partial class UIElementContentControl : TemplateControl<Border>
{
    public IProperty<UIElement?> Content { get; } = Auto<UIElement?>(null);

    protected override void Initialize(Border rootElement)
    {
        Content.ApplyAndRegisterForNewValue((_, x) =>
        {
            rootElement.Child = x;
        });
    }
}