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

        public override DocumentScoring Search(SearchExpression searchExpression)
        {
            if (searchExpression is TextSearchExpression textSearchExpression)
            {
                return
                    this.textDocumentStore.Search(
                        textSearchExpression.SearchMode,
                        textSearchExpression.Text,
                        textSearchExpression.PositionBoost);
            }

            return base.Search(searchExpression);
        }
    }
}
