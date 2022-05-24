using System.Collections.Generic;

namespace Clara.Analysis.Synonyms
{
    public class SynonymMapOptions
    {
        public bool PermutatePhraseTokens { get; set; } = false;

        public int MaximumPermutationTokenCount { get; set; } = 3;

        internal IEnumerable<IEnumerable<string>> GetPhraseTokensPermutations(List<string> phraseTokens)
        {
            return
                this.PermutatePhraseTokens && phraseTokens.Count <= this.MaximumPermutationTokenCount
                    ? PermutationHelper.Dijkstra(phraseTokens)
                    : PermutationHelper.Identity(phraseTokens);
        }
    }
}
