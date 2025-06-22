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

        public override DocumentScoring Search(ScoringSearchExpression scoringSearchExpression)
        {
            if (scoringSearchExpression is TextScoringSearchExpression textScoringSearchExpression)
            {
                return
                    this.textDocumentStore.Search(
                        textScoringSearchExpression.SearchMode,
                        textScoringSearchExpression.Text,
                        textScoringSearchExpression.PositionBoost);
            }

            return base.Search(scoringSearchExpression);
        }
    }
}
