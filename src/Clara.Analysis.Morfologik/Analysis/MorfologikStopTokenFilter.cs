namespace Clara.Analysis
{
    public sealed class MorfologikStopTokenFilter : ResourceStopTokenFilter
    {
        public MorfologikStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(MorfologikStopTokenFilter));
    }
}
