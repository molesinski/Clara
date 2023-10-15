namespace Clara.Analysis
{
    public sealed class FrenchStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public FrenchStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(FrenchStopTokenFilter));
    }
}
