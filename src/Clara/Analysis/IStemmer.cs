namespace Clara.Analysis
{
    public interface IStemmer
    {
        Token Stem(Token token);
    }
}
