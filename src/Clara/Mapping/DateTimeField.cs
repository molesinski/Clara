using System;

namespace Clara.Mapping
{
    public sealed class DateTimeField<TSource> : RangeField<TSource, DateTime>
    {
        public DateTimeField(Func<TSource, RangeValue<DateTime>> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: DateTime.MinValue,
                maxValue: DateTime.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }
    }
}
