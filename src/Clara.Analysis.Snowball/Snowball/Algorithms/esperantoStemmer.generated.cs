// Generated from esperanto.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from esperanto.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
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


        protected override bool stem()
        {
            int among_var;
            bool B_foreign;
            {
                int c1 = cursor;
                B_foreign = false;
                while (true)
                {
                    int c2 = cursor;
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
                    cursor = c2;
                    break;
                }
                if (B_foreign)
                {
                    return false;
                }
                cursor = c1;
            }
            {
                int c3 = cursor;
                bra = cursor;
                if (!(eq_s("'")))
                {
                    goto lab1;
                }
                ket = cursor;
                if (!(eq_s("st")))
                {
                    goto lab1;
                }
                if (find_among(a_1, null) == 0)
                {
                    goto lab1;
                }
                if (cursor < limit)
                {
                    goto lab1;
                }
                slice_from("e");
            lab1: ;
                cursor = c3;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c4 = limit - cursor;
                ket = cursor;
                {
                    int c5 = limit - cursor;
                    if (!(eq_s_b("n")))
                    {
                        {
                            cursor = limit - c5;
                            goto lab3;
                        }
                    }
                lab3: ;
                }
                bra = cursor;
                if (find_among_b(a_2, null) == 0)
                {
                    goto lab2;
                }
                if (cursor > limit_backward)
                {
                    goto lab5;
                }
                goto lab4;
            lab5: ;
                if (!(eq_s_b("-")))
                {
                    goto lab2;
                }
            lab4: ;
                slice_del();
                return false;
            lab2: ;
                cursor = limit - c4;
            }
            {
                int c6 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("'")))
                {
                    goto lab6;
                }
                bra = cursor;
                {
                    int c7 = limit - cursor;
                    if (!(eq_s_b("l")))
                    {
                        goto lab8;
                    }
                    if (cursor > limit_backward)
                    {
                        goto lab8;
                    }
                    slice_from("a");
                    goto lab7;
                lab8: ;
                    cursor = limit - c7;
                    if (!(eq_s_b("un")))
                    {
                        goto lab9;
                    }
                    if (cursor > limit_backward)
                    {
                        goto lab9;
                    }
                    slice_from("u");
                    goto lab7;
                lab9: ;
                    cursor = limit - c7;
                    if (find_among_b(a_3, null) == 0)
                    {
                        goto lab10;
                    }
                    if (cursor > limit_backward)
                    {
                        goto lab12;
                    }
                    goto lab11;
                lab12: ;
                    if (!(eq_s_b("-")))
                    {
                        goto lab10;
                    }
                lab11: ;
                    slice_from("aŭ");
                    goto lab7;
                lab10: ;
                    cursor = limit - c7;
                    slice_from("o");
                }
            lab7: ;
            lab6: ;
                cursor = limit - c6;
            }
            {
                int c8 = limit - cursor;
                ket = cursor;
                bra = cursor;
                {
                    int c9 = limit - cursor;
                    {
                        int c10 = limit - cursor;
                        {
                            int c11 = limit - cursor;
                            if (!(eq_s_b("n")))
                            {
                                {
                                    cursor = limit - c11;
                                    goto lab16;
                                }
                            }
                        lab16: ;
                        }
                        bra = cursor;
                        if (!(eq_s_b("e")))
                        {
                            goto lab15;
                        }
                        goto lab14;
                    lab15: ;
                        cursor = limit - c10;
                        {
                            int c12 = limit - cursor;
                            if (!(eq_s_b("n")))
                            {
                                {
                                    cursor = limit - c12;
                                    goto lab17;
                                }
                            }
                        lab17: ;
                        }
                        {
                            int c13 = limit - cursor;
                            if (!(eq_s_b("j")))
                            {
                                {
                                    cursor = limit - c13;
                                    goto lab18;
                                }
                            }
                        lab18: ;
                        }
                        bra = cursor;
                        if (in_grouping_b(g_aou, 97, 117, false) != 0)
                        {
                            goto lab13;
                        }
                    }
                lab14: ;
                    if (!(eq_s_b("i")))
                    {
                        goto lab13;
                    }
                    {
                        int c14 = limit - cursor;
                        if (find_among_b(a_8, null) == 0)
                        {
                            {
                                cursor = limit - c14;
                                goto lab19;
                            }
                        }
                    lab19: ;
                    }
                    if (cursor > limit_backward)
                    {
                        goto lab21;
                    }
                    goto lab20;
                lab21: ;
                    if (!(eq_s_b("-")))
                    {
                        goto lab13;
                    }
                lab20: ;
                    cursor = limit - c9;
                }
                slice_del();
                return false;
            lab13: ;
                cursor = limit - c8;
            }
            {
                int c15 = limit - cursor;
                if (find_among_b(a_5, null) == 0)
                {
                    goto lab22;
                }
                if (cursor > limit_backward)
                {
                    goto lab24;
                }
                goto lab23;
            lab24: ;
                if (!(eq_s_b("-")))
                {
                    goto lab22;
                }
            lab23: ;
                return false;
            lab22: ;
                cursor = limit - c15;
            }
            {
                int c16 = limit - cursor;
                if (find_among_b(a_6, null) == 0)
                {
                    goto lab25;
                }
                if (find_among_b(a_7, null) == 0)
                {
                    goto lab25;
                }
                return false;
            lab25: ;
                cursor = limit - c16;
            }
            {
                int c17 = limit - cursor;
                ket = cursor;
                {
                    int c18 = limit - cursor;
                    if (!(eq_s_b("n")))
                    {
                        {
                            cursor = limit - c18;
                            goto lab27;
                        }
                    }
                lab27: ;
                }
                {
                    int c19 = limit - cursor;
                    if (!(eq_s_b("j")))
                    {
                        {
                            cursor = limit - c19;
                            goto lab28;
                        }
                    }
                lab28: ;
                }
                bra = cursor;
                if (find_among_b(a_4, null) == 0)
                {
                    goto lab26;
                }
                if (cursor > limit_backward)
                {
                    goto lab30;
                }
                goto lab29;
            lab30: ;
                if (!(eq_s_b("-")))
                {
                    goto lab26;
                }
            lab29: ;
                slice_del();
                return false;
            lab26: ;
                cursor = limit - c17;
            }
            {
                int c20 = limit - cursor;
                {
                    int c21 = limit - cursor;
                    for (int c22 = 2; c22 > 0; c22--)
                    {
                        {

                            int ret = out_grouping_b(g_vowel, 97, 117, true);
                            if (ret < 0)
                            {
                                goto lab32;
                            }

                            cursor -= ret;
                        }
                    }
                    goto lab31;
                lab32: ;
                    cursor = limit - c21;
                    while (true)
                    {
                        if (!(eq_s_b("-")))
                        {
                            goto lab34;
                        }
                        break;
                    lab34: ;
                        if (cursor <= limit_backward)
                        {
                            goto lab33;
                        }
                        cursor--;
                    }
                    if (cursor <= limit_backward)
                    {
                        goto lab33;
                    }
                    cursor--;
                    goto lab31;
                lab33: ;
                    cursor = limit - c21;
                    {

                        int ret = out_grouping_b(g_digit, 48, 57, true);
                        if (ret < 0)
                        {
                            return false;
                        }

                        cursor -= ret;
                    }
                }
            lab31: ;
                cursor = limit - c20;
            }
            ket = cursor;
            among_var = find_among_b(a_9, null);
            if (among_var == 0)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    {
                        int c23 = limit - cursor;
                        if (!(eq_s_b("-")))
                        {
                            goto lab36;
                        }
                        goto lab35;
                    lab36: ;
                        if (in_grouping_b(g_digit, 48, 57, false) != 0)
                        {
                            return false;
                        }
                    lab35: ;
                        cursor = limit - c23;
                    }
                    break;
                }
            }
            {
                int c24 = limit - cursor;
                if (!(eq_s_b("-")))
                {
                    {
                        cursor = limit - c24;
                        goto lab37;
                    }
                }
            lab37: ;
            }
            bra = cursor;
            slice_del();
            cursor = limit_backward;
            return true;
        }

    }
}

