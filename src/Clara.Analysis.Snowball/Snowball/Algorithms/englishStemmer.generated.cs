﻿// Generated from english.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from english.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class EnglishStemmer : Stemmer
    {
        private bool B_Y_found;
        private int I_p2;
        private int I_p1;

        private const string g_aeo = "aeo";
        private const string g_v = "aeiouy";
        private const string g_v_WXY = "aeiouywxY";
        private const string g_valid_LI = "cdeghkmnrt";

        private static readonly Among[] a_0 = new[]
        {
            new Among("arsen", -1, -1),
            new Among("commun", -1, -1),
            new Among("emerg", -1, -1),
            new Among("gener", -1, -1),
            new Among("later", -1, -1),
            new Among("organ", -1, -1),
            new Among("past", -1, -1),
            new Among("univers", -1, -1)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("'", -1, 1),
            new Among("'s'", 0, 1),
            new Among("'s", -1, 1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ied", -1, 2),
            new Among("s", -1, 3),
            new Among("ies", 1, 2),
            new Among("sses", 1, 1),
            new Among("ss", 1, -1),
            new Among("us", 1, -1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("succ", -1, 1),
            new Among("proc", -1, 1),
            new Among("exc", -1, 1)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("even", -1, 2),
            new Among("cann", -1, 2),
            new Among("inn", -1, 2),
            new Among("earr", -1, 2),
            new Among("herr", -1, 2),
            new Among("out", -1, 2),
            new Among("y", -1, 1)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("", -1, -1),
            new Among("ed", 0, 2),
            new Among("eed", 1, 1),
            new Among("ing", 0, 3),
            new Among("edly", 0, 2),
            new Among("eedly", 4, 1),
            new Among("ingly", 0, 2)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("", -1, 3),
            new Among("bb", 0, 2),
            new Among("dd", 0, 2),
            new Among("ff", 0, 2),
            new Among("gg", 0, 2),
            new Among("bl", 0, 1),
            new Among("mm", 0, 2),
            new Among("nn", 0, 2),
            new Among("pp", 0, 2),
            new Among("rr", 0, 2),
            new Among("at", 0, 1),
            new Among("tt", 0, 2),
            new Among("iz", 0, 1)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("anci", -1, 3),
            new Among("enci", -1, 2),
            new Among("ogi", -1, 14),
            new Among("li", -1, 16),
            new Among("bli", 3, 12),
            new Among("abli", 4, 4),
            new Among("alli", 3, 8),
            new Among("fulli", 3, 9),
            new Among("lessli", 3, 15),
            new Among("ousli", 3, 10),
            new Among("entli", 3, 5),
            new Among("aliti", -1, 8),
            new Among("biliti", -1, 12),
            new Among("iviti", -1, 11),
            new Among("tional", -1, 1),
            new Among("ational", 14, 7),
            new Among("alism", -1, 8),
            new Among("ation", -1, 7),
            new Among("ization", 17, 6),
            new Among("izer", -1, 6),
            new Among("ator", -1, 7),
            new Among("iveness", -1, 11),
            new Among("fulness", -1, 9),
            new Among("ousness", -1, 10),
            new Among("ogist", -1, 13)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("icate", -1, 4),
            new Among("ative", -1, 6),
            new Among("alize", -1, 3),
            new Among("iciti", -1, 4),
            new Among("ical", -1, 4),
            new Among("tional", -1, 1),
            new Among("ational", 5, 2),
            new Among("ful", -1, 5),
            new Among("ness", -1, 5)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("ic", -1, 1),
            new Among("ance", -1, 1),
            new Among("ence", -1, 1),
            new Among("able", -1, 1),
            new Among("ible", -1, 1),
            new Among("ate", -1, 1),
            new Among("ive", -1, 1),
            new Among("ize", -1, 1),
            new Among("iti", -1, 1),
            new Among("al", -1, 1),
            new Among("ism", -1, 1),
            new Among("ion", -1, 2),
            new Among("er", -1, 1),
            new Among("ous", -1, 1),
            new Among("ant", -1, 1),
            new Among("ent", -1, 1),
            new Among("ment", 15, 1),
            new Among("ement", 16, 1)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("e", -1, 1),
            new Among("l", -1, 2)
        };

        private static readonly Among[] a_11 = new[]
        {
            new Among("andes", -1, -1),
            new Among("atlas", -1, -1),
            new Among("bias", -1, -1),
            new Among("cosmos", -1, -1),
            new Among("early", -1, 5),
            new Among("gently", -1, 3),
            new Among("howe", -1, -1),
            new Among("idly", -1, 2),
            new Among("news", -1, -1),
            new Among("only", -1, 6),
            new Among("singly", -1, 7),
            new Among("skies", -1, 1),
            new Among("sky", -1, -1),
            new Among("ugly", -1, 4)
        };


        private bool r_prelude()
        {
            B_Y_found = false;
            {
                int c1 = cursor;
                bra = cursor;
                if (!(eq_s("'")))
                {
                    goto lab0;
                }
                ket = cursor;
                slice_del();
            lab0: ;
                cursor = c1;
            }
            {
                int c2 = cursor;
                bra = cursor;
                if (!(eq_s("y")))
                {
                    goto lab1;
                }
                ket = cursor;
                slice_from("Y");
                B_Y_found = true;
            lab1: ;
                cursor = c2;
            }
            {
                int c3 = cursor;
                while (true)
                {
                    int c4 = cursor;
                    while (true)
                    {
                        int c5 = cursor;
                        if (in_grouping(g_v, 97, 121, false) != 0)
                        {
                            goto lab4;
                        }
                        bra = cursor;
                        if (!(eq_s("y")))
                        {
                            goto lab4;
                        }
                        ket = cursor;
                        cursor = c5;
                        break;
                    lab4: ;
                        cursor = c5;
                        if (cursor >= limit)
                        {
                            goto lab3;
                        }
                        cursor++;
                    }
                    slice_from("Y");
                    B_Y_found = true;
                    continue;
                lab3: ;
                    cursor = c4;
                    break;
                }
                cursor = c3;
            }
            return true;
        }

        private bool r_mark_regions()
        {
            I_p1 = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    if (find_among(a_0) == 0)
                    {
                        goto lab2;
                    }
                    goto lab1;
                lab2: ;
                    cursor = c2;
                    {

                        int ret = out_grouping(g_v, 97, 121, true);
                        if (ret < 0)
                        {
                            goto lab0;
                        }

                        cursor += ret;
                    }
                    {

                        int ret = in_grouping(g_v, 97, 121, true);
                        if (ret < 0)
                        {
                            goto lab0;
                        }

                        cursor += ret;
                    }
                }
            lab1: ;
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 121, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 121, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab0: ;
                cursor = c1;
            }
            return true;
        }

        private bool r_shortv()
        {
            {
                int c1 = limit - cursor;
                if (out_grouping_b(g_v_WXY, 89, 121, false) != 0)
                {
                    goto lab1;
                }
                if (in_grouping_b(g_v, 97, 121, false) != 0)
                {
                    goto lab1;
                }
                if (out_grouping_b(g_v, 97, 121, false) != 0)
                {
                    goto lab1;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                if (out_grouping_b(g_v, 97, 121, false) != 0)
                {
                    goto lab2;
                }
                if (in_grouping_b(g_v, 97, 121, false) != 0)
                {
                    goto lab2;
                }
                if (cursor > limit_backward)
                {
                    goto lab2;
                }
                goto lab0;
            lab2: ;
                cursor = limit - c1;
                if (!(eq_s_b("past")))
                {
                    return false;
                }
            }
        lab0: ;
            return true;
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
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_1) == 0)
                {
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                }
                bra = cursor;
                slice_del();
            lab0: ;
            }
            ket = cursor;
            among_var = find_among_b(a_2);
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
                    {
                        int c2 = limit - cursor;
                        {
                            int c = cursor - 2;
                            if (c < limit_backward)
                            {
                                goto lab2;
                            }
                            cursor = c;
                        }
                        slice_from("i");
                        goto lab1;
                    lab2: ;
                        cursor = limit - c2;
                        slice_from("ie");
                    }
                lab1: ;
                    break;
                }
                case 3: {
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    {

                        int ret = out_grouping_b(g_v, 97, 121, true);
                        if (ret < 0)
                        {
                            return false;
                        }

                        cursor -= ret;
                    }
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
            among_var = find_among_b(a_5);
            bra = cursor;
            {
                int c1 = limit - cursor;
                switch (among_var) {
                    case 1: {
                        {
                            int c2 = limit - cursor;
                            {
                                int c3 = limit - cursor;
                                if (find_among_b(a_3) == 0)
                                {
                                    goto lab4;
                                }
                                if (cursor > limit_backward)
                                {
                                    goto lab4;
                                }
                                goto lab3;
                            lab4: ;
                                cursor = limit - c3;
                                if (!r_R1())
                                    goto lab2;
                                slice_from("ee");
                            }
                        lab3: ;
                        lab2: ;
                            cursor = limit - c2;
                        }
                        break;
                    }
                    case 2: {
                        goto lab1;
                        break;
                    }
                    case 3: {
                        among_var = find_among_b(a_4);
                        if (among_var == 0)
                        {
                            goto lab1;
                        }
                        switch (among_var) {
                            case 1: {
                                {
                                    int c4 = limit - cursor;
                                    if (out_grouping_b(g_v, 97, 121, false) != 0)
                                    {
                                        goto lab1;
                                    }
                                    if (cursor > limit_backward)
                                    {
                                        goto lab1;
                                    }
                                    cursor = limit - c4;
                                }
                                bra = cursor;
                                slice_from("ie");
                                break;
                            }
                            case 2: {
                                if (cursor > limit_backward)
                                {
                                    goto lab1;
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                {
                    int c5 = limit - cursor;
                    {

                        int ret = out_grouping_b(g_v, 97, 121, true);
                        if (ret < 0)
                        {
                            return false;
                        }

                        cursor -= ret;
                    }
                    cursor = limit - c5;
                }
                slice_del();
                ket = cursor;
                bra = cursor;
                {
                    int c6 = limit - cursor;
                    among_var = find_among_b(a_6);
                    switch (among_var) {
                        case 1: {
                            slice_from("e");
                            return false;
                            break;
                        }
                        case 2: {
                            {
                                int c7 = limit - cursor;
                                if (in_grouping_b(g_aeo, 97, 111, false) != 0)
                                {
                                    goto lab5;
                                }
                                if (cursor > limit_backward)
                                {
                                    goto lab5;
                                }
                                return false;
                            lab5: ;
                                cursor = limit - c7;
                            }
                            break;
                        }
                        case 3: {
                            if (cursor != I_p1)
                            {
                                return false;
                            }
                            {
                                int c8 = limit - cursor;
                                if (!r_shortv())
                                    return false;
                                cursor = limit - c8;
                            }
                            slice_from("e");
                            return false;
                            break;
                        }
                    }
                    cursor = limit - c6;
                }
                ket = cursor;
                if (cursor <= limit_backward)
                {
                    return false;
                }
                cursor--;
                bra = cursor;
                slice_del();
            }
        lab0: ;
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
            if (out_grouping_b(g_v, 97, 121, false) != 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                goto lab2;
            }
            return false;
        lab2: ;
            slice_from("i");
            return true;
        }

        private bool r_Step_2()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_7);
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
                    slice_from("ize");
                    break;
                }
                case 7: {
                    slice_from("ate");
                    break;
                }
                case 8: {
                    slice_from("al");
                    break;
                }
                case 9: {
                    slice_from("ful");
                    break;
                }
                case 10: {
                    slice_from("ous");
                    break;
                }
                case 11: {
                    slice_from("ive");
                    break;
                }
                case 12: {
                    slice_from("ble");
                    break;
                }
                case 13: {
                    slice_from("og");
                    break;
                }
                case 14: {
                    if (!(eq_s_b("l")))
                    {
                        return false;
                    }
                    slice_from("og");
                    break;
                }
                case 15: {
                    slice_from("less");
                    break;
                }
                case 16: {
                    if (in_grouping_b(g_valid_LI, 99, 116, false) != 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_Step_3()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_8);
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
                    slice_from("ate");
                    break;
                }
                case 3: {
                    slice_from("al");
                    break;
                }
                case 4: {
                    slice_from("ic");
                    break;
                }
                case 5: {
                    slice_del();
                    break;
                }
                case 6: {
                    if (!r_R2())
                        return false;
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
            among_var = find_among_b(a_9);
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

        private bool r_Step_5()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_10);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
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
                    break;
                }
                case 2: {
                    if (!r_R2())
                        return false;
                    if (!(eq_s_b("l")))
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_exception1()
        {
            int among_var;
            bra = cursor;
            among_var = find_among(a_11);
            if (among_var == 0)
            {
                return false;
            }
            ket = cursor;
            if (cursor < limit)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    slice_from("sky");
                    break;
                }
                case 2: {
                    slice_from("idl");
                    break;
                }
                case 3: {
                    slice_from("gentl");
                    break;
                }
                case 4: {
                    slice_from("ugli");
                    break;
                }
                case 5: {
                    slice_from("earli");
                    break;
                }
                case 6: {
                    slice_from("onli");
                    break;
                }
                case 7: {
                    slice_from("singl");
                    break;
                }
            }
            return true;
        }

        private bool r_postlude()
        {
            if (!(B_Y_found))
            {
                return false;
            }
            while (true)
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    bra = cursor;
                    if (!(eq_s("Y")))
                    {
                        goto lab1;
                    }
                    ket = cursor;
                    cursor = c2;
                    break;
                lab1: ;
                    cursor = c2;
                    if (cursor >= limit)
                    {
                        goto lab0;
                    }
                    cursor++;
                }
                slice_from("y");
                continue;
            lab0: ;
                cursor = c1;
                break;
            }
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                if (!r_exception1())
                    goto lab1;
                goto lab0;
            lab1: ;
                cursor = c1;
                {
                    int c2 = cursor;
                    {
                        int c = cursor + 3;
                        if (c > limit)
                        {
                            goto lab3;
                        }
                        cursor = c;
                    }
                    goto lab2;
                lab3: ;
                    cursor = c2;
                }
                goto lab0;
            lab2: ;
                cursor = c1;
                r_prelude();
                r_mark_regions();
                limit_backward = cursor;
                cursor = limit;
                {
                    int c3 = limit - cursor;
                    r_Step_1a();
                    cursor = limit - c3;
                }
                {
                    int c4 = limit - cursor;
                    r_Step_1b();
                    cursor = limit - c4;
                }
                {
                    int c5 = limit - cursor;
                    r_Step_1c();
                    cursor = limit - c5;
                }
                {
                    int c6 = limit - cursor;
                    r_Step_2();
                    cursor = limit - c6;
                }
                {
                    int c7 = limit - cursor;
                    r_Step_3();
                    cursor = limit - c7;
                }
                {
                    int c8 = limit - cursor;
                    r_Step_4();
                    cursor = limit - c8;
                }
                {
                    int c9 = limit - cursor;
                    r_Step_5();
                    cursor = limit - c9;
                }
                cursor = limit_backward;
                {
                    int c10 = cursor;
                    r_postlude();
                    cursor = c10;
                }
            }
        lab0: ;
            return true;
        }

    }
}

