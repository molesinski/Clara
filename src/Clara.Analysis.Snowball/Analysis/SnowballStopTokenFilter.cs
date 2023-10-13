using System.Text.RegularExpressions;
using Clara.Utils;

namespace Clara.Analysis
{
    public abstract partial class SnowballStopTokenFilter : StopTokenFilter
    {
        protected SnowballStopTokenFilter(IEnumerable<string> stopwords)
            : base(stopwords)
        {
        }

        public static IReadOnlyCollection<string> ParseSnowball(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<string>();
            }

            using var stringReader = new StringReader(text);

            return Parse(stringReader);
        }

        public static IReadOnlyCollection<string> ParseSnowball(TextReader textReader)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            var result = new HashSetSlim<string>();

            while (textReader.ReadLine() is string line)
            {
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
                var commentIndex = line.IndexOf('|', StringComparison.Ordinal);
#else
                var commentIndex = line.IndexOf('|');
#endif

                if (commentIndex >= 0)
                {
                    line = line.Substring(0, commentIndex);
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var words = Split(line);

                foreach (var word in words)
                {
                    if (!string.IsNullOrWhiteSpace(word))
                    {
                        result.Add(word);
                    }
                }
            }

            return result;
        }

#if NET7_0_OR_GREATER
        private static IEnumerable<string> Split(string text)
        {
            return WhitespaceRegex().Split(text);
        }

        [GeneratedRegex("\\s+", RegexOptions.Compiled)]
        private static partial Regex WhitespaceRegex();
#else
        private static IEnumerable<string> Split(string text)
        {
            return Regex.Split(text, "\\s+", RegexOptions.Compiled);
        }
#endif
    }
}
