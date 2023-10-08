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
        private readonly ITokenEncoder tokenEncoder;
        private readonly TextDocumentStore textDocumentStore;

        public TextFieldStore(
            IAnalyzer analyzer,
            ISynonymMap? synonymMap,
            ITokenEncoder tokenEncoder,
            TextDocumentStore textDocumentStore)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (textDocumentStore is null)
            {
                throw new ArgumentNullException(nameof(textDocumentStore));
            }

            this.analyzer = analyzer;
            this.synonymMap = synonymMap;
            this.tokenEncoder = tokenEncoder;
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
            var matchExpression = Match.Empty;

            try
            {
                using (var tokens = SharedObjectPools.MatchTokens.Lease())
                {
                    foreach (var token in this.analyzer.GetTokens(searchExpression.Text))
                    {
                        var readOnlyToken = this.tokenEncoder.ToReadOnly(token)
                                         ?? this.synonymMap?.ToReadOnly(token)
                                         ?? default;

                        tokens.Instance.Add(readOnlyToken);
                    }

                    matchExpression =
                        searchExpression.SearchMode == SearchMode.All
                            ? Match.All(ScoreAggregation.Sum, tokens.Instance)
                            : Match.Any(ScoreAggregation.Sum, tokens.Instance);
                }

                if (this.synonymMap is not null)
                {
                    if (matchExpression is not EmptyMatchExpression)
                    {
                        matchExpression = this.synonymMap.Process(matchExpression);
                    }
                }

                return this.textDocumentStore.Search(searchExpression.Field, matchExpression, ref documentResultBuilder);
            }
            finally
            {
                matchExpression.Dispose();
            }
        }
    }
}
