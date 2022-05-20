using System;

namespace Clara.Mapping
{
    internal sealed class KeywordFieldValue : FieldValue
    {
        public KeywordFieldValue(KeywordField field, string[] keywords)
            : base(field)
        {
            if (keywords is null)
            {
                throw new ArgumentNullException(nameof(keywords));
            }

            this.Keywords = keywords;
        }

        public string[] Keywords { get; }
    }
}
