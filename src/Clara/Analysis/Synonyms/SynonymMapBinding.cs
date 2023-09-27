using Clara.Mapping;

namespace Clara.Analysis.Synonyms
{
    public class SynonymMapBinding
    {
        public SynonymMapBinding(ISynonymMap synonymMap, TextField field)
        {
            if (synonymMap is null)
            {
                throw new ArgumentNullException(nameof(synonymMap));
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!ReferenceEquals(field.Analyzer, synonymMap.Analyzer))
            {
                throw new ArgumentException("Synonym map must use same analyzer instance as text field.", nameof(synonymMap));
            }

            this.SynonymMap = synonymMap;
            this.Field = field;
        }

        public ISynonymMap SynonymMap { get; }

        public TextField Field { get; }
    }
}
