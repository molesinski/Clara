using System;
using System.Text;

namespace Clara.Analysis
{
    public interface IStringFactory
    {
        string Create(string value);

        string Create(StringBuilder builder);

        string Create(char[] chars, int index, int count);

        string Create(byte[] bytes, int index, int count, Encoding encoding);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        string Create(ReadOnlySpan<char> chars);

        string Create(ReadOnlySpan<byte> bytes, Encoding encoding);
#endif
    }
}
