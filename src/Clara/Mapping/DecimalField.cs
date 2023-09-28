namespace Clara.Mapping
{
    public sealed class DecimalField<TSource> : RangeField<TSource, decimal>
    {
        public DecimalField(Func<TSource, decimal?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: decimal.MinValue,
                maxValue: decimal.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }

        public DecimalField(Func<TSource, IEnumerable<decimal>?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: decimal.MinValue,
                maxValue: decimal.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }

        public DecimalField(Func<TSource, IEnumerable<decimal?>?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: decimal.MinValue,
                maxValue: decimal.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }
    }
}
