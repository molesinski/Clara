namespace Clara.Storage
{
    internal readonly struct SearchTermStoreIndex
    {
        public SearchTermStoreIndex(SearchTerm searchTerm, int storeIndex)
        {
            this.SearchTerm = searchTerm;
            this.StoreIndex = storeIndex;
        }

        public SearchTerm SearchTerm { get; }

        public int StoreIndex { get; }
    }
}
