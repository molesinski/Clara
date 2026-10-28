using Clara.Analysis;
using Clara.Mapping;
using Clara.Storage;

namespace Clara.Querying
{
    public sealed class TextSearchExpression : SearchExpression
    {
        public TextSearchExpression(TextField field, SearchMode searchMode, string text, Func<Position, float>? positionBoost = null, ScoreAggregation? searchScoreAggregation = null, bool isFuzzy = false, PrefixMode prefixMode = PrefixMode.None)
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

            if (prefixMode != PrefixMode.None && prefixMode != PrefixMode.Last && prefixMode != PrefixMode.All)
            {
                throw new ArgumentException("Illegal prefix mode enum value.", nameof(prefixMode));
            }

            if ((isFuzzy || prefixMode != PrefixMode.None) && field.TokenCorrection is null)
            {
                throw new ArgumentException("Field must have token correction configured to use fuzzy or prefix matching.", isFuzzy ? nameof(isFuzzy) : nameof(prefixMode));
            }

            this.Field = field;
            this.SearchMode = searchMode;
            this.Text = text;
            this.PositionBoost = positionBoost;
            this.SearchScoreAggregation = searchScoreAggregation;
            this.IsFuzzy = isFuzzy;
            this.PrefixMode = prefixMode;
        }

        public TextField Field { get; }

        public SearchMode SearchMode { get; }

        public string Text { get; }

        public Func<Position, float>? PositionBoost { get; }

        public ScoreAggregation? SearchScoreAggregation { get; }

        public bool IsFuzzy { get; }

        public PrefixMode PrefixMode { get; }

        internal override bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Text);
            }
        }

        internal override DocumentScoring Search(Index index)
        {
            var store = index.GetFieldStore(this.Field);

            return store.Search(this);
        }
    }
}
