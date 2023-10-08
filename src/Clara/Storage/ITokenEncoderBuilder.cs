using Clara.Analysis;

namespace Clara.Storage
{
    internal interface ITokenEncoderBuilder
    {
        ITokenEncoder Build();

        int Encode(Token token);
    }
}
