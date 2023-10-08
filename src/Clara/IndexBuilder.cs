﻿using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Storage;
using Clara.Utils;

namespace Clara
{
    public abstract class IndexBuilder
    {
        internal IndexBuilder()
        {
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> indexMapper)
        {
            return Build(source, indexMapper, Array.Empty<SynonymMapBinding>());
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> indexMapper,
            IEnumerable<SynonymMapBinding> synonymMapBindings)
        {
            return Build(source, indexMapper, new InstanceTokenEncoderStore(), synonymMapBindings);
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> indexMapper,
            TokenEncoderStore tokenEncoderStore)
        {
            return Build(source, indexMapper, tokenEncoderStore, Array.Empty<SynonymMapBinding>());
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> indexMapper,
            TokenEncoderStore tokenEncoderStore,
            IEnumerable<SynonymMapBinding> synonymMapBindings)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (indexMapper is null)
            {
                throw new ArgumentNullException(nameof(indexMapper));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

            if (synonymMapBindings is null)
            {
                throw new ArgumentNullException(nameof(synonymMapBindings));
            }

            lock (tokenEncoderStore.SyncRoot)
            {
                var builder = new IndexBuilder<TSource, TDocument>(indexMapper, tokenEncoderStore, synonymMapBindings);

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
        private readonly IIndexMapper<TSource, TDocument> indexMapper;
        private readonly DictionarySlim<int, TDocument> documentMap;
        private readonly ITokenEncoderBuilder tokenEncoderBuilder;
        private readonly Dictionary<Field, FieldStoreBuilder<TSource>> fieldBuilders;
        private bool isBuilt;

        public IndexBuilder(
            IIndexMapper<TSource, TDocument> indexMapper,
            TokenEncoderStore tokenEncoderStore,
            IEnumerable<SynonymMapBinding> synonymMapBindings)
        {
            if (indexMapper is null)
            {
                throw new ArgumentNullException(nameof(indexMapper));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

            if (synonymMapBindings is null)
            {
                throw new ArgumentNullException(nameof(synonymMapBindings));
            }

            this.indexMapper = indexMapper;
            this.documentMap = new DictionarySlim<int, TDocument>();
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder();
            this.fieldBuilders = new Dictionary<Field, FieldStoreBuilder<TSource>>();

            var fields = new HashSet<Field>();
            var fieldSynonymMaps = new Dictionary<Field, ISynonymMap>();

            foreach (var field in indexMapper.GetFields())
            {
                if (fields.Contains(field))
                {
                    throw new InvalidOperationException("Duplicated fields detected in index mapper field specification.");
                }

                fields.Add(field);
            }

            foreach (var synonymMapBinding in synonymMapBindings)
            {
                if (!fields.Contains(synonymMapBinding.Field))
                {
                    throw new InvalidOperationException("Synonym map references field not belonging to current index mapper fields.");
                }

                if (fieldSynonymMaps.ContainsKey(synonymMapBinding.Field))
                {
                    throw new InvalidOperationException("Synonym map field assignment must be unique.");
                }

                fieldSynonymMaps.Add(synonymMapBinding.Field, synonymMapBinding.SynonymMap);
            }

            foreach (var field in fields)
            {
                fieldSynonymMaps.TryGetValue(field, out var synonymMap);

                if (field.CreateFieldStoreBuilder(tokenEncoderStore, synonymMap) is not FieldStoreBuilder<TSource> fieldBuilder)
                {
                    throw new InvalidOperationException("One of the field value mappers is based on different source type than current index mapper.");
                }

                this.fieldBuilders.Add(field, fieldBuilder);
            }
        }

        public void Index(TSource item)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var documentKey = this.indexMapper.GetDocumentKey(item);
            var documentId = this.tokenEncoderBuilder.Encode(new Token(documentKey));

            ref var document = ref this.documentMap.GetValueRefOrAddDefault(documentId, out var exists);

            if (exists)
            {
                throw new InvalidOperationException("Attempt to index document with duplicate key.");
            }

            document = this.indexMapper.GetDocument(item);

            foreach (var pair in this.fieldBuilders)
            {
                var fieldBuilder = pair.Value;

                fieldBuilder.Index(documentId, item);
            }
        }

        public Index<TDocument> Build()
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var tokenEncoder = this.tokenEncoderBuilder.Build();
            var fieldStores = new Dictionary<Field, FieldStore>();

            foreach (var pair in this.fieldBuilders)
            {
                var field = pair.Key;
                var builder = pair.Value;

                fieldStores.Add(field, builder.Build());
            }

            var index = new Index<TDocument>(tokenEncoder, this.documentMap, fieldStores);

            this.isBuilt = true;

            return index;
        }
    }
}
