using System.Text;
using System.Text.RegularExpressions;
using Clara.Utils;

namespace Clara.Analysis
{
    public abstract partial class SnowballResourceStopTokenFilter<TFilter> : StopTokenFilter
        where TFilter : SnowballResourceStopTokenFilter<TFilter>
    {
        protected SnowballResourceStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Intended access via non-generic subclass")]
        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource();

        protected static IReadOnlyCollection<string> LoadResource()
        {
            var type = typeof(TFilter);
            var assembly = type.Assembly;
            var resourceName = $"{type.FullName}.txt";
            var encoding = Encoding.UTF8;

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream is null)
            {
                throw new InvalidOperationException("Unable to find stopwords resource file in assembly.");
            }

            using var reader = new StreamReader(stream, encoding);

            var stopwords = new HashSetSlim<string>();

            while (reader.ReadLine() is string line)
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
                        stopwords.Add(word);
                    }
                }
            }

            return stopwords;
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
