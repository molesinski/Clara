namespace Clara.Storage
{
    internal interface ITokenEncoderBuilder : IDisposable
    {
        ITokenEncoder Build();

        int Encode(string token);
    }
}
