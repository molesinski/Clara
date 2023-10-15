namespace Clara.Analysis
{
    public sealed class PortugueseStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public PortugueseStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(PortugueseStopTokenFilter));
    }
}
