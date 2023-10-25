using Clara.Utils;

namespace Clara.Analysis
{
    public abstract class Synonym
    {
        private static readonly string[] MappingSeparator = new[] { "=>" };
        private static readonly char[] PhraseSeparator = new[] { ',' };

        private readonly HashSetSlim<string> phrases = new();

        internal Synonym(IEnumerable<string> phrases)
        {
            if (phrases is null)
            {
                throw new ArgumentNullException(nameof(phrases));
            }

            foreach (var phrase in phrases)
            {
                if (string.IsNullOrWhiteSpace(phrase))
                {
                    throw new ArgumentException("Phrases cannot be null, empty or whitespace.", nameof(phrases));
                }

                this.phrases.Add(phrase);
            }
        }

        public IReadOnlyCollection<string> Phrases
        {
            get
            {
                return this.phrases;
            }
        }

        public static IReadOnlyCollection<Synonym> Parse(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<Synonym>();
            }

            using var stringReader = new StringReader(text);

            return Parse(stringReader);
        }

        public static IReadOnlyCollection<Synonym> Parse(TextReader textReader)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            var result = new ListSlim<Synonym>();
            var equivalentPhrases = new HashSetSlim<string>();
            var mappedPhrases = new HashSetSlim<string>();

            while (textReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                {
                    continue;
                }

                var parts = line.Split(MappingSeparator, StringSplitOptions.None);

                if (parts.Length <= 2)
                {
                    equivalentPhrases.Clear();
                    mappedPhrases.Clear();

                    foreach (var phrase in parts[0].Split(PhraseSeparator))
                    {
                        if (!string.IsNullOrWhiteSpace(phrase))
                        {
                            equivalentPhrases.Add(phrase.Trim());
                        }
                    }

                    if (parts.Length == 2)
                    {
                        foreach (var phrase in parts[1].Split(PhraseSeparator))
                        {
                            if (!string.IsNullOrWhiteSpace(phrase))
                            {
                                mappedPhrases.Add(phrase.Trim());
                            }
                        }
                    }

                    if (parts.Length == 2)
                    {
                        if (equivalentPhrases.Count >= 1 && mappedPhrases.Count >= 1)
                        {
                            result.Add(new MappingSynonym(equivalentPhrases, mappedPhrases));
                        }
                    }
                    else
                    {
                        if (equivalentPhrases.Count >= 2)
                        {
                            result.Add(new EquivalencySynonym(equivalentPhrases));
                        }
                    }
                }
            }

            return result;
        }
    }
}
