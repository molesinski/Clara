namespace Clara.Analysis
{
    public sealed class IndonesianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public IndonesianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(IndonesianStopTokenFilter));
    }
}
