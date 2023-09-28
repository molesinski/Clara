namespace Clara.Mapping
{
    public sealed class DoubleField<TSource> : RangeField<TSource, double>
    {
        public DoubleField(Func<TSource, double?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: double.MinValue,
                maxValue: double.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }

        public DoubleField(Func<TSource, IEnumerable<double>?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: double.MinValue,
                maxValue: double.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }

        public DoubleField(Func<TSource, IEnumerable<double?>?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: double.MinValue,
                maxValue: double.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }
    }
}
