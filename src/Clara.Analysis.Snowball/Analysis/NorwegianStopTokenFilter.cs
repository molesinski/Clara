namespace Clara.Analysis
{
    public sealed class NorwegianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public NorwegianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(NorwegianStopTokenFilter));
    }
}
