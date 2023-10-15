namespace Clara.Analysis
{
    public sealed class PorterStopTokenFilter : ResourceStopTokenFilter
    {
        public PorterStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(PorterStopTokenFilter));
    }
}
