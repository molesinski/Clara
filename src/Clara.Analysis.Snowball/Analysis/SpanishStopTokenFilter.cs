namespace Clara.Analysis
{
    public sealed class SpanishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public SpanishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(SpanishStopTokenFilter));
    }
}
