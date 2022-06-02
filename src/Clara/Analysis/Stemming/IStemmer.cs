namespace Clara.Analysis.Stemming
{
    public interface IStemmer
    {
        Token Stem(Token token);
    }
}
