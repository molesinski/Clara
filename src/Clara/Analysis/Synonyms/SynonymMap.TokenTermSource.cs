﻿using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1214:Readonly fields should appear before non-readonly fields", Justification = "For clarity")]
        private sealed class TokenTermSource : ITokenTermSource, IEnumerable<TokenTerm>, IEnumerator<TokenTerm>
        {
            private readonly ITokenTermSource tokenTermSource;
            private readonly TokenNode root;
            private readonly StringPoolSlim stringPool;
            private string text = string.Empty;
            private TokenTerm current;
            private IEnumerator<TokenTerm>? enumerator;
            private bool isEnumerated;
            private TokenTerm? peekedTerm;
            private TokenNode? backtrackingNode;
            private readonly Stack<Offset> backtrackingOffsets = new();
            private ListSlim<string>? replacementTokens;
            private int replacementIndex;
            private Offset replacementOffset;
            private TokenNode currentNode = default!;

            public TokenTermSource(SynonymMap synonymMap)
            {
                if (synonymMap is null)
                {
                    throw new ArgumentNullException(nameof(synonymMap));
                }

                this.tokenTermSource = synonymMap.Analyzer.CreateTokenTermSource();
                this.root = synonymMap.root;
                this.stringPool = synonymMap.stringPool;
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

                while (this.backtrackingNode is not null || this.replacementTokens is not null || !this.isEnumerated)
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
                                this.replacementTokens = this.backtrackingNode.ReplacementTokens;
                                this.replacementIndex = 0;
                                this.replacementOffset = this.backtrackingOffsets.Peek();

                                this.backtrackingNode = null;
                                this.backtrackingOffsets.Clear();
                            }
                            else
                            {
                                this.current = new TokenTerm(new Token(this.backtrackingNode.Token), this.backtrackingOffsets.Pop());
                                this.backtrackingNode = this.backtrackingNode.Parent;

                                return true;
                            }
                        }
                    }

                    if (this.replacementTokens is not null)
                    {
                        this.current = new TokenTerm(new Token(this.replacementTokens[this.replacementIndex++]), this.replacementOffset);

                        if (this.replacementIndex == this.replacementTokens.Count)
                        {
                            this.replacementTokens = default;
                            this.replacementIndex = default;
                            this.replacementOffset = default;
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
                                this.backtrackingOffsets.Push(currentTerm.Offset);

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

                        this.current = new TokenTerm(currentTerm.Token, currentTerm.Offset);

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
                this.enumerator?.Dispose();
                this.enumerator = default;
                this.isEnumerated = default;
                this.peekedTerm = default;
                this.backtrackingNode = default;
                this.backtrackingOffsets.Clear();
                this.replacementTokens = default;
                this.replacementIndex = default;
                this.replacementOffset = default;
                this.currentNode = this.root;
            }

            void IDisposable.Dispose()
            {
                ((IEnumerator)this).Reset();

                this.text = string.Empty;
            }
        }
    }
}