namespace Get.UI.Data;
public abstract class TemplateControl<T> : TemplatedControlBase where T : DependencyObject
{
	readonly static ControlTemplate ControlTemplate = BuildTemplate<T>();

    public TemplateControl() : base(ControlTemplate)
    {
		Name = typeof(T).Name;
    }
    protected sealed override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
		Initialize((T)GetTemplateChild(TemplateChildName));

    }
	protected abstract void Initialize(T rootElement);
}

public abstract class TemplateControl : TemplateControl<UserControl>;

public abstract class TemplatedControlBase : Control
{
    protected const string TemplateChildName = "TemplatedControlChild";
    protected static ControlTemplate BuildTemplate<T>()
    {
        return (ControlTemplate)XamlReader.Load(
        $"""
		<{nameof(ControlTemplate)} TargetType="templateControlRoot:${nameof(TemplatedControlBase)}">
			xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
			xmlns:x= "http://schemas.microsoft.com/winfx/2006/xaml"
			xmlns:templateControlRoot="using:{typeof(TemplatedControlBase).Namespace}"
			xmlns:userControlRoot="using:{typeof(T).Namespace}"
		>
			<userControlRoot:{typeof(T).Name} x:Name="{TemplateChildName}" />
		</{nameof(ControlTemplate)}>
		""");

    }
    internal TemplatedControlBase(ControlTemplate template)
    {
        Template = template;
    }
}