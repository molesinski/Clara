namespace Clara.Utils
{
    internal static class SpanHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Justification = "By design")]
        internal static int GetHashCode(ReadOnlySpan<char> span)
        {
#if NET6_0_OR_GREATER
            return string.GetHashCode(span);
#else
            unchecked
            {
                var hash1 = 5381;
                var hash2 = hash1;

                for (var i = 0; i < span.Length && span[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ span[i];

                    if (i == span.Length - 1 || span[i + 1] == '\0')
                    {
                        break;
                    }

                    hash2 = ((hash2 << 5) + hash2) ^ span[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
#endif
        }
    }
}
