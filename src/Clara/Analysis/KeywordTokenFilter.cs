using Clara.Utils;

namespace Clara.Analysis
{
    public class KeywordTokenFilter : ITokenFilter
    {
        private readonly HashSetSlim<Token> keywords = new();

        public KeywordTokenFilter(IEnumerable<string> keywords)
        {
            if (keywords is null)
            {
                throw new ArgumentNullException(nameof(keywords));
            }

            foreach (var keyword in keywords)
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    this.keywords.Add(new Token(keyword));
                }
            }
        }

        public IEnumerable<string> Keywords
        {
            get
            {
                return this.keywords.Select(x => x.ToString());
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

            if (this.keywords.Count > 0)
            {
                if (this.keywords.Contains(token))
                {
                    return;
                }
            }

            next(ref token);
        }
    }
}
