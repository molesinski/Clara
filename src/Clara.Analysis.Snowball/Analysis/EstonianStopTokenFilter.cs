namespace Clara.Analysis
{
    public sealed class EstonianStopTokenFilter : ResourceStopTokenFilter
    {
        public EstonianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(EstonianStopTokenFilter));
    }
}
