using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Clara.Analysis
{
    public abstract partial class SnowballResourceStopTokenFilter : StopTokenFilter
    {
        protected internal SnowballResourceStopTokenFilter(Assembly assembly, string name, Encoding encoding)
            : base(LoadStopwords(assembly, name, encoding))
        {
        }

        private static IEnumerable<string> LoadStopwords(Assembly assembly, string name, Encoding encoding)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var stream = assembly.GetManifestResourceStream(name);

            if (stream is null)
            {
                throw new InvalidOperationException("Unable to find stopwords resource file in assembly.");
            }

            using var reader = new StreamReader(stream, encoding);

            var stopwords = new HashSet<string>();

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
