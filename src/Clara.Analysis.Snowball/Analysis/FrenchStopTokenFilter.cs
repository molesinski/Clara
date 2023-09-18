using System.Text;

namespace Clara.Analysis
{
    public sealed class FrenchStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public FrenchStopTokenFilter()
            : base(
                  typeof(FrenchStopTokenFilter).Assembly,
                  $"{typeof(FrenchStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
