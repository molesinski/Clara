﻿using System.Text;
using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class ExplicitMappingSynonym : Synonym
    {
        private readonly HashSetSlim<string> mappedPhrases = new();

        public ExplicitMappingSynonym(IEnumerable<string> phrases, IEnumerable<string> mappedPhrases)
            : base(phrases)
        {
            if (mappedPhrases is null)
            {
                throw new ArgumentNullException(nameof(mappedPhrases));
            }

            foreach (var mappedPhrase in mappedPhrases)
            {
                if (string.IsNullOrWhiteSpace(mappedPhrase))
                {
                    throw new ArgumentException("Mapped phrases cannot be null, empty or whitespace.", nameof(mappedPhrases));
                }

                this.mappedPhrases.Add(mappedPhrase);
            }

            if (!(this.Phrases.Count >= 1))
            {
                throw new ArgumentException("At least one unique phrase must be specified.");
            }

            if (!(this.MappedPhrases.Count >= 1))
            {
                throw new ArgumentException("At least one unique mapped phrase must be specified.");
            }
        }

        public IReadOnlyCollection<string> MappedPhrases
        {
            get
            {
                return this.mappedPhrases;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var isFirst = true;

            foreach (var phrase in this.Phrases)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append(phrase);
                isFirst = false;
            }

            builder.Append(" => ");
            isFirst = true;

            foreach (var mappedPhrase in this.mappedPhrases)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append(mappedPhrase);
                isFirst = false;
            }

            return builder.ToString();
        }
    }
}
