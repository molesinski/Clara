namespace Clara.Mapping
{
    public abstract class TokenField : Field
    {
        protected internal TokenField(bool isFilterable, bool isFacetable, bool isSortable)
            : base(isFilterable, isFacetable, isSortable)
        {
        }
    }
}
