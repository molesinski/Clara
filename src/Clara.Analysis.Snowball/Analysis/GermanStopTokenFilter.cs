namespace Clara.Analysis
{
    public sealed class GermanStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public GermanStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(GermanStopTokenFilter));
    }
}
