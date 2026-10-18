using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1214:Readonly fields should appear before non-readonly fields", Justification = "By design")]
        private sealed class TokenTermSource : ITokenTermSource, IEnumerable<TokenTerm>, IEnumerator<TokenTerm>
        {
            private readonly ITokenTermSource tokenTermSource;
            private readonly StringPoolSlim stringPool;
            private readonly Node root;
            private string text = string.Empty;
            private TokenTerm current;
            private IEnumerator<TokenTerm>? enumerator;
            private bool isEnumerated;
            private bool isBacktracking;
            private readonly ListSlim<TokenTerm> pendingTerms = new();
            private readonly ListSlim<BacktrackingEntry> backtrackingEntries = new();
            private IReadOnlyList<string>? replacementTokens;
            private int replacementIndex;
            private Position replacementPosition;
            private Node currentNode;

            public TokenTermSource(SynonymMap synonymMap)
            {
                if (synonymMap is null)
                {
                    throw new ArgumentNullException(nameof(synonymMap));
                }

                this.tokenTermSource = synonymMap.Analyzer.CreateTokenTermSource();
                this.stringPool = synonymMap.stringPool;
                this.root = synonymMap.root;
                this.currentNode = this.root;
            }

            TokenTerm IEnumerator<TokenTerm>.Current
            {
                get
                {
                    return this.current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public IEnumerable<TokenTerm> GetTerms(string text)
            {
                if (text is null)
                {
                    throw new ArgumentNullException(nameof(text));
                }

                this.text = text;

                ((IEnumerator)this).Reset();

                return this;
            }

            IEnumerator<TokenTerm> IEnumerable<TokenTerm>.GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            bool IEnumerator.MoveNext()
            {
                this.enumerator ??= this.tokenTermSource.GetTerms(this.text).GetEnumerator();

                while (this.isBacktracking || this.replacementTokens is not null || this.pendingTerms.Count > 0 || !this.isEnumerated || !this.currentNode.IsRoot)
                {
                    if (this.isBacktracking)
                    {
                        var lastIndex = this.backtrackingEntries.Count - 1;
                        var index = lastIndex;

                        while (index >= 0)
                        {
                            var entry = this.backtrackingEntries[index];
                            var replacements = entry.Node.TokenReplacements;

                            if (replacements.Count > 0)
                            {
                                this.replacementTokens = replacements;
                                this.replacementIndex = 0;
                                this.replacementPosition = CombinePositions(this.backtrackingEntries, 0, index + 1);

                                break;
                            }

                            index--;
                        }

                        var first = this.backtrackingEntries[0];
                        var consumedCount = index >= 0 ? index + 1 : 1;

                        // Replay only unconsumed input, before the token that failed to extend the match.
                        // Node tokens are immutable, unlike the analyzer's reusable token buffer.
                        for (var i = lastIndex; i >= consumedCount; i--)
                        {
                            var entry = this.backtrackingEntries[i];

                            this.pendingTerms.Add(new TokenTerm(new Token(entry.Node.Token), entry.Position));
                        }

                        this.backtrackingEntries.Clear();
                        this.isBacktracking = false;
                        this.currentNode = this.root;

                        if (index < 0)
                        {
                            this.current = new TokenTerm(new Token(first.Node.Token), first.Position);

                            return true;
                        }
                    }

                    if (this.replacementTokens is not null)
                    {
                        this.current = new TokenTerm(new Token(this.replacementTokens[this.replacementIndex++]), this.replacementPosition);

                        if (this.replacementIndex == this.replacementTokens.Count)
                        {
                            this.replacementTokens = default;
                            this.replacementIndex = default;
                            this.replacementPosition = default;
                        }

                        return true;
                    }

                    TokenTerm currentTerm;

                    if (this.pendingTerms.Count > 0)
                    {
                        var lastIndex = this.pendingTerms.Count - 1;

                        currentTerm = this.pendingTerms[lastIndex];
                        this.pendingTerms.RemoveAt(lastIndex);
                    }
                    else if (!this.isEnumerated && this.enumerator.MoveNext())
                    {
                        currentTerm = this.enumerator.Current;
                    }
                    else
                    {
                        this.isEnumerated = true;
                        this.isBacktracking = !this.currentNode.IsRoot;

                        continue;
                    }

                    if (this.stringPool.TryGet(currentTerm.Token, out var value))
                    {
                        if (this.currentNode.Children.TryGetValue(value, out var node))
                        {
                            this.backtrackingEntries.Add(new BacktrackingEntry(node, currentTerm.Position));
                            this.currentNode = node;

                            continue;
                        }
                    }

                    if (!this.currentNode.IsRoot)
                    {
                        this.pendingTerms.Add(currentTerm);
                        this.isBacktracking = true;

                        continue;
                    }

                    this.current = new TokenTerm(currentTerm.Token, currentTerm.Position);

                    return true;
                }

                this.current = default;

                return false;
            }

            void IEnumerator.Reset()
            {
                this.current = default;
                this.enumerator?.Dispose();
                this.enumerator = default;
                this.isEnumerated = default;
                this.isBacktracking = default;
                this.pendingTerms.Clear();
                this.backtrackingEntries.Clear();
                this.replacementTokens = default;
                this.replacementIndex = default;
                this.replacementPosition = default;
                this.currentNode = this.root;
            }

            void IDisposable.Dispose()
            {
                ((IEnumerator)this).Reset();

                this.text = string.Empty;
            }

            private static Position CombinePositions(ListSlim<BacktrackingEntry> entries, int index, int count)
            {
                var start = int.MaxValue;
                var end = int.MinValue;

                for (var i = index; i < index + count; i++)
                {
                    var entry = entries[i];

                    start = start < entry.Position.Start ? start : entry.Position.Start;
                    end = end > entry.Position.End ? end : entry.Position.End;
                }

                return new Position(start, end);
            }

            private readonly struct BacktrackingEntry
            {
                public BacktrackingEntry(
                    Node node,
                    Position position)
                {
                    this.Node = node;
                    this.Position = position;
                }

                public Node Node { get; }

                public Position Position { get; }
            }
        }
    }
}
