﻿namespace Clara.Analysis
{
    public readonly record struct TokenTerm
    {
        public TokenTerm(Token token, TokenPosition position)
        {
            this.Token = token;
            this.Position = position;
        }

        public Token Token { get; }

        public TokenPosition Position { get; }
    }
}
