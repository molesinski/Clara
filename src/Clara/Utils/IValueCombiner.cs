namespace Clara.Utils
{
    public interface IValueCombiner<TValue>
    {
        bool IsDefaultNeutral { get; }

        TValue Combine(TValue a, TValue b);
    }
}
