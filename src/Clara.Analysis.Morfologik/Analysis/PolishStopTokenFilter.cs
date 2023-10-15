namespace Clara.Analysis
{
    public sealed class PolishStopTokenFilter : ResourceStopTokenFilter
    {
        public PolishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(PolishStopTokenFilter));
    }
}
