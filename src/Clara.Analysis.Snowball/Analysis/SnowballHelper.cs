using System.Linq.Expressions;
using System.Text;
using Snowball;

namespace Clara.Analysis
{
    internal static class SnowballHelper
    {
        public static readonly Action<Stemmer, Token> SetBufferContents = CreateBufferContentsSetter();

        private static Action<Stemmer, Token> CreateBufferContentsSetter()
        {
            var getCurrent = CreateFieldGetter<Stemmer, StringBuilder>("current");
            var setCursor = CreateFieldSetter<Stemmer, int>("cursor");
            var setLimit = CreateFieldSetter<Stemmer, int>("limit");
            var setLimitBackward = CreateFieldSetter<Stemmer, int>("limit_backward");
            var setBra = CreateFieldSetter<Stemmer, int>("bra");
            var setKet = CreateFieldSetter<Stemmer, int>("ket");

            return
                (stemmer, token) =>
                {
                    var current = getCurrent(stemmer);

                    current.Clear();
                    token.CopyTo(current);

                    setCursor(stemmer, 0);
                    setLimit(stemmer, current.Length);
                    setLimitBackward(stemmer, 0);
                    setBra(stemmer, 0);
                    setKet(stemmer, current.Length);
                };
        }

        private static Func<TTarget, TValue> CreateFieldGetter<TTarget, TValue>(string fieldName)
        {
            var targetExpression = Expression.Parameter(typeof(TTarget), "target");
            var fieldExpression = Expression.Field(targetExpression, fieldName);

            return Expression.Lambda<Func<TTarget, TValue>>(fieldExpression, targetExpression).Compile();
        }

        private static Action<TTarget, TValue> CreateFieldSetter<TTarget, TValue>(string fieldName)
        {
            var targetExpression = Expression.Parameter(typeof(TTarget), "target");
            var valueExpression = Expression.Parameter(typeof(int), "value");
            var fieldExpression = Expression.Field(targetExpression, fieldName);
            var assignExpression = Expression.Assign(fieldExpression, valueExpression);

            return Expression.Lambda<Action<TTarget, TValue>>(assignExpression, targetExpression, valueExpression).Compile();
        }
    }
}
