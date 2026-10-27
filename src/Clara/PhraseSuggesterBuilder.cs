using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara
{
    public sealed class PhraseSuggesterBuilder
    {
        private readonly Index index;
        private readonly TextField field;
        private readonly IAnalyzer analyzer;
        private readonly TokenMatching matching;
        private readonly HashSet<string> stopwords;
        private readonly List<KeyValuePair<string, string>> variants;
        private readonly int minimumVariantLength;
        private readonly bool removeUnmatched;
        private readonly string highlightStart;
        private readonly string highlightEnd;
        private readonly List<PendingPhrase> phrases;
        private bool isBuilt;

        public PhraseSuggesterBuilder(
            Index index,
            TextField field,
            IAnalyzer analyzer,
            TokenMatching? matching = null,
            IEnumerable<string>? stopwords = null,
            IEnumerable<KeyValuePair<string, string>>? variants = null,
            int minimumVariantLength = TokenCorrection.DefaultMinimumVariantLength,
            bool removeUnmatched = true,
            string highlightStart = "<b>",
            string highlightEnd = "</b>")
        {
            if (index is null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (minimumVariantLength < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumVariantLength));
            }

            if (highlightStart is null)
            {
                throw new ArgumentNullException(nameof(highlightStart));
            }

            if (highlightEnd is null)
            {
                throw new ArgumentNullException(nameof(highlightEnd));
            }

            index.GetFieldStore(field);

            this.index = index;
            this.field = field;
            this.analyzer = analyzer;
            this.matching = matching ?? TokenMatching.Default;
            this.stopwords = new HashSet<string>(stopwords ?? Enumerable.Empty<string>(), StringComparer.Ordinal);
            this.variants = variants?.ToList() ?? new List<KeyValuePair<string, string>>();
            this.minimumVariantLength = minimumVariantLength;
            this.removeUnmatched = removeUnmatched;
            this.highlightStart = highlightStart;
            this.highlightEnd = highlightEnd;
            this.phrases = new List<PendingPhrase>();
        }

        public void Add(string text, float weight, IEnumerable<string>? aliases = null)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.phrases.Add(new PendingPhrase(text, weight, aliases?.Where(o => o is not null).ToArray() ?? Array.Empty<string>()));
        }

        public PhraseSuggester Build()
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var source = this.analyzer.CreateTokenTermSource();
            var tokenVariants = new Dictionary<string, List<string>>(StringComparer.Ordinal);

            foreach (var pair in this.variants)
            {
                if (pair.Key is null || pair.Value is null)
                {
                    continue;
                }

                if (!tokenVariants.TryGetValue(pair.Value, out var list))
                {
                    tokenVariants.Add(pair.Value, list = new List<string>());
                }

                list.Add(pair.Key);
            }

            var documents = new List<PhraseSuggester.PhraseDocument>();
            var tokenIndex = new Dictionary<string, DictionarySlim<int, int>>(StringComparer.Ordinal);
            var variantIndex = new Dictionary<string, DictionarySlim<int, int>>(StringComparer.Ordinal);
            var singleTokenPhrases = new HashSet<string>(StringComparer.Ordinal);

            foreach (var phrase in this.phrases)
            {
                if (string.IsNullOrWhiteSpace(phrase.Text))
                {
                    continue;
                }

                var postings = this.GetPostings(phrase.Text);

                if (this.removeUnmatched && postings.Length == 0)
                {
                    continue;
                }

                var documentId = documents.Count;
                var tokens = GetTokens(source, phrase.Text);

                documents.Add(new PhraseSuggester.PhraseDocument(phrase.Text, phrase.Weight, tokens, postings));

                this.IndexTokens(documentId, tokens, tokenIndex, variantIndex, tokenVariants, singleTokenPhrases);

                foreach (var alias in phrase.Aliases)
                {
                    this.IndexTokens(documentId, GetTokens(source, alias), tokenIndex, variantIndex, tokenVariants, singleTokenPhrases);
                }
            }

            var suggester =
                new PhraseSuggester(
                    this.index,
                    this.analyzer,
                    this.matching,
                    this.stopwords,
                    this.minimumVariantLength,
                    this.highlightStart,
                    this.highlightEnd,
                    documents.ToArray(),
                    tokenIndex,
                    variantIndex,
                    singleTokenPhrases);

            this.isBuilt = true;

            return suggester;
        }

        private static string[] GetTokens(ITokenTermSource source, string text)
        {
            var tokens = new List<string>();

            foreach (var term in source.GetTerms(text))
            {
                tokens.Add(term.Token.ToString());
            }

            return tokens.ToArray();
        }

        private int[] GetPostings(string text)
        {
            using var scoring = new TextSearchExpression(this.field, SearchMode.All, text).Search(this.index);

            var postings = new int[scoring.Value.Count];
            var i = 0;

            foreach (var pair in scoring.Value)
            {
                postings[i++] = pair.Key;
            }

            Array.Sort(postings);

            return postings;
        }

        private void IndexTokens(
            int documentId,
            string[] tokens,
            Dictionary<string, DictionarySlim<int, int>> tokenIndex,
            Dictionary<string, DictionarySlim<int, int>> variantIndex,
            Dictionary<string, List<string>> tokenVariants,
            HashSet<string> singleTokenPhrases)
        {
            if (tokens.Length == 1)
            {
                singleTokenPhrases.Add(tokens[0]);
            }

            var position = 1;

            foreach (var token in tokens)
            {
                AddPosition(tokenIndex, token, documentId, position);

                if (tokenVariants.TryGetValue(token, out var variants))
                {
                    foreach (var variant in variants)
                    {
                        AddPosition(variantIndex, variant, documentId, position);
                    }
                }

                if (!this.stopwords.Contains(token))
                {
                    position++;
                }
            }

            static void AddPosition(Dictionary<string, DictionarySlim<int, int>> index, string token, int documentId, int position)
            {
                if (!index.TryGetValue(token, out var documents))
                {
                    index.Add(token, documents = new DictionarySlim<int, int>());
                }

                ref var current = ref documents.GetValueRefOrAddDefault(documentId, out var exists);

                if (!exists || position < current)
                {
                    current = position;
                }
            }
        }

        private readonly struct PendingPhrase
        {
            public PendingPhrase(string text, float weight, string[] aliases)
            {
                this.Text = text;
                this.Weight = weight;
                this.Aliases = aliases;
            }

            public string Text { get; }

            public float Weight { get; }

            public string[] Aliases { get; }
        }
    }
}
