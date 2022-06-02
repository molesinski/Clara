using System;
using System.Collections.Generic;
using System.Linq;

namespace Clara.Analysis
{
    public sealed class Analyzer : IAnalyzer
    {
        private readonly ITokenizer tokenizer;
        private readonly ITokenFilter[] tokenFilters;

        public Analyzer(ITokenizer tokenizer)
            : this(tokenizer, (IEnumerable<ITokenFilter>)Array.Empty<ITokenFilter>())
        {
        }

        public Analyzer(ITokenizer tokenizer, params ITokenFilter[] tokenFilters)
            : this(tokenizer, (IEnumerable<ITokenFilter>)tokenFilters)
        {
        }

        public Analyzer(ITokenizer tokenizer, IEnumerable<ITokenFilter> tokenFilters)
        {
            if (tokenizer is null)
            {
                throw new ArgumentNullException(nameof(tokenizer));
            }

            if (tokenFilters is null)
            {
                throw new ArgumentNullException(nameof(tokenFilters));
            }

            this.tokenizer = tokenizer;
            this.tokenFilters = tokenFilters.ToArray();
        }

        public Analyzer(Analyzer analyzer, params ITokenFilter[] tokenFilters)
            : this(analyzer, (IEnumerable<ITokenFilter>)tokenFilters)
        {
        }

        public Analyzer(Analyzer analyzer, IEnumerable<ITokenFilter> tokenFilters)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (tokenFilters is null)
            {
                throw new ArgumentNullException(nameof(tokenFilters));
            }

            this.tokenizer = analyzer.tokenizer;
            this.tokenFilters = analyzer.tokenFilters.Concat(tokenFilters).ToArray();
        }

        public IEnumerable<string> GetTokens(string text)
        {
            foreach (var token in this.tokenizer.GetTokens(text))
            {
                var result = token;

                for (var i = 0; i < this.tokenFilters.Length; i++)
                {
                    result = this.tokenFilters[i].Filter(result);

                    if (result.IsEmpty)
                    {
                        break;
                    }
                }

                if (!result.IsEmpty)
                {
                    yield return result.ToString();
                }
            }
        }
    }
}
