namespace Clara.Analysis
{
    public sealed class ArmenianStopTokenFilter : ResourceStopTokenFilter
    {
        public ArmenianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(ArmenianStopTokenFilter));
    }
}
