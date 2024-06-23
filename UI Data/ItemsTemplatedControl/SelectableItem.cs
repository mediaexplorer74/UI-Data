using Get.Data.Bindings;
using Get.Data;
using Get.Data.Properties;
using Get.Data.Bindings.Linq;

namespace Get.UI.Data;
public class SelectableItem<T>(IReadOnlyBinding<IndexItem<T>> itemBinding, Property<int> sourceIdxProperty)
{
    public IReadOnlyBinding<IndexItem<T>> IndexItemBinding { get; } = itemBinding;
    public IBinding<bool> IsSelected { get; } =
        sourceIdxProperty.Zip(itemBinding, (x, item) => item.Index == x,
            delegate (bool output, ref int srcSelectedIdx, IndexItem<T> curItemIdx)
            {
                if (output && srcSelectedIdx != curItemIdx.Index)
                {
                    srcSelectedIdx = curItemIdx.Index;
                }
                else if (!output && srcSelectedIdx == curItemIdx.Index)
                {
                    srcSelectedIdx = -1;
                }
            });
    public void Select()
    {
        IsSelected.CurrentValue = true;
    }
}
