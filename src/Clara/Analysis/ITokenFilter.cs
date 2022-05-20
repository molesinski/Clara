using System.Collections.Generic;

namespace Clara.Analysis
{
    public interface ITokenFilter
    {
        IEnumerable<string> Filter(IEnumerable<string> tokens);
    }
}
