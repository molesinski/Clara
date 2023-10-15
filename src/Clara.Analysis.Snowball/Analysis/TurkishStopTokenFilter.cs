namespace Clara.Analysis
{
    public sealed class TurkishStopTokenFilter : ResourceStopTokenFilter
    {
        public TurkishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(TurkishStopTokenFilter));
    }
}
