using Clara.Analysis;

namespace Clara.Storage
{
    internal interface ITokenEncoder
    {
        string Decode(int id);

        Token? ToReadOnly(Token token);

        bool TryEncode(Token token, out int id);
    }
}
