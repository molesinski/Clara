using System;
using System.Collections.Generic;
using Clara.Analysis.Synonyms;
using Clara.Collections;
using Clara.Mapping;
using Clara.Storage;

namespace Clara
{
    public abstract class IndexBuilder
    {
        protected internal IndexBuilder()
        {
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> mapper)
        {
            return Build(source, mapper, Array.Empty<SynonymMap>());
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> mapper,
            IEnumerable<ISynonymMap> synonymMaps)
        {
            return Build(source, mapper, synonymMaps, new InstanceTokenEncoderStore());
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> mapper,
            IEnumerable<ISynonymMap> synonymMaps,
            TokenEncoderStore tokenEncoderStore)
        {
            lock (tokenEncoderStore.SyncRoot)
            {
                var builder = new IndexBuilder<TSource, TDocument>(mapper, synonymMaps, tokenEncoderStore);

                foreach (var item in source)
                {
                    builder.Index(item);
                }

                return builder.Build();
            }
        }
    }

    public sealed class IndexBuilder<TSource, TDocument> : IndexBuilder
    {
        private readonly IIndexMapper<TSource, TDocument> mapper;
        private readonly PooledDictionary<int, TDocument> documents;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly Dictionary<Field, FieldStoreBuilder> fieldBuilders;

        public IndexBuilder(
            IIndexMapper<TSource, TDocument> mapper)
            : this(mapper, Array.Empty<ISynonymMap>())
        {
        }

        public IndexBuilder(
            IIndexMapper<TSource, TDocument> mapper,
            IEnumerable<ISynonymMap> synonymMaps)
            : this(mapper, synonymMaps, new InstanceTokenEncoderStore())
        {
        }

        public IndexBuilder(
            IIndexMapper<TSource, TDocument> mapper,
            IEnumerable<ISynonymMap> synonymMaps,
            TokenEncoderStore tokenEncoderStore)
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (synonymMaps is null)
            {
                throw new ArgumentNullException(nameof(synonymMaps));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

            this.mapper = mapper;
            this.documents = new PooledDictionary<int, TDocument>();
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder();
            this.fieldBuilders = new Dictionary<Field, FieldStoreBuilder>();

            var fields = new HashSet<Field>();
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

                this.fieldBuilders.Add(field, field.CreateFieldStoreBuilder(tokenEncoderStore, synonymMap));
            }
        }

        public void Index(TSource item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var documentKey = this.mapper.GetDocumentKey(item);
            var documentId = this.tokenEncoderBuilder.Encode(documentKey);

            ref var document = ref this.documents.GetValueRefOrAddDefault(documentId, out var exists);

            if (exists)
            {
                throw new InvalidOperationException("Attempt to index document with duplicate key.");
            }

            document = this.mapper.GetDocument(item);

            foreach (var fieldValue in this.mapper.GetFieldValues(item))
            {
                var field = fieldValue.Field;

                if (!this.fieldBuilders.TryGetValue(field, out var fieldBuilder))
                {
                    throw new InvalidOperationException("Field value references field not belonging to current index mapper fields.");
                }

                fieldBuilder.Index(documentId, fieldValue);
            }
        }

        public Index<TDocument> Build()
        {
            var tokenEncoder = this.tokenEncoderBuilder.Build();
            var fieldStores = new Dictionary<Field, FieldStore>();

            foreach (var pair in this.fieldBuilders)
            {
                var field = pair.Key;
                var builder = pair.Value;

                fieldStores.Add(field, builder.Build());
            }

            return new Index<TDocument>(tokenEncoder, this.documents, fieldStores);
        }
    }
}
