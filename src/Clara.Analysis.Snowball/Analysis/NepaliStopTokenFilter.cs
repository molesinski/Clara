namespace Clara.Analysis
{
    public sealed class NepaliStopTokenFilter : ResourceStopTokenFilter
    {
        public NepaliStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = LoadResource(typeof(NepaliStopTokenFilter));
    }
}
