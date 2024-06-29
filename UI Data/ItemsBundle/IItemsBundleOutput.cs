using Get.Data.Collections.Implementation;
using Get.Data.Collections.Update;

namespace Get.UI.Data;

public interface IUpdateItemsBundleOutputCollection<T> :
    IUpdateReadOnlyCollection<T>
{
    void RemoveAt(int index);
    // Because this does not implement IGDCollection<T>
    // remove does not exist, so this is provided as a helper
    void Remove(T item);
    void Move(int index1, int index2);
}