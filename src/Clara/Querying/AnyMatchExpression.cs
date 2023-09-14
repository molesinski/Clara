﻿namespace Clara.Querying
{
    public sealed class AnyMatchExpression : MatchExpression
    {
        private readonly IReadOnlyCollection<string> values;

        internal AnyMatchExpression(IReadOnlyCollection<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.values = values;
        }

        public IReadOnlyCollection<string> Values
        {
            get
            {
                return this.values;
            }
        }
    }
}