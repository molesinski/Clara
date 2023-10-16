namespace Clara.Analysis
{
    public interface ITokenizer : IEquatable<ITokenizer>
    {
        ITokenTermSource CreateTokenTermSource();
    }
}
