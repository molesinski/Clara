namespace Clara.Storage
{
    internal interface ITokenEncoder : IDisposable
    {
        string Decode(int id);

        bool TryEncode(string token, out int id);
    }
}
