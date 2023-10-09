namespace Clara.Analysis
{
    public sealed class PorterStemTokenFilter : ITokenFilter
    {
        public void Process(in Token token, TokenFilterDelegate next)
        {
            if (token.Length > 2)
            {
                Step1(in token);
                Step2(in token);
                Step3(in token);
                Step4(in token);
                Step5(in token);
                Step6(in token);
            }
        }

        private static void Step1(in Token token)
        {
            if (token[token.Length - 1] == 's')
            {
                if (EndsWith(token, "sses", out _))
                {
                    token.Remove(token.Length - 2);
                }
                else if (EndsWith(token, "ies", out _))
                {
                    token.Remove(token.Length - 2);
                }
                else if (token.Length >= 2 && token[token.Length - 2] != 's')
                {
                    token.Remove(token.Length - 1);
                }
            }

            if (EndsWith(token, "eed", out var j))
            {
                if (NumberOfConsonantSequences(token, j) > 0)
                {
                    token.Remove(token.Length - 1);
                }
            }
            else if ((EndsWith(token, "ed", out j) || EndsWith(token, "ing", out j)) && ContainsVowel(token, j))
            {
                token.Remove(j + 1);

                if (EndsWith(token, "at", out _) || EndsWith(token, "bl", out _) || EndsWith(token, "iz", out _))
                {
                    token.Append("e");
                }
                else if (ContainsDoubleConsonantAt(token, token.Length - 1))
                {
                    var c = token[token.Length - 1];

                    if (c != 'l' && c != 's' && c != 'z')
                    {
                        token.Remove(token.Length - 1);
                    }
                }
                else if (NumberOfConsonantSequences(token, token.Length - 1) == 1 && HasCvcAt(token, token.Length - 1))
                {
                    token.Append("e");
                }
            }
        }

        private static void Step2(in Token token)
        {
            if (EndsWith(token, "y", out var j) && ContainsVowel(token, j))
            {
                token[token.Length - 1] = 'i';
            }
        }

        private static void Step3(in Token token)
        {
            if (token.Length < 2)
            {
                return;
            }

            switch (token[token.Length - 2])
            {
                case 'a':
                    if (ChangeSuffix(in token, "ational", "ate"))
                    {
                        break;
                    }

                    ChangeSuffix(in token, "tional", "tion");
                    break;

                case 'c':
                    if (ChangeSuffix(in token, "enci", "ence"))
                    {
                        break;
                    }

                    ChangeSuffix(in token, "anci", "ance");
                    break;

                case 'e':
                    ChangeSuffix(in token, "izer", "ize");
                    break;

                case 'l':
                    if (ChangeSuffix(in token, "bli", "ble"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(in token, "alli", "al"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(in token, "entli", "ent"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(in token, "eli", "e"))
                    {
                        break;
                    }

                    ChangeSuffix(in token, "ousli", "ous");
                    break;

                case 'o':
                    if (ChangeSuffix(in token, "ization", "ize"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(in token, "ation", "ate"))
                    {
                        break;
                    }

                    ChangeSuffix(in token, "ator", "ate");
                    break;

                case 's':
                    if (ChangeSuffix(in token, "alism", "al"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(in token, "iveness", "ive"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(in token, "fulness", "ful"))
                    {
                        break;
                    }

                    ChangeSuffix(in token, "ousness", "ous");
                    break;

                case 't':
                    if (ChangeSuffix(in token, "aliti", "al"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(in token, "iviti", "ive"))
                    {
                        break;
                    }

                    ChangeSuffix(in token, "biliti", "ble");
                    break;

                case 'g':
                    ChangeSuffix(in token, "logi", "log");
                    break;
            }
        }

        private static void Step4(in Token token)
        {
            if (token.Length < 1)
            {
                return;
            }

            switch (token[token.Length - 1])
            {
                case 'e':
                    if (ChangeSuffix(in token, "icate", "ic"))
                    {
                        break;
                    }
                    else if (RemoveSuffix(in token, "ative"))
                    {
                        break;
                    }

                    ChangeSuffix(in token, "alize", "al");
                    break;

                case 'i':
                    ChangeSuffix(in token, "iciti", "ic");
                    break;

                case 'l':
                    if (ChangeSuffix(in token, "ical", "ic"))
                    {
                        break;
                    }

                    RemoveSuffix(in token, "ful");
                    break;

                case 's':
                    RemoveSuffix(in token, "ness");
                    break;
            }
        }

        private static void Step5(in Token token)
        {
            if (token.Length < 2)
            {
                return;
            }

            int j;

            switch (token[token.Length - 2])
            {
                case 'a':
                    if (EndsWith(token, "al", out j))
                    {
                        break;
                    }

                    return;

                case 'c':
                    if (EndsWith(token, "ance", out j))
                    {
                        break;
                    }
                    else if (EndsWith(token, "ence", out j))
                    {
                        break;
                    }

                    return;

                case 'e':
                    if (EndsWith(token, "er", out j))
                    {
                        break;
                    }

                    return;

                case 'i':
                    if (EndsWith(token, "ic", out j))
                    {
                        break;
                    }

                    return;

                case 'l':
                    if (EndsWith(token, "able", out j))
                    {
                        break;
                    }
                    else if (EndsWith(token, "ible", out j))
                    {
                        break;
                    }

                    return;

                case 'n':
                    if (EndsWith(token, "ant", out j))
                    {
                        break;
                    }
                    else if (EndsWith(token, "ement", out j))
                    {
                        break;
                    }
                    else if (EndsWith(token, "ment", out j))
                    {
                        break;
                    }
                    else if (EndsWith(token, "ent", out j))
                    {
                        break;
                    }

                    return;

                case 'o':
                    if (EndsWith(token, "ion", out j) && j >= 0 && (token[j] == 's' || token[j] == 't'))
                    {
                        break;
                    }
                    else if (EndsWith(token, "ou", out j))
                    {
                        break;
                    }

                    return;

                case 's':
                    if (EndsWith(token, "ism", out j))
                    {
                        break;
                    }

                    return;

                case 't':
                    if (EndsWith(token, "ate", out j))
                    {
                        break;
                    }
                    else if (EndsWith(token, "iti", out j))
                    {
                        break;
                    }

                    return;

                case 'u':
                    if (EndsWith(token, "ous", out j))
                    {
                        break;
                    }

                    return;

                case 'v':
                    if (EndsWith(token, "ive", out j))
                    {
                        break;
                    }

                    return;

                case 'z':
                    if (EndsWith(token, "ize", out j))
                    {
                        break;
                    }

                    return;

                default:
                    return;
            }

            if (NumberOfConsonantSequences(token, j) > 1)
            {
                token.Remove(j + 1);
            }
        }

        private static void Step6(in Token token)
        {
            if (token.Length < 1)
            {
                return;
            }

            if (token[token.Length - 1] == 'e')
            {
                var a = NumberOfConsonantSequences(token, token.Length - 1);

                if (a > 1 || (a == 1 && !HasCvcAt(token, token.Length - 2)))
                {
                    token.Remove(token.Length - 1);
                }
            }

            if (token[token.Length - 1] == 'l')
            {
                if (ContainsDoubleConsonantAt(token, token.Length - 1) && NumberOfConsonantSequences(token, token.Length - 1) > 1)
                {
                    token.Remove(token.Length - 1);
                }
            }
        }

        private static bool ChangeSuffix(in Token token, string suffix, string replacement)
        {
            if (!EndsWith(token, suffix, out var j))
            {
                return false;
            }

            if (NumberOfConsonantSequences(token, j) < 1)
            {
                return false;
            }

            token.Write(j + 1, replacement);
            return true;
        }

        private static bool RemoveSuffix(in Token token, string suffix)
        {
            if (!EndsWith(token, suffix, out var j))
            {
                return false;
            }

            if (NumberOfConsonantSequences(token, j) < 1)
            {
                return false;
            }

            token.Remove(j + 1);
            return true;
        }

        private static bool EndsWith(ReadOnlySpan<char> span, string suffix, out int j)
        {
            var chars = suffix.AsSpan();

            if (span.EndsWith(chars))
            {
                j = span.Length - chars.Length - 1;
                return true;
            }

            j = 0;
            return false;
        }

        private static bool ContainsVowel(ReadOnlySpan<char> span, int j)
        {
            for (var i = 0; i <= j; i++)
            {
                if (!IsConsonant(span, i))
                {
                    return true;
                }
            }

            return false;
        }

        private static int NumberOfConsonantSequences(ReadOnlySpan<char> span, int j)
        {
            var n = 0;
            var i = 0;

            while (true)
            {
                if (i > j)
                {
                    return n;
                }

                if (!IsConsonant(span, i))
                {
                    break;
                }

                i++;
            }

            i++;

            while (true)
            {
                while (true)
                {
                    if (i > j)
                    {
                        return n;
                    }

                    if (IsConsonant(span, i))
                    {
                        break;
                    }

                    i++;
                }

                i++;
                n++;

                while (true)
                {
                    if (i > j)
                    {
                        return n;
                    }

                    if (!IsConsonant(span, i))
                    {
                        break;
                    }

                    i++;
                }

                i++;
            }
        }

        private static bool HasCvcAt(ReadOnlySpan<char> span, int i)
        {
            if (i < 2 || !IsConsonant(span, i) || IsConsonant(span, i - 1) || !IsConsonant(span, i - 2))
            {
                return false;
            }
            else
            {
                var c = span[i];

                if (c == 'w' || c == 'x' || c == 'y')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ContainsDoubleConsonantAt(ReadOnlySpan<char> span, int i)
        {
            if (i < 1)
            {
                return false;
            }

            if (span[i] != span[i - 1])
            {
                return false;
            }

            return IsConsonant(span, i);
        }

        private static bool IsConsonant(ReadOnlySpan<char> span, int i)
        {
            return
                span[i] switch
                {
                    'a' or 'e' or 'i' or 'o' or 'u' => false,
                    'y' => i == 0 || !IsConsonant(span, i - 1),
                    _ => true,
                };
        }
    }
}
