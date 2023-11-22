using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class Field
    {
        internal Field(
            bool isSearchable,
            bool isFilterable,
            bool isFacetable,
            bool isSortable)
        {
            this.IsSearchable = isSearchable;
            this.IsFilterable = isFilterable;
            this.IsFacetable = isFacetable;
            this.IsSortable = isSortable;
        }

        public bool IsSearchable { get; }

        public bool IsFilterable { get; }

        public bool IsFacetable { get; }

        public bool IsSortable { get; }

        internal abstract FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderBuilder tokenEncoderBuilder);
    }
}
