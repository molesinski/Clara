namespace Clara.Analysis
{
    public sealed class ArabicStopTokenFilter : ResourceStopTokenFilter
    {
        public ArabicStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(ArabicStopTokenFilter));
    }
}
