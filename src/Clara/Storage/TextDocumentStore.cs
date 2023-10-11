using Clara.Analysis.MatchExpressions;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextDocumentStore
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;

        public TextDocumentStore(
            TokenEncoder tokenEncoder,
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (tokenDocumentScores is null)
            {
                throw new ArgumentNullException(nameof(tokenDocumentScores));
            }

            this.tokenEncoder = tokenEncoder;
            this.tokenDocumentScores = tokenDocumentScores;
        }

        public void Search(SearchMode mode, ListSlim<SearchTerm> terms, DictionarySlim<int, float> partialScores)
        {
            if (mode == SearchMode.Any)
            {
                foreach (var term in terms)
                {
                    if (term.Token is string token)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                partialScores.UnionWith(documents, ScoreCombiner.Sum);
                            }
                        }
                    }
                    else if (term.Expression is MatchExpression expression)
                    {
                        using var tempScores = SharedObjectPools.DocumentScores.Lease();

                        this.Search(expression, tempScores.Instance);

                        partialScores.UnionWith(tempScores.Instance, ScoreCombiner.Sum);
                    }
                }
            }
            else
            {
                var isFirst = true;

                foreach (var term in terms)
                {
                    if (term.Token is string token)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                if (isFirst)
                                {
                                    partialScores.UnionWith(documents, ScoreCombiner.Sum);
                                    isFirst = false;
                                }
                                else
                                {
                                    partialScores.IntersectWith(documents, ScoreCombiner.Sum);
                                }
                            }
                            else
                            {
                                partialScores.Clear();
                            }
                        }
                        else
                        {
                            partialScores.Clear();
                        }
                    }
                    else if (term.Expression is MatchExpression expression)
                    {
                        using var tempScores = SharedObjectPools.DocumentScores.Lease();

                        this.Search(expression, tempScores.Instance);

                        if (tempScores.Instance.Count > 0)
                        {
                            if (isFirst)
                            {
                                partialScores.UnionWith(tempScores.Instance, ScoreCombiner.Sum);
                                isFirst = false;
                            }
                            else
                            {
                                partialScores.IntersectWith(tempScores.Instance, ScoreCombiner.Sum);
                            }
                        }
                        else
                        {
                            partialScores.Clear();
                        }
                    }

                    if (partialScores.Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        private void Search(MatchExpression matchExpression, DictionarySlim<int, float> partialScores)
        {
            if (matchExpression is AnyTokensMatchExpression anyTokensMatchExpression)
            {
                for (var i = 0; i < anyTokensMatchExpression.Tokens.Count; i++)
                {
                    if (this.tokenEncoder.TryEncode(anyTokensMatchExpression.Tokens[i], out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            partialScores.UnionWith(documents, ScoreCombiner.For(anyTokensMatchExpression.ScoreAggregation));
                        }
                    }
                }
            }
            else if (matchExpression is AllTokensMatchExpression allTokensMatchExpression)
            {
                var isFirst = true;

                for (var i = 0; i < allTokensMatchExpression.Tokens.Count; i++)
                {
                    if (this.tokenEncoder.TryEncode(allTokensMatchExpression.Tokens[i], out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                partialScores.UnionWith(documents, ScoreCombiner.For(allTokensMatchExpression.ScoreAggregation));
                                isFirst = false;
                            }
                            else
                            {
                                partialScores.IntersectWith(documents, ScoreCombiner.For(allTokensMatchExpression.ScoreAggregation));
                            }
                        }
                        else
                        {
                            partialScores.Clear();
                        }
                    }
                    else
                    {
                        partialScores.Clear();
                    }

                    if (partialScores.Count == 0)
                    {
                        break;
                    }
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                for (var i = 0; i < orMatchExpression.Expressions.Count; i++)
                {
                    tempScores.Instance.Clear();

                    this.Search(orMatchExpression.Expressions[i], tempScores.Instance);

                    partialScores.UnionWith(tempScores.Instance, ScoreCombiner.For(orMatchExpression.ScoreAggregation));
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var isFirst = true;

                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                for (var i = 0; i < andMatchExpression.Expressions.Count; i++)
                {
                    tempScores.Instance.Clear();

                    this.Search(andMatchExpression.Expressions[i], tempScores.Instance);

                    if (tempScores.Instance.Count > 0)
                    {
                        if (isFirst)
                        {
                            partialScores.UnionWith(tempScores.Instance, ScoreCombiner.For(andMatchExpression.ScoreAggregation));
                            isFirst = false;
                        }
                        else
                        {
                            partialScores.IntersectWith(tempScores.Instance, ScoreCombiner.For(andMatchExpression.ScoreAggregation));
                        }
                    }
                    else
                    {
                        partialScores.Clear();
                    }

                    if (partialScores.Count == 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }
    }
}
