using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymAnalyzerTermEnumerable : IEnumerable<SynonymAnalyzerTermResult>
        {
            private readonly SynonymMap synonymMap;
            private readonly PrimitiveEnumerable<AnalyzerTerm> tokens;

            public SynonymAnalyzerTermEnumerable(SynonymMap synonymMap, PrimitiveEnumerable<AnalyzerTerm> tokens)
            {
                if (synonymMap is null)
                {
                    throw new ArgumentNullException(nameof(synonymMap));
                }

                this.synonymMap = synonymMap;
                this.tokens = tokens;
            }

            public readonly Enumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            readonly IEnumerator<SynonymAnalyzerTermResult> IEnumerable<SynonymAnalyzerTermResult>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            readonly IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public struct Enumerator : IEnumerator<SynonymAnalyzerTermResult>
            {
                private readonly StringPoolSlim stringPool;
                private readonly TokenNode root;
                private readonly PrimitiveEnumerable<AnalyzerTerm> tokens;
                private PrimitiveEnumerable<AnalyzerTerm>.Enumerator enumerator;
                private bool isEnumeratorSet;
                private bool isEnumerated;
                private AnalyzerTerm? peekedTerm;
                private TokenNode? backtrackingNode;
                private TokenNode currentNode;
                private SynonymAnalyzerTermResult current;
                private ObjectPoolLease<Stack<int>>? backtrackingOrdinals;

                public Enumerator(SynonymAnalyzerTermEnumerable source)
                {
                    this.stringPool = source.synonymMap.stringPool;
                    this.root = source.synonymMap.root;
                    this.tokens = source.tokens;
                    this.enumerator = default;
                    this.isEnumeratorSet = default;
                    this.isEnumerated = default;
                    this.peekedTerm = default;
                    this.backtrackingNode = default;
                    this.currentNode = this.root;
                    this.current = default;
                    this.backtrackingOrdinals = default;
                }

                public readonly SynonymAnalyzerTermResult Current
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
                                this.current = new SynonymAnalyzerTermResult(this.backtrackingOrdinals!.Value.Instance.Pop(), this.backtrackingNode);
                                this.backtrackingNode = null;
                                this.backtrackingOrdinals!.Value.Instance.Clear();

                                return true;
                            }

                            this.current = new SynonymAnalyzerTermResult(this.backtrackingOrdinals!.Value.Instance.Pop(), new Token(this.backtrackingNode.Token));
                            this.backtrackingNode = this.backtrackingNode.Parent;

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
                                    this.backtrackingOrdinals ??= SharedObjectPools.BacktrackingOrdinals.Lease();
                                    this.backtrackingOrdinals.Value.Instance.Push(currentTerm.Ordinal);

                                    this.currentNode = node;

                                    continue;
                                }
                            }

                            if (!this.currentNode.IsRoot)
                            {
                                this.peekedTerm = currentTerm;
                                this.backtrackingNode = this.currentNode;
                                this.currentNode = this.root;

                                continue;
                            }

                            this.current = new SynonymAnalyzerTermResult(currentTerm.Ordinal, currentTerm.Token);

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

                    this.backtrackingOrdinals?.Dispose();
                    this.backtrackingOrdinals = default;
                }
            }
        }
    }
}
