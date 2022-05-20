using System.Collections.Generic;

namespace Clara.Analysis
{
    public interface ITokenizer
    {
        IEnumerable<string> GetTokens(string text);
    }
}
