namespace Get.UI.Data;

public class NamedPanel : Panel
{
    public NamedPanel()
    {
        Name = GetType().ToReadableString();
    }
}