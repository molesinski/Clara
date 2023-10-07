namespace Clara.Mapping
{
    public sealed class Int32Field<TSource> : RangeField<TSource, int>
    {
        public Int32Field(Func<TSource, int?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: int.MinValue,
                maxValue: int.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }

        public Int32Field(Func<TSource, IEnumerable<int>?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                valueMapper: valueMapper,
                minValue: int.MinValue,
                maxValue: int.MaxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
        }
    }
}
