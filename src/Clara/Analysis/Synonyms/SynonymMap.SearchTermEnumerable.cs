using System.Collections;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1214:Readonly fields should appear before non-readonly fields", Justification = "For clarity")]
        private sealed class SearchTermEnumerable : IEnumerable<SearchTerm>, IEnumerator<SearchTerm>
        {
            private readonly TokenNode root;
            private IList<SearchTerm> terms = Array.Empty<SearchTerm>();
            private SearchTerm current;
            private int termIndex = -1;
            private bool isEnumerated;
            private SearchTerm? peekedTerm;
            private TokenNode? backtrackingNode;
            private readonly Stack<Offset> backtrackingOffsets = new();
            private TokenNode currentNode = default!;

            public SearchTermEnumerable(SynonymMap synonymMap)
            {
                if (synonymMap is null)
                {
                    throw new ArgumentNullException(nameof(synonymMap));
                }

                this.root = synonymMap.root;
            }

            SearchTerm IEnumerator<SearchTerm>.Current
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

            public IEnumerable<SearchTerm> GetTerms(IList<SearchTerm> terms)
            {
                if (terms is null)
                {
                    throw new ArgumentNullException(nameof(terms));
                }

                this.terms = terms;

                ((IEnumerator)this).Reset();

                return this;
            }

            IEnumerator<SearchTerm> IEnumerable<SearchTerm>.GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                while (this.backtrackingNode is not null || !this.isEnumerated)
                {
                    if (this.backtrackingNode is not null)
                    {
                        if (this.backtrackingNode.IsRoot)
                        {
                            this.backtrackingNode = null;
                        }
                        else
                        {
                            if (this.backtrackingNode.HasSynonyms)
                            {
                                this.current = new SearchTerm(this.backtrackingNode.MatchExpression, this.backtrackingOffsets.Peek());
                                this.backtrackingNode = null;
                                this.backtrackingOffsets.Clear();

                                return true;
                            }
                            else
                            {
                                this.current = new SearchTerm(this.backtrackingNode.Token, this.backtrackingOffsets.Pop());
                                this.backtrackingNode = this.backtrackingNode.Parent;

                                return true;
                            }
                        }
                    }

                    if (this.peekedTerm is not null || (!this.isEnumerated && ++this.termIndex < this.terms.Count))
                    {
                        var currentTerm = this.peekedTerm ?? this.terms[this.termIndex];

                        this.peekedTerm = null;

                        if (this.currentNode.Children.TryGetValue(currentTerm.Token!, out var node))
                        {
                            this.backtrackingOffsets.Push(currentTerm.Offset);

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

                        this.current = new SearchTerm(currentTerm.Token!, currentTerm.Offset);

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

            void IEnumerator.Reset()
            {
                this.current = default;
                this.termIndex = -1;
                this.isEnumerated = default;
                this.peekedTerm = default;
                this.backtrackingNode = default;
                this.backtrackingOffsets.Clear();
                this.currentNode = this.root;
            }

            void IDisposable.Dispose()
            {
                ((IEnumerator)this).Reset();

                this.terms = Array.Empty<SearchTerm>();
            }
        }
    }
}
