namespace Clara.Analysis
{
    public sealed class GreekStopTokenFilter : ResourceStopTokenFilter
    {
        public GreekStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(GreekStopTokenFilter));
    }
}
