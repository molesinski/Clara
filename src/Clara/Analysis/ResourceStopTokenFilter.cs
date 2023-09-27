using System.Text;

namespace Clara.Analysis
{
    public abstract class ResourceStopTokenFilter<TFilter> : StopTokenFilter
        where TFilter : ResourceStopTokenFilter<TFilter>
    {
        protected ResourceStopTokenFilter()
            : base(Stopwords)
        {
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static IReadOnlyCollection<string> Stopwords { get; } = LoadResource();
#pragma warning restore CA1000 // Do not declare static members on generic types

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

            var stopwords = new HashSet<string>();

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
