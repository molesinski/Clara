using System;
using System.Buffers;

namespace Clara.Utils
{
    public abstract class Allocator
    {
        public static Allocator New { get; } = new NewAllocator();

        public static Allocator ArrayPool { get; } = new ArrayPoolAllocator();

        public static Allocator Mixed { get; } = new MixedAllocator();

        public abstract int MinimumSize { get; }

        public abstract TItem[] Allocate<TItem>(int size, bool clearArray = false);

        public abstract void Release<TItem>(TItem[] array);

        private sealed class NewAllocator : Allocator
        {
            public override int MinimumSize
            {
                get
                {
                    return 4;
                }
            }

            public override TItem[] Allocate<TItem>(int size, bool clearArray = false)
            {
                return new TItem[size];
            }

            public override void Release<TItem>(TItem[] array)
            {
            }
        }

        private sealed class ArrayPoolAllocator : Allocator
        {
            public override int MinimumSize
            {
                get
                {
                    return 16;
                }
            }

            public override TItem[] Allocate<TItem>(int size, bool clearArray = false)
            {
                var array = ArrayPool<TItem>.Shared.Rent(size);

                if (clearArray)
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

        private sealed class MixedAllocator : Allocator
        {
            public override int MinimumSize
            {
                get
                {
                    return 4;
                }
            }

            public override TItem[] Allocate<TItem>(int size, bool clearArray = false)
            {
                if (size < 16)
                {
                    return new TItem[size];
                }

                var array = ArrayPool<TItem>.Shared.Rent(size);

                if (clearArray)
                {
                    Array.Clear(array, 0, size);
                }

                return array;
            }

            public override void Release<TItem>(TItem[] array)
            {
                if (array.Length < 16)
                {
                    return;
                }

                ArrayPool<TItem>.Shared.Return(array, clearArray: false);
            }
        }
    }
}
