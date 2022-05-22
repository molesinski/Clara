using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Egothor.Stemmer;
using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class LucenePolishStemTokenFilter : ITokenFilter
    {
        private static readonly ThreadLocal<StringBuilder> Buffer = new(() => new StringBuilder());

        private readonly Trie stemmer = PolishAnalyzer.DefaultTable;
        private readonly IStringFactory stringFactory;

        public LucenePolishStemTokenFilter()
            : this(DefaultStringFactory.Instance)
        {
        }

        public LucenePolishStemTokenFilter(IStringFactory stringFactory)
        {
            if (stringFactory is null)
            {
                throw new ArgumentNullException(nameof(stringFactory));
            }

            this.stringFactory = stringFactory;
        }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var buffer = Buffer.Value!;

            foreach (var token in tokens)
            {
                var result = this.stemmer.GetLastOnPath(token);

                if (result is not null)
                {
                    buffer.Clear();
                    buffer.Append(token);

                    Diff.Apply(buffer, result);

                    if (buffer.Length > 0)
                    {
                        yield return this.stringFactory.Create(buffer);

                        continue;
                    }
                }
            }
        }
    }
}
