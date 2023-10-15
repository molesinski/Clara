namespace Clara.Analysis
{
    public sealed class ItalianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public ItalianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(ItalianStopTokenFilter));
    }
}
