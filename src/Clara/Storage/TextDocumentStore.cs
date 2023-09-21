using Clara.Mapping;
using Clara.Querying;
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
            DictionarySlim<int, int> documentLengths,
            Weight weight)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (tokenDocumentScores is null)
            {
                throw new ArgumentNullException(nameof(tokenDocumentScores));
            }

            if (weight is null)
            {
                throw new ArgumentNullException(nameof(weight));
            }

            weight.Process(tokenDocumentScores, documentLengths);

            this.tokenEncoder = tokenEncoder;
            this.tokenDocumentScores = tokenDocumentScores;
        }

        public DocumentScoring Search(Field field, MatchExpression matchExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (matchExpression is AnyValuesMatchExpression anyValuesMatchExpression)
            {
                if (anyValuesMatchExpression.Values.Count == 1)
                {
                    foreach (var token in (HashSetSlim<string>)anyValuesMatchExpression.Values)
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

                    foreach (var token in (HashSetSlim<string>)anyValuesMatchExpression.Values)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentScores.Instance.UnionWith(documents, ValueCombiner.Sum);
                            }
                        }
                    }

                    documentResultBuilder.IntersectWith(field, documentScores.Instance.Keys);

                    return new DocumentScoring(documentScores);
                }
            }
            else if (matchExpression is AllValuesMatchExpression allValuesMatchExpression)
            {
                if (allValuesMatchExpression.Values.Count == 1)
                {
                    foreach (var token in (HashSetSlim<string>)allValuesMatchExpression.Values)
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

                    foreach (var token in (HashSetSlim<string>)allValuesMatchExpression.Values)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                if (isFirst)
                                {
                                    documentScores.Instance.UnionWith(documents, ValueCombiner.Sum);
                                    isFirst = false;
                                }
                                else
                                {
                                    documentScores.Instance.IntersectWith(documents, ValueCombiner.Sum);
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

                        this.SearchPartial(expression, tempScores.Instance);

                        documentScores.Instance.UnionWith(tempScores.Instance, ValueCombiner.Max);
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

                        this.SearchPartial(expression, tempScores.Instance);

                        if (isFirst)
                        {
                            documentScores.Instance.UnionWith(tempScores.Instance, ValueCombiner.Max);
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.Instance.IntersectWith(tempScores.Instance, ValueCombiner.Max);
                        }
                    }
                }

                documentResultBuilder.IntersectWith(field, documentScores.Instance.Keys);

                return new DocumentScoring(documentScores);
            }
            else if (matchExpression is EmptyValuesMatchExpression)
            {
                documentResultBuilder.Clear();

                return default;
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }

        private void SearchPartial(MatchExpression matchExpression, DictionarySlim<int, float> partialScores)
        {
            if (matchExpression is AnyValuesMatchExpression anyValuesMatchExpression)
            {
                foreach (var token in (HashSetSlim<string>)anyValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            partialScores.UnionWith(documents, ValueCombiner.Sum);
                        }
                    }
                }
            }
            else if (matchExpression is AllValuesMatchExpression allValuesMatchExpression)
            {
                var isFirst = true;

                foreach (var token in (HashSetSlim<string>)allValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                partialScores.UnionWith(documents, ValueCombiner.Sum);
                                isFirst = false;
                            }
                            else
                            {
                                partialScores.IntersectWith(documents, ValueCombiner.Sum);
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

                    this.SearchPartial(expression, tempScores.Instance);

                    partialScores.UnionWith(tempScores.Instance, ValueCombiner.Max);
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var isFirst = true;

                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                foreach (var expression in (ListSlim<MatchExpression>)andMatchExpression.Expressions)
                {
                    tempScores.Instance.Clear();

                    this.SearchPartial(expression, tempScores.Instance);

                    if (isFirst)
                    {
                        partialScores.UnionWith(tempScores.Instance, ValueCombiner.Max);
                        isFirst = false;
                    }
                    else
                    {
                        partialScores.IntersectWith(tempScores.Instance, ValueCombiner.Max);
                    }
                }
            }
            else if (matchExpression is EmptyValuesMatchExpression)
            {
                partialScores.Clear();
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }
    }
}
