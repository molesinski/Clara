namespace Clara.Analysis
{
    public sealed class IrishStopTokenFilter : ResourceStopTokenFilter
    {
        public IrishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(IrishStopTokenFilter));
    }
}
