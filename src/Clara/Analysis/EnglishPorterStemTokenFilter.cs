namespace Clara.Analysis
{
    public class EnglishPorterStemTokenFilter : ITokenFilter
    {
        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (token.Length > 2)
            {
                Step1(ref token);
                Step2(ref token);
                Step3(ref token);
                Step4(ref token);
                Step5(ref token);
                Step6(ref token);
            }

            return token;
        }

        private static void Step1(ref Token token)
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
                if (NumberOfConsoantSequences(token, j) > 0)
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
                else if (NumberOfConsoantSequences(token, token.Length - 1) == 1 && HasCvcAt(token, token.Length - 1))
                {
                    token.Append("e");
                }
            }
        }

        private static void Step2(ref Token token)
        {
            if (EndsWith(token, "y", out var j) && ContainsVowel(token, j))
            {
                token[token.Length - 1] = 'i';
            }
        }

        private static void Step3(ref Token token)
        {
            if (token.Length < 2)
            {
                return;
            }

            switch (token[token.Length - 2])
            {
                case 'a':
                    if (ChangeSuffix(ref token, "ational", "ate"))
                    {
                        break;
                    }

                    ChangeSuffix(ref token, "tional", "tion");
                    break;

                case 'c':
                    if (ChangeSuffix(ref token, "enci", "ence"))
                    {
                        break;
                    }

                    ChangeSuffix(ref token, "anci", "ance");
                    break;

                case 'e':
                    ChangeSuffix(ref token, "izer", "ize");
                    break;

                case 'l':
                    if (ChangeSuffix(ref token, "bli", "ble"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(ref token, "alli", "al"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(ref token, "entli", "ent"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(ref token, "eli", "e"))
                    {
                        break;
                    }

                    ChangeSuffix(ref token, "ousli", "ous");
                    break;

                case 'o':
                    if (ChangeSuffix(ref token, "ization", "ize"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(ref token, "ation", "ate"))
                    {
                        break;
                    }

                    ChangeSuffix(ref token, "ator", "ate");
                    break;

                case 's':
                    if (ChangeSuffix(ref token, "alism", "al"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(ref token, "iveness", "ive"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(ref token, "fulness", "ful"))
                    {
                        break;
                    }

                    ChangeSuffix(ref token, "ousness", "ous");
                    break;

                case 't':
                    if (ChangeSuffix(ref token, "aliti", "al"))
                    {
                        break;
                    }
                    else if (ChangeSuffix(ref token, "iviti", "ive"))
                    {
                        break;
                    }

                    ChangeSuffix(ref token, "biliti", "ble");
                    break;

                case 'g':
                    ChangeSuffix(ref token, "logi", "log");
                    break;
            }
        }

        private static void Step4(ref Token token)
        {
            if (token.Length < 1)
            {
                return;
            }

            switch (token[token.Length - 1])
            {
                case 'e':
                    if (ChangeSuffix(ref token, "icate", "ic"))
                    {
                        break;
                    }
                    else if (RemoveSuffix(ref token, "ative"))
                    {
                        break;
                    }

                    ChangeSuffix(ref token, "alize", "al");
                    break;

                case 'i':
                    ChangeSuffix(ref token, "iciti", "ic");
                    break;

                case 'l':
                    if (ChangeSuffix(ref token, "ical", "ic"))
                    {
                        break;
                    }

                    RemoveSuffix(ref token, "ful");
                    break;

                case 's':
                    RemoveSuffix(ref token, "ness");
                    break;
            }
        }

        private static void Step5(ref Token token)
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

            if (NumberOfConsoantSequences(token, j) > 1)
            {
                token.Remove(j + 1);
            }
        }

        private static void Step6(ref Token token)
        {
            if (token.Length < 1)
            {
                return;
            }

            if (token[token.Length - 1] == 'e')
            {
                var a = NumberOfConsoantSequences(token, token.Length - 1);

                if (a > 1 || (a == 1 && !HasCvcAt(token, token.Length - 2)))
                {
                    token.Remove(token.Length - 1);
                }
            }

            if (token[token.Length - 1] == 'l')
            {
                if (ContainsDoubleConsonantAt(token, token.Length - 1) && NumberOfConsoantSequences(token, token.Length - 1) > 1)
                {
                    token.Remove(token.Length - 1);
                }
            }
        }

        private static bool ChangeSuffix(ref Token token, string suffix, string replacement)
        {
            if (!EndsWith(token, suffix, out var j))
            {
                return false;
            }

            if (NumberOfConsoantSequences(token, j) < 1)
            {
                return false;
            }

            token.Write(j + 1, replacement);
            return true;
        }

        private static bool RemoveSuffix(ref Token token, string suffix)
        {
            if (!EndsWith(token, suffix, out var j))
            {
                return false;
            }

            if (NumberOfConsoantSequences(token, j) < 1)
            {
                return false;
            }

            token.Remove(j + 1);
            return true;
        }

        private static bool EndsWith(Token token, string suffix, out int j)
        {
            var span = token.Span;
            var chars = suffix.AsSpan();

            if (span.EndsWith(chars))
            {
                j = span.Length - chars.Length - 1;
                return true;
            }

            j = 0;
            return false;
        }

        private static bool ContainsVowel(Token token, int j)
        {
            for (var i = 0; i <= j; i++)
            {
                if (!IsConsonant(token, i))
                {
                    return true;
                }
            }

            return false;
        }

        private static int NumberOfConsoantSequences(Token token, int j)
        {
            var n = 0;
            var i = 0;

            while (true)
            {
                if (i > j)
                {
                    return n;
                }

                if (!IsConsonant(token, i))
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

                    if (IsConsonant(token, i))
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

                    if (!IsConsonant(token, i))
                    {
                        break;
                    }

                    i++;
                }

                i++;
            }
        }

        private static bool HasCvcAt(Token token, int i)
        {
            if (i < 2 || !IsConsonant(token, i) || IsConsonant(token, i - 1) || !IsConsonant(token, i - 2))
            {
                return false;
            }
            else
            {
                var c = token[i];

                if (c == 'w' || c == 'x' || c == 'y')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ContainsDoubleConsonantAt(Token token, int i)
        {
            if (i < 1)
            {
                return false;
            }

            if (token[i] != token[i - 1])
            {
                return false;
            }

            return IsConsonant(token, i);
        }

        private static bool IsConsonant(Token token, int i)
        {
            return
                token[i] switch
                {
                    'a' or 'e' or 'i' or 'o' or 'u' => false,
                    'y' => i == 0 || !IsConsonant(token, i - 1),
                    _ => true,
                };
        }
    }
}
