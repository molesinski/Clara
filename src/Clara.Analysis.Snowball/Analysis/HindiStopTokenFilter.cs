namespace Clara.Analysis
{
    public sealed class HindiStopTokenFilter : ResourceStopTokenFilter
    {
        public HindiStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(HindiStopTokenFilter));
    }
}
