namespace Clara.Analysis
{
    public sealed class DutchStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public DutchStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(DutchStopTokenFilter));
    }
}
