namespace Clara.Utils
{
    public interface IReadOnlyHashCollection<TItem> : IReadOnlyCollection<TItem>
        where TItem : notnull
    {
        bool Contains(TItem item);
    }
}
