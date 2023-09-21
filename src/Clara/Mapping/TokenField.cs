namespace Clara.Mapping
{
    public abstract class TokenField : Field
    {
        internal TokenField(bool isFilterable, bool isFacetable)
            : base(isFilterable, isFacetable, isSortable: false)
        {
            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Either filtering or faceting must be enabled for given field.");
            }
        }
    }
}
