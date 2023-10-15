namespace Clara.Analysis
{
    public interface ITokenFilter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "By design")]
        void Process(ref Token token, TokenFilterDelegate next);
    }
}
