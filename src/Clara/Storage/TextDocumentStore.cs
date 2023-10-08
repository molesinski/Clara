using Clara.Analysis;
using Clara.Analysis.MatchExpressions;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextDocumentStore
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;

        public TextDocumentStore(
            ITokenEncoder tokenEncoder,
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

        public DocumentScoring Search(Field field, MatchExpression matchExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (matchExpression is AnyMatchExpression anyMatchExpression)
            {
                if (anyMatchExpression.Tokens.Count == 1)
                {
                    foreach (var token in (ListSlim<Token>)anyMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentResultBuilder.IntersectWith(field, documents);

                                return new DocumentScoring(documents);
                            }
                        }
                    }

                    documentResultBuilder.Clear();

                    return default;
                }
                else
                {
                    var documentScores = SharedObjectPools.DocumentScores.Lease();

                    foreach (var token in (ListSlim<Token>)anyMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentScores.Instance.UnionWith(documents, ScoreCombiner.Get(anyMatchExpression.ScoreAggregation));
                            }
                        }
                    }

                    documentResultBuilder.IntersectWith(field, documentScores.Instance);

                    return new DocumentScoring(documentScores);
                }
            }
            else if (matchExpression is AllMatchExpression allMatchExpression)
            {
                if (allMatchExpression.Tokens.Count == 1)
                {
                    foreach (var token in (ListSlim<Token>)allMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentResultBuilder.IntersectWith(field, documents);

                                return new DocumentScoring(documents);
                            }
                        }
                    }

                    documentResultBuilder.Clear();

                    return default;
                }
                else
                {
                    var documentScores = SharedObjectPools.DocumentScores.Lease();
                    var isFirst = true;

                    foreach (var token in (ListSlim<Token>)allMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                if (isFirst)
                                {
                                    documentScores.Instance.UnionWith(documents, ScoreCombiner.Get(allMatchExpression.ScoreAggregation));
                                    isFirst = false;
                                }
                                else
                                {
                                    documentScores.Instance.IntersectWith(documents, ScoreCombiner.Get(allMatchExpression.ScoreAggregation));
                                }

                                continue;
                            }
                        }

                        documentScores.Instance.Clear();

                        break;
                    }

                    documentResultBuilder.IntersectWith(field, documentScores.Instance);

                    return new DocumentScoring(documentScores);
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                var documentScores = SharedObjectPools.DocumentScores.Lease();

                using (var tempScores = SharedObjectPools.DocumentScores.Lease())
                {
                    foreach (var expression in (ListSlim<MatchExpression>)orMatchExpression.Expressions)
                    {
                        tempScores.Instance.Clear();

                        this.Search(expression, tempScores.Instance);

                        documentScores.Instance.UnionWith(tempScores.Instance, ScoreCombiner.Get(orMatchExpression.ScoreAggregation));
                    }
                }

                documentResultBuilder.IntersectWith(field, documentScores.Instance);

                return new DocumentScoring(documentScores);
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var documentScores = SharedObjectPools.DocumentScores.Lease();
                var isFirst = true;

                using (var tempScores = SharedObjectPools.DocumentScores.Lease())
                {
                    foreach (var expression in (ListSlim<MatchExpression>)andMatchExpression.Expressions)
                    {
                        tempScores.Instance.Clear();

                        this.Search(expression, tempScores.Instance);

                        if (isFirst)
                        {
                            documentScores.Instance.UnionWith(tempScores.Instance, ScoreCombiner.Get(andMatchExpression.ScoreAggregation));
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.Instance.IntersectWith(tempScores.Instance, ScoreCombiner.Get(andMatchExpression.ScoreAggregation));
                        }
                    }
                }

                documentResultBuilder.IntersectWith(field, documentScores.Instance);

                return new DocumentScoring(documentScores);
            }
            else if (matchExpression is EmptyMatchExpression)
            {
                documentResultBuilder.Clear();

                return default;
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }

        private void Search(MatchExpression matchExpression, DictionarySlim<int, float> partialScores)
        {
            if (matchExpression is AnyMatchExpression anyMatchExpression)
            {
                foreach (var token in (ListSlim<Token>)anyMatchExpression.Tokens)
                {
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

                foreach (var token in (ListSlim<Token>)allMatchExpression.Tokens)
                {
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

                foreach (var expression in (ListSlim<MatchExpression>)orMatchExpression.Expressions)
                {
                    tempScores.Instance.Clear();

                    this.Search(expression, tempScores.Instance);

                    partialScores.UnionWith(tempScores.Instance, ScoreCombiner.Get(orMatchExpression.ScoreAggregation));
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var isFirst = true;

                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                foreach (var expression in (ListSlim<MatchExpression>)andMatchExpression.Expressions)
                {
                    tempScores.Instance.Clear();

                    this.Search(expression, tempScores.Instance);

                    if (isFirst)
                    {
                        partialScores.UnionWith(tempScores.Instance, ScoreCombiner.Get(andMatchExpression.ScoreAggregation));
                        isFirst = false;
                    }
                    else
                    {
                        partialScores.IntersectWith(tempScores.Instance, ScoreCombiner.Get(andMatchExpression.ScoreAggregation));
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
