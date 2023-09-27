using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Clara.Analysis
{
    public abstract partial class SnowballResourceStopTokenFilter : StopTokenFilter
    {
        protected SnowballResourceStopTokenFilter(Type type)
            : base(LoadResource(type))
        {
        }

        protected SnowballResourceStopTokenFilter(Type type, Encoding encoding)
            : base(LoadResource(type, encoding))
        {
        }

        protected SnowballResourceStopTokenFilter(Assembly assembly, string resourceName)
            : base(LoadResource(assembly, resourceName))
        {
        }

        protected SnowballResourceStopTokenFilter(Assembly assembly, string resourceName, Encoding encoding)
            : base(LoadResource(assembly, resourceName, encoding))
        {
        }

        protected static IEnumerable<string> LoadResource(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return LoadResource(type.Assembly, $"{type.FullName}.txt", Encoding.UTF8);
        }

        protected static IEnumerable<string> LoadResource(Type type, Encoding encoding)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return LoadResource(type.Assembly, $"{type.FullName}.txt", encoding);
        }

        protected static IEnumerable<string> LoadResource(Assembly assembly, string resourceName)
        {
            return LoadResource(assembly, resourceName, Encoding.UTF8);
        }

        protected static IEnumerable<string> LoadResource(Assembly assembly, string resourceName, Encoding encoding)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var stream = assembly.GetManifestResourceStream(resourceName);

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
