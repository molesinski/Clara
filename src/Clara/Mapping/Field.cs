using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class Field
    {
        internal Field(bool isFilterable, bool isFacetable, bool isSortable)
        {
            this.IsFilterable = isFilterable;
            this.IsFacetable = isFacetable;
            this.IsSortable = isSortable;
        }

        public bool IsFilterable { get; }

        public bool IsFacetable { get; }

        public bool IsSortable { get; }

        internal abstract FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderBuilder tokenEncoderBuilder,
            ISynonymMap? synonymMap);
    }
}
