using System;
using System.Collections.Generic;
using System.Text;
using Egothor.Stemmer;
using Lucene.Net.Analysis.Pl;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LucenePolishStemTokenFilter : ITokenFilter
    {
        private static readonly DisposableThreadLocal<StringBuilder> Buffer = new(() => new StringBuilder());

        private readonly Trie stemmer;

        public LucenePolishStemTokenFilter()
        {
            this.stemmer = PolishAnalyzer.DefaultTable;
        }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var buffer = Buffer.Value;

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
                        yield return buffer.ToString();

                        continue;
                    }
                }
            }
        }
    }
}
