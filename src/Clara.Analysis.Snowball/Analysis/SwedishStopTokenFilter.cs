namespace Clara.Analysis
{
    public sealed class SwedishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public SwedishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(SwedishStopTokenFilter));
    }
}
