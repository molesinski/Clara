using Clara.Mapping;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Querying
{
    internal sealed class FilterExpressionComparer : IComparer<FilterExpression>, IResettable
    {
        private static readonly HashSet<Field> EmptyFacetFields = new();
        private static readonly Dictionary<Field, FieldStore> EmptyFieldStores = new();

        private HashSet<Field> facetFields;
        private Dictionary<Field, FieldStore> fieldStores;

        public FilterExpressionComparer()
        {
            this.facetFields = EmptyFacetFields;
            this.fieldStores = EmptyFieldStores;
        }

        public void Initialize(HashSet<Field> facetFields, Dictionary<Field, FieldStore> fieldStores)
        {
            if (facetFields is null)
            {
                throw new ArgumentNullException(nameof(facetFields));
            }

            if (fieldStores is null)
            {
                throw new ArgumentNullException(nameof(fieldStores));
            }

            this.facetFields = facetFields;
            this.fieldStores = fieldStores;
        }

        void IResettable.Reset()
        {
            this.facetFields = EmptyFacetFields;
            this.fieldStores = EmptyFieldStores;
        }

        public int Compare(FilterExpression? x, FilterExpression? y)
        {
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            var a = x.HasPersistedFacets && this.facetFields.Contains(x.Field) ? 1 : 0;
            var b = y.HasPersistedFacets && this.facetFields.Contains(y.Field) ? 1 : 0;

            var result = a.CompareTo(b);

            if (result != 0)
            {
                return result;
            }

            var c = this.fieldStores.TryGetValue(x.Field, out var store1) ? (store1.FilterOrder ?? double.MaxValue) : double.MinValue;
            var d = this.fieldStores.TryGetValue(y.Field, out var store2) ? (store2.FilterOrder ?? double.MaxValue) : double.MinValue;

            return c.CompareTo(d);
        }
    }
}
