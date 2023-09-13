using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class TextFieldStore : FieldStore
    {
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap synonymMap;
        private readonly TextDocumentStore textDocumentStore;

        public TextFieldStore(
            IAnalyzer analyzer,
            ISynonymMap synonymMap,
            TextDocumentStore textDocumentStore)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (synonymMap is null)
            {
                throw new ArgumentNullException(nameof(synonymMap));
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

        public override void Search(SearchExpression searchExpression, DocumentSet documentSet)
        {
            var tokens = this.analyzer.GetTokens(searchExpression.Text).ToList();

            if (tokens.Count == 0)
            {
                documentSet.Clear();
                return;
            }

            var matchExpression =
                searchExpression.Mode == SearchMode.All
                    ? Match.All(tokens)
                    : Match.Any(tokens);

            matchExpression = this.synonymMap.Process(matchExpression);

            this.textDocumentStore.Search(searchExpression.Field, matchExpression, documentSet);
        }
    }
}
