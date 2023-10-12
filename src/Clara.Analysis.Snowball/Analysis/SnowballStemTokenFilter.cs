using System.Linq.Expressions;
using System.Text;
using Clara.Utils;
using Snowball;

namespace Clara.Analysis
{
    public abstract class SnowballStemTokenFilter<TStemmer> : ITokenFilter
        where TStemmer : Stemmer, new()
    {
        private static readonly ObjectPool<TStemmer> Pool = new(() => new());
        private static readonly Action<TStemmer, Token> SetBufferContents = CreateBufferContentsSetter();

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var stemmer = Pool.Lease();

            SetBufferContents(stemmer.Instance, token);

            stemmer.Instance.Stem();

            var buffer = stemmer.Instance.Buffer;

            if (buffer.Length > 0 && buffer.Length <= Token.MaximumLength)
            {
                token.Set(buffer);
            }

            return token;
        }

        private static Action<TStemmer, Token> CreateBufferContentsSetter()
        {
            var getCurrent = CreateFieldGetter<StringBuilder>("current");
            var setCursor = CreateFieldSetter<int>("cursor");
            var setLimit = CreateFieldSetter<int>("limit");
            var setLimitBackward = CreateFieldSetter<int>("limit_backward");
            var setBra = CreateFieldSetter<int>("bra");
            var setKet = CreateFieldSetter<int>("ket");

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

        private static Func<TStemmer, TValue> CreateFieldGetter<TValue>(string fieldName)
        {
            var targetExpression = Expression.Parameter(typeof(TStemmer), "target");
            var fieldExpression = Expression.Field(targetExpression, fieldName);

            return Expression.Lambda<Func<TStemmer, TValue>>(fieldExpression, targetExpression).Compile();
        }

        private static Action<TStemmer, TValue> CreateFieldSetter<TValue>(string fieldName)
        {
            var targetExpression = Expression.Parameter(typeof(TStemmer), "target");
            var valueExpression = Expression.Parameter(typeof(int), "value");
            var fieldExpression = Expression.Field(targetExpression, fieldName);
            var assignExpression = Expression.Assign(fieldExpression, valueExpression);

            return Expression.Lambda<Action<TStemmer, TValue>>(assignExpression, targetExpression, valueExpression).Compile();
        }
    }
}
