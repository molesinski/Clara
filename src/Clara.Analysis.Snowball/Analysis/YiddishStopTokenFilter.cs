namespace Clara.Analysis
{
    public sealed class YiddishStopTokenFilter : ResourceStopTokenFilter
    {
        public YiddishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(YiddishStopTokenFilter));
    }
}
