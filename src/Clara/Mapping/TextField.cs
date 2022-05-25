using System;
using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class TextField : TokenField
    {
        protected internal TextField(ITokenizer tokenizer)
            : base(
                isFilterable: true,
                isFacetable: false,
                isSortable: false)
        {
            if (tokenizer is null)
            {
                throw new ArgumentNullException(nameof(tokenizer));
            }

            this.Tokenizer = tokenizer;
        }

        public ITokenizer Tokenizer { get; }
    }

    public sealed class TextField<TSource> : TextField
    {
        public TextField(Func<TSource, string?> valueMapper, ITokenizer tokenizer)
            : base(tokenizer)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = valueMapper;
        }

        public Func<TSource, string?> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new TextFieldStoreBuilder<TSource>(this, tokenEncoderStore, synonymMap);
        }
    }
}
