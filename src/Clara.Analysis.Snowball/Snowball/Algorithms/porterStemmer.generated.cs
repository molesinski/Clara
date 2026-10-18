// Generated from porter.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from porter.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class PorterStemmer : Stemmer
    {
        private const string g_v = "aeiouy";
        private const string g_v_WXY = "Yaeiouwxy";

        private static readonly Among[] a_0 = new[]
        {
            new Among("s", -1, 3, 0),
            new Among("ies", 0, 2, 0),
            new Among("sses", 0, 1, 0),
            new Among("ss", 0, -1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 3, 0),
            new Among("bb", 0, 2, 0),
            new Among("dd", 0, 2, 0),
            new Among("ff", 0, 2, 0),
            new Among("gg", 0, 2, 0),
            new Among("bl", 0, 1, 0),
            new Among("mm", 0, 2, 0),
            new Among("nn", 0, 2, 0),
            new Among("pp", 0, 2, 0),
            new Among("rr", 0, 2, 0),
            new Among("at", 0, 1, 0),
            new Among("tt", 0, 2, 0),
            new Among("iz", 0, 1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ed", -1, 2, 0),
            new Among("eed", 0, 1, 0),
            new Among("ing", -1, 2, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("anci", -1, 3, 0),
            new Among("enci", -1, 2, 0),
            new Among("abli", -1, 4, 0),
            new Among("eli", -1, 6, 0),
            new Among("alli", -1, 9, 0),
            new Among("ousli", -1, 11, 0),
            new Among("entli", -1, 5, 0),
            new Among("aliti", -1, 9, 0),
            new Among("biliti", -1, 13, 0),
            new Among("iviti", -1, 12, 0),
            new Among("tional", -1, 1, 0),
            new Among("ational", 10, 8, 0),
            new Among("alism", -1, 9, 0),
            new Among("ation", -1, 8, 0),
            new Among("ization", 13, 7, 0),
            new Among("izer", -1, 7, 0),
            new Among("ator", -1, 8, 0),
            new Among("iveness", -1, 12, 0),
            new Among("fulness", -1, 10, 0),
            new Among("ousness", -1, 11, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("icate", -1, 2, 0),
            new Among("ative", -1, 3, 0),
            new Among("alize", -1, 1, 0),
            new Among("iciti", -1, 2, 0),
            new Among("ical", -1, 2, 0),
            new Among("ful", -1, 3, 0),
            new Among("ness", -1, 3, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ic", -1, 1, 0),
            new Among("ance", -1, 1, 0),
            new Among("ence", -1, 1, 0),
            new Among("able", -1, 1, 0),
            new Among("ible", -1, 1, 0),
            new Among("ate", -1, 1, 0),
            new Among("ive", -1, 1, 0),
            new Among("ize", -1, 1, 0),
            new Among("iti", -1, 1, 0),
            new Among("al", -1, 1, 0),
            new Among("ism", -1, 1, 0),
            new Among("ion", -1, 2, 0),
            new Among("er", -1, 1, 0),
            new Among("ous", -1, 1, 0),
            new Among("ant", -1, 1, 0),
            new Among("ent", -1, 1, 0),
            new Among("ment", 15, 1, 0),
            new Among("ement", 16, 1, 0),
            new Among("ou", -1, 1, 0)
        };


        private bool r_shortv()
        {
            if (out_grouping_b(g_v_WXY, 89, 121, false) != 0)
            {
                return false;
            }
            if (in_grouping_b(g_v, 97, 121, false) != 0)
            {
                return false;
            }
            return (out_grouping_b(g_v, 97, 121, false) == 0);
        }

        protected override bool stem()
        {
            int among_var;
            bool B_Y_found;
            int I_p2;
            int I_p1;
            B_Y_found = false;
            {
                int c1 = cursor;
                bra = cursor;
                if (!(eq_s("y")))
                {
                    goto lab0;
                }
                ket = cursor;
                slice_from("Y");
                B_Y_found = true;
            lab0: ;
                cursor = c1;
            }
            {
                int c2 = cursor;
                while (true)
                {
                    int c3 = cursor;
                    while (true)
                    {
                        int c4 = cursor;
                        if (in_grouping(g_v, 97, 121, false) != 0)
                        {
                            goto lab3;
                        }
                        bra = cursor;
                        if (!(eq_s("y")))
                        {
                            goto lab3;
                        }
                        ket = cursor;
                        cursor = c4;
                        break;
                    lab3: ;
                        cursor = c4;
                        if (cursor >= limit)
                        {
                            goto lab2;
                        }
                        cursor++;
                    }
                    slice_from("Y");
                    B_Y_found = true;
                    continue;
                lab2: ;
                    cursor = c3;
                    break;
                }
                cursor = c2;
            }
            I_p1 = limit;
            I_p2 = limit;
            {
                int c5 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 121, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 121, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 121, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 121, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab4: ;
                cursor = c5;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c6 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_0, null);
                if (among_var == 0)
                {
                    goto lab5;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("ss");
                        break;
                    }
                    case 2: {
                        slice_from("i");
                        break;
                    }
                    case 3: {
                        slice_del();
                        break;
                    }
                }
            lab5: ;
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_2, null);
                if (among_var == 0)
                {
                    goto lab6;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (I_p1 > cursor)
                        {
                            goto lab6;
                        }
                        slice_from("ee");
                        break;
                    }
                    case 2: {
                        {
                            int c8 = limit - cursor;
                            {

                                int ret = out_grouping_b(g_v, 97, 121, true);
                                if (ret < 0)
                                {
                                    goto lab6;
                                }

                                cursor -= ret;
                            }
                            cursor = limit - c8;
                        }
                        slice_del();
                        {
                            int c9 = limit - cursor;
                            among_var = find_among_b(a_1, null);
                            cursor = limit - c9;
                        }
                        switch (among_var) {
                            case 1: {
                                {
                                    int c = cursor;
                                    insert(cursor, cursor, "e");
                                    cursor = c;
                                }
                                break;
                            }
                            case 2: {
                                ket = cursor;
                                if (cursor <= limit_backward)
                                {
                                    goto lab6;
                                }
                                cursor--;
                                bra = cursor;
                                slice_del();
                                break;
                            }
                            case 3: {
                                if (cursor != I_p1)
                                {
                                    goto lab6;
                                }
                                {
                                    int c10 = limit - cursor;
                                    if (!r_shortv())
                                        goto lab6;
                                    cursor = limit - c10;
                                }
                                {
                                    int c = cursor;
                                    insert(cursor, cursor, "e");
                                    cursor = c;
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            lab6: ;
                cursor = limit - c7;
            }
            {
                int c11 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("y")))
                {
                    goto lab9;
                }
                goto lab8;
            lab9: ;
                if (!(eq_s_b("Y")))
                {
                    goto lab7;
                }
            lab8: ;
                bra = cursor;
                {

                    int ret = out_grouping_b(g_v, 97, 121, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor -= ret;
                }
                slice_from("i");
            lab7: ;
                cursor = limit - c11;
            }
            {
                int c12 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_3, null);
                if (among_var == 0)
                {
                    goto lab10;
                }
                bra = cursor;
                if (I_p1 > cursor)
                {
                    goto lab10;
                }
                switch (among_var) {
                    case 1: {
                        slice_from("tion");
                        break;
                    }
                    case 2: {
                        slice_from("ence");
                        break;
                    }
                    case 3: {
                        slice_from("ance");
                        break;
                    }
                    case 4: {
                        slice_from("able");
                        break;
                    }
                    case 5: {
                        slice_from("ent");
                        break;
                    }
                    case 6: {
                        slice_from("e");
                        break;
                    }
                    case 7: {
                        slice_from("ize");
                        break;
                    }
                    case 8: {
                        slice_from("ate");
                        break;
                    }
                    case 9: {
                        slice_from("al");
                        break;
                    }
                    case 10: {
                        slice_from("ful");
                        break;
                    }
                    case 11: {
                        slice_from("ous");
                        break;
                    }
                    case 12: {
                        slice_from("ive");
                        break;
                    }
                    case 13: {
                        slice_from("ble");
                        break;
                    }
                }
            lab10: ;
                cursor = limit - c12;
            }
            {
                int c13 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_4, null);
                if (among_var == 0)
                {
                    goto lab11;
                }
                bra = cursor;
                if (I_p1 > cursor)
                {
                    goto lab11;
                }
                switch (among_var) {
                    case 1: {
                        slice_from("al");
                        break;
                    }
                    case 2: {
                        slice_from("ic");
                        break;
                    }
                    case 3: {
                        slice_del();
                        break;
                    }
                }
            lab11: ;
                cursor = limit - c13;
            }
            {
                int c14 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_5, null);
                if (among_var == 0)
                {
                    goto lab12;
                }
                bra = cursor;
                if (I_p2 > cursor)
                {
                    goto lab12;
                }
                switch (among_var) {
                    case 1: {
                        slice_del();
                        break;
                    }
                    case 2: {
                        if (!(eq_s_b("s")))
                        {
                            goto lab14;
                        }
                        goto lab13;
                    lab14: ;
                        if (!(eq_s_b("t")))
                        {
                            goto lab12;
                        }
                    lab13: ;
                        slice_del();
                        break;
                    }
                }
            lab12: ;
                cursor = limit - c14;
            }
            {
                int c15 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("e")))
                {
                    goto lab15;
                }
                bra = cursor;
                if (I_p2 > cursor)
                {
                    goto lab17;
                }
                goto lab16;
            lab17: ;
                if (I_p1 > cursor)
                {
                    goto lab15;
                }
                {
                    int c16 = limit - cursor;
                    if (!r_shortv())
                        goto lab18;
                    goto lab15;
                lab18: ;
                    cursor = limit - c16;
                }
            lab16: ;
                slice_del();
            lab15: ;
                cursor = limit - c15;
            }
            {
                int c17 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("l")))
                {
                    goto lab19;
                }
                bra = cursor;
                if (I_p2 > cursor)
                {
                    goto lab19;
                }
                if (!(eq_s_b("l")))
                {
                    goto lab19;
                }
                slice_del();
            lab19: ;
                cursor = limit - c17;
            }
            cursor = limit_backward;
            {
                int c18 = cursor;
                if (!B_Y_found)
                {
                    goto lab20;
                }
                while (true)
                {
                    int c19 = cursor;
                    while (true)
                    {
                        int c20 = cursor;
                        bra = cursor;
                        if (!(eq_s("Y")))
                        {
                            goto lab22;
                        }
                        ket = cursor;
                        cursor = c20;
                        break;
                    lab22: ;
                        cursor = c20;
                        if (cursor >= limit)
                        {
                            goto lab21;
                        }
                        cursor++;
                    }
                    slice_from("y");
                    continue;
                lab21: ;
                    cursor = c19;
                    break;
                }
            lab20: ;
                cursor = c18;
            }
            return true;
        }

    }
}

