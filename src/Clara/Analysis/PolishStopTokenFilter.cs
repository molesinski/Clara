using System;
using System.Text;

namespace Clara.Analysis
{
    public sealed class PolishStopTokenFilter : ResourceStopTokenFilter
    {
        public PolishStopTokenFilter()
            : base(
                  typeof(PolishStopTokenFilter).Assembly,
                  $"{typeof(PolishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8,
                  StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
