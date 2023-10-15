namespace Clara.Analysis
{
    public sealed class BasqueStopTokenFilter : ResourceStopTokenFilter
    {
        public BasqueStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(BasqueStopTokenFilter));
    }
}
