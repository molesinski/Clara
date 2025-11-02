namespace Clara.Analysis
{
    public sealed class PolishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public PolishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(PolishStopTokenFilter));
    }
}
