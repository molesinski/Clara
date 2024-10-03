using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1214:Readonly fields should appear before non-readonly fields", Justification = "By design")]
        private sealed class SynonymTermSource : ISynonymTermSource, IEnumerable<SynonymTerm>, IEnumerator<SynonymTerm>
        {
            private readonly ITokenTermSource tokenTermSource;
            private readonly StringPoolSlim stringPool;
            private readonly Node root;
            private string text = string.Empty;
            private SynonymTerm current;
            private IEnumerator<TokenTerm>? enumerator;
            private bool isEnumerated;
            private TokenTerm? peekedTerm;
            private int backtrackingState;
            private int backtrackingIndex;
            private readonly ListSlim<BacktrackingEntry> backtrackingEntries = new();
            private SynonymPhraseCollection? replacementTokens;
            private Position replacementPosition;
            private Node currentNode;

            public SynonymTermSource(SynonymMap synonymMap)
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

            SynonymTerm IEnumerator<SynonymTerm>.Current
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

            public IEnumerable<SynonymTerm> GetTerms(string text)
            {
                if (text is null)
                {
                    throw new ArgumentNullException(nameof(text));
                }

                this.text = text;

                ((IEnumerator)this).Reset();

                return this;
            }

            IEnumerator<SynonymTerm> IEnumerable<SynonymTerm>.GetEnumerator()
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

                while (this.backtrackingState > 0 || this.replacementTokens is not null || !this.isEnumerated)
                {
                    if (this.backtrackingState == 1)
                    {
                        var lastIndex = this.backtrackingEntries.Count - 1;
                        var index = lastIndex;

                        while (index >= 0)
                        {
                            var entry = this.backtrackingEntries[index];

                            var replacementTokens = entry.Node.SynonymTermReplacements;

                            if (replacementTokens.Count > 0)
                            {
                                var position = CombinePositions(this.backtrackingEntries, 0, index + 1);

                                this.replacementTokens = replacementTokens;
                                this.replacementPosition = position;

                                break;
                            }

                            index--;
                        }

                        if (index < lastIndex)
                        {
                            this.backtrackingState = 2;
                            this.backtrackingIndex = index + 1;
                        }
                        else
                        {
                            this.backtrackingState = 0;
                            this.backtrackingEntries.Clear();
                        }
                    }

                    if (this.replacementTokens is not null)
                    {
                        this.current = new SynonymTerm(this.replacementTokens.Value, this.replacementPosition);

                        this.replacementTokens = default;
                        this.replacementPosition = default;

                        return true;
                    }

                    if (this.backtrackingState == 2)
                    {
                        var entry = this.backtrackingEntries[this.backtrackingIndex++];

                        this.current = new SynonymTerm(new Token(entry.Node.Token), entry.Position);

                        if (this.backtrackingIndex == this.backtrackingEntries.Count)
                        {
                            this.backtrackingState = 0;
                            this.backtrackingEntries.Clear();
                        }

                        return true;
                    }

                    if (this.peekedTerm is not null || (!this.isEnumerated && this.enumerator.MoveNext()))
                    {
                        var currentTerm = this.peekedTerm ?? this.enumerator.Current;

                        this.peekedTerm = null;

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
                            this.peekedTerm = currentTerm;
                            this.backtrackingState = 1;
                            this.currentNode = this.root;

                            continue;
                        }

                        this.current = new SynonymTerm(currentTerm.Token, currentTerm.Position);

                        return true;
                    }
                    else
                    {
                        if (!this.isEnumerated)
                        {
                            this.isEnumerated = true;

                            if (!this.currentNode.IsRoot)
                            {
                                this.peekedTerm = null;
                                this.backtrackingState = 1;
                                this.currentNode = this.root;

                                continue;
                            }
                        }
                    }
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
                this.peekedTerm = default;
                this.backtrackingState = default;
                this.backtrackingIndex = default;
                this.backtrackingEntries.Clear();
                this.replacementTokens = default;
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
