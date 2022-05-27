using System;
using System.Collections.Generic;

namespace Clara.Storage
{
    internal interface IDocumentSet : IReadOnlyCollection<int>, IDisposable
    {
    }
}
