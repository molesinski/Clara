using Clara.Analysis.Synonyms;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class HierarchyField : Field
    {
        internal HierarchyField(string separator, string root, HierarchyValueHandling valueHandling, bool isFilterable, bool isFacetable)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: false)
        {
            if (separator is null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            if (string.IsNullOrWhiteSpace(separator))
            {
                throw new ArgumentException("Hierarchy separator value must not be empty or whitespace.", nameof(separator));
            }

            if (separator.Trim().Length != separator.Length)
            {
                throw new ArgumentException("Hierarchy separator value must not start or end with whitespace.", nameof(separator));
            }

            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (string.IsNullOrWhiteSpace(root))
            {
                throw new ArgumentException("Hierarchy root value must not be empty or whitespace.", nameof(root));
            }

            if (root.Trim().Length != root.Length)
            {
                throw new ArgumentException("Hierarchy root value must not start or end with whitespace.", nameof(root));
            }

            if (valueHandling != HierarchyValueHandling.Identifiers && valueHandling != HierarchyValueHandling.Path)
            {
                throw new ArgumentException("Illegal value handling enum value.", nameof(valueHandling));
            }

            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Either filtering or faceting must be enabled for given field.");
            }

            this.Separator = separator.Trim();
            this.Root = root.Trim();
            this.ValueHandling = valueHandling;
        }

        public string Separator { get; }

        public string Root { get; }

        public HierarchyValueHandling ValueHandling { get; }
    }

    public sealed class HierarchyField<TSource> : HierarchyField
    {
        public HierarchyField(Func<TSource, string?> valueMapper, string separator, string root, HierarchyValueHandling valueHandling = HierarchyValueHandling.Identifiers, bool isFilterable = false, bool isFacetable = false)
            : base(
                separator: separator,
                root: root,
                valueHandling: valueHandling,
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source), trim: true);
        }

        public HierarchyField(Func<TSource, IEnumerable<string?>?> valueMapper, string separator, string root, HierarchyValueHandling valueHandling = HierarchyValueHandling.Identifiers, bool isFilterable = false, bool isFacetable = false)
            : base(
                separator: separator,
                root: root,
                valueHandling: valueHandling,
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source), trim: true);
        }

        internal Func<TSource, StringEnumerable> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new HierarchyFieldStoreBuilder<TSource>(this, tokenEncoderStore);
        }
    }
}
