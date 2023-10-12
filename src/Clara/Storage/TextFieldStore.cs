using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class TextFieldStore : FieldStore
    {
        private readonly TextDocumentStore textDocumentStore;

        public TextFieldStore(
            TextDocumentStore textDocumentStore)
        {
            if (textDocumentStore is null)
            {
                throw new ArgumentNullException(nameof(textDocumentStore));
            }

            this.textDocumentStore = textDocumentStore;
        }

        public override SearchFieldStore GetSearchFieldStore(SearchField searchField)
        {
            return new SearchFieldStore(searchField, this.textDocumentStore);
        }
    }
}
