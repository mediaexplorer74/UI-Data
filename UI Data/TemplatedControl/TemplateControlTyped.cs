
namespace Get.UI.Data;

public abstract class TemplateControl<T> : TemplatedControlBase where T : UIElement, new()
{
	readonly static ControlTemplate ControlTemplate = BuildTemplate<T>();
    public TemplateControl() : base(ControlTemplate)
    {
		Name = $"{GetType().ToReadableString()} ({typeof(T).ToReadableString()})";
    }
    protected sealed override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var templateChild = GetTemplateChild(TemplateChildName);
        if (templateChild is T typed)
            Initialize(typed);
        else
        {
            var uc = (UserControl)GetTemplateChild(TemplateChildName);
            var t = new T();
            uc.Content = t;
            Initialize(t);
        }
    }
	protected abstract void Initialize(T rootElement);
}
