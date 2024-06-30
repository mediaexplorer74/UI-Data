
namespace Get.UI.Data;
static class TypeExtension
{
    public static string ToReadableString(this Type type)
    {
        if (type.IsGenericType)
        {
            var name = type.Name;
            int typeIndex = name.IndexOf('`');
            string baseType = name[..typeIndex];
            Type[] typeArguments = type.GetGenericArguments();

            string arguments = string.Join(", ", typeArguments.Select(ToReadableString));
            return $"{baseType}<{arguments}>";
        }
        else
        {
            return SimplifyName(type.Name);
        }
    }
    private static string SimplifyName(string typeName)
    {
        return typeName switch
        {
            "Boolean" => "bool",
            "Byte" => "byte",
            "SByte" => "sbyte",
            "Char" => "char",
            "Decimal" => "decimal",
            "Double" => "double",
            "Single" => "float",
            "Int32" => "int",
            "UInt32" => "uint",
            "Int64" => "long",
            "UInt64" => "ulong",
            "Int16" => "short",
            "UInt16" => "ushort",
            "String" => "string",
            _ => typeName,
        };
    }
}
public abstract class TemplateControl<T> : TemplatedControlBase where T : UIElement, new()
{
	readonly static ControlTemplate ControlTemplate = BuildTemplate<UserControl>();

    public TemplateControl() : base(ControlTemplate)
    {
		Name = $"{GetType().ToReadableString()} ({typeof(T).ToReadableString()})";
    }
    protected sealed override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var uc = (UserControl)GetTemplateChild(TemplateChildName);
        var t = new T();
        uc.Content = t;
        Initialize(t);

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
        <{nameof(ControlTemplate)}
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:templateControlRoot="using:{typeof(TemplatedControlBase).Namespace}"
            xmlns:userControlRoot="using:{typeof(T).Namespace}">
            <userControlRoot:{typeof(T).Name} x:Name="{TemplateChildName}" />
        </{nameof(ControlTemplate)}>
        """);
    }
    internal TemplatedControlBase(ControlTemplate template)
    {
        Template = template;
    }

}