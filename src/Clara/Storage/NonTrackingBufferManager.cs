using System.Collections.Generic;

namespace Clara.Storage
{
    public sealed class NonTrackingBufferManager : BufferManager
    {
        internal override BufferScope CreateScope()
        {
            return new NonTrackingBufferScope();
        }

        private sealed class NonTrackingBufferScope : BufferScope
        {
            public override HashSet<int> CreateDocumentSet()
            {
                return new HashSet<int>();
            }

            public override HashSet<int> CreateDocumentSet(IReadOnlyCollection<int> collection)
            {
#if NETSTANDARD2_1_OR_GREATER || NET472_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                if (collection is not HashSet<int>)
                {
                    var result = new HashSet<int>(capacity: collection.Count);

                    foreach (var item in collection)
                    {
                        result.Add(item);
                    }

                    return result;
                }
#endif

                return new HashSet<int>(collection);
            }
        }
    }
}
