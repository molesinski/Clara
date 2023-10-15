namespace Clara.Analysis
{
    public sealed class CatalanStopTokenFilter : ResourceStopTokenFilter
    {
        public CatalanStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(CatalanStopTokenFilter));
    }
}
