namespace Clara.Analysis
{
    public sealed class FinnishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public FinnishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(FinnishStopTokenFilter));
    }
}
