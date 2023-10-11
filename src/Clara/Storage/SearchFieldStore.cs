using Clara.Querying;

namespace Clara.Storage
{
    internal readonly record struct SearchFieldStore
    {
        public SearchFieldStore(SearchField searchField, TextFieldStore store)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            this.SearchField = searchField;
            this.Store = store;
        }

        public SearchField SearchField { get; }

        public TextFieldStore Store { get; }
    }
}
