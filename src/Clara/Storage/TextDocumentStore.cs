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
            if (matchExpression is AnyTokensMatchExpression anyValuesMatchExpression)
            {
                if (anyValuesMatchExpression.Tokens.Count == 1)
                {
                    foreach (var token in (ListSlim<string>)anyValuesMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentResultBuilder.IntersectWith(field, documents.Keys);

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

                    foreach (var token in (ListSlim<string>)anyValuesMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentScores.Instance.UnionWith(documents, ScoringValueCombiner.Get(anyValuesMatchExpression.ScoringMode));
                            }
                        }
                    }

                    documentResultBuilder.IntersectWith(field, documentScores.Instance.Keys);

                    return new DocumentScoring(documentScores);
                }
            }
            else if (matchExpression is AllTokensMatchExpression allValuesMatchExpression)
            {
                if (allValuesMatchExpression.Tokens.Count == 1)
                {
                    foreach (var token in (ListSlim<string>)allValuesMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentResultBuilder.IntersectWith(field, documents.Keys);

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

                    foreach (var token in (ListSlim<string>)allValuesMatchExpression.Tokens)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                if (isFirst)
                                {
                                    documentScores.Instance.UnionWith(documents, ScoringValueCombiner.Get(allValuesMatchExpression.ScoringMode));
                                    isFirst = false;
                                }
                                else
                                {
                                    documentScores.Instance.IntersectWith(documents, ScoringValueCombiner.Get(allValuesMatchExpression.ScoringMode));
                                }

                                continue;
                            }
                        }

                        documentScores.Instance.Clear();

                        break;
                    }

                    documentResultBuilder.IntersectWith(field, documentScores.Instance.Keys);

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

                        documentScores.Instance.UnionWith(tempScores.Instance, ScoringValueCombiner.Get(orMatchExpression.ScoringMode));
                    }
                }

                documentResultBuilder.IntersectWith(field, documentScores.Instance.Keys);

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
                            documentScores.Instance.UnionWith(tempScores.Instance, ScoringValueCombiner.Get(andMatchExpression.ScoringMode));
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.Instance.IntersectWith(tempScores.Instance, ScoringValueCombiner.Get(andMatchExpression.ScoringMode));
                        }
                    }
                }

                documentResultBuilder.IntersectWith(field, documentScores.Instance.Keys);

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
            if (matchExpression is AnyTokensMatchExpression anyValuesMatchExpression)
            {
                foreach (var token in (ListSlim<string>)anyValuesMatchExpression.Tokens)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            partialScores.UnionWith(documents, ScoringValueCombiner.Get(anyValuesMatchExpression.ScoringMode));
                        }
                    }
                }
            }
            else if (matchExpression is AllTokensMatchExpression allValuesMatchExpression)
            {
                var isFirst = true;

                foreach (var token in (ListSlim<string>)allValuesMatchExpression.Tokens)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                partialScores.UnionWith(documents, ScoringValueCombiner.Get(allValuesMatchExpression.ScoringMode));
                                isFirst = false;
                            }
                            else
                            {
                                partialScores.IntersectWith(documents, ScoringValueCombiner.Get(allValuesMatchExpression.ScoringMode));
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

                    partialScores.UnionWith(tempScores.Instance, ScoringValueCombiner.Get(orMatchExpression.ScoringMode));
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
                        partialScores.UnionWith(tempScores.Instance, ScoringValueCombiner.Get(andMatchExpression.ScoringMode));
                        isFirst = false;
                    }
                    else
                    {
                        partialScores.IntersectWith(tempScores.Instance, ScoringValueCombiner.Get(andMatchExpression.ScoringMode));
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
