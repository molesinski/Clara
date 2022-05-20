using System;

namespace Clara.Mapping
{
    internal sealed class TextFieldValue : FieldValue
    {
        public TextFieldValue(TextField field, string text)
            : base(field)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.Text = text;
        }

        public string Text { get; }
    }
}
