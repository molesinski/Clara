using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Clara.Analysis
{
    public class RegexStopTokenFilter : ITokenFilter
    {
        private readonly Regex regex;

        public RegexStopTokenFilter(Regex regex)
        {
            if (regex is null)
            {
                throw new ArgumentNullException(nameof(regex));
            }

            this.regex = regex;
        }

        public RegexStopTokenFilter(string pattern, RegexOptions options)
        {
            if (pattern is null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            this.regex = new Regex(pattern, options);
        }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            foreach (var token in tokens)
            {
                if (this.regex.IsMatch(token))
                {
                    continue;
                }

                yield return token;
            }
        }
    }
}
