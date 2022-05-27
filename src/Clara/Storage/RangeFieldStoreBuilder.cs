using System;
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
        private readonly PooledList<DocumentValue<TValue>>? sortedDocumentValues;
        private readonly PooledDictionarySlim<int, MinMax<TValue>>? documentValueMinMax;

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
                this.sortedDocumentValues = new();
            }

            if (field.IsFacetable || field.IsSortable)
            {
                this.documentValueMinMax = new();
            }
        }

        public override void Index(int documentId, TSource item)
        {
            var values = this.field.ValueMapper(item);

            if (values is null)
            {
                return;
            }

            var hadValues = false;
            var min = this.maxValue;
            var max = this.minValue;

            foreach (var value in values)
            {
                hadValues = true;

                if (this.sortedDocumentValues is not null)
                {
                    this.sortedDocumentValues.Add(new DocumentValue<TValue>(documentId, value));
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

            if (hadValues)
            {
                if (this.documentValueMinMax is not null)
                {
                    this.documentValueMinMax.Add(documentId, new MinMax<TValue>(min, max));
                }
            }
        }

        public override FieldStore Build()
        {
            return
                new RangeFieldStore<TValue>(
                    this.sortedDocumentValues is not null ? new RangeSortedDocumentValueStore<TValue>(this.sortedDocumentValues) : null,
                    this.documentValueMinMax is not null ? new RangeDocumentValueMinMaxStore<TValue>(this.minValue, this.maxValue, this.documentValueMinMax) : null);
        }
    }
}
