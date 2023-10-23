namespace Clara.Analysis
{
    public sealed class SearchTermComparer : IComparer<SearchTerm>
    {
        private SearchTermComparer()
        {
        }

        public static SearchTermComparer Instance { get; } = new SearchTermComparer();

        public int Compare(SearchTerm x, SearchTerm y)
        {
            var result = x.Position.Start.CompareTo(y.Position.Start);

            if (result != 0)
            {
                return result;
            }

            return x.Position.End.CompareTo(y.Position.End);
        }
    }
}
