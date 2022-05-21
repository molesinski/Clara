using System;
using System.Collections.Generic;
using Clara.Analysis;
using Clara.Collections;
using Clara.Mapping;
using Clara.Storage;

namespace Clara
{
    public static class IndexBuilder
    {
        public static Index<TDocument> Build<TSource, TDocument>(
            IIndexMapper<TSource, TDocument> mapper,
            IEnumerable<TSource> source)
        {
            return Build(mapper, source, Array.Empty<SynonymMap>());
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IIndexMapper<TSource, TDocument> mapper,
            IEnumerable<TSource> source,
            IEnumerable<ISynonymMap> synonymMaps)
        {
            return Build(mapper, source, synonymMaps, new InstanceTokenEncoderStore());
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IIndexMapper<TSource, TDocument> mapper,
            IEnumerable<TSource> source,
            IEnumerable<ISynonymMap> synonymMaps,
            TokenEncoderStore tokenEncoderStore)
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (synonymMaps is null)
            {
                throw new ArgumentNullException(nameof(synonymMaps));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

            lock (tokenEncoderStore.SyncRoot)
            {
                var tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder();
                var documents = new PooledDictionary<int, TDocument>();
                var fields = new HashSet<Field>();
                var fieldBuilders = new Dictionary<Field, FieldStoreBuilder>();
                var fieldSynonymMaps = new Dictionary<Field, ISynonymMap>();

                foreach (var field in mapper.GetFields())
                {
                    if (fields.Contains(field))
                    {
                        throw new InvalidOperationException("Duplicated fields detected in index mapper field specification.");
                    }

                    fields.Add(field);
                }

                foreach (var synonymMap in synonymMaps)
                {
                    if (!fields.Contains(synonymMap.Field))
                    {
                        throw new InvalidOperationException("Syononym map references field not belonging to current index mapper fields.");
                    }

                    if (fieldSynonymMaps.ContainsKey(synonymMap.Field))
                    {
                        throw new InvalidOperationException("Synonym map field assignment must be unique.");
                    }

                    fieldSynonymMaps.Add(synonymMap.Field, synonymMap);
                }

                foreach (var field in fields)
                {
                    fieldSynonymMaps.TryGetValue(field, out var synonymMap);

                    fieldBuilders.Add(field, field.CreateFieldStoreBuilder(tokenEncoderStore, synonymMap));
                }

                foreach (var item in source)
                {
                    var documentKey = mapper.GetDocumentKey(item);
                    var documentId = tokenEncoderBuilder.Encode(documentKey);

                    ref var document = ref documents.GetValueRefOrAddDefault(documentId, out var exists);

                    if (exists)
                    {
                        throw new InvalidOperationException("Attempt to index document with duplicate key.");
                    }

                    document = mapper.GetDocument(item);

                    foreach (var fieldValue in mapper.GetFieldValues(item))
                    {
                        var field = fieldValue.Field;

                        if (!fieldBuilders.TryGetValue(field, out var fieldBuilder))
                        {
                            throw new InvalidOperationException("Field value references field not belonging to current index mapper fields.");
                        }

                        fieldBuilder.Index(documentId, fieldValue);
                    }
                }

                var tokenEncoder = tokenEncoderBuilder.Build();
                var fieldStores = new Dictionary<Field, FieldStore>();

                foreach (var pair in fieldBuilders)
                {
                    var field = pair.Key;
                    var builder = pair.Value;

                    fieldStores.Add(field, builder.Build());
                }

                return new Index<TDocument>(tokenEncoder, documents, fieldStores);
            }
        }
    }
}
