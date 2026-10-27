using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class TokenMatching
    {
        public static TokenMatching Default { get; } = new LevenshteinTokenMatching();

        public virtual char Normalize(char value)
        {
            return value;
        }

        public abstract int GetMaximumDistance(int length);

        public abstract int GetMaximumLengthDifference(int length);

        public abstract int CalculateDistance(ReadOnlySpan<char> a, ReadOnlySpan<char> b, int maximumDistance);

        public abstract float GetBoost(int distance, bool isPrefix);

        internal bool TryMatch(string token, string query, out int distance)
        {
            distance = 0;

            if (this.Normalize(token[0]) != this.Normalize(query[0]))
            {
                return false;
            }

            if (Math.Abs(token.Length - query.Length) > this.GetMaximumLengthDifference(query.Length))
            {
                return false;
            }

            var maximumDistance = this.GetMaximumDistance(query.Length);

            distance = this.CalculateDistance(token.AsSpan(), query.AsSpan(), maximumDistance);

            return distance <= maximumDistance;
        }

        internal bool TryMatchPrefix(string token, string query, out int distance, out int length)
        {
            distance = 0;
            length = 0;

            if (this.Normalize(token[0]) != this.Normalize(query[0]))
            {
                return false;
            }

            var maximumDistance = this.GetMaximumDistance(query.Length);
            var maximumLengthDifference = this.GetMaximumLengthDifference(query.Length);
            var minimumLength = Math.Max(1, query.Length - maximumLengthDifference);
            var maximumLength = Math.Min(token.Length, query.Length + maximumLengthDifference);
            var bestDistance = maximumDistance + 1;
            var bestLength = 0;

            for (var i = minimumLength; i <= maximumLength; i++)
            {
                var current = this.CalculateDistance(token.AsSpan(0, i), query.AsSpan(), maximumDistance);

                if (current < bestDistance)
                {
                    bestDistance = current;
                    bestLength = i;
                }
            }

            if (bestDistance > maximumDistance)
            {
                return false;
            }

            distance = bestDistance;
            length = bestLength;

            return true;
        }

        private sealed class LevenshteinTokenMatching : TokenMatching
        {
            public override int GetMaximumDistance(int length)
            {
                return
                    length switch
                    {
                        <= 3 => 0,
                        <= 6 => 1,
                        _ => 2,
                    };
            }

            public override int GetMaximumLengthDifference(int length)
            {
                return this.GetMaximumDistance(length);
            }

            public override int CalculateDistance(ReadOnlySpan<char> a, ReadOnlySpan<char> b, int maximumDistance)
            {
                return Levenshtein.CalculateDistance(a, b, maximumDistance);
            }

            public override float GetBoost(int distance, bool isPrefix)
            {
                var cost = distance + (isPrefix ? 0.2f : 0f);

                return 1f / (1f + cost);
            }
        }
    }
}
