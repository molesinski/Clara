// Generated from dutch.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from dutch.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class DutchStemmer : Stemmer
    {
        private bool B_GE_removed;
        private int I_p2;
        private int I_p1;
        private StringBuilder S_ch = new StringBuilder();

        private const string g_E = "eèéêë";
        private const string g_AIOU = "aiouàáâäìíîïòóôöùúûü";
        private const string g_AEIOU = "aeiouàáâäèéêëìíîïòóôöùúûü";
        private const string g_v = "aeiouyàáâäèéêëìíîïòóôöùúûü";
        private const string g_v_WX = "aeiouwxyàáâäèéêëìíîïòóôöùúûü";

        private static readonly Among[] a_0 = new[]
        {
            new Among("a", -1, 1, 0),
            new Among("e", -1, 2, 0),
            new Among("o", -1, 1, 0),
            new Among("u", -1, 1, 0),
            new Among("à", -1, 1, 0),
            new Among("á", -1, 1, 0),
            new Among("â", -1, 1, 0),
            new Among("ä", -1, 1, 0),
            new Among("è", -1, 2, 0),
            new Among("é", -1, 2, 0),
            new Among("ê", -1, 2, 0),
            new Among("eë", -1, 3, 0),
            new Among("ië", -1, 4, 0),
            new Among("ò", -1, 1, 0),
            new Among("ó", -1, 1, 0),
            new Among("ô", -1, 1, 0),
            new Among("ö", -1, 1, 0),
            new Among("ù", -1, 1, 0),
            new Among("ú", -1, 1, 0),
            new Among("û", -1, 1, 0),
            new Among("ü", -1, 1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("nde", -1, 8, 0),
            new Among("en", -1, 7, 0),
            new Among("s", -1, 2, 0),
            new Among("'s", 2, 1, 0),
            new Among("es", 2, 4, 0),
            new Among("ies", 4, 3, 0),
            new Among("aus", 2, 6, 0),
            new Among("és", 2, 5, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("de", -1, 5, 0),
            new Among("ge", -1, 2, 0),
            new Among("ische", -1, 4, 0),
            new Among("je", -1, 1, 0),
            new Among("lijke", -1, 3, 0),
            new Among("le", -1, 9, 0),
            new Among("ene", -1, 10, 0),
            new Among("re", -1, 8, 0),
            new Among("se", -1, 7, 0),
            new Among("te", -1, 6, 0),
            new Among("ieve", -1, 11, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("heid", -1, 3, 0),
            new Among("fie", -1, 7, 0),
            new Among("gie", -1, 8, 0),
            new Among("atie", -1, 1, 0),
            new Among("isme", -1, 5, 0),
            new Among("ing", -1, 5, 0),
            new Among("arij", -1, 6, 0),
            new Among("erij", -1, 5, 0),
            new Among("sel", -1, 3, 0),
            new Among("rder", -1, 4, 0),
            new Among("ster", -1, 3, 0),
            new Among("iteit", -1, 2, 0),
            new Among("dst", -1, 10, 0),
            new Among("tst", -1, 9, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("end", -1, 9, 0),
            new Among("atief", -1, 2, 0),
            new Among("erig", -1, 9, 0),
            new Among("achtig", -1, 3, 0),
            new Among("ioneel", -1, 1, 0),
            new Among("baar", -1, 3, 0),
            new Among("laar", -1, 5, 0),
            new Among("naar", -1, 4, 0),
            new Among("raar", -1, 6, 0),
            new Among("eriger", -1, 9, 0),
            new Among("achtiger", -1, 3, 0),
            new Among("lijker", -1, 8, 0),
            new Among("tant", -1, 7, 0),
            new Among("erigst", -1, 9, 0),
            new Among("achtigst", -1, 3, 0),
            new Among("lijkst", -1, 8, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ig", -1, 1, 0),
            new Among("iger", -1, 1, 0),
            new Among("igst", -1, 1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("ft", -1, 2, 0),
            new Among("kt", -1, 1, 0),
            new Among("pt", -1, 3, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("bb", -1, 1, 0),
            new Among("cc", -1, 2, 0),
            new Among("dd", -1, 3, 0),
            new Among("ff", -1, 4, 0),
            new Among("gg", -1, 5, 0),
            new Among("hh", -1, 6, 0),
            new Among("jj", -1, 7, 0),
            new Among("kk", -1, 8, 0),
            new Among("ll", -1, 9, 0),
            new Among("mm", -1, 10, 0),
            new Among("nn", -1, 11, 0),
            new Among("pp", -1, 12, 0),
            new Among("qq", -1, 13, 0),
            new Among("rr", -1, 14, 0),
            new Among("ss", -1, 15, 0),
            new Among("tt", -1, 16, 0),
            new Among("v", -1, 4, 0),
            new Among("vv", 16, 17, 0),
            new Among("ww", -1, 18, 0),
            new Among("xx", -1, 19, 0),
            new Among("z", -1, 15, 0),
            new Among("zz", 20, 20, 0)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("d", -1, 1, 0),
            new Among("t", -1, 2, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("", -1, -1, 0),
            new Among("eft", 0, 1, 0),
            new Among("vaa", 0, 1, 0),
            new Among("val", 0, 1, 0),
            new Among("vali", 3, -1, 0),
            new Among("vare", 0, 1, 0)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("ë", -1, 1, 0),
            new Among("ï", -1, 2, 0)
        };

        private static readonly Among[] a_11 = new[]
        {
            new Among("ë", -1, 1, 0),
            new Among("ï", -1, 2, 0)
        };


        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_R2()
        {
            return I_p2 <= cursor;
        }

        private bool r_V()
        {
            {
                int c1 = limit - cursor;
                {
                    int c2 = limit - cursor;
                    if (in_grouping_b(g_v, 97, 252, false) != 0)
                    {
                        goto lab1;
                    }
                    goto lab0;
                lab1: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("ij")))
                    {
                        return false;
                    }
                }
            lab0: ;
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_VX()
        {
            {
                int c1 = limit - cursor;
                if (cursor <= limit_backward)
                {
                    return false;
                }
                cursor--;
                {
                    int c2 = limit - cursor;
                    if (in_grouping_b(g_v, 97, 252, false) != 0)
                    {
                        goto lab1;
                    }
                    goto lab0;
                lab1: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("ij")))
                    {
                        return false;
                    }
                }
            lab0: ;
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_C()
        {
            {
                int c1 = limit - cursor;
                {
                    int c2 = limit - cursor;
                    if (!(eq_s_b("ij")))
                    {
                        goto lab0;
                    }
                    return false;
                lab0: ;
                    cursor = limit - c2;
                }
                if (out_grouping_b(g_v, 97, 252, false) != 0)
                {
                    return false;
                }
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_lengthen_V()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                if (out_grouping_b(g_v_WX, 97, 252, false) != 0)
                {
                    goto lab0;
                }
                ket = cursor;
                among_var = find_among_b(a_0, null);
                if (among_var == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        {
                            int c2 = limit - cursor;
                            {
                                int c3 = limit - cursor;
                                if (out_grouping_b(g_AEIOU, 97, 252, false) != 0)
                                {
                                    goto lab2;
                                }
                                goto lab1;
                            lab2: ;
                                cursor = limit - c3;
                                if (cursor > limit_backward)
                                {
                                    goto lab0;
                                }
                            }
                        lab1: ;
                            cursor = limit - c2;
                        }
                        slice_to(S_ch);
                        {
                            int c = cursor;
                            insert(cursor, cursor, S_ch);
                            cursor = c;
                        }
                        break;
                    }
                    case 2: {
                        {
                            int c4 = limit - cursor;
                            {
                                int c5 = limit - cursor;
                                if (out_grouping_b(g_AEIOU, 97, 252, false) != 0)
                                {
                                    goto lab4;
                                }
                                goto lab3;
                            lab4: ;
                                cursor = limit - c5;
                                if (cursor > limit_backward)
                                {
                                    goto lab0;
                                }
                            }
                        lab3: ;
                            {
                                int c6 = limit - cursor;
                                {
                                    int c7 = limit - cursor;
                                    if (in_grouping_b(g_AIOU, 97, 252, false) != 0)
                                    {
                                        goto lab7;
                                    }
                                    goto lab6;
                                lab7: ;
                                    cursor = limit - c7;
                                    if (in_grouping_b(g_E, 101, 235, false) != 0)
                                    {
                                        goto lab5;
                                    }
                                    if (cursor > limit_backward)
                                    {
                                        goto lab5;
                                    }
                                }
                            lab6: ;
                                goto lab0;
                            lab5: ;
                                cursor = limit - c6;
                            }
                            {
                                int c8 = limit - cursor;
                                if (cursor <= limit_backward)
                                {
                                    goto lab8;
                                }
                                cursor--;
                                if (in_grouping_b(g_AIOU, 97, 252, false) != 0)
                                {
                                    goto lab8;
                                }
                                if (out_grouping_b(g_AEIOU, 97, 252, false) != 0)
                                {
                                    goto lab8;
                                }
                                goto lab0;
                            lab8: ;
                                cursor = limit - c8;
                            }
                            cursor = limit - c4;
                        }
                        slice_to(S_ch);
                        {
                            int c = cursor;
                            insert(cursor, cursor, S_ch);
                            cursor = c;
                        }
                        break;
                    }
                    case 3: {
                        slice_from("eëe");
                        break;
                    }
                    case 4: {
                        slice_from("iee");
                        break;
                    }
                }
            lab0: ;
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_Step_1()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_1, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_del();
                    break;
                }
                case 2: {
                    if (!r_R1())
                        return false;
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("t")))
                        {
                            goto lab0;
                        }
                        if (!r_R1())
                            goto lab0;
                        return false;
                    lab0: ;
                        cursor = limit - c1;
                    }
                    if (!r_C())
                        return false;
                    slice_del();
                    break;
                }
                case 3: {
                    if (!r_R1())
                        return false;
                    slice_from("ie");
                    break;
                }
                case 4: {
                    {
                        int c2 = limit - cursor;
                        {
                            int c3 = limit - cursor;
                            if (!(eq_s_b("ar")))
                            {
                                goto lab2;
                            }
                            if (!r_R1())
                                goto lab2;
                            if (!r_C())
                                goto lab2;
                            cursor = limit - c3;
                        }
                        slice_del();
                        r_lengthen_V();
                        goto lab1;
                    lab2: ;
                        cursor = limit - c2;
                        {
                            int c4 = limit - cursor;
                            if (!(eq_s_b("er")))
                            {
                                goto lab3;
                            }
                            if (!r_R1())
                                goto lab3;
                            if (!r_C())
                                goto lab3;
                            cursor = limit - c4;
                        }
                        slice_del();
                        goto lab1;
                    lab3: ;
                        cursor = limit - c2;
                        if (!r_R1())
                            return false;
                        if (!r_C())
                            return false;
                        slice_from("e");
                    }
                lab1: ;
                    break;
                }
                case 5: {
                    if (!r_R1())
                        return false;
                    slice_from("é");
                    break;
                }
                case 6: {
                    if (!r_R1())
                        return false;
                    if (!r_V())
                        return false;
                    slice_from("au");
                    break;
                }
                case 7: {
                    {
                        int c5 = limit - cursor;
                        if (!(eq_s_b("hed")))
                        {
                            goto lab5;
                        }
                        if (!r_R1())
                            goto lab5;
                        bra = cursor;
                        slice_from("heid");
                        goto lab4;
                    lab5: ;
                        cursor = limit - c5;
                        if (!(eq_s_b("nd")))
                        {
                            goto lab6;
                        }
                        slice_del();
                        goto lab4;
                    lab6: ;
                        cursor = limit - c5;
                        if (!(eq_s_b("d")))
                        {
                            goto lab7;
                        }
                        if (!r_R1())
                            goto lab7;
                        if (!r_C())
                            goto lab7;
                        bra = cursor;
                        slice_del();
                        goto lab4;
                    lab7: ;
                        cursor = limit - c5;
                        {
                            int c6 = limit - cursor;
                            if (!(eq_s_b("i")))
                            {
                                goto lab10;
                            }
                            goto lab9;
                        lab10: ;
                            cursor = limit - c6;
                            if (!(eq_s_b("j")))
                            {
                                goto lab8;
                            }
                        }
                    lab9: ;
                        if (!r_V())
                            goto lab8;
                        slice_del();
                        goto lab4;
                    lab8: ;
                        cursor = limit - c5;
                        if (!r_R1())
                            return false;
                        if (!r_C())
                            return false;
                        slice_del();
                        r_lengthen_V();
                    }
                lab4: ;
                    break;
                }
                case 8: {
                    slice_from("nd");
                    break;
                }
            }
            return true;
        }

        private bool r_Step_2()
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
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("'t")))
                        {
                            goto lab1;
                        }
                        bra = cursor;
                        slice_del();
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("et")))
                        {
                            goto lab2;
                        }
                        bra = cursor;
                        if (!r_R1())
                            goto lab2;
                        if (!r_C())
                            goto lab2;
                        slice_del();
                        goto lab0;
                    lab2: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("rnt")))
                        {
                            goto lab3;
                        }
                        bra = cursor;
                        slice_from("rn");
                        goto lab0;
                    lab3: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("t")))
                        {
                            goto lab4;
                        }
                        bra = cursor;
                        if (!r_R1())
                            goto lab4;
                        if (!r_VX())
                            goto lab4;
                        slice_del();
                        goto lab0;
                    lab4: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("ink")))
                        {
                            goto lab5;
                        }
                        bra = cursor;
                        slice_from("ing");
                        goto lab0;
                    lab5: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("mp")))
                        {
                            goto lab6;
                        }
                        bra = cursor;
                        slice_from("m");
                        goto lab0;
                    lab6: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("'")))
                        {
                            goto lab7;
                        }
                        bra = cursor;
                        if (!r_R1())
                            goto lab7;
                        slice_del();
                        goto lab0;
                    lab7: ;
                        cursor = limit - c1;
                        bra = cursor;
                        if (!r_R1())
                            return false;
                        if (!r_C())
                            return false;
                        slice_del();
                    }
                lab0: ;
                    break;
                }
                case 2: {
                    if (!r_R1())
                        return false;
                    slice_from("g");
                    break;
                }
                case 3: {
                    if (!r_R1())
                        return false;
                    slice_from("lijk");
                    break;
                }
                case 4: {
                    if (!r_R1())
                        return false;
                    slice_from("isch");
                    break;
                }
                case 5: {
                    if (!r_R1())
                        return false;
                    if (!r_C())
                        return false;
                    slice_del();
                    break;
                }
                case 6: {
                    if (!r_R1())
                        return false;
                    slice_from("t");
                    break;
                }
                case 7: {
                    if (!r_R1())
                        return false;
                    slice_from("s");
                    break;
                }
                case 8: {
                    if (!r_R1())
                        return false;
                    slice_from("r");
                    break;
                }
                case 9: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    insert(cursor, cursor, "l");
                    r_lengthen_V();
                    break;
                }
                case 10: {
                    if (!r_R1())
                        return false;
                    if (!r_C())
                        return false;
                    slice_del();
                    insert(cursor, cursor, "en");
                    r_lengthen_V();
                    break;
                }
                case 11: {
                    if (!r_R1())
                        return false;
                    if (!r_C())
                        return false;
                    slice_from("ief");
                    break;
                }
            }
            return true;
        }

        private bool r_Step_3()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_3, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R1())
                        return false;
                    slice_from("eer");
                    break;
                }
                case 2: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    r_lengthen_V();
                    break;
                }
                case 3: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    break;
                }
                case 4: {
                    slice_from("r");
                    break;
                }
                case 5: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("ild")))
                        {
                            goto lab1;
                        }
                        slice_from("er");
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!r_R1())
                            return false;
                        slice_del();
                        r_lengthen_V();
                    }
                lab0: ;
                    break;
                }
                case 6: {
                    if (!r_R1())
                        return false;
                    if (!r_C())
                        return false;
                    slice_from("aar");
                    break;
                }
                case 7: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    insert(cursor, cursor, "f");
                    r_lengthen_V();
                    break;
                }
                case 8: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    insert(cursor, cursor, "g");
                    r_lengthen_V();
                    break;
                }
                case 9: {
                    if (!r_R1())
                        return false;
                    if (!r_C())
                        return false;
                    slice_from("t");
                    break;
                }
                case 10: {
                    if (!r_R1())
                        return false;
                    if (!r_C())
                        return false;
                    slice_from("d");
                    break;
                }
            }
            return true;
        }

        private bool r_Step_4()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_4, null);
                if (among_var == 0)
                {
                    goto lab1;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (!r_R1())
                            goto lab1;
                        slice_from("ie");
                        break;
                    }
                    case 2: {
                        if (!r_R1())
                            goto lab1;
                        slice_from("eer");
                        break;
                    }
                    case 3: {
                        if (!r_R1())
                            goto lab1;
                        slice_del();
                        break;
                    }
                    case 4: {
                        if (!r_R1())
                            goto lab1;
                        if (!r_V())
                            goto lab1;
                        slice_from("n");
                        break;
                    }
                    case 5: {
                        if (!r_R1())
                            goto lab1;
                        if (!r_V())
                            goto lab1;
                        slice_from("l");
                        break;
                    }
                    case 6: {
                        if (!r_R1())
                            goto lab1;
                        if (!r_V())
                            goto lab1;
                        slice_from("r");
                        break;
                    }
                    case 7: {
                        if (!r_R1())
                            goto lab1;
                        slice_from("teer");
                        break;
                    }
                    case 8: {
                        if (!r_R1())
                            goto lab1;
                        slice_from("lijk");
                        break;
                    }
                    case 9: {
                        if (!r_R1())
                            goto lab1;
                        if (!r_C())
                            goto lab1;
                        slice_del();
                        r_lengthen_V();
                        break;
                    }
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                if (find_among_b(a_5, null) == 0)
                {
                    return false;
                }
                bra = cursor;
                if (!r_R1())
                    return false;
                {
                    int c2 = limit - cursor;
                    if (!(eq_s_b("inn")))
                    {
                        goto lab2;
                    }
                    if (cursor > limit_backward)
                    {
                        goto lab2;
                    }
                    return false;
                lab2: ;
                    cursor = limit - c2;
                }
                if (!r_C())
                    return false;
                slice_del();
                r_lengthen_V();
            }
        lab0: ;
            return true;
        }

        private bool r_Step_7()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_6, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_from("k");
                    break;
                }
                case 2: {
                    slice_from("f");
                    break;
                }
                case 3: {
                    slice_from("p");
                    break;
                }
            }
            return true;
        }

        private bool r_Step_6()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_7, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_from("b");
                    break;
                }
                case 2: {
                    slice_from("c");
                    break;
                }
                case 3: {
                    slice_from("d");
                    break;
                }
                case 4: {
                    slice_from("f");
                    break;
                }
                case 5: {
                    slice_from("g");
                    break;
                }
                case 6: {
                    slice_from("h");
                    break;
                }
                case 7: {
                    slice_from("j");
                    break;
                }
                case 8: {
                    slice_from("k");
                    break;
                }
                case 9: {
                    slice_from("l");
                    break;
                }
                case 10: {
                    slice_from("m");
                    break;
                }
                case 11: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("i")))
                        {
                            goto lab0;
                        }
                        if (cursor > limit_backward)
                        {
                            goto lab0;
                        }
                        return false;
                    lab0: ;
                        cursor = limit - c1;
                    }
                    slice_from("n");
                    break;
                }
                case 12: {
                    slice_from("p");
                    break;
                }
                case 13: {
                    slice_from("q");
                    break;
                }
                case 14: {
                    slice_from("r");
                    break;
                }
                case 15: {
                    slice_from("s");
                    break;
                }
                case 16: {
                    slice_from("t");
                    break;
                }
                case 17: {
                    slice_from("v");
                    break;
                }
                case 18: {
                    slice_from("w");
                    break;
                }
                case 19: {
                    slice_from("x");
                    break;
                }
                case 20: {
                    slice_from("z");
                    break;
                }
            }
            return true;
        }

        private bool r_Step_1c()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_8, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            if (!r_C())
                return false;
            switch (among_var) {
                case 1: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("n")))
                        {
                            goto lab0;
                        }
                        if (!r_R1())
                            goto lab0;
                        return false;
                    lab0: ;
                        cursor = limit - c1;
                    }
                    {
                        int c2 = limit - cursor;
                        if (!(eq_s_b("in")))
                        {
                            goto lab2;
                        }
                        if (cursor > limit_backward)
                        {
                            goto lab2;
                        }
                        slice_from("n");
                        goto lab1;
                    lab2: ;
                        cursor = limit - c2;
                        slice_del();
                    }
                lab1: ;
                    break;
                }
                case 2: {
                    {
                        int c3 = limit - cursor;
                        if (!(eq_s_b("h")))
                        {
                            goto lab3;
                        }
                        if (!r_R1())
                            goto lab3;
                        return false;
                    lab3: ;
                        cursor = limit - c3;
                    }
                    {
                        int c4 = limit - cursor;
                        if (!(eq_s_b("en")))
                        {
                            goto lab4;
                        }
                        if (cursor > limit_backward)
                        {
                            goto lab4;
                        }
                        return false;
                    lab4: ;
                        cursor = limit - c4;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_Lose_prefix()
        {
            int among_var;
            bra = cursor;
            if (!(eq_s("ge")))
            {
                return false;
            }
            ket = cursor;
            {
                int c1 = cursor;
                {
                    int c = cursor + 3;
                    if (c > limit)
                    {
                        return false;
                    }
                    cursor = c;
                }
                cursor = c1;
            }
            {
                int c2 = cursor;
                while (true)
                {
                    int c3 = cursor;
                    {
                        int c4 = cursor;
                        if (!(eq_s("ij")))
                        {
                            goto lab2;
                        }
                        goto lab1;
                    lab2: ;
                        cursor = c4;
                        if (in_grouping(g_v, 97, 252, false) != 0)
                        {
                            goto lab0;
                        }
                    }
                lab1: ;
                    break;
                lab0: ;
                    cursor = c3;
                    if (cursor >= limit)
                    {
                        return false;
                    }
                    cursor++;
                }
                while (true)
                {
                    int c5 = cursor;
                    {
                        int c6 = cursor;
                        if (!(eq_s("ij")))
                        {
                            goto lab5;
                        }
                        goto lab4;
                    lab5: ;
                        cursor = c6;
                        if (in_grouping(g_v, 97, 252, false) != 0)
                        {
                            goto lab3;
                        }
                    }
                lab4: ;
                    continue;
                lab3: ;
                    cursor = c5;
                    break;
                }
                if (cursor < limit)
                {
                    goto lab6;
                }
                return false;
            lab6: ;
                cursor = c2;
            }
            among_var = find_among(a_9, null);
            switch (among_var) {
                case 1: {
                    return false;
                    break;
                }
            }
            B_GE_removed = true;
            slice_del();
            {
                int c7 = cursor;
                bra = cursor;
                among_var = find_among(a_10, null);
                if (among_var == 0)
                {
                    goto lab7;
                }
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("e");
                        break;
                    }
                    case 2: {
                        slice_from("i");
                        break;
                    }
                }
            lab7: ;
                cursor = c7;
            }
            return true;
        }

        private bool r_Lose_infix()
        {
            int among_var;
            if (cursor >= limit)
            {
                return false;
            }
            cursor++;
            while (true)
            {
                bra = cursor;
                if (!(eq_s("ge")))
                {
                    goto lab0;
                }
                ket = cursor;
                break;
            lab0: ;
                if (cursor >= limit)
                {
                    return false;
                }
                cursor++;
            }
            {
                int c1 = cursor;
                {
                    int c = cursor + 3;
                    if (c > limit)
                    {
                        return false;
                    }
                    cursor = c;
                }
                cursor = c1;
            }
            {
                int c2 = cursor;
                while (true)
                {
                    int c3 = cursor;
                    {
                        int c4 = cursor;
                        if (!(eq_s("ij")))
                        {
                            goto lab3;
                        }
                        goto lab2;
                    lab3: ;
                        cursor = c4;
                        if (in_grouping(g_v, 97, 252, false) != 0)
                        {
                            goto lab1;
                        }
                    }
                lab2: ;
                    break;
                lab1: ;
                    cursor = c3;
                    if (cursor >= limit)
                    {
                        return false;
                    }
                    cursor++;
                }
                while (true)
                {
                    int c5 = cursor;
                    {
                        int c6 = cursor;
                        if (!(eq_s("ij")))
                        {
                            goto lab6;
                        }
                        goto lab5;
                    lab6: ;
                        cursor = c6;
                        if (in_grouping(g_v, 97, 252, false) != 0)
                        {
                            goto lab4;
                        }
                    }
                lab5: ;
                    continue;
                lab4: ;
                    cursor = c5;
                    break;
                }
                if (cursor < limit)
                {
                    goto lab7;
                }
                return false;
            lab7: ;
                cursor = c2;
            }
            B_GE_removed = true;
            slice_del();
            {
                int c7 = cursor;
                bra = cursor;
                among_var = find_among(a_11, null);
                if (among_var == 0)
                {
                    goto lab8;
                }
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("e");
                        break;
                    }
                    case 2: {
                        slice_from("i");
                        break;
                    }
                }
            lab8: ;
                cursor = c7;
            }
            return true;
        }

        private bool r_measure()
        {
            I_p1 = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                while (true)
                {
                    if (out_grouping(g_v, 97, 252, false) != 0)
                    {
                        goto lab1;
                    }
                    continue;
                lab1: ;
                    break;
                }
                {
                    int c2 = 1;
                    while (true)
                    {
                        int c3 = cursor;
                        {
                            int c4 = cursor;
                            if (!(eq_s("ij")))
                            {
                                goto lab4;
                            }
                            goto lab3;
                        lab4: ;
                            cursor = c4;
                            if (in_grouping(g_v, 97, 252, false) != 0)
                            {
                                goto lab2;
                            }
                        }
                    lab3: ;
                        c2--;
                        continue;
                    lab2: ;
                        cursor = c3;
                        break;
                    }
                    if (c2 > 0)
                    {
                        goto lab0;
                    }
                }
                if (out_grouping(g_v, 97, 252, false) != 0)
                {
                    goto lab0;
                }
                I_p1 = cursor;
                while (true)
                {
                    if (out_grouping(g_v, 97, 252, false) != 0)
                    {
                        goto lab5;
                    }
                    continue;
                lab5: ;
                    break;
                }
                {
                    int c5 = 1;
                    while (true)
                    {
                        int c6 = cursor;
                        {
                            int c7 = cursor;
                            if (!(eq_s("ij")))
                            {
                                goto lab8;
                            }
                            goto lab7;
                        lab8: ;
                            cursor = c7;
                            if (in_grouping(g_v, 97, 252, false) != 0)
                            {
                                goto lab6;
                            }
                        }
                    lab7: ;
                        c5--;
                        continue;
                    lab6: ;
                        cursor = c6;
                        break;
                    }
                    if (c5 > 0)
                    {
                        goto lab0;
                    }
                }
                if (out_grouping(g_v, 97, 252, false) != 0)
                {
                    goto lab0;
                }
                I_p2 = cursor;
            lab0: ;
                cursor = c1;
            }
            return true;
        }

        protected override bool stem()
        {
            bool B_stemmed;
            B_stemmed = false;
            r_measure();
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                if (!r_Step_1())
                    goto lab0;
                B_stemmed = true;
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c2 = limit - cursor;
                if (!r_Step_2())
                    goto lab1;
                B_stemmed = true;
            lab1: ;
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                if (!r_Step_3())
                    goto lab2;
                B_stemmed = true;
            lab2: ;
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                if (!r_Step_4())
                    goto lab3;
                B_stemmed = true;
            lab3: ;
                cursor = limit - c4;
            }
            cursor = limit_backward;
            B_GE_removed = false;
            {
                int c5 = cursor;
                int c6 = cursor;
                if (!r_Lose_prefix())
                    goto lab4;
                cursor = c6;
                r_measure();
            lab4: ;
                cursor = c5;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c7 = limit - cursor;
                if (!B_GE_removed)
                {
                    goto lab5;
                }
                B_stemmed = true;
                if (!r_Step_1c())
                    goto lab5;
            lab5: ;
                cursor = limit - c7;
            }
            cursor = limit_backward;
            B_GE_removed = false;
            {
                int c8 = cursor;
                int c9 = cursor;
                if (!r_Lose_infix())
                    goto lab6;
                cursor = c9;
                r_measure();
            lab6: ;
                cursor = c8;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c10 = limit - cursor;
                if (!B_GE_removed)
                {
                    goto lab7;
                }
                B_stemmed = true;
                if (!r_Step_1c())
                    goto lab7;
            lab7: ;
                cursor = limit - c10;
            }
            cursor = limit_backward;
            limit_backward = cursor;
            cursor = limit;
            {
                int c11 = limit - cursor;
                if (!r_Step_7())
                    goto lab8;
                B_stemmed = true;
            lab8: ;
                cursor = limit - c11;
            }
            {
                int c12 = limit - cursor;
                if (!B_stemmed)
                {
                    goto lab9;
                }
                if (!r_Step_6())
                    goto lab9;
            lab9: ;
                cursor = limit - c12;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

