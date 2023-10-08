using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymResult
        {
            public SynonymResult(string token)
            {
                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                this.Token = token;
                this.Node = null;
            }

            public SynonymResult(TokenNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Token = null;
                this.Node = node;
            }

            public string? Token { get; }

            public TokenNode? Node { get; }
        }

        private readonly struct SynonymResultEnumerable : IEnumerable<SynonymResult>
        {
            private readonly TokenNode root;
            private readonly StringEnumerable tokens;

            public SynonymResultEnumerable(TokenNode root, StringEnumerable tokens)
            {
                if (root is null)
                {
                    throw new ArgumentNullException(nameof(root));
                }

                this.root = root;
                this.tokens = tokens;
            }

            public readonly Enumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            readonly IEnumerator<SynonymResult> IEnumerable<SynonymResult>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            readonly IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public struct Enumerator : IEnumerator<SynonymResult>
            {
                private readonly TokenNode root;
                private readonly StringEnumerable tokens;
                private StringEnumerable.Enumerator enumerator;
                private bool isEnumeratorSet;
                private bool isEnumerated;
                private TokenNode currentNode;
                private TokenNode? backtrackingNode;
                private string? previousToken;
                private SynonymResult current;

                public Enumerator(SynonymResultEnumerable source)
                {
                    this.root = source.root;
                    this.tokens = source.tokens;
                    this.enumerator = default;
                    this.isEnumeratorSet = false;
                    this.isEnumerated = false;
                    this.currentNode = this.root;
                    this.backtrackingNode = null;
                    this.previousToken = null;
                    this.current = default;
                }

                public readonly SynonymResult Current
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
                    if (this.isEnumerated)
                    {
                        return false;
                    }

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
                                this.current = new SynonymResult(this.backtrackingNode);
                                this.backtrackingNode = null;

                                return true;
                            }

                            this.current = new SynonymResult(this.backtrackingNode.Token);
                            this.backtrackingNode = this.backtrackingNode.Parent;

                            return true;
                        }

                        if (this.previousToken is not null || (!this.isEnumerated && this.enumerator.MoveNext()))
                        {
                            var currentToken = this.previousToken ?? this.enumerator.Current;

                            this.previousToken = null;

                            if (this.currentNode.Children.TryGetValue(currentToken, out var node))
                            {
                                this.currentNode = node;
                                continue;
                            }

                            if (!this.currentNode.IsRoot)
                            {
                                this.backtrackingNode = this.currentNode;
                                this.currentNode = this.root;
                                this.previousToken = currentToken;

                                continue;
                            }

                            this.current = new SynonymResult(currentToken);

                            return true;
                        }
                        else
                        {
                            if (!this.isEnumerated)
                            {
                                this.isEnumerated = true;

                                if (!this.currentNode.IsRoot)
                                {
                                    this.backtrackingNode = this.currentNode;
                                    this.currentNode = this.root;
                                    this.previousToken = null;

                                    continue;
                                }
                            }
                        }
                    }

                    this.isEnumerated = true;
                    this.current = default;

                    return false;
                }

                public void Reset()
                {
                    this.currentNode = this.root;
                    this.backtrackingNode = null;
                    this.previousToken = null;

                    if (this.isEnumeratorSet)
                    {
                        this.enumerator.Dispose();
                        this.enumerator = default;
                    }

                    this.isEnumerated = false;
                    this.current = default;
                }

                public void Dispose()
                {
                    this.Reset();
                }
            }
        }
    }
}
