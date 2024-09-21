using System.Collections;

namespace Extensions
{
    public static class IListExtensions
    {
        public static bool IsIndexInBounds(this IList list, int index)
        {
            return index >= 0 && index < list.Count;
        }
    }
}