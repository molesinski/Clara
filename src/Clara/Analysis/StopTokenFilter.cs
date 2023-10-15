using Clara.Utils;

namespace Clara.Analysis
{
    public class StopTokenFilter : ITokenFilter
    {
        private readonly HashSetSlim<Token> stopwords = new();

        public StopTokenFilter(IEnumerable<string> stopwords)
        {
            if (stopwords is null)
            {
                throw new ArgumentNullException(nameof(stopwords));
            }

            foreach (var stopword in stopwords)
            {
                if (!string.IsNullOrWhiteSpace(stopword))
                {
                    this.stopwords.Add(new Token(stopword));
                }
            }
        }

        public IEnumerable<string> Stopwords
        {
            get
            {
                return this.stopwords.Select(x => x.ToString());
            }
        }

        public static IReadOnlyCollection<string> Parse(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<string>();
            }

            using var stringReader = new StringReader(text);

            return Parse(stringReader);
        }

        public static IReadOnlyCollection<string> Parse(TextReader textReader)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            var result = new HashSetSlim<string>();

            while (textReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                {
                    continue;
                }

                var word = line.Trim();

                if (!string.IsNullOrWhiteSpace(word))
                {
                    result.Add(word);
                }
            }

            return result;
        }

        public void Process(ref Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (this.stopwords.Count > 0)
            {
                if (this.stopwords.Contains(token))
                {
                    token.Clear();

                    return;
                }
            }

            next(ref token);
        }
    }
}
