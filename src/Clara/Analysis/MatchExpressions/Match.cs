using Clara.Querying;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public static class Match
    {
        public static MatchExpression Empty
        {
            get
            {
                return EmptyMatchExpression.Instance;
            }
        }

        public static MatchExpression All(IEnumerable<string>? tokens)
        {
            return All(new StringEnumerable(tokens));
        }

        public static MatchExpression Any(IEnumerable<string>? tokens)
        {
            return Any(new StringEnumerable(tokens));
        }

        public static MatchExpression And(IEnumerable<MatchExpression?>? expressions)
        {
            return And(new ObjectEnumerable<MatchExpression>(expressions));
        }

        public static MatchExpression Or(IEnumerable<MatchExpression?>? expressions)
        {
            return Or(new ObjectEnumerable<MatchExpression>(expressions));
        }

        public static MatchExpression Search(SearchMode mode, IList<SearchTerm> terms)
        {
            if (terms is null)
            {
                throw new ArgumentNullException(nameof(terms));
            }

            var expressions = new ListSlim<MatchExpression>();

            var start = int.MaxValue;
            var end = int.MinValue;

            for (var i = 0; i < terms.Count; i++)
            {
                var term = terms[i];

                start = start < term.Position.Start ? start : term.Position.Start;
                end = end > term.Position.End ? end : term.Position.End;
            }

            for (var position = start; position <= end; position++)
            {
                var tokens = new ListSlim<string>();

                for (var i = 0; i < terms.Count; i++)
                {
                    var term = terms[i];

                    if (term.Position.Overlaps(position))
                    {
                        tokens.Add(term.Token);
                    }
                }

                if (tokens.Count > 0)
                {
                    expressions.Add(new AnyTokensMatchExpression(tokens));
                }
            }

            if (expressions.Count == 0)
            {
                return EmptyMatchExpression.Instance;
            }

            return
                mode == SearchMode.All
                    ? new AndMatchExpression(expressions)
                    : new OrMatchExpression(expressions);
        }

        private static MatchExpression All(StringEnumerable tokens)
        {
            var result = default(ListSlim<string>?);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AllTokensMatchExpression(result);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Any(StringEnumerable tokens)
        {
            var result = default(ListSlim<string>?);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AnyTokensMatchExpression(result);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression And(ObjectEnumerable<MatchExpression> expressions)
        {
            var queue = new Queue<MatchExpression>();
            var result = default(ListSlim<MatchExpression>?);

            foreach (var expression in expressions)
            {
                queue.Enqueue(expression);
            }

            while (queue.Count > 0)
            {
                var expression = queue.Dequeue();

                if (expression is EmptyMatchExpression)
                {
                    continue;
                }

                if (expression is AndMatchExpression andMatchExpression)
                {
                    for (var i = 0; i < andMatchExpression.Expressions.Count; i++)
                    {
                        queue.Enqueue(andMatchExpression.Expressions[i]);
                    }

                    continue;
                }

                result ??= new();
                result.Add(expression);
            }

            if (result is not null)
            {
                if (result.Count == 1)
                {
                    return result[0];
                }

                return new AndMatchExpression(result);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Or(ObjectEnumerable<MatchExpression> expressions)
        {
            var queue = new Queue<MatchExpression>();
            var result = default(ListSlim<MatchExpression>?);

            foreach (var expression in expressions)
            {
                queue.Enqueue(expression);
            }

            while (queue.Count > 0)
            {
                var expression = queue.Dequeue();

                if (expression is EmptyMatchExpression)
                {
                    continue;
                }

                if (expression is OrMatchExpression orMatchExpression)
                {
                    for (var i = 0; i < orMatchExpression.Expressions.Count; i++)
                    {
                        queue.Enqueue(orMatchExpression.Expressions[i]);
                    }

                    continue;
                }

                result ??= new();
                result.Add(expression);
            }

            if (result is not null)
            {
                if (result.Count == 1)
                {
                    return result[0];
                }

                return new OrMatchExpression(result);
            }

            return EmptyMatchExpression.Instance;
        }
    }
}
