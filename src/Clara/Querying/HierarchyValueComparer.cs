using System.Collections.Generic;

namespace Clara.Querying
{
    public sealed class HierarchyValueComparer : IComparer<HierarchyValue>
    {
        private HierarchyValueComparer()
        {
        }

        public static IComparer<HierarchyValue> Default { get; } = new HierarchyValueComparer();

        public int Compare(HierarchyValue x, HierarchyValue y)
        {
            return y.Count.CompareTo(x.Count);
        }
    }
}
