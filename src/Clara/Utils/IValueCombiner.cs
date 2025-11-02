namespace Clara.Utils
{
    public interface IValueCombiner<TValue>
    {
        TValue Combine(TValue current, TValue value, TValue valueBoost);
    }
}
