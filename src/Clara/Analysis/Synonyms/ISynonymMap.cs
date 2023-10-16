using Clara.Querying;

namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap
    {
        IAnalyzer Analyzer { get; }

        ITokenTermSource CreateTokenTermSource();

        void Process(SearchMode mode, IList<SearchTerm> terms);

        string? ToReadOnly(Token token);
    }
}
