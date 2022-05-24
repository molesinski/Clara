using System;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public sealed class KeywordField : TokenField
    {
        public KeywordField(bool isFilterable = false, bool isFacetable = false)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: false)
        {
            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Either filtering or faceting must be enabled for given field.");
            }
        }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderStore tokenEncoderStore, ISynonymMap? synonymMap)
        {
            return new KeywordFieldStoreBuilder(this, tokenEncoderStore);
        }
    }
}
