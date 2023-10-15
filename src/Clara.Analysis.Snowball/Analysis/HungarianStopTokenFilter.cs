namespace Clara.Analysis
{
    public sealed class HungarianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public HungarianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(HungarianStopTokenFilter));
    }
}
