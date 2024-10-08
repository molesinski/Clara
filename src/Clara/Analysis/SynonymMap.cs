﻿using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap : ISynonymMap
    {
        private readonly HashSet<Synonym> synonyms = new();
        private readonly StringPoolSlim stringPool = new();
        private readonly IAnalyzer analyzer;
        private readonly bool expand;
        private readonly Node root;

        public SynonymMap(
            IAnalyzer analyzer,
            IEnumerable<Synonym> synonyms,
            bool expand = true)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (synonyms is null)
            {
                throw new ArgumentNullException(nameof(synonyms));
            }

            foreach (var synonym in synonyms)
            {
                if (synonym is null)
                {
                    throw new ArgumentException("Synonym values must not be null.", nameof(synonyms));
                }

                this.synonyms.Add(synonym);
            }

            this.analyzer = analyzer;
            this.expand = expand;
            this.root = Node.BuildTree(this.analyzer, this.synonyms, this.expand, this.stringPool);
        }

        public IAnalyzer Analyzer
        {
            get
            {
                return this.analyzer;
            }
        }

        public IReadOnlyCollection<Synonym> Synonyms
        {
            get
            {
                return this.synonyms;
            }
        }

        public ITokenTermSource CreateTokenTermSource()
        {
            return new TokenTermSource(this);
        }

        public IPhraseTermSource CreatePhraseTermSource()
        {
            return new PhraseTermSource(this);
        }
    }
}
