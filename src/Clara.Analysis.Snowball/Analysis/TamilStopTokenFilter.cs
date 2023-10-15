namespace Clara.Analysis
{
    public sealed class TamilStopTokenFilter : ResourceStopTokenFilter
    {
        public TamilStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(TamilStopTokenFilter));
    }
}
