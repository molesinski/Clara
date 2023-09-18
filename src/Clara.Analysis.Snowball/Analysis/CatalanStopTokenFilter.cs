using System.Text;

namespace Clara.Analysis
{
    public sealed class CatalanStopTokenFilter : ResourceStopTokenFilter
    {
        public CatalanStopTokenFilter()
            : base(
                  typeof(CatalanStopTokenFilter).Assembly,
                  $"{typeof(CatalanStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
