namespace Clara.Analysis
{
    public interface IAnalyzer
    {
        ITokenizer Tokenizer { get; }

        IEnumerable<AnalyzerTerm> GetTerms(string text);
    }
}
