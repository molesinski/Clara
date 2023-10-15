namespace Clara.Analysis
{
    public sealed class SerbianStopTokenFilter : ResourceStopTokenFilter
    {
        public SerbianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(SerbianStopTokenFilter));
    }
}
