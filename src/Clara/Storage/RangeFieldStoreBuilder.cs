using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class RangeFieldStoreBuilder<TSource, TValue> : FieldStoreBuilder<TSource>
        where TValue : struct, IComparable<TValue>
    {
        private readonly RangeField<TSource, TValue> field;
        private readonly TValue minValue;
        private readonly TValue maxValue;
        private readonly ListSlim<DocumentValue<TValue>>? documentValues;
        private readonly DictionarySlim<int, MinMax<TValue>>? documentValueMinMax;
        private bool isBuilt;

        public RangeFieldStoreBuilder(RangeField<TSource, TValue> field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            this.field = field;
            this.minValue = field.MinValue;
            this.maxValue = field.MaxValue;

            if (field.IsFilterable)
            {
                this.documentValues = new();
            }

            if (field.IsFacetable || field.IsSortable)
            {
                this.documentValueMinMax = new();
            }
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var values = this.field.ValueMapper(item);

            var hadValues = false;
            var min = this.maxValue;
            var max = this.minValue;

            foreach (var value in values)
            {
                if (this.minValue.CompareTo(value) <= 0 && value.CompareTo(this.maxValue) <= 0)
                {
                    hadValues = true;

                    if (this.documentValues is not null)
                    {
                        this.documentValues.Add(new DocumentValue<TValue>(documentId, value));
                    }

                    if (this.documentValueMinMax is not null)
                    {
                        if (value.CompareTo(min) < 0)
                        {
                            min = value;
                        }

                        if (value.CompareTo(max) > 0)
                        {
                            max = value;
                        }
                    }
                }
            }

            if (hadValues)
            {
                if (this.documentValueMinMax is not null)
                {
                    ref var value = ref this.documentValueMinMax.GetValueRefOrAddDefault(documentId, out _);

                    value = new MinMax<TValue>(min, max);
                }
            }
        }

        public override FieldStore Build(TokenEncoder tokenEncoder)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var store =
                new RangeFieldStore<TValue>(
                    this.documentValues is not null ? new RangeSortedDocumentValueStore<TValue>(this.documentValues) : null,
                    this.documentValueMinMax is not null ? new RangeDocumentValueMinMaxStore<TValue>(this.field, this.minValue, this.maxValue, this.documentValueMinMax) : null);

            this.isBuilt = true;

            return store;
        }
    }
}
