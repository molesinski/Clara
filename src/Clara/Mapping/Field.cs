using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class Field : IEquatable<Field>
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

        public bool Equals(Field? other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Field);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal abstract FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap);
    }
}
