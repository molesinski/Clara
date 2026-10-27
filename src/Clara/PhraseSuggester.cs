using System.Text;
using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara
{
    public sealed class PhraseSuggester
    {
        private const int BaseScore = 1_000_000;
        private const int Penalty = 1;

        private static readonly ObjectPoolSlim<DictionarySlim<int, int>> Scores = new(() => new(), sizeFactor: 2);
        private static readonly ObjectPoolSlim<ListSlim<ScoredPhrase>> ScoredPhrases = new(() => new());

        private readonly Index index;
        private readonly TokenMatching matching;
        private readonly HashSet<string> stopwords;
        private readonly int minimumVariantLength;
        private readonly string highlightStart;
        private readonly string highlightEnd;
        private readonly PhraseDocument[] documents;
        private readonly int[] weightOrder;
        private readonly Dictionary<char, TokenDocuments[]> tokenBuckets;
        private readonly TokenDocuments[] variants;
        private readonly HashSet<string> singleTokenPhrases;
        private readonly ObjectPoolSlim<ITokenTermSource> termSourcePool;

        internal PhraseSuggester(
            Index index,
            IAnalyzer analyzer,
            TokenMatching matching,
            HashSet<string> stopwords,
            int minimumVariantLength,
            string highlightStart,
            string highlightEnd,
            PhraseDocument[] documents,
            Dictionary<string, DictionarySlim<int, int>> tokenIndex,
            Dictionary<string, DictionarySlim<int, int>> variantIndex,
            HashSet<string> singleTokenPhrases)
        {
            this.index = index;
            this.matching = matching;
            this.stopwords = stopwords;
            this.minimumVariantLength = minimumVariantLength;
            this.highlightStart = highlightStart;
            this.highlightEnd = highlightEnd;
            this.documents = documents;
            this.singleTokenPhrases = singleTokenPhrases;
            this.termSourcePool = new ObjectPoolSlim<ITokenTermSource>(analyzer.CreateTokenTermSource);
            this.tokenBuckets = CreateBuckets(matching, tokenIndex);
            this.variants = variantIndex.Select(o => new TokenDocuments(o.Key, o.Value)).ToArray();
            this.weightOrder = Enumerable.Range(0, documents.Length).ToArray();

            Array.Sort(this.weightOrder, new WeightComparer(documents));
        }

        public int Count
        {
            get
            {
                return this.documents.Length;
            }
        }

        internal Index Index
        {
            get
            {
                return this.index;
            }
        }

        internal IReadOnlyList<SuggestionResult> Suggest(string text, int count, HashSetSlim<int>? allowedDocuments)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return this.SuggestByWeight(count, allowedDocuments);
            }

            using var queryTokensLease = SharedObjectPools.SuggestTokens.Lease();
            using var globalScoresLease = Scores.Lease();
            using var localScoresLease = Scores.Lease();
            using var scoredLease = ScoredPhrases.Lease();

            var queryTokens = queryTokensLease.Instance;
            var globalScores = globalScoresLease.Instance;
            var localScores = localScoresLease.Instance;
            var queryPosition = 1;

            using (var source = this.termSourcePool.Lease())
            {
                foreach (var term in source.Instance.GetTerms(text))
                {
                    queryTokens.Add(term.Token.ToString());
                }
            }

            for (var i = 0; i < queryTokens.Count; i++)
            {
                var queryToken = queryTokens[i];
                var isPrefix = i == queryTokens.Count - 1 && !char.IsWhiteSpace(text[text.Length - 1]);

                this.MatchTokens(queryToken, queryPosition, isPrefix, localScores);
                this.MatchVariants(queryToken, queryPosition, isPrefix, localScores);

                if (globalScores.Count == 0)
                {
                    UnionSum(globalScores, localScores);
                }
                else
                {
                    IntersectSum(globalScores, localScores);
                }

                if (globalScores.Count == 0)
                {
                    break;
                }

                localScores.Clear();

                if (!this.stopwords.Contains(queryToken))
                {
                    queryPosition++;
                }
            }

            var scored = scoredLease.Instance;

            foreach (var pair in globalScores)
            {
                scored.Add(new ScoredPhrase(pair.Key, pair.Value, this.documents[pair.Key].Weight));
            }

            scored.Sort(ScoredPhraseComparer.Instance);

            var results = new List<SuggestionResult>(Math.Min(count, scored.Count));
            var builder = default(StringBuilder);

            foreach (var item in scored)
            {
                var document = this.documents[item.DocumentId];

                if (allowedDocuments is not null && !Intersects(document.Postings, allowedDocuments))
                {
                    continue;
                }

                builder ??= new StringBuilder();

                results.Add(new SuggestionResult(document.Text, this.BuildHighlightedText(builder, document.Tokens, queryTokens), item.Score, document.Weight));

                if (results.Count == count)
                {
                    break;
                }
            }

            return results;
        }

        private static Dictionary<char, TokenDocuments[]> CreateBuckets(TokenMatching matching, Dictionary<string, DictionarySlim<int, int>> tokenIndex)
        {
            var lists = new Dictionary<char, List<TokenDocuments>>();

            foreach (var pair in tokenIndex)
            {
                var key = matching.Normalize(pair.Key[0]);

                if (!lists.TryGetValue(key, out var list))
                {
                    lists.Add(key, list = new List<TokenDocuments>());
                }

                list.Add(new TokenDocuments(pair.Key, pair.Value));
            }

            var buckets = new Dictionary<char, TokenDocuments[]>(lists.Count);

            foreach (var pair in lists)
            {
                var bucket = pair.Value.ToArray();

                Array.Sort(bucket, TokenDocumentsComparer.Instance);

                buckets.Add(pair.Key, bucket);
            }

            return buckets;
        }

        private static int FindFirst(TokenDocuments[] bucket, int minimumLength)
        {
            var low = 0;
            var high = bucket.Length;

            while (low < high)
            {
                var middle = low + ((high - low) / 2);

                if (bucket[middle].Token.Length < minimumLength)
                {
                    low = middle + 1;
                }
                else
                {
                    high = middle;
                }
            }

            return low;
        }

        private static bool Intersects(int[] postings, HashSetSlim<int> allowedDocuments)
        {
            if (postings.Length <= allowedDocuments.Count)
            {
                foreach (var documentId in postings)
                {
                    if (allowedDocuments.Contains(documentId))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (var documentId in allowedDocuments)
                {
                    if (Array.BinarySearch(postings, documentId) >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void UnionMax(DictionarySlim<int, int> scores, int documentId, int score)
        {
            ref var current = ref scores.GetValueRefOrAddDefault(documentId, out var exists);

            if (!exists || score > current)
            {
                current = score;
            }
        }

        private static void UnionSum(DictionarySlim<int, int> target, DictionarySlim<int, int> source)
        {
            foreach (var pair in source)
            {
                ref var current = ref target.GetValueRefOrAddDefault(pair.Key, out _);

                current += pair.Value;
            }
        }

        private static void IntersectSum(DictionarySlim<int, int> target, DictionarySlim<int, int> source)
        {
            using var removedLease = SharedObjectPools.Documents.Lease();

            var removed = removedLease.Instance;

            foreach (var pair in target)
            {
                if (!source.ContainsKey(pair.Key))
                {
                    removed.Add(pair.Key);
                }
            }

            foreach (var documentId in removed)
            {
                target.Remove(documentId);
            }

            foreach (var pair in source)
            {
                if (target.ContainsKey(pair.Key))
                {
                    ref var current = ref target.GetValueRefOrAddDefault(pair.Key, out _);

                    current += pair.Value;
                }
            }
        }

        private List<SuggestionResult> SuggestByWeight(int count, HashSetSlim<int>? allowedDocuments)
        {
            var results = new List<SuggestionResult>(Math.Min(count, this.documents.Length));

            foreach (var documentId in this.weightOrder)
            {
                var document = this.documents[documentId];

                if (allowedDocuments is not null && !Intersects(document.Postings, allowedDocuments))
                {
                    continue;
                }

                results.Add(new SuggestionResult(document.Text, document.Text, 0, document.Weight));

                if (results.Count == count)
                {
                    break;
                }
            }

            return results;
        }

        private void MatchTokens(string queryToken, int queryPosition, bool isPrefix, DictionarySlim<int, int> scores)
        {
            if (!this.tokenBuckets.TryGetValue(this.matching.Normalize(queryToken[0]), out var bucket))
            {
                return;
            }

            var maximumLengthDifference = this.matching.GetMaximumLengthDifference(queryToken.Length);
            var minimumLength = Math.Max(1, queryToken.Length - maximumLengthDifference);
            var maximumLength = queryToken.Length + maximumLengthDifference;

            for (var i = FindFirst(bucket, minimumLength); i < bucket.Length; i++)
            {
                var entry = bucket[i];

                if (!isPrefix && entry.Token.Length > maximumLength)
                {
                    break;
                }

                var isStopword = this.stopwords.Contains(entry.Token);

                if (this.matching.TryMatch(entry.Token, queryToken, out var distance))
                {
                    foreach (var pair in entry.Documents)
                    {
                        UnionMax(scores, pair.Key, this.ComputeScore(pair.Key, queryPosition, pair.Value, isStopword, distance, isPrefix: false));
                    }
                }

                if (isPrefix)
                {
                    if (this.matching.TryMatchPrefix(entry.Token, queryToken, out distance, out _))
                    {
                        foreach (var pair in entry.Documents)
                        {
                            UnionMax(scores, pair.Key, this.ComputeScore(pair.Key, queryPosition, pair.Value, isStopword, distance, isPrefix: true));
                        }
                    }
                }
            }
        }

        private void MatchVariants(string queryToken, int queryPosition, bool isPrefix, DictionarySlim<int, int> scores)
        {
            if (queryToken.Length < this.minimumVariantLength || this.singleTokenPhrases.Contains(queryToken))
            {
                return;
            }

            foreach (var variant in this.variants)
            {
                var isVariantPrefix = false;

                if (!string.Equals(variant.Token, queryToken, StringComparison.Ordinal))
                {
                    if (!(isPrefix && variant.Token.StartsWith(queryToken, StringComparison.Ordinal)))
                    {
                        continue;
                    }

                    isVariantPrefix = true;
                }

                foreach (var pair in variant.Documents)
                {
                    UnionMax(scores, pair.Key, this.ComputeScore(pair.Key, queryPosition, pair.Value, isStopword: false, distance: 0, isPrefix: isVariantPrefix));
                }
            }
        }

        private int ComputeScore(int documentId, int queryPosition, int documentPosition, bool isStopword, int distance, bool isPrefix)
        {
            var document = this.documents[documentId];
            var score = BaseScore;

            score -= queryPosition;
            score -= documentPosition;
            score -= document.Tokens.Length;
            score -= distance > 0 || isPrefix ? Penalty : 0;
            score -= distance;
            score -= !(document.Weight > 0) ? Penalty : 0;
            score -= isPrefix ? Penalty : 0;
            score -= isStopword ? Penalty : 0;

            return score;
        }

        private string BuildHighlightedText(StringBuilder builder, string[] documentTokens, ListSlim<string> queryTokens)
        {
            using var bestMatchesLease = SharedObjectPools.TokenCounts.Lease();
            using var bestLengthsLease = SharedObjectPools.TokenCounts.Lease();

            var bestMatches = bestMatchesLease.Instance;
            var bestLengths = bestLengthsLease.Instance;

            builder.Clear();

            foreach (var queryToken in queryTokens)
            {
                var bestIndex = -1;
                var bestDistance = int.MaxValue;
                var bestLength = int.MinValue;

                for (var i = 0; i < documentTokens.Length; i++)
                {
                    if (this.matching.TryMatchPrefix(documentTokens[i], queryToken, out var distance, out var length))
                    {
                        if (distance < bestDistance)
                        {
                            bestIndex = i;
                            bestDistance = distance;
                            bestLength = length;
                        }
                    }
                }

                if (bestLength > 0)
                {
                    if (!bestMatches.TryGetValue(bestIndex, out var currentDistance) || currentDistance > bestDistance)
                    {
                        ref var matchDistance = ref bestMatches.GetValueRefOrAddDefault(bestIndex, out _);
                        ref var matchLength = ref bestLengths.GetValueRefOrAddDefault(bestIndex, out _);

                        matchDistance = bestDistance;
                        matchLength = bestLength;
                    }
                }
            }

            for (var i = 0; i < documentTokens.Length; i++)
            {
                if (builder.Length > 0)
                {
                    builder.Append(' ');
                }

                var documentToken = documentTokens[i];

                if (bestLengths.TryGetValue(i, out var length))
                {
                    builder.Append(documentToken, 0, length);

                    if (length < documentToken.Length)
                    {
                        builder.Append(this.highlightStart);
                        builder.Append(documentToken, length, documentToken.Length - length);
                        builder.Append(this.highlightEnd);
                    }
                }
                else
                {
                    builder.Append(this.highlightStart);
                    builder.Append(documentToken);
                    builder.Append(this.highlightEnd);
                }
            }

            return builder.ToString();
        }

        private readonly struct TokenDocuments
        {
            public TokenDocuments(string token, DictionarySlim<int, int> documents)
            {
                this.Token = token;
                this.Documents = documents;
            }

            public string Token { get; }

            public DictionarySlim<int, int> Documents { get; }
        }

        private readonly struct ScoredPhrase
        {
            public ScoredPhrase(int documentId, int score, float weight)
            {
                this.DocumentId = documentId;
                this.Score = score;
                this.Weight = weight;
            }

            public int DocumentId { get; }

            public int Score { get; }

            public float Weight { get; }
        }

        internal sealed class PhraseDocument
        {
            public PhraseDocument(string text, float weight, string[] tokens, int[] postings)
            {
                this.Text = text;
                this.Weight = weight;
                this.Tokens = tokens;
                this.Postings = postings;
            }

            public string Text { get; }

            public float Weight { get; }

            public string[] Tokens { get; }

            public int[] Postings { get; }
        }

        private sealed class TokenDocumentsComparer : IComparer<TokenDocuments>
        {
            public static TokenDocumentsComparer Instance { get; } = new();

            public int Compare(TokenDocuments x, TokenDocuments y)
            {
                var result = x.Token.Length.CompareTo(y.Token.Length);

                if (result != 0)
                {
                    return result;
                }

                return string.CompareOrdinal(x.Token, y.Token);
            }
        }

        private sealed class ScoredPhraseComparer : IComparer<ScoredPhrase>
        {
            public static ScoredPhraseComparer Instance { get; } = new();

            public int Compare(ScoredPhrase x, ScoredPhrase y)
            {
                var result = y.Score.CompareTo(x.Score);

                if (result != 0)
                {
                    return result;
                }

                result = y.Weight.CompareTo(x.Weight);

                if (result != 0)
                {
                    return result;
                }

                return x.DocumentId.CompareTo(y.DocumentId);
            }
        }

        private sealed class WeightComparer : IComparer<int>
        {
            private readonly PhraseDocument[] documents;

            public WeightComparer(PhraseDocument[] documents)
            {
                this.documents = documents;
            }

            public int Compare(int x, int y)
            {
                var result = this.documents[y].Weight.CompareTo(this.documents[x].Weight);

                if (result != 0)
                {
                    return result;
                }

                return x.CompareTo(y);
            }
        }
    }
}
