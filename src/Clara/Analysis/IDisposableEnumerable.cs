namespace Clara.Analysis
{
    public interface IDisposableEnumerable<TValue> : IEnumerable<TValue>, IDisposable
    {
    }
}
