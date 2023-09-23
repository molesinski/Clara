namespace Clara.Mapping
{
    public interface IIndexMapper<TSource, TDocument>
    {
        IEnumerable<Field> GetFields();

        string GetDocumentKey(TSource item);

        TDocument GetDocument(TSource item);
    }

    public interface IIndexMapper<TSource> : IIndexMapper<TSource, TSource>
    {
    }
}
