using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Clara.Analysis
{
    public abstract class SnowballResourceStopTokenFilter : StopTokenFilter
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
#pragma warning disable CA1307 // Specify StringComparison for clarity
                var commentIndex = line.IndexOf('|');
#pragma warning restore CA1307 // Specify StringComparison for clarity

                if (commentIndex >= 0)
                {
                    line = line.Substring(0, commentIndex);
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var words = Regex.Split(line, "\\s+", RegexOptions.Compiled);

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
    }
}
