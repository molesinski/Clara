using System;

namespace Clara.Storage
{
    internal readonly struct MinMax<TValue>
        where TValue : struct, IComparable<TValue>
    {
        public MinMax(TValue min, TValue max)
        {
            this.Min = min;
            this.Max = max;
        }

        public TValue Min { get; }

        public TValue Max { get; }
    }
}
