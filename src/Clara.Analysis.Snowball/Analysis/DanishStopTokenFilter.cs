namespace Clara.Analysis
{
    public sealed class DanishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public DanishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(DanishStopTokenFilter));
    }
}
