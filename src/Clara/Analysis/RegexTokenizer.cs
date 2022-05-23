using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Clara.Analysis
{
    public class RegexTokenizer : ITokenizer
    {
        private readonly Regex regex;

        public RegexTokenizer(Regex regex)
        {
            if (regex is null)
            {
                throw new ArgumentNullException(nameof(regex));
            }

            this.regex = regex;
        }

        public RegexTokenizer(string pattern, RegexOptions options)
        {
            if (pattern is null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            this.regex = new Regex(pattern, options);
        }

        public IEnumerable<string> GetTokens(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                yield break;
            }

            var matches = this.regex.Matches(text);

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                if (match.Length == 0)
                {
                    continue;
                }

                yield return match.Value;
            }
        }
    }
}
