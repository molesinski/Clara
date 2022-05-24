namespace Clara.Analysis.Stemming
{
    public interface IStemmer
    {
        StemResult Stem(string token);
    }
}
