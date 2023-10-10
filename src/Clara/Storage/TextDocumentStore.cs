using Clara.Analysis.MatchExpressions;
using Clara.Mapping;
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
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, float> documentLengths,
            Similarity similarity)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (tokenDocumentScores is null)
            {
                throw new ArgumentNullException(nameof(tokenDocumentScores));
            }

            if (similarity is null)
            {
                throw new ArgumentNullException(nameof(similarity));
            }

            similarity.Process(tokenDocumentScores, documentLengths);

            this.tokenEncoder = tokenEncoder;
            this.tokenDocumentScores = tokenDocumentScores;
        }

        public DocumentScoring Search(SearchMode mode, ListSlim<SearchTerm> terms, ref DocumentResultBuilder documentResultBuilder)
        {
            if (terms.Count == 0)
            {
                documentResultBuilder.Clear();

                return default;
            }

            if (terms.Count == 1 && terms[0].Token is not null)
            {
                var token = terms[0].Token!;

                if (this.tokenEncoder.TryEncode(token, out var tokenId))
                {
                    if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                    {
                        documentResultBuilder.IntersectWith(documents);

                        return new DocumentScoring(documents);
                    }
                }

                documentResultBuilder.Clear();

                return default;
            }

            var documentScores = SharedObjectPools.DocumentScores.Lease();

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
                                documentScores.Instance.UnionWith(documents, ScoreCombiner.Sum);
                            }
                        }
                    }
                    else if (term.Expression is MatchExpression expression)
                    {
                        using var tempScores = SharedObjectPools.DocumentScores.Lease();

                        this.Search(expression, tempScores.Instance);

                        documentScores.Instance.UnionWith(tempScores.Instance, ScoreCombiner.Sum);
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
                                    documentScores.Instance.UnionWith(documents, ScoreCombiner.Sum);
                                    isFirst = false;
                                }
                                else
                                {
                                    documentScores.Instance.IntersectWith(documents, ScoreCombiner.Sum);
                                }

                                continue;
                            }
                        }

                        documentScores.Instance.Clear();
                        break;
                    }
                    else if (term.Expression is MatchExpression expression)
                    {
                        using var tempScores = SharedObjectPools.DocumentScores.Lease();

                        this.Search(expression, tempScores.Instance);

                        if (tempScores.Instance.Count > 0)
                        {
                            if (isFirst)
                            {
                                documentScores.Instance.UnionWith(tempScores.Instance, ScoreCombiner.Sum);
                                isFirst = false;
                            }
                            else
                            {
                                documentScores.Instance.IntersectWith(tempScores.Instance, ScoreCombiner.Sum);
                            }

                            continue;
                        }

                        documentScores.Instance.Clear();
                        break;
                    }
                }
            }

            documentResultBuilder.IntersectWith(documentScores.Instance);

            return new DocumentScoring(documentScores);
        }

        private void Search(MatchExpression matchExpression, DictionarySlim<int, float> partialScores)
        {
            if (matchExpression is AnyMatchExpression anyMatchExpression)
            {
                for (var i = 0; i < anyMatchExpression.Tokens.Count; i++)
                {
                    var token = anyMatchExpression.Tokens[i];

                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            partialScores.UnionWith(documents, ScoreCombiner.Get(anyMatchExpression.ScoreAggregation));
                        }
                    }
                }
            }
            else if (matchExpression is AllMatchExpression allMatchExpression)
            {
                var isFirst = true;

                for (var i = 0; i < allMatchExpression.Tokens.Count; i++)
                {
                    var token = allMatchExpression.Tokens[i];

                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                partialScores.UnionWith(documents, ScoreCombiner.Get(allMatchExpression.ScoreAggregation));
                                isFirst = false;
                            }
                            else
                            {
                                partialScores.IntersectWith(documents, ScoreCombiner.Get(allMatchExpression.ScoreAggregation));
                            }

                            continue;
                        }
                    }

                    partialScores.Clear();
                    break;
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                for (var i = 0; i < orMatchExpression.Expressions.Count; i++)
                {
                    var expression = orMatchExpression.Expressions[i];

                    tempScores.Instance.Clear();

                    this.Search(expression, tempScores.Instance);

                    partialScores.UnionWith(tempScores.Instance, ScoreCombiner.Get(orMatchExpression.ScoreAggregation));
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var isFirst = true;

                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                for (var i = 0; i < andMatchExpression.Expressions.Count; i++)
                {
                    var expression = andMatchExpression.Expressions[i];

                    tempScores.Instance.Clear();

                    this.Search(expression, tempScores.Instance);

                    if (tempScores.Instance.Count > 0)
                    {
                        if (isFirst)
                        {
                            partialScores.UnionWith(tempScores.Instance, ScoreCombiner.Get(andMatchExpression.ScoreAggregation));
                            isFirst = false;
                        }
                        else
                        {
                            partialScores.IntersectWith(tempScores.Instance, ScoreCombiner.Get(andMatchExpression.ScoreAggregation));
                        }

                        continue;
                    }

                    partialScores.Clear();
                    break;
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }
    }
}
