namespace Clara.Analysis
{
    public sealed class LithuanianStopTokenFilter : ResourceStopTokenFilter
    {
        public LithuanianStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(LithuanianStopTokenFilter));
    }
}
