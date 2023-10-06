using Clara.Analysis;
using Clara.Analysis.MatchExpressions;
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
            using var tokenList = SharedObjectPools.Tokens.Lease();
            using var tokens = this.analyzer.GetTokens(searchExpression.Text);

            foreach (var token in tokens)
            {
                tokenList.Instance.Add(token);
            }

            var matchExpression =
                tokenList.Instance.Count == 0
                    ? EmptyMatchExpression.Instance
                    : searchExpression.SearchMode == SearchMode.All
                        ? new AllTokensMatchExpression(ScoringMode.Sum, tokenList.Instance)
                        : new AnyTokensMatchExpression(ScoringMode.Sum, tokenList.Instance);

            if (this.synonymMap is not null)
            {
                matchExpression = this.synonymMap.Process(matchExpression);
            }

            return this.textDocumentStore.Search(searchExpression.Field, matchExpression, ref documentResultBuilder);
        }
    }
}
