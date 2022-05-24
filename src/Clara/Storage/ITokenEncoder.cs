namespace Clara.Storage
{
    internal interface ITokenEncoder
    {
        string Decode(int id);

        bool TryEncode(string token, out int id);
    }
}
