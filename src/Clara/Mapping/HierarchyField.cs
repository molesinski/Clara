using System;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public sealed class HierarchyField : TokenField
    {
        public const string DefaultRoot = "0";

        public HierarchyField(bool isFilterable = false, bool isFacetable = false, char separator = ',', string root = DefaultRoot)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: false)
        {
            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Either filtering or faceting must be enabled for given field.");
            }

            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            this.Separator = separator;
            this.Root = root;
        }

        public char Separator { get; }

        public string Root { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderStore tokenEncoderStore, ISynonymMap? synonymMap)
        {
            return new HierarchyFieldStoreBuilder(this, tokenEncoderStore);
        }
    }
}
