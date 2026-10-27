using Clara.Mapping;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class SuggestExpression
    {
        public SuggestExpression(PhraseField field, string text, int count)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            this.Field = field;
            this.Text = text;
            this.Count = count;
        }

        public PhraseField Field { get; }

        public string Text { get; }

        public int Count { get; }

        internal List<SuggestionResult> Suggest(Index index, HashSetSlim<int>? allowedDocuments)
        {
            if (index.GetFieldStore(this.Field) is not PhraseFieldStore store)
            {
                throw new InvalidOperationException("Field does not support suggestions.");
            }

            return store.Suggest(this.Text, this.Count, allowedDocuments);
        }
    }
}
