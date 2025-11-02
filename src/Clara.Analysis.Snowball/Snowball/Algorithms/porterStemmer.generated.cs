// Generated from porter.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from porter.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class PorterStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;

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

        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_R2()
        {
            return I_p2 <= cursor;
        }

        private bool r_Step_1a()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_0, null);
            if (among_var == 0)
            {
                return false;
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
            return true;
        }

        private bool r_Step_1b()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_2, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R1())
                        return false;
                    slice_from("ee");
                    break;
                }
                case 2: {
                    {
                        int c1 = limit - cursor;
                        {

                            int ret = out_grouping_b(g_v, 97, 121, true);
                            if (ret < 0)
                            {
                                return false;
                            }

                            cursor -= ret;
                        }
                        cursor = limit - c1;
                    }
                    slice_del();
                    {
                        int c2 = limit - cursor;
                        among_var = find_among_b(a_1, null);
                        cursor = limit - c2;
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
                                return false;
                            }
                            cursor--;
                            bra = cursor;
                            slice_del();
                            break;
                        }
                        case 3: {
                            if (cursor != I_p1)
                            {
                                return false;
                            }
                            {
                                int c3 = limit - cursor;
                                if (!r_shortv())
                                    return false;
                                cursor = limit - c3;
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
            return true;
        }

        private bool r_Step_1c()
        {
            ket = cursor;
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("y")))
                {
                    goto lab1;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                if (!(eq_s_b("Y")))
                {
                    return false;
                }
            }
        lab0: ;
            bra = cursor;
            {

                int ret = out_grouping_b(g_v, 97, 121, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor -= ret;
            }
            slice_from("i");
            return true;
        }

        private bool r_Step_2()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_3, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
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
            return true;
        }

        private bool r_Step_3()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_4, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
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
            return true;
        }

        private bool r_Step_4()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_5, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R2())
                return false;
            switch (among_var) {
                case 1: {
                    slice_del();
                    break;
                }
                case 2: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("s")))
                        {
                            goto lab1;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("t")))
                        {
                            return false;
                        }
                    }
                lab0: ;
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_Step_5a()
        {
            ket = cursor;
            if (!(eq_s_b("e")))
            {
                return false;
            }
            bra = cursor;
            if (!r_R2())
                goto lab1;
            goto lab0;
        lab1: ;
            if (!r_R1())
                return false;
            {
                int c1 = limit - cursor;
                if (!r_shortv())
                    goto lab2;
                return false;
            lab2: ;
                cursor = limit - c1;
            }
        lab0: ;
            slice_del();
            return true;
        }

        private bool r_Step_5b()
        {
            ket = cursor;
            if (!(eq_s_b("l")))
            {
                return false;
            }
            bra = cursor;
            if (!r_R2())
                return false;
            if (!(eq_s_b("l")))
            {
                return false;
            }
            slice_del();
            return true;
        }

        protected override bool stem()
        {
            bool B_Y_found;
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
                r_Step_1a();
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                r_Step_1b();
                cursor = limit - c7;
            }
            {
                int c8 = limit - cursor;
                r_Step_1c();
                cursor = limit - c8;
            }
            {
                int c9 = limit - cursor;
                r_Step_2();
                cursor = limit - c9;
            }
            {
                int c10 = limit - cursor;
                r_Step_3();
                cursor = limit - c10;
            }
            {
                int c11 = limit - cursor;
                r_Step_4();
                cursor = limit - c11;
            }
            {
                int c12 = limit - cursor;
                r_Step_5a();
                cursor = limit - c12;
            }
            {
                int c13 = limit - cursor;
                r_Step_5b();
                cursor = limit - c13;
            }
            cursor = limit_backward;
            {
                int c14 = cursor;
                if (!B_Y_found)
                {
                    goto lab5;
                }
                while (true)
                {
                    int c15 = cursor;
                    while (true)
                    {
                        int c16 = cursor;
                        bra = cursor;
                        if (!(eq_s("Y")))
                        {
                            goto lab7;
                        }
                        ket = cursor;
                        cursor = c16;
                        break;
                    lab7: ;
                        cursor = c16;
                        if (cursor >= limit)
                        {
                            goto lab6;
                        }
                        cursor++;
                    }
                    slice_from("y");
                    continue;
                lab6: ;
                    cursor = c15;
                    break;
                }
            lab5: ;
                cursor = c14;
            }
            return true;
        }

    }
}

