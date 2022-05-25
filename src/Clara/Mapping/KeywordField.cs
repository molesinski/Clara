using System;
using System.Collections.Generic;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class KeywordField : TokenField
    {
        protected internal KeywordField(bool isFilterable, bool isFacetable)
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
    }

    public sealed class KeywordField<TSource> : KeywordField
    {
        public KeywordField(Func<TSource, IEnumerable<string>?> valueMapper, bool isFilterable = false, bool isFacetable = false)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Either filtering or faceting must be enabled for given field.");
            }

            this.ValueMapper = valueMapper;
        }

        public Func<TSource, IEnumerable<string>?> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new KeywordFieldStoreBuilder<TSource>(this, tokenEncoderStore);
        }
    }
}
