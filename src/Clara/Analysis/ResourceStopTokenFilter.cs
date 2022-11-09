using System.Reflection;
using System.Text;

namespace Clara.Analysis
{
    public abstract class ResourceStopTokenFilter : StopTokenFilter
    {
        protected ResourceStopTokenFilter(Assembly assembly, string name, Encoding encoding)
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

            var stopwords = new List<string>();

            while (reader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                {
                    continue;
                }

                stopwords.Add(line.Trim());
            }

            return stopwords;
        }
    }
}
