using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class HierarchyField : TokenField
    {
        public const string DefaultRoot = "0";

        internal HierarchyField(bool isFilterable, bool isFacetable, char separator, string root)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            this.Separator = separator;
            this.Root = root;
        }

        public char Separator { get; }

        public string Root { get; }
    }

    public sealed class HierarchyField<TSource> : HierarchyField
    {
        public HierarchyField(Func<TSource, string?> valueMapper, bool isFilterable = false, bool isFacetable = false, char separator = ',', string root = DefaultRoot)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                separator: separator,
                root: root)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new TokenValue(valueMapper(source));
        }

        public HierarchyField(Func<TSource, IEnumerable<string>?> valueMapper, bool isFilterable = false, bool isFacetable = false, char separator = ',', string root = DefaultRoot)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                separator: separator,
                root: root)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new TokenValue(valueMapper(source));
        }

        internal Func<TSource, TokenValue> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new HierarchyFieldStoreBuilder<TSource>(this, tokenEncoderStore);
        }
    }
}
