using System.Text;
using Clara.Utils;

namespace Clara.Analysis
{
    public abstract class ResourceStopTokenFilter<TFilter> : StopTokenFilter
        where TFilter : ResourceStopTokenFilter<TFilter>
    {
        protected ResourceStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Intended for use via non-generic subclass")]
        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource();

        private static IReadOnlyCollection<string> LoadResource()
        {
            var type = typeof(TFilter);
            var assembly = type.Assembly;
            var resourceName = $"{type.FullName}.txt";
            var encoding = Encoding.UTF8;

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream is null)
            {
                throw new InvalidOperationException($"Unable to find stopwords resource '{resourceName}' in assembly '{assembly.FullName}'.");
            }

            using var reader = new StreamReader(stream, encoding);

            var stopwords = new HashSetSlim<string>();

            while (reader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                {
                    continue;
                }

                var word = line.Trim();

                if (!string.IsNullOrWhiteSpace(word))
                {
                    stopwords.Add(word);
                }
            }

            return stopwords;
        }
    }
}
