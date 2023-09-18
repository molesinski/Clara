namespace Clara.Utils
{
    public interface IReadOnlyHashCollection<TItem> : IReadOnlyCollection<TItem>
        where TItem : notnull, IEquatable<TItem>
    {
        bool Contains(TItem item);
    }
}
