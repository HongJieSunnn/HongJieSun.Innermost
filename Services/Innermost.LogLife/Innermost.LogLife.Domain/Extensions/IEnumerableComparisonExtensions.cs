namespace Innermost.LogLife.Domain.AggregatesModels.Extensions
{
    internal static class IEnumerableComparisonExtensions
    {
        public static bool EqualList<T>(this IEnumerable<T> list1, IEnumerable<T> list2) where T : Enumeration
        {
            var setOfList2 = list2.ToHashSet();
            foreach (T ele in list1)
            {
                if (!setOfList2.Contains(ele))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
