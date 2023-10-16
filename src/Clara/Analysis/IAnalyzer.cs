namespace Clara.Analysis
{
    public interface IAnalyzer
    {
        ITokenizer Tokenizer { get; }

        ITokenTermSource CreateTokenTermSource();
    }
}
