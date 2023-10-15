namespace Clara.Analysis
{
    public sealed class RussianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public RussianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(RussianStopTokenFilter));
    }
}
