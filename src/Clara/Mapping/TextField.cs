using System;
using Clara.Analysis;
using Clara.Storage;

namespace Clara.Mapping
{
    public sealed class TextField : TokenField
    {
        public TextField(ITokenizer tokenizer)
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

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderStore tokenEncoderStore, ISynonymMap? synonymMap)
        {
            return new TextFieldStoreBuilder(this, tokenEncoderStore, synonymMap);
        }
    }
}
