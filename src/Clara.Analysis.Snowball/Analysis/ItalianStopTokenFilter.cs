using System.Text;

namespace Clara.Analysis
{
    public sealed class ItalianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public ItalianStopTokenFilter()
            : base(
                  typeof(ItalianStopTokenFilter).Assembly,
                  $"{typeof(ItalianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
