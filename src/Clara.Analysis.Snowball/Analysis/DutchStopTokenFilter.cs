using System.Text;

namespace Clara.Analysis
{
    public sealed class DutchStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public DutchStopTokenFilter()
            : base(
                  typeof(DutchStopTokenFilter).Assembly,
                  $"{typeof(DutchStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
