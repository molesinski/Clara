using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymSearchTermEnumerable : IEnumerable<SynonymSearchTermResult>
        {
            private readonly SynonymMap synonymMap;
            private readonly PrimitiveEnumerable<SearchTerm> terms;

            public SynonymSearchTermEnumerable(SynonymMap synonymMap, PrimitiveEnumerable<SearchTerm> terms)
            {
                if (synonymMap is null)
                {
                    throw new ArgumentNullException(nameof(synonymMap));
                }

                this.synonymMap = synonymMap;
                this.terms = terms;
            }

            public readonly Enumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            readonly IEnumerator<SynonymSearchTermResult> IEnumerable<SynonymSearchTermResult>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            readonly IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public struct Enumerator : IEnumerator<SynonymSearchTermResult>
            {
                private readonly TokenNode root;
                private readonly PrimitiveEnumerable<SearchTerm> tokens;
                private PrimitiveEnumerable<SearchTerm>.Enumerator enumerator;
                private bool isEnumeratorSet;
                private bool isEnumerated;
                private SearchTerm? peekedTerm;
                private TokenNode? backtrackingNode;
                private TokenNode currentNode;
                private SynonymSearchTermResult current;
                private ObjectPoolLease<Stack<int>>? backtrackingPositions;

                public Enumerator(SynonymSearchTermEnumerable source)
                {
                    this.root = source.synonymMap.root;
                    this.tokens = source.terms;
                    this.enumerator = default;
                    this.isEnumeratorSet = default;
                    this.isEnumerated = default;
                    this.peekedTerm = default;
                    this.backtrackingNode = default;
                    this.currentNode = this.root;
                    this.current = default;
                    this.backtrackingPositions = default;
                }

                public readonly SynonymSearchTermResult Current
                {
                    get
                    {
                        return this.current;
                    }
                }

                readonly object IEnumerator.Current
                {
                    get
                    {
                        return this.current;
                    }
                }

                public bool MoveNext()
                {
                    if (!this.isEnumeratorSet)
                    {
                        this.enumerator = this.tokens.GetEnumerator();
                        this.isEnumeratorSet = true;
                    }

                    while (this.backtrackingNode is not null || !this.isEnumerated)
                    {
                        while (this.backtrackingNode is not null)
                        {
                            if (this.backtrackingNode.IsRoot)
                            {
                                this.backtrackingNode = null;

                                break;
                            }

                            if (this.backtrackingNode.HasSynonyms)
                            {
                                this.current = new SynonymSearchTermResult(this.backtrackingPositions!.Value.Instance.Pop(), this.backtrackingNode);
                                this.backtrackingNode = null;
                                this.backtrackingPositions!.Value.Instance.Clear();

                                return true;
                            }

                            this.current = new SynonymSearchTermResult(this.backtrackingPositions!.Value.Instance.Pop(), this.backtrackingNode.Token);
                            this.backtrackingNode = this.backtrackingNode.Parent;

                            return true;
                        }

                        if (this.peekedTerm is not null || (!this.isEnumerated && this.enumerator.MoveNext()))
                        {
                            var currentTerm = this.peekedTerm ?? this.enumerator.Current;

                            this.peekedTerm = null;

                            if (this.currentNode.Children.TryGetValue(currentTerm.Token!, out var node))
                            {
                                this.backtrackingPositions ??= SharedObjectPools.BacktrackingPositions.Lease();
                                this.backtrackingPositions.Value.Instance.Push(currentTerm.Position);

                                this.currentNode = node;

                                continue;
                            }

                            if (!this.currentNode.IsRoot)
                            {
                                this.peekedTerm = currentTerm;
                                this.backtrackingNode = this.currentNode;
                                this.currentNode = this.root;

                                continue;
                            }

                            this.current = new SynonymSearchTermResult(currentTerm.Position, currentTerm.Token!);

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
                                    this.backtrackingNode = this.currentNode;
                                    this.currentNode = this.root;

                                    continue;
                                }
                            }
                        }
                    }

                    this.current = default;

                    return false;
                }

                public void Reset()
                {
                    this.peekedTerm = default;
                    this.backtrackingNode = default;
                    this.currentNode = this.root;

                    if (this.isEnumeratorSet)
                    {
                        this.enumerator.Dispose();
                        this.enumerator = default;
                    }

                    this.isEnumerated = default;
                    this.current = default;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.backtrackingPositions?.Dispose();
                    this.backtrackingPositions = default;
                }
            }
        }
    }
}
