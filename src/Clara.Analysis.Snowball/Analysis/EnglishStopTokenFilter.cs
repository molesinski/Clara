namespace Clara.Analysis
{
    public sealed class EnglishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public EnglishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(EnglishStopTokenFilter));
    }
}
