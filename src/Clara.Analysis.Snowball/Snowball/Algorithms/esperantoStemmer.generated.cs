// Generated from esperanto.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from esperanto.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class EsperantoStemmer : Stemmer
    {
        private const string g_vowel = "aeiou";
        private const string g_aou = "aou";
        private const string g_digit = "0123456789";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 14, 0),
            new Among("-", 0, 13, 0),
            new Among("cx", 0, 1, 0),
            new Among("gx", 0, 2, 0),
            new Among("hx", 0, 3, 0),
            new Among("jx", 0, 4, 0),
            new Among("q", 0, 12, 0),
            new Among("sx", 0, 5, 0),
            new Among("ux", 0, 6, 0),
            new Among("w", 0, 12, 0),
            new Among("x", 0, 12, 0),
            new Among("y", 0, 12, 0),
            new Among("á", 0, 7, 0),
            new Among("é", 0, 8, 0),
            new Among("í", 0, 9, 0),
            new Among("ó", 0, 10, 0),
            new Among("ú", 0, 11, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("as", -1, -1, 0),
            new Among("i", -1, -1, 0),
            new Among("is", 1, -1, 0),
            new Among("os", -1, -1, 0),
            new Among("u", -1, -1, 0),
            new Among("us", 4, -1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ci", -1, -1, 0),
            new Among("gi", -1, -1, 0),
            new Among("hi", -1, -1, 0),
            new Among("li", -1, -1, 0),
            new Among("ili", 3, -1, 0),
            new Among("ŝli", 3, -1, 0),
            new Among("mi", -1, -1, 0),
            new Among("ni", -1, -1, 0),
            new Among("oni", 7, -1, 0),
            new Among("ri", -1, -1, 0),
            new Among("si", -1, -1, 0),
            new Among("vi", -1, -1, 0),
            new Among("ivi", 11, -1, 0),
            new Among("ĝi", -1, -1, 0),
            new Among("ŝi", -1, -1, 0),
            new Among("iŝi", 14, -1, 0),
            new Among("malŝi", 14, -1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("amb", -1, -1, 0),
            new Among("bald", -1, -1, 0),
            new Among("malbald", 1, -1, 0),
            new Among("morg", -1, -1, 0),
            new Among("postmorg", 3, -1, 0),
            new Among("adi", -1, -1, 0),
            new Among("hodi", -1, -1, 0),
            new Among("ank", -1, -1, 0),
            new Among("ĉirk", -1, -1, 0),
            new Among("tutĉirk", 8, -1, 0),
            new Among("presk", -1, -1, 0),
            new Among("almen", -1, -1, 0),
            new Among("apen", -1, -1, 0),
            new Among("hier", -1, -1, 0),
            new Among("antaŭhier", 13, -1, 0),
            new Among("malgr", -1, -1, 0),
            new Among("ankor", -1, -1, 0),
            new Among("kontr", -1, -1, 0),
            new Among("anstat", -1, -1, 0),
            new Among("kvaz", -1, -1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("aliu", -1, -1, 0),
            new Among("unu", -1, -1, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("aha", -1, -1, 0),
            new Among("haha", 0, -1, 0),
            new Among("haleluja", -1, -1, 0),
            new Among("hola", -1, -1, 0),
            new Among("hosana", -1, -1, 0),
            new Among("maltra", -1, -1, 0),
            new Among("hura", -1, -1, 0),
            new Among("ĥaĥa", -1, -1, 0),
            new Among("ekde", -1, -1, 0),
            new Among("elde", -1, -1, 0),
            new Among("disde", -1, -1, 0),
            new Among("ehe", -1, -1, 0),
            new Among("maltre", -1, -1, 0),
            new Among("dirlididi", -1, -1, 0),
            new Among("malpli", -1, -1, 0),
            new Among("malĉi", -1, -1, 0),
            new Among("malkaj", -1, -1, 0),
            new Among("amen", -1, -1, 0),
            new Among("tamen", 17, -1, 0),
            new Among("oho", -1, -1, 0),
            new Among("maltro", -1, -1, 0),
            new Among("minus", -1, -1, 0),
            new Among("uhu", -1, -1, 0),
            new Among("muu", -1, -1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("tri", -1, -1, 0),
            new Among("du", -1, -1, 0),
            new Among("unu", -1, -1, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("dek", -1, -1, 0),
            new Among("cent", -1, -1, 0)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("k", -1, -1, 0),
            new Among("kelk", 0, -1, 0),
            new Among("nen", -1, -1, 0),
            new Among("t", -1, -1, 0),
            new Among("mult", 3, -1, 0),
            new Among("samt", 3, -1, 0),
            new Among("ĉ", -1, -1, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("a", -1, -1, 0),
            new Among("e", -1, -1, 0),
            new Among("i", -1, -1, 0),
            new Among("j", -1, 1, 0),
            new Among("aj", 3, -1, 0),
            new Among("oj", 3, -1, 0),
            new Among("n", -1, 1, 0),
            new Among("an", 6, -1, 0),
            new Among("en", 6, -1, 0),
            new Among("jn", 6, 1, 0),
            new Among("ajn", 9, -1, 0),
            new Among("ojn", 9, -1, 0),
            new Among("on", 6, -1, 0),
            new Among("o", -1, -1, 0),
            new Among("as", -1, -1, 0),
            new Among("is", -1, -1, 0),
            new Among("os", -1, -1, 0),
            new Among("us", -1, -1, 0),
            new Among("u", -1, -1, 0)
        };


        private bool r_canonical_form()
        {
            bool B_foreign;
            int among_var;
            B_foreign = false;
            while (true)
            {
                int c1 = cursor;
                bra = cursor;
                among_var = find_among(a_0, null);
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("ĉ");
                        break;
                    }
                    case 2: {
                        slice_from("ĝ");
                        break;
                    }
                    case 3: {
                        slice_from("ĥ");
                        break;
                    }
                    case 4: {
                        slice_from("ĵ");
                        break;
                    }
                    case 5: {
                        slice_from("ŝ");
                        break;
                    }
                    case 6: {
                        slice_from("ŭ");
                        break;
                    }
                    case 7: {
                        slice_from("a");
                        B_foreign = true;
                        break;
                    }
                    case 8: {
                        slice_from("e");
                        B_foreign = true;
                        break;
                    }
                    case 9: {
                        slice_from("i");
                        B_foreign = true;
                        break;
                    }
                    case 10: {
                        slice_from("o");
                        B_foreign = true;
                        break;
                    }
                    case 11: {
                        slice_from("u");
                        B_foreign = true;
                        break;
                    }
                    case 12: {
                        B_foreign = true;
                        break;
                    }
                    case 13: {
                        B_foreign = false;
                        break;
                    }
                    case 14: {
                        if (cursor >= limit)
                        {
                            goto lab0;
                        }
                        cursor++;
                        break;
                    }
                }
                continue;
            lab0: ;
                cursor = c1;
                break;
            }
            return !B_foreign;
        }

        private bool r_initial_apostrophe()
        {
            bra = cursor;
            if (!(eq_s("'")))
            {
                return false;
            }
            ket = cursor;
            if (!(eq_s("st")))
            {
                return false;
            }
            if (find_among(a_1, null) == 0)
            {
                return false;
            }
            if (cursor < limit)
            {
                return false;
            }
            slice_from("e");
            return true;
        }

        private bool r_pronoun()
        {
            ket = cursor;
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("n")))
                {
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                }
            lab0: ;
            }
            bra = cursor;
            if (find_among_b(a_2, null) == 0)
            {
                return false;
            }
            {
                int c2 = limit - cursor;
                if (cursor > limit_backward)
                {
                    goto lab2;
                }
                goto lab1;
            lab2: ;
                cursor = limit - c2;
                if (!(eq_s_b("-")))
                {
                    return false;
                }
            }
        lab1: ;
            slice_del();
            return true;
        }

        private bool r_final_apostrophe()
        {
            ket = cursor;
            if (!(eq_s_b("'")))
            {
                return false;
            }
            bra = cursor;
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("l")))
                {
                    goto lab1;
                }
                if (cursor > limit_backward)
                {
                    goto lab1;
                }
                slice_from("a");
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                if (!(eq_s_b("un")))
                {
                    goto lab2;
                }
                if (cursor > limit_backward)
                {
                    goto lab2;
                }
                slice_from("u");
                goto lab0;
            lab2: ;
                cursor = limit - c1;
                if (find_among_b(a_3, null) == 0)
                {
                    goto lab3;
                }
                {
                    int c2 = limit - cursor;
                    if (cursor > limit_backward)
                    {
                        goto lab5;
                    }
                    goto lab4;
                lab5: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("-")))
                    {
                        goto lab3;
                    }
                }
            lab4: ;
                slice_from("aŭ");
                goto lab0;
            lab3: ;
                cursor = limit - c1;
                slice_from("o");
            }
        lab0: ;
            return true;
        }

        private bool r_ujn_suffix()
        {
            ket = cursor;
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("n")))
                {
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                }
            lab0: ;
            }
            {
                int c2 = limit - cursor;
                if (!(eq_s_b("j")))
                {
                    {
                        cursor = limit - c2;
                        goto lab1;
                    }
                }
            lab1: ;
            }
            bra = cursor;
            if (find_among_b(a_4, null) == 0)
            {
                return false;
            }
            {
                int c3 = limit - cursor;
                if (cursor > limit_backward)
                {
                    goto lab3;
                }
                goto lab2;
            lab3: ;
                cursor = limit - c3;
                if (!(eq_s_b("-")))
                {
                    return false;
                }
            }
        lab2: ;
            slice_del();
            return true;
        }

        private bool r_uninflected()
        {
            if (find_among_b(a_5, null) == 0)
            {
                return false;
            }
            {
                int c1 = limit - cursor;
                if (cursor > limit_backward)
                {
                    goto lab1;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                if (!(eq_s_b("-")))
                {
                    return false;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_merged_numeral()
        {
            if (find_among_b(a_6, null) == 0)
            {
                return false;
            }
            return find_among_b(a_7, null) != 0;
        }

        private bool r_correlative()
        {
            ket = cursor;
            bra = cursor;
            {
                int c1 = limit - cursor;
                {
                    int c2 = limit - cursor;
                    {
                        int c3 = limit - cursor;
                        if (!(eq_s_b("n")))
                        {
                            {
                                cursor = limit - c3;
                                goto lab2;
                            }
                        }
                    lab2: ;
                    }
                    bra = cursor;
                    if (!(eq_s_b("e")))
                    {
                        goto lab1;
                    }
                    goto lab0;
                lab1: ;
                    cursor = limit - c2;
                    {
                        int c4 = limit - cursor;
                        if (!(eq_s_b("n")))
                        {
                            {
                                cursor = limit - c4;
                                goto lab3;
                            }
                        }
                    lab3: ;
                    }
                    {
                        int c5 = limit - cursor;
                        if (!(eq_s_b("j")))
                        {
                            {
                                cursor = limit - c5;
                                goto lab4;
                            }
                        }
                    lab4: ;
                    }
                    bra = cursor;
                    if (in_grouping_b(g_aou, 97, 117, false) != 0)
                    {
                        return false;
                    }
                }
            lab0: ;
                if (!(eq_s_b("i")))
                {
                    return false;
                }
                {
                    int c6 = limit - cursor;
                    if (find_among_b(a_8, null) == 0)
                    {
                        {
                            cursor = limit - c6;
                            goto lab5;
                        }
                    }
                lab5: ;
                }
                {
                    int c7 = limit - cursor;
                    if (cursor > limit_backward)
                    {
                        goto lab7;
                    }
                    goto lab6;
                lab7: ;
                    cursor = limit - c7;
                    if (!(eq_s_b("-")))
                    {
                        return false;
                    }
                }
            lab6: ;
                cursor = limit - c1;
            }
            slice_del();
            return true;
        }

        private bool r_long_word()
        {
            {
                int c1 = limit - cursor;
                for (int c2 = 2; c2 > 0; c2--)
                {
                    {

                        int ret = out_grouping_b(g_vowel, 97, 117, true);
                        if (ret < 0)
                        {
                            goto lab1;
                        }

                        cursor -= ret;
                    }
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                while (true)
                {
                    if (!(eq_s_b("-")))
                    {
                        goto lab3;
                    }
                    break;
                lab3: ;
                    if (cursor <= limit_backward)
                    {
                        goto lab2;
                    }
                    cursor--;
                }
                if (cursor <= limit_backward)
                {
                    goto lab2;
                }
                cursor--;
                goto lab0;
            lab2: ;
                cursor = limit - c1;
                {

                    int ret = out_grouping_b(g_digit, 48, 57, true);
                    if (ret < 0)
                    {
                        return false;
                    }

                    cursor -= ret;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_standard_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_9, null);
            if (among_var == 0)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    {
                        int c1 = limit - cursor;
                        {
                            int c2 = limit - cursor;
                            if (!(eq_s_b("-")))
                            {
                                goto lab1;
                            }
                            goto lab0;
                        lab1: ;
                            cursor = limit - c2;
                            if (in_grouping_b(g_digit, 48, 57, false) != 0)
                            {
                                return false;
                            }
                        }
                    lab0: ;
                        cursor = limit - c1;
                    }
                    break;
                }
            }
            {
                int c3 = limit - cursor;
                if (!(eq_s_b("-")))
                {
                    {
                        cursor = limit - c3;
                        goto lab2;
                    }
                }
            lab2: ;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                if (!r_canonical_form())
                    return false;
                cursor = c1;
            }
            {
                int c2 = cursor;
                r_initial_apostrophe();
                cursor = c2;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c3 = limit - cursor;
                if (!r_pronoun())
                    goto lab0;
                return false;
            lab0: ;
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_final_apostrophe();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                if (!r_correlative())
                    goto lab1;
                return false;
            lab1: ;
                cursor = limit - c5;
            }
            {
                int c6 = limit - cursor;
                if (!r_uninflected())
                    goto lab2;
                return false;
            lab2: ;
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                if (!r_merged_numeral())
                    goto lab3;
                return false;
            lab3: ;
                cursor = limit - c7;
            }
            {
                int c8 = limit - cursor;
                if (!r_ujn_suffix())
                    goto lab4;
                return false;
            lab4: ;
                cursor = limit - c8;
            }
            {
                int c9 = limit - cursor;
                if (!r_long_word())
                    return false;
                cursor = limit - c9;
            }
            if (!r_standard_suffix())
                return false;
            cursor = limit_backward;
            return true;
        }

    }
}

