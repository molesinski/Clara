namespace Clara.Storage
{
    internal interface ITokenEncoderBuilder
    {
        ITokenEncoder Build();

        int Encode(string token);
    }
}
