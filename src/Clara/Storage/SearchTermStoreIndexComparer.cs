namespace Clara.Storage
{
    internal sealed class SearchTermStoreIndexComparer : IComparer<SearchTermStoreIndex>
    {
        private SearchTermStoreIndexComparer()
        {
        }

        public static SearchTermStoreIndexComparer Instance { get; } = new();

        public int Compare(SearchTermStoreIndex x, SearchTermStoreIndex y)
        {
            var result = x.SearchTerm.Ordinal.CompareTo(y.SearchTerm.Ordinal);

            if (result != 0)
            {
                return result;
            }

            return x.StoreIndex.CompareTo(y.StoreIndex);
        }
    }
}
