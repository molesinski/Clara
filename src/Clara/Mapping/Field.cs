using Clara.Analysis;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class Field
    {
        protected internal Field(bool isFilterable, bool isFacetable, bool isSortable)
        {
            this.IsFilterable = isFilterable;
            this.IsFacetable = isFacetable;
            this.IsSortable = isSortable;
        }

        public bool IsFilterable { get; }

        public bool IsFacetable { get; }

        public bool IsSortable { get; }

        internal abstract FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderStore tokenEncoderStore, ISynonymMap? synonymMap);
    }
}
