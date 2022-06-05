using System;
using System.Buffers;

namespace Clara.Utils
{
    public abstract class Allocator
    {
        public static Allocator Default { get; } = new DefaultAllocator();

        public static Allocator ArrayPool { get; } = new ArrayPoolAllocator();

        public abstract int MinimumCapacity { get; }

        public int Size(int capacity)
        {
            return HashHelper.PowerOf2(Math.Max(capacity, this.MinimumCapacity));
        }

        public abstract TItem[] Allocate<TItem>(int size, bool clear = false);

        public abstract void Release<TItem>(TItem[] array);

        private sealed class DefaultAllocator : Allocator
        {
            public override int MinimumCapacity
            {
                get
                {
                    return 2;
                }
            }

            public override TItem[] Allocate<TItem>(int size, bool clear = false)
            {
                return new TItem[size];
            }

            public override void Release<TItem>(TItem[] array)
            {
            }
        }

        private sealed class ArrayPoolAllocator : Allocator
        {
            public override int MinimumCapacity
            {
                get
                {
                    return 16;
                }
            }

            public override TItem[] Allocate<TItem>(int size, bool clear = false)
            {
                var array = ArrayPool<TItem>.Shared.Rent(size);

                if (clear)
                {
                    Array.Clear(array, 0, size);
                }

                return array;
            }

            public override void Release<TItem>(TItem[] array)
            {
                ArrayPool<TItem>.Shared.Return(array, clearArray: false);
            }
        }
    }
}
