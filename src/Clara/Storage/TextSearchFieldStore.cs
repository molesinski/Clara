using Clara.Mapping;
using Clara.Querying;

namespace Clara.Storage
{
    internal readonly record struct TextSearchFieldStore
    {
        public TextSearchFieldStore(TextSearchField textSearchField, TextDocumentStore store)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            this.Field = textSearchField.Field;
            this.Boost = textSearchField.Boost;
            this.Store = store;
        }

        public TextField Field { get; }

        public float Boost { get; }

        public TextDocumentStore Store { get; }
    }
}
