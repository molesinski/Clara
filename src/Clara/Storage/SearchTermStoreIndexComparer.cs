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
            var result = x.SearchTerm.Position.Start.CompareTo(y.SearchTerm.Position.Start);

            if (result != 0)
            {
                return result;
            }

            return x.StoreIndex.CompareTo(y.StoreIndex);
        }
    }
}
