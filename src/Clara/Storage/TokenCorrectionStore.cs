using Clara.Analysis;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenCorrectionStore
    {
        private readonly TokenMatching matching;
        private readonly int minimumVariantLength;
        private readonly DictionarySlim<string, int> surfaceTokens;
        private readonly Dictionary<char, SurfaceToken[]> buckets;
        private readonly SurfaceToken[] variants;
        private readonly ObjectPoolSlim<ITokenTermSource> termSourcePool;

        public TokenCorrectionStore(TokenCorrection tokenCorrection, DictionarySlim<string, int> surfaceTokens)
        {
            if (tokenCorrection is null)
            {
                throw new ArgumentNullException(nameof(tokenCorrection));
            }

            if (surfaceTokens is null)
            {
                throw new ArgumentNullException(nameof(surfaceTokens));
            }

            this.matching = tokenCorrection.Matching;
            this.minimumVariantLength = tokenCorrection.MinimumVariantLength;
            this.surfaceTokens = surfaceTokens;
            this.termSourcePool = new ObjectPoolSlim<ITokenTermSource>(tokenCorrection.Analyzer.CreateTokenTermSource);
            this.buckets = CreateBuckets(this.matching, surfaceTokens);
            this.variants = this.CreateVariants(tokenCorrection.Variants);
        }

        public int GetSurfaceTerms(string text, DictionarySlim<int, string> surfaceTerms)
        {
            using var source = this.termSourcePool.Lease();

            var lastPosition = -1;

            foreach (var term in source.Instance.GetTerms(text))
            {
                ref var surface = ref surfaceTerms.GetValueRefOrAddDefault(term.Position.Start, out _);

                surface = term.Token.ToString();

                if (term.Position.Start > lastPosition)
                {
                    lastPosition = term.Position.Start;
                }
            }

            return lastPosition;
        }

        public void GetCandidates(string surface, bool isFuzzy, bool isPrefix, DictionarySlim<int, float> candidates)
        {
            if (surface.Length == 0)
            {
                return;
            }

            if (this.buckets.TryGetValue(this.matching.Normalize(surface[0]), out var bucket))
            {
                var maximumDistance = isFuzzy ? this.matching.GetMaximumDistance(surface.Length) : 0;
                var maximumLengthDifference = isFuzzy ? this.matching.GetMaximumLengthDifference(surface.Length) : 0;
                var minimumLength = Math.Max(1, surface.Length - maximumLengthDifference);
                var maximumLength = surface.Length + maximumLengthDifference;

                for (var i = FindFirst(bucket, minimumLength); i < bucket.Length; i++)
                {
                    var entry = bucket[i];

                    if (!isPrefix && entry.Value.Length > maximumLength)
                    {
                        break;
                    }

                    if (isFuzzy && this.matching.TryMatch(entry.Value, surface, maximumDistance, maximumLengthDifference, out var distance))
                    {
                        AddCandidate(candidates, entry.TokenId, this.matching.GetBoost(distance, isPrefix: false));
                    }

                    if (isPrefix && this.matching.TryMatchPrefix(entry.Value, surface, maximumDistance, maximumLengthDifference, out distance, out _))
                    {
                        AddCandidate(candidates, entry.TokenId, this.matching.GetBoost(distance, isPrefix: true));
                    }
                }
            }

            if (isFuzzy && surface.Length >= this.minimumVariantLength && !this.surfaceTokens.ContainsKey(surface))
            {
                foreach (var variant in this.variants)
                {
                    if (string.Equals(variant.Value, surface, StringComparison.Ordinal))
                    {
                        AddCandidate(candidates, variant.TokenId, this.matching.GetBoost(0, isPrefix: false));
                    }
                    else if (isPrefix && variant.Value.StartsWith(surface, StringComparison.Ordinal))
                    {
                        AddCandidate(candidates, variant.TokenId, this.matching.GetBoost(0, isPrefix: true));
                    }
                }
            }
        }

        private static Dictionary<char, SurfaceToken[]> CreateBuckets(TokenMatching matching, DictionarySlim<string, int> surfaceTokens)
        {
            var lists = new Dictionary<char, List<SurfaceToken>>();

            foreach (var pair in surfaceTokens)
            {
                if (pair.Key.Length == 0)
                {
                    continue;
                }

                var key = matching.Normalize(pair.Key[0]);

                if (!lists.TryGetValue(key, out var list))
                {
                    lists.Add(key, list = new List<SurfaceToken>());
                }

                list.Add(new SurfaceToken(pair.Key, pair.Value));
            }

            var buckets = new Dictionary<char, SurfaceToken[]>(lists.Count);

            foreach (var pair in lists)
            {
                var bucket = pair.Value.ToArray();

                Array.Sort(bucket, SurfaceTokenComparer.Instance);

                buckets.Add(pair.Key, bucket);
            }

            return buckets;
        }

        private static int FindFirst(SurfaceToken[] bucket, int minimumLength)
        {
            var low = 0;
            var high = bucket.Length;

            while (low < high)
            {
                var middle = low + ((high - low) / 2);

                if (bucket[middle].Value.Length < minimumLength)
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

        private static void AddCandidate(DictionarySlim<int, float> candidates, int tokenId, float boost)
        {
            ref var current = ref candidates.GetValueRefOrAddDefault(tokenId, out var exists);

            if (!exists || boost > current)
            {
                current = boost;
            }
        }

        private SurfaceToken[] CreateVariants(IEnumerable<KeyValuePair<string, string>> variants)
        {
            var result = new List<SurfaceToken>();

            using var source = this.termSourcePool.Lease();

            foreach (var pair in variants)
            {
                var variant = GetSingleToken(source.Instance, pair.Key);
                var canonical = GetSingleToken(source.Instance, pair.Value);

                if (variant is null || canonical is null || string.Equals(variant, canonical, StringComparison.Ordinal))
                {
                    continue;
                }

                if (this.surfaceTokens.TryGetValue(canonical, out var tokenId))
                {
                    result.Add(new SurfaceToken(variant, tokenId));
                }
            }

            return result.ToArray();

            static string? GetSingleToken(ITokenTermSource source, string? text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return null;
                }

                var token = default(string);

                foreach (var term in source.GetTerms(text!))
                {
                    if (token is not null)
                    {
                        return null;
                    }

                    token = term.Token.ToString();
                }

                return token;
            }
        }

        private readonly struct SurfaceToken
        {
            public SurfaceToken(string value, int tokenId)
            {
                this.Value = value;
                this.TokenId = tokenId;
            }

            public string Value { get; }

            public int TokenId { get; }
        }

        private sealed class SurfaceTokenComparer : IComparer<SurfaceToken>
        {
            public static SurfaceTokenComparer Instance { get; } = new();

            public int Compare(SurfaceToken x, SurfaceToken y)
            {
                var result = x.Value.Length.CompareTo(y.Value.Length);

                if (result != 0)
                {
                    return result;
                }

                return string.CompareOrdinal(x.Value, y.Value);
            }
        }
    }
}
