using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class TextSearchExpression : IDisposable
    {
        private readonly TextSearchFields fields;
        private bool isDisposed;

        public TextSearchExpression(TextField field, SearchMode searchMode, string text)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (searchMode != SearchMode.All && searchMode != SearchMode.Any)
            {
                throw new ArgumentException("Illegal search mode enum value.", nameof(searchMode));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.fields = new TextSearchFields(field);
            this.SearchMode = searchMode;
            this.Text = text;

            this.ValidateFields();
        }

        public TextSearchExpression(IEnumerable<TextField> fields, SearchMode searchMode, string text)
        {
            if (fields is null)
            {
                throw new ArgumentNullException(nameof(fields));
            }

            if (searchMode != SearchMode.All && searchMode != SearchMode.Any)
            {
                throw new ArgumentException("Illegal search mode enum value.", nameof(searchMode));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.fields = new TextSearchFields(fields);
            this.SearchMode = searchMode;
            this.Text = text;

            this.ValidateFields();
        }

        public TextSearchExpression(IEnumerable<TextSearchField> fields, SearchMode searchMode, string text)
        {
            if (fields is null)
            {
                throw new ArgumentNullException(nameof(fields));
            }

            if (searchMode != SearchMode.All && searchMode != SearchMode.Any)
            {
                throw new ArgumentException("Illegal search mode enum value.", nameof(searchMode));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.fields = new TextSearchFields(fields);
            this.SearchMode = searchMode;
            this.Text = text;

            this.ValidateFields();
        }

        public IReadOnlyList<TextSearchField> Fields
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                return this.fields.Value;
            }
        }

        public SearchMode SearchMode { get; }

        public string Text { get; }

        internal bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Text);
            }
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.fields.Dispose();

                this.isDisposed = true;
            }
        }

        private void ValidateFields()
        {
            if (this.fields.Value.Count == 0)
            {
                throw new ArgumentException("At least one field must be specified.", nameof(this.fields));
            }

            var tokenizer = default(ITokenizer?);

            foreach (var field in this.fields.Value)
            {
                if (tokenizer is null)
                {
                    tokenizer = field.Field.Analyzer.Tokenizer;
                }
                else
                {
                    if (!field.Field.Analyzer.Tokenizer.Equals(tokenizer))
                    {
                        throw new ArgumentException("All fields must use same tokenizer.", nameof(this.fields));
                    }
                }
            }
        }
    }
}
