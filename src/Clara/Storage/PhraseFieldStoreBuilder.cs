using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class PhraseFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly PhraseField<TSource> field;
        private readonly Dictionary<string, DictionarySlim<int, float>> phraseDocuments;
        private bool isBuilt;

        public PhraseFieldStoreBuilder(PhraseField<TSource> field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            this.field = field;
            this.phraseDocuments = new Dictionary<string, DictionarySlim<int, float>>(StringComparer.Ordinal);
        }

        public override bool RequiresFieldStores
        {
            get
            {
                return this.field.ValidationField is not null;
            }
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var values = this.field.ValueMapper(item);

            if (values is null)
            {
                return;
            }

            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(value.Phrase))
                {
                    continue;
                }

                if (!this.phraseDocuments.TryGetValue(value.Phrase!, out var documents))
                {
                    this.phraseDocuments.Add(value.Phrase!, documents = new DictionarySlim<int, float>());
                }

                ref var weight = ref documents.GetValueRefOrAddDefault(documentId, out _);

                weight += value.Weight;
            }
        }

        public override FieldStore Build(TokenEncoder tokenEncoder)
        {
            return this.Build(tokenEncoder, new Dictionary<Field, FieldStore>());
        }

        public override FieldStore Build(TokenEncoder tokenEncoder, IReadOnlyDictionary<Field, FieldStore> fieldStores)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var validationField = this.field.ValidationField;
            var validationStore = default(FieldStore);

            if (validationField is not null)
            {
                if (!fieldStores.TryGetValue(validationField, out validationStore))
                {
                    throw new InvalidOperationException("Phrase field validation field must belong to the same index.");
                }
            }

            var phrases = new List<PhraseFieldStore.PhraseEntry>(this.phraseDocuments.Count);

            foreach (var pair in this.phraseDocuments)
            {
                var documents = pair.Value;
                var postings = default(DocumentScoring);

                if (validationStore is not null)
                {
                    postings = validationStore.Search(new TextSearchExpression(validationField!, SearchMode.All, pair.Key));
                }

                using (postings)
                {
                    var documentIds = new List<int>(documents.Count);

                    foreach (var document in documents)
                    {
                        if (validationStore is null || postings.Value.ContainsKey(document.Key))
                        {
                            documentIds.Add(document.Key);
                        }
                    }

                    if (documentIds.Count == 0)
                    {
                        continue;
                    }

                    documentIds.Sort();

                    var weights = new float[documentIds.Count];
                    var total = 0f;

                    for (var i = 0; i < weights.Length; i++)
                    {
                        weights[i] = documents[documentIds[i]];
                        total += weights[i];
                    }

                    phrases.Add(new PhraseFieldStore.PhraseEntry(pair.Key, total, documentIds.ToArray(), weights));
                }
            }

            var store = new PhraseFieldStore(this.field, phrases);

            this.isBuilt = true;

            return store;
        }
    }
}
