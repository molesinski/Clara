using System.Text;

namespace Clara.Analysis
{
    public abstract partial class SnowballResourceStopTokenFilter<TFilter> : SnowballStopTokenFilter
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

            using var textReader = new StreamReader(stream, encoding);

            return ParseSnowball(textReader);
        }
    }
}
