using System.Text;
using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class PhraseFieldStore : FieldStore
    {
        private const int BaseScore = 1_000_000;
        private const int Penalty = 1;

        private static readonly ObjectPoolSlim<DictionarySlim<int, int>> Scores = new(() => new(), sizeFactor: 2);
        private static readonly ObjectPoolSlim<ListSlim<Candidate>> Candidates = new(() => new(), sizeFactor: 2);

        private readonly TokenMatching matching;
        private readonly HashSet<string> stopwords;
        private readonly int minimumVariantLength;
        private readonly string highlightStart;
        private readonly string highlightEnd;
        private readonly PhraseDocument[] documents;
        private readonly Dictionary<char, TokenDocuments[]> tokenBuckets;
        private readonly TokenDocuments[] variants;
        private readonly HashSet<string> singleTokenPhrases;
        private readonly ObjectPoolSlim<ITokenTermSource> termSourcePool;

        public PhraseFieldStore(PhraseField field, List<PhraseEntry> phrases)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (phrases is null)
            {
                throw new ArgumentNullException(nameof(phrases));
            }

            this.matching = field.Matching;
            this.stopwords = new HashSet<string>(field.Stopwords, StringComparer.Ordinal);
            this.minimumVariantLength = field.MinimumVariantLength;
            this.highlightStart = field.HighlightStart;
            this.highlightEnd = field.HighlightEnd;
            this.termSourcePool = new ObjectPoolSlim<ITokenTermSource>(field.Analyzer.CreateTokenTermSource);

            phrases.Sort(PhraseEntryComparer.Instance);

            var source = field.Analyzer.CreateTokenTermSource();
            var documentIds = new Dictionary<string, int>(phrases.Count, StringComparer.Ordinal);
            var tokenVariants = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            var tokenIndex = new Dictionary<string, DictionarySlim<int, int>>(StringComparer.Ordinal);
            var variantIndex = new Dictionary<string, DictionarySlim<int, int>>(StringComparer.Ordinal);

            this.documents = new PhraseDocument[phrases.Count];
            this.singleTokenPhrases = new HashSet<string>(StringComparer.Ordinal);

            foreach (var pair in field.Variants)
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

            for (var i = 0; i < phrases.Count; i++)
            {
                var phrase = phrases[i];
                var tokens = GetTokens(source, phrase.Text);

                this.documents[i] = new PhraseDocument(phrase.Text, phrase.Weight, tokens, phrase.DocumentIds, phrase.DocumentWeights);
                documentIds.Add(phrase.Text, i);

                this.IndexTokens(i, tokens, tokenIndex, variantIndex, tokenVariants);
            }

            foreach (var pair in field.Aliases)
            {
                if (pair.Key is not null && pair.Value is not null && documentIds.TryGetValue(pair.Key, out var documentId))
                {
                    this.IndexTokens(documentId, GetTokens(source, pair.Value), tokenIndex, variantIndex, tokenVariants);
                }
            }

            this.tokenBuckets = CreateBuckets(this.matching, tokenIndex);
            this.variants = variantIndex.Select(o => new TokenDocuments(o.Key, o.Value)).ToArray();
        }

        public List<SuggestionResult> Suggest(string text, int count, HashSetSlim<int>? allowedDocuments)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return this.SuggestByWeight(count, allowedDocuments);
            }

            using var queryTokensLease = SharedObjectPools.SuggestTokens.Lease();
            using var globalScoresLease = Scores.Lease();
            using var localScoresLease = Scores.Lease();

            var queryTokens = queryTokensLease.Instance;
            var globalScores = globalScoresLease.Instance;
            var localScores = localScoresLease.Instance;
            var queryPosition = 1;
            var matchedTokens = 0;

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

                matchedTokens++;
                localScores.Clear();

                if (!this.stopwords.Contains(queryToken))
                {
                    queryPosition++;
                }
            }

            return this.Rank(globalScores, matchedTokens, queryTokens, count, allowedDocuments);
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

        private static bool TryGetWeight(PhraseDocument document, HashSetSlim<int>? allowedDocuments, out float weight, out int count)
        {
            if (allowedDocuments is null)
            {
                weight = document.Weight;
                count = document.DocumentIds.Length;

                return true;
            }

            weight = 0f;
            count = 0;

            if (document.DocumentIds.Length <= allowedDocuments.Count)
            {
                for (var i = 0; i < document.DocumentIds.Length; i++)
                {
                    if (allowedDocuments.Contains(document.DocumentIds[i]))
                    {
                        weight += document.DocumentWeights[i];
                        count++;
                    }
                }
            }
            else
            {
                foreach (var documentId in allowedDocuments)
                {
                    var index = Array.BinarySearch(document.DocumentIds, documentId);

                    if (index >= 0)
                    {
                        weight += document.DocumentWeights[index];
                        count++;
                    }
                }
            }

            return count > 0;
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

        private List<SuggestionResult> Rank(DictionarySlim<int, int> scores, int matchedTokens, ListSlim<string> queryTokens, int count, HashSetSlim<int>? allowedDocuments)
        {
            using var candidatesLease = Candidates.Lease();
            using var pendingLease = Candidates.Lease();

            var candidates = candidatesLease.Instance;
            var pending = pendingLease.Instance;
            var results = new List<SuggestionResult>(Math.Min(count, scores.Count));
            var builder = new StringBuilder();

            foreach (var pair in scores)
            {
                candidates.Add(new Candidate(pair.Key, pair.Value, 0f, 0));
            }

            candidates.Sort(CandidateComparer.Instance);

            var index = 0;

            while (index < candidates.Count && results.Count < count)
            {
                var groupScore = candidates[index].Score;

                while (index < candidates.Count && candidates[index].Score == groupScore)
                {
                    var documentId = candidates[index].DocumentId;

                    if (TryGetWeight(this.documents[documentId], allowedDocuments, out var weight, out var documentCount))
                    {
                        var score = weight > 0 ? groupScore : groupScore - (Penalty * matchedTokens);

                        pending.Add(new Candidate(documentId, score, weight, documentCount));
                    }

                    index++;
                }

                var nextScore = index < candidates.Count ? candidates[index].Score : int.MinValue;

                pending.Sort(CandidateComparer.Instance);

                var emitted = 0;

                while (emitted < pending.Count && pending[emitted].Score > nextScore && results.Count < count)
                {
                    results.Add(this.CreateResult(builder, pending[emitted], queryTokens));
                    emitted++;
                }

                if (emitted > 0)
                {
                    var remaining = pending.Count - emitted;

                    for (var i = 0; i < remaining; i++)
                    {
                        pending[i] = pending[emitted + i];
                    }

                    while (pending.Count > remaining)
                    {
                        pending.RemoveAt(pending.Count - 1);
                    }
                }
            }

            return results;
        }

        private SuggestionResult CreateResult(StringBuilder builder, Candidate candidate, ListSlim<string> queryTokens)
        {
            var document = this.documents[candidate.DocumentId];

            return new SuggestionResult(document.Text, this.BuildHighlightedText(builder, document.Tokens, queryTokens), candidate.Score, candidate.Weight, candidate.Count);
        }

        private List<SuggestionResult> SuggestByWeight(int count, HashSetSlim<int>? allowedDocuments)
        {
            var results = new List<SuggestionResult>(Math.Min(count, this.documents.Length));

            if (allowedDocuments is null)
            {
                for (var i = 0; i < this.documents.Length && results.Count < count; i++)
                {
                    results.Add(new SuggestionResult(this.documents[i].Text, this.documents[i].Text, 0, this.documents[i].Weight, this.documents[i].DocumentIds.Length));
                }

                return results;
            }

            using var selectedLease = Candidates.Lease();

            var selected = selectedLease.Instance;

            for (var i = 0; i < this.documents.Length; i++)
            {
                if (selected.Count == count && this.documents[i].Weight <= selected[selected.Count - 1].Weight)
                {
                    break;
                }

                if (!TryGetWeight(this.documents[i], allowedDocuments, out var weight, out var documentCount))
                {
                    continue;
                }

                if (selected.Count == count)
                {
                    if (weight <= selected[selected.Count - 1].Weight)
                    {
                        continue;
                    }

                    selected.RemoveAt(selected.Count - 1);
                }

                var position = selected.Count;

                while (position > 0 && selected[position - 1].Weight < weight)
                {
                    position--;
                }

                selected.Insert(position, new Candidate(i, 0, weight, documentCount));
            }

            foreach (var candidate in selected)
            {
                results.Add(new SuggestionResult(this.documents[candidate.DocumentId].Text, this.documents[candidate.DocumentId].Text, 0, candidate.Weight, candidate.Count));
            }

            return results;
        }

        private void IndexTokens(
            int documentId,
            string[] tokens,
            Dictionary<string, DictionarySlim<int, int>> tokenIndex,
            Dictionary<string, DictionarySlim<int, int>> variantIndex,
            Dictionary<string, List<string>> tokenVariants)
        {
            if (tokens.Length == 1)
            {
                this.singleTokenPhrases.Add(tokens[0]);
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
            var score = BaseScore;

            score -= queryPosition;
            score -= documentPosition;
            score -= this.documents[documentId].Tokens.Length;
            score -= distance > 0 || isPrefix ? Penalty : 0;
            score -= distance;
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

        internal readonly struct PhraseEntry
        {
            public PhraseEntry(string text, float weight, int[] documentIds, float[] documentWeights)
            {
                this.Text = text;
                this.Weight = weight;
                this.DocumentIds = documentIds;
                this.DocumentWeights = documentWeights;
            }

            public string Text { get; }

            public float Weight { get; }

            public int[] DocumentIds { get; }

            public float[] DocumentWeights { get; }
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

        private readonly struct Candidate
        {
            public Candidate(int documentId, int score, float weight, int count)
            {
                this.DocumentId = documentId;
                this.Score = score;
                this.Weight = weight;
                this.Count = count;
            }

            public int DocumentId { get; }

            public int Score { get; }

            public float Weight { get; }

            public int Count { get; }
        }

        private sealed class PhraseDocument
        {
            public PhraseDocument(string text, float weight, string[] tokens, int[] documentIds, float[] documentWeights)
            {
                this.Text = text;
                this.Weight = weight;
                this.Tokens = tokens;
                this.DocumentIds = documentIds;
                this.DocumentWeights = documentWeights;
            }

            public string Text { get; }

            public float Weight { get; }

            public string[] Tokens { get; }

            public int[] DocumentIds { get; }

            public float[] DocumentWeights { get; }
        }

        private sealed class PhraseEntryComparer : IComparer<PhraseEntry>
        {
            public static PhraseEntryComparer Instance { get; } = new();

            public int Compare(PhraseEntry x, PhraseEntry y)
            {
                var result = y.Weight.CompareTo(x.Weight);

                if (result != 0)
                {
                    return result;
                }

                return string.CompareOrdinal(x.Text, y.Text);
            }
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

        private sealed class CandidateComparer : IComparer<Candidate>
        {
            public static CandidateComparer Instance { get; } = new();

            public int Compare(Candidate x, Candidate y)
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
    }
}
