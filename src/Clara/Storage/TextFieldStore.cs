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

        public override DocumentScoring Search(TextSearchExpression searchExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            return this.textDocumentStore.Search(searchExpression.SearchMode, searchExpression.Text, searchExpression.PositionBoost, ref documentResultBuilder);
        }
    }
}
