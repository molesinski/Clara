using System;
using System.Collections.Generic;

namespace Clara.Analysis
{
    public sealed class LowerInvariantTokenFilter : ITokenFilter
    {
        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            foreach (var token in tokens)
            {
                yield return token.ToLowerInvariant();
            }
        }
    }
}
