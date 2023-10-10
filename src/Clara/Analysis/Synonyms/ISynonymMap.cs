using Clara.Querying;

namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap
    {
        IAnalyzer Analyzer { get; }

        IEnumerable<AnalyzerTerm> GetTerms(string text);

        void Process(SearchMode mode, IList<SearchTerm> terms);

        string? ToReadOnly(Token token);
    }
}
