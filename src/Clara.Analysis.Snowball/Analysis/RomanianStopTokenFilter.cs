namespace Clara.Analysis
{
    public sealed class RomanianStopTokenFilter : ResourceStopTokenFilter
    {
        public RomanianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(RomanianStopTokenFilter));
    }
}
