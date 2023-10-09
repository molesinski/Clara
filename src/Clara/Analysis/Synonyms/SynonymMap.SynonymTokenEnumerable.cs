using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymTokenEnumerable : IEnumerable<SynonymTokenResult>
        {
            private readonly SynonymMap synonymMap;
            private readonly PrimitiveEnumerable<Token> tokens;

            public SynonymTokenEnumerable(SynonymMap synonymMap, PrimitiveEnumerable<Token> tokens)
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

            readonly IEnumerator<SynonymTokenResult> IEnumerable<SynonymTokenResult>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            readonly IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public struct Enumerator : IEnumerator<SynonymTokenResult>
            {
                private readonly StringPoolSlim stringPool;
                private readonly TokenNode root;
                private readonly PrimitiveEnumerable<Token> tokens;
                private PrimitiveEnumerable<Token>.Enumerator enumerator;
                private bool isEnumeratorSet;
                private bool isEnumerated;
                private TokenNode? previousNode;
                private Token? previousToken;
                private TokenNode currentNode;
                private SynonymTokenResult current;

                public Enumerator(SynonymTokenEnumerable source)
                {
                    this.stringPool = source.synonymMap.stringPool;
                    this.root = source.synonymMap.root;
                    this.tokens = source.tokens;
                    this.enumerator = default;
                    this.isEnumeratorSet = false;
                    this.isEnumerated = false;
                    this.previousToken = null;
                    this.previousNode = null;
                    this.currentNode = this.root;
                    this.current = default;
                }

                public readonly SynonymTokenResult Current
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

                    while (this.previousNode is not null || !this.isEnumerated)
                    {
                        while (this.previousNode is not null)
                        {
                            if (this.previousNode.IsRoot)
                            {
                                this.previousNode = null;

                                break;
                            }

                            if (this.previousNode.HasSynonyms)
                            {
                                this.current = new SynonymTokenResult(this.previousNode);
                                this.previousNode = null;

                                return true;
                            }

                            this.current = new SynonymTokenResult(new Token(this.previousNode.Token));
                            this.previousNode = this.previousNode.Parent;

                            return true;
                        }

                        if (this.previousToken is not null || (!this.isEnumerated && this.enumerator.MoveNext()))
                        {
                            var currentToken = this.previousToken ?? this.enumerator.Current;

                            this.previousToken = null;

                            if (this.stringPool.TryGet(currentToken, out var value))
                            {
                                if (this.currentNode.Children.TryGetValue(value, out var node))
                                {
                                    this.currentNode = node;
                                    continue;
                                }
                            }

                            if (!this.currentNode.IsRoot)
                            {
                                this.previousToken = currentToken;
                                this.previousNode = this.currentNode;
                                this.currentNode = this.root;

                                continue;
                            }

                            this.current = new SynonymTokenResult(currentToken);

                            return true;
                        }
                        else
                        {
                            if (!this.isEnumerated)
                            {
                                this.isEnumerated = true;

                                if (!this.currentNode.IsRoot)
                                {
                                    this.previousToken = null;
                                    this.previousNode = this.currentNode;
                                    this.currentNode = this.root;

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
                    this.previousToken = null;
                    this.previousNode = null;
                    this.currentNode = this.root;

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
