using System.Collections.Generic;

namespace Clara.Analysis
{
    public interface IAnalyzer
    {
        IEnumerable<string> GetTokens(string text);
    }
}
