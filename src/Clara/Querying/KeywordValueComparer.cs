using System.Collections.Generic;

namespace Clara.Querying
{
    public sealed class KeywordValueComparer : IComparer<KeywordValue>
    {
        private KeywordValueComparer()
        {
        }

        public static IComparer<KeywordValue> Default { get; } = new KeywordValueComparer();

        public int Compare(KeywordValue x, KeywordValue y)
        {
            var result = y.IsSelected.CompareTo(x.IsSelected);

            if (result != 0)
            {
                return result;
            }

            return y.Count.CompareTo(x.Count);
        }
    }
}
