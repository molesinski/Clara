using System.Collections.Generic;

namespace Clara.Mapping
{
    public interface IIndexMapper<TSource, TDocument>
    {
        IEnumerable<Field> GetFields();

        string GetDocumentKey(TSource item);

        TDocument GetDocument(TSource item);
    }
}
