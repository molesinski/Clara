using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class TextFieldStore : FieldStore
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap? synonymMap;
        private readonly TextDocumentStore textDocumentStore;

        public TextFieldStore(
            TokenEncoder tokenEncoder,
            IAnalyzer analyzer,
            ISynonymMap? synonymMap,
            TextDocumentStore textDocumentStore)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (textDocumentStore is null)
            {
                throw new ArgumentNullException(nameof(textDocumentStore));
            }

            this.tokenEncoder = tokenEncoder;
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
            using var terms = SharedObjectPools.SearchTerms.Lease();
            var hasInvalid = false;

            foreach (var term in this.analyzer.GetTerms(searchExpression.Text))
            {
                var readOnlyToken = this.tokenEncoder.ToReadOnly(term.Token) ?? this.synonymMap?.ToReadOnly(term.Token);

                if (readOnlyToken is null)
                {
                    hasInvalid = true;
                    continue;
                }

                terms.Instance.Add(new SearchTerm(term.Ordinal, readOnlyToken));
            }

            if (hasInvalid)
            {
                if (searchExpression.SearchMode == SearchMode.All || terms.Instance.Count == 0)
                {
                    documentResultBuilder.Clear();

                    return default;
                }
            }

            if (this.synonymMap is not null)
            {
                if (terms.Instance.Count > 0)
                {
                    this.synonymMap.Process(searchExpression.SearchMode, terms.Instance);
                }
            }

            return this.textDocumentStore.Search(searchExpression.SearchMode, terms.Instance, ref documentResultBuilder);
        }
    }
}
