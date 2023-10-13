using System.Collections;
using Clara.Analysis;
using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct SearchTermStoreIndexEnumerable
    {
        private readonly ListSlim<SearchTermStoreIndex> source;

        public SearchTermStoreIndexEnumerable(ListSlim<SearchTermStoreIndex> source)
        {
            this.source = source;
        }

        public PositionRangeEnumerator GetEnumerator()
        {
            return new PositionRangeEnumerator(this.source);
        }

        public readonly record struct PositionRange
        {
            private readonly ListSlim<SearchTermStoreIndex> source;
            private readonly int offset;
            private readonly int count;

            public PositionRange(ListSlim<SearchTermStoreIndex> source, int position, int offset, int count)
            {
                this.source = source;
                this.Position = position;
                this.offset = offset;
                this.count = count;
            }

            public int Position { get; }

            public StoreRangeIndexEnumerator GetEnumerator()
            {
                return new StoreRangeIndexEnumerator(this.source, this.offset, this.count);
            }
        }

        public readonly record struct StoreRange
        {
            private readonly ListSlim<SearchTermStoreIndex> source;
            private readonly int offset;
            private readonly int count;

            public StoreRange(ListSlim<SearchTermStoreIndex> source, int storeIndex, int offset, int count)
            {
                this.source = source;
                this.StoreIndex = storeIndex;
                this.offset = offset;
                this.count = count;
            }

            public int StoreIndex { get; }

            public SearchTermEnumerator GetEnumerator()
            {
                return new SearchTermEnumerator(this.source.GetRangeEnumerator(this.offset, this.count));
            }
        }

        public struct PositionRangeEnumerator : IEnumerator<PositionRange>
        {
            private readonly ListSlim<SearchTermStoreIndex> source;
            private PositionRange current;
            private int position = -1;
            private int offset;
            private int count;
            private int i;

            internal PositionRangeEnumerator(ListSlim<SearchTermStoreIndex> source)
            {
                this.source = source;
            }

            public readonly PositionRange Current
            {
                get
                {
                    return this.current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public bool MoveNext()
            {
                while (this.i < this.source.Count)
                {
                    var position = this.source[this.i].SearchTerm.Position;

                    if (this.position == -1)
                    {
                        this.position = position;
                        this.offset = this.i;
                        this.count = 1;
                        this.i++;
                    }
                    else if (this.position == position)
                    {
                        this.count++;
                        this.i++;
                    }
                    else
                    {
                        this.current = new PositionRange(this.source, this.position, this.offset, this.count);
                        this.position = -1;

                        return true;
                    }
                }

                if (this.position != -1)
                {
                    this.current = new PositionRange(this.source, this.position, this.offset, this.count);
                    this.position = -1;

                    return true;
                }

                this.current = default;

                return false;
            }

            public void Reset()
            {
                this.current = default;
                this.position = -1;
                this.offset = default;
                this.count = default;
                this.i = default;
            }

            public void Dispose()
            {
            }
        }

        public struct StoreRangeIndexEnumerator : IEnumerator<StoreRange>
        {
            private readonly ListSlim<SearchTermStoreIndex> source;
            private readonly int sourceOffset;
            private readonly int sourceCount;
            private StoreRange current;
            private int storeIndex;
            private int offset;
            private int count;
            private int i;

            internal StoreRangeIndexEnumerator(ListSlim<SearchTermStoreIndex> source, int sourceOffset, int sourceCount)
            {
                this.source = source;
                this.sourceOffset = sourceOffset;
                this.sourceCount = sourceCount;
                this.storeIndex = -1;
                this.i = sourceOffset;
            }

            public readonly StoreRange Current
            {
                get
                {
                    return this.current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public bool MoveNext()
            {
                while (this.i < this.sourceOffset + this.sourceCount)
                {
                    var storeIndex = this.source[this.i].StoreIndex;

                    if (this.storeIndex == -1)
                    {
                        this.storeIndex = storeIndex;
                        this.offset = this.i;
                        this.count = 1;
                        this.i++;
                    }
                    else if (this.storeIndex == storeIndex)
                    {
                        this.count++;
                        this.i++;
                    }
                    else
                    {
                        this.current = new StoreRange(this.source, this.storeIndex, this.offset, this.count);
                        this.storeIndex = -1;

                        return true;
                    }
                }

                if (this.storeIndex != -1)
                {
                    this.current = new StoreRange(this.source, this.storeIndex, this.offset, this.count);
                    this.storeIndex = -1;

                    return true;
                }

                this.current = default;

                return false;
            }

            public void Reset()
            {
                this.current = default;
                this.storeIndex = -1;
                this.offset = default;
                this.count = default;
                this.i = this.sourceOffset;
            }

            public void Dispose()
            {
            }
        }

        public struct SearchTermEnumerator : IEnumerator<SearchTerm>
        {
            private ListSlim<SearchTermStoreIndex>.Enumerator enumerator;

            internal SearchTermEnumerator(ListSlim<SearchTermStoreIndex>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly SearchTerm Current
            {
                get
                {
                    return this.enumerator.Current.SearchTerm;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.enumerator.Current.SearchTerm;
                }
            }

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
            }
        }
    }
}
