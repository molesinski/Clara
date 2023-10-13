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
            return Build(source, indexMapper, SharedTokenEncoderStore.Default);
        }

        public static Index<TDocument> Build<TSource, TDocument>(
            IEnumerable<TSource> source,
            IIndexMapper<TSource, TDocument> indexMapper,
            TokenEncoderStore tokenEncoderStore)
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

            lock (tokenEncoderStore.SyncRoot)
            {
                var builder = new IndexBuilder<TSource, TDocument>(indexMapper, tokenEncoderStore);

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
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly Dictionary<Field, FieldStoreBuilder<TSource>> fieldBuilders;
        private bool isBuilt;

        public IndexBuilder(
            IIndexMapper<TSource, TDocument> indexMapper,
            TokenEncoderStore tokenEncoderStore)
        {
            if (indexMapper is null)
            {
                throw new ArgumentNullException(nameof(indexMapper));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

            this.indexMapper = indexMapper;
            this.documentMap = new DictionarySlim<int, TDocument>();
            this.tokenEncoderBuilder = tokenEncoderStore.CreateBuilder();
            this.fieldBuilders = new Dictionary<Field, FieldStoreBuilder<TSource>>();

            var fields = new HashSet<Field>();

            foreach (var field in indexMapper.GetFields())
            {
                if (!fields.Add(field))
                {
                    throw new InvalidOperationException("Duplicate index mapper field detected.");
                }
            }

            foreach (var field in fields)
            {
                if (field.CreateFieldStoreBuilder(this.tokenEncoderBuilder) is not FieldStoreBuilder<TSource> fieldBuilder)
                {
                    throw new InvalidOperationException($"Index mapper fields must be based off \"{typeof(TSource).FullName}\" type.");
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
            var documentId = this.tokenEncoderBuilder.Encode(documentKey);

            ref var document = ref this.documentMap.GetValueRefOrAddDefault(documentId, out var exists);

            if (exists)
            {
                throw new InvalidOperationException($"Duplicate source document key \"{documentKey}\" detected.");
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

                fieldStores.Add(field, builder.Build(tokenEncoder));
            }

            var index = new Index<TDocument>(tokenEncoder, this.documentMap, fieldStores);

            this.isBuilt = true;

            return index;
        }
    }
}
