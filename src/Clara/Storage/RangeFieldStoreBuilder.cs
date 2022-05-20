using System;
using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Storage
{
    internal sealed class RangeFieldStoreBuilder<TValue> : FieldStoreBuilder
        where TValue : struct, IComparable<TValue>
    {
        private readonly TValue minValue;
        private readonly TValue maxValue;
        private readonly List<DocumentValue<TValue>>? sortedDocumentValues;
        private readonly Dictionary<int, MinMax<TValue>>? documentValueMinMax;

        public RangeFieldStoreBuilder(RangeField<TValue> field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

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

        public override void Index(int documentId, FieldValue fieldValue)
        {
            if (fieldValue is RangeFieldValue<TValue> rangeFieldValue)
            {
                if (rangeFieldValue.Values.Length > 0)
                {
                    if (this.sortedDocumentValues is not null)
                    {
                        foreach (var value in rangeFieldValue.Values)
                        {
                            this.sortedDocumentValues.Add(new DocumentValue<TValue>(documentId, value));
                        }
                    }

                    if (this.documentValueMinMax is not null)
                    {
                        var min = this.maxValue;
                        var max = this.minValue;

                        foreach (var value in rangeFieldValue.Values)
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

                        this.documentValueMinMax.Add(documentId, new MinMax<TValue>(min, max));
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Indexing of non range field values is not supported.");
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
