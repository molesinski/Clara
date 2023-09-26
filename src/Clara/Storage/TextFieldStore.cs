using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class TextFieldStore : FieldStore
    {
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap? synonymMap;
        private readonly TextDocumentStore textDocumentStore;

        public TextFieldStore(
            IAnalyzer analyzer,
            ISynonymMap? synonymMap,
            TextDocumentStore textDocumentStore)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (textDocumentStore is null)
            {
                throw new ArgumentNullException(nameof(textDocumentStore));
            }

            this.analyzer = analyzer;
            this.synonymMap = synonymMap;
            this.textDocumentStore = textDocumentStore;
        }

        public override double FilterOrder
        {
            get
            {
                return double.MinValue;
            }
        }

        public override DocumentScoring Search(SearchExpression searchExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            using var tokens = SharedObjectPools.Tokens.Lease();

            foreach (var token in this.analyzer.GetTokens(searchExpression.Text))
            {
                tokens.Instance.Add(token);
            }

            MatchExpression matchExpression =
                tokens.Instance.Count == 0
                    ? EmptyTokensMatchExpression.Instance
                    : searchExpression.Mode == SearchMode.All
                        ? new AllTokensMatchExpression(tokens.Instance)
                        : new AnyTokensMatchExpression(tokens.Instance);

            if (this.synonymMap is not null)
            {
                matchExpression = this.synonymMap.Process(matchExpression);
            }

            return this.textDocumentStore.Search(searchExpression.Field, matchExpression, ref documentResultBuilder);
        }
    }
}
