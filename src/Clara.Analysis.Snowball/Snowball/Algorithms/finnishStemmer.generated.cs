// Generated from finnish.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from finnish.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class FinnishStemmer : Stemmer
    {
        private StringBuilder S_x = new StringBuilder();

        private const string g_AEI = "aeiä";
        private const string g_C = "bcdfghjklmnpqrstvwxz";
        private const string g_v = "aeiouyäö";
        private const string g_particle_end = "aeinotuyäö";

        private static readonly Among[] a_0 = new[]
        {
            new Among("pa", -1, 1, 0),
            new Among("sti", -1, 2, 0),
            new Among("kaan", -1, 1, 0),
            new Among("han", -1, 1, 0),
            new Among("kin", -1, 1, 0),
            new Among("hän", -1, 1, 0),
            new Among("kään", -1, 1, 0),
            new Among("ko", -1, 1, 0),
            new Among("pä", -1, 1, 0),
            new Among("kö", -1, 1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("lla", -1, -1, 0),
            new Among("na", -1, -1, 0),
            new Among("ssa", -1, -1, 0),
            new Among("ta", -1, -1, 0),
            new Among("lta", 3, -1, 0),
            new Among("sta", 3, -1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("llä", -1, -1, 0),
            new Among("nä", -1, -1, 0),
            new Among("ssä", -1, -1, 0),
            new Among("tä", -1, -1, 0),
            new Among("ltä", 3, -1, 0),
            new Among("stä", 3, -1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("lle", -1, -1, 0),
            new Among("ine", -1, -1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("nsa", -1, 3, 0),
            new Among("mme", -1, 3, 0),
            new Among("nne", -1, 3, 0),
            new Among("ni", -1, 2, 0),
            new Among("si", -1, 1, 0),
            new Among("an", -1, 4, 0),
            new Among("en", -1, 6, 0),
            new Among("än", -1, 5, 0),
            new Among("nsä", -1, 3, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("aa", -1, -1, 0),
            new Among("ee", -1, -1, 0),
            new Among("ii", -1, -1, 0),
            new Among("oo", -1, -1, 0),
            new Among("uu", -1, -1, 0),
            new Among("ää", -1, -1, 0),
            new Among("öö", -1, -1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("'", -1, -1, 0),
            new Among("ai", -1, -1, 0),
            new Among("ei", -1, -1, 0),
            new Among("ii", -1, -1, 0),
            new Among("oi", -1, -1, 0),
            new Among("ui", -1, -1, 0),
            new Among("äi", -1, -1, 0),
            new Among("öi", -1, -1, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("a", -1, 2, 0),
            new Among("lla", 0, -1, 0),
            new Among("na", 0, -1, 0),
            new Among("ssa", 0, -1, 0),
            new Among("ta", 0, -1, 0),
            new Among("lta", 4, -1, 0),
            new Among("sta", 4, -1, 0),
            new Among("tta", 4, 3, 0),
            new Among("lle", -1, -1, 0),
            new Among("ine", -1, -1, 0),
            new Among("ksi", -1, -1, 0),
            new Among("n", -1, 1, 0),
            new Among("han", 11, -1, 3),
            new Among("den", 11, -1, 8),
            new Among("seen", 11, -1, 9),
            new Among("hen", 11, -1, 4),
            new Among("tten", 11, -1, 8),
            new Among("hin", 11, -1, 5),
            new Among("siin", 11, -1, 8),
            new Among("hon", 11, -1, 6),
            new Among("hun", 11, -1, 7),
            new Among("hän", 11, -1, 1),
            new Among("hön", 11, -1, 2),
            new Among("ä", -1, 2, 0),
            new Among("llä", 23, -1, 0),
            new Among("nä", 23, -1, 0),
            new Among("ssä", 23, -1, 0),
            new Among("tä", 23, -1, 0),
            new Among("ltä", 27, -1, 0),
            new Among("stä", 27, -1, 0),
            new Among("ttä", 27, 3, 0)
        };

        private bool af_7() {
            switch (af) {
                case 1: return r_A_();
                case 2: return r_O_();
                case 3: return r_A();
                case 4: return r_E();
                case 5: return r_I();
                case 6: return r_O();
                case 7: return r_U();
                case 8: return r_VI();
                case 9: return r_LV();
            }
            return false;
        }

        private static readonly Among[] a_8 = new[]
        {
            new Among("eja", -1, -1, 0),
            new Among("mma", -1, 1, 0),
            new Among("imma", 1, -1, 0),
            new Among("mpa", -1, 1, 0),
            new Among("impa", 3, -1, 0),
            new Among("mmi", -1, 1, 0),
            new Among("immi", 5, -1, 0),
            new Among("mpi", -1, 1, 0),
            new Among("impi", 7, -1, 0),
            new Among("ejä", -1, -1, 0),
            new Among("mmä", -1, 1, 0),
            new Among("immä", 10, -1, 0),
            new Among("mpä", -1, 1, 0),
            new Among("impä", 12, -1, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("i", -1, -1, 0),
            new Among("j", -1, -1, 0)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("mma", -1, 1, 0),
            new Among("imma", 0, -1, 0)
        };


        private bool r_LV()
        {
            return find_among_b(a_5, null) != 0;
        }

        private bool r_VI()
        {
            return find_among_b(a_6, null) != 0;
        }

        private bool r_A()
        {
            if (!(eq_s_b("a")))
            {
                goto lab1;
            }
            goto lab0;
        lab1: ;
            if (!(eq_s_b("'")))
            {
                return false;
            }
        lab0: ;
            return true;
        }

        private bool r_E()
        {
            if (!(eq_s_b("e")))
            {
                goto lab1;
            }
            goto lab0;
        lab1: ;
            if (!(eq_s_b("'")))
            {
                return false;
            }
        lab0: ;
            return true;
        }

        private bool r_I()
        {
            if (!(eq_s_b("i")))
            {
                goto lab1;
            }
            goto lab0;
        lab1: ;
            if (!(eq_s_b("'")))
            {
                return false;
            }
        lab0: ;
            return true;
        }

        private bool r_O()
        {
            if (!(eq_s_b("o")))
            {
                goto lab1;
            }
            goto lab0;
        lab1: ;
            if (!(eq_s_b("'")))
            {
                return false;
            }
        lab0: ;
            return true;
        }

        private bool r_U()
        {
            if (!(eq_s_b("u")))
            {
                goto lab1;
            }
            goto lab0;
        lab1: ;
            if (!(eq_s_b("'")))
            {
                return false;
            }
        lab0: ;
            return true;
        }

        private bool r_A_()
        {
            if (!(eq_s_b("ä")))
            {
                goto lab1;
            }
            goto lab0;
        lab1: ;
            if (!(eq_s_b("'")))
            {
                return false;
            }
        lab0: ;
            return true;
        }

        private bool r_O_()
        {
            if (!(eq_s_b("ö")))
            {
                goto lab1;
            }
            goto lab0;
        lab1: ;
            if (!(eq_s_b("ø")))
            {
                goto lab2;
            }
            goto lab0;
        lab2: ;
            if (!(eq_s_b("'")))
            {
                return false;
            }
        lab0: ;
            return true;
        }

        protected override bool stem()
        {
            int among_var;
            bool B_ending_removed;
            int I_p2;
            int I_p1;
            {
                int c1 = cursor;
                I_p1 = limit;
                I_p2 = limit;
                {

                    int ret = out_grouping(g_v, 97, 246, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 246, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 246, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 246, true);
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
            B_ending_removed = false;
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab1;
                }
                int c3 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                among_var = find_among_b(a_0, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c3;
                        goto lab1;
                    }
                }
                bra = cursor;
                limit_backward = c3;
                switch (among_var) {
                    case 1: {
                        if (in_grouping_b(g_particle_end, 97, 246, false) != 0)
                        {
                            goto lab1;
                        }
                        break;
                    }
                    case 2: {
                        if (I_p2 > cursor)
                        {
                            goto lab1;
                        }
                        break;
                    }
                }
                slice_del();
            lab1: ;
                cursor = limit - c2;
            }
            {
                int c4 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab2;
                }
                int c5 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                among_var = find_among_b(a_4, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c5;
                        goto lab2;
                    }
                }
                bra = cursor;
                limit_backward = c5;
                switch (among_var) {
                    case 1: {
                        if (!(eq_s_b("k")))
                        {
                            goto lab3;
                        }
                        goto lab2;
                    lab3: ;
                        slice_del();
                        break;
                    }
                    case 2: {
                        slice_del();
                        ket = cursor;
                        if (!(eq_s_b("kse")))
                        {
                            goto lab2;
                        }
                        bra = cursor;
                        slice_from("ksi");
                        break;
                    }
                    case 3: {
                        slice_del();
                        break;
                    }
                    case 4: {
                        if (find_among_b(a_1, null) == 0)
                        {
                            goto lab2;
                        }
                        slice_del();
                        break;
                    }
                    case 5: {
                        if (find_among_b(a_2, null) == 0)
                        {
                            goto lab2;
                        }
                        slice_del();
                        break;
                    }
                    case 6: {
                        if (find_among_b(a_3, null) == 0)
                        {
                            goto lab2;
                        }
                        slice_del();
                        break;
                    }
                }
            lab2: ;
                cursor = limit - c4;
            }
            {
                int c6 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab4;
                }
                int c7 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                among_var = find_among_b(a_7, af_7);
                if (among_var == 0)
                {
                    {
                        limit_backward = c7;
                        goto lab4;
                    }
                }
                bra = cursor;
                limit_backward = c7;
                switch (among_var) {
                    case 1: {
                        {
                            int c8 = limit - cursor;
                            int c9 = limit - cursor;
                            {
                                int c10 = limit - cursor;
                                if (!r_LV())
                                    goto lab7;
                                goto lab6;
                            lab7: ;
                                cursor = limit - c10;
                                if (!(eq_s_b("ie")))
                                {
                                    {
                                        cursor = limit - c8;
                                        goto lab5;
                                    }
                                }
                            }
                        lab6: ;
                            cursor = limit - c9;
                            if (cursor <= limit_backward)
                            {
                                {
                                    cursor = limit - c8;
                                    goto lab5;
                                }
                            }
                            cursor--;
                            bra = cursor;
                        lab5: ;
                        }
                        break;
                    }
                    case 2: {
                        if (in_grouping_b(g_v, 97, 246, false) != 0)
                        {
                            goto lab4;
                        }
                        if (in_grouping_b(g_C, 98, 122, false) != 0)
                        {
                            goto lab4;
                        }
                        break;
                    }
                    case 3: {
                        if (!(eq_s_b("e")))
                        {
                            goto lab4;
                        }
                        break;
                    }
                }
                slice_del();
                B_ending_removed = true;
            lab4: ;
                cursor = limit - c6;
            }
            {
                int c11 = limit - cursor;
                if (cursor < I_p2)
                {
                    goto lab8;
                }
                int c12 = limit_backward;
                limit_backward = I_p2;
                ket = cursor;
                among_var = find_among_b(a_8, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c12;
                        goto lab8;
                    }
                }
                bra = cursor;
                limit_backward = c12;
                switch (among_var) {
                    case 1: {
                        if (!(eq_s_b("po")))
                        {
                            goto lab9;
                        }
                        goto lab8;
                    lab9: ;
                        break;
                    }
                }
                slice_del();
            lab8: ;
                cursor = limit - c11;
            }
            if (!B_ending_removed)
            {
                goto lab11;
            }
            {
                int c13 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab12;
                }
                int c14 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                if (find_among_b(a_9, null) == 0)
                {
                    {
                        limit_backward = c14;
                        goto lab12;
                    }
                }
                bra = cursor;
                limit_backward = c14;
                slice_del();
            lab12: ;
                cursor = limit - c13;
            }
            goto lab10;
        lab11: ;
            {
                int c15 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab13;
                }
                int c16 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                if (!(eq_s_b("t")))
                {
                    {
                        limit_backward = c16;
                        goto lab13;
                    }
                }
                bra = cursor;
                {
                    int c17 = limit - cursor;
                    if (in_grouping_b(g_v, 97, 246, false) != 0)
                    {
                        {
                            limit_backward = c16;
                            goto lab13;
                        }
                    }
                    cursor = limit - c17;
                }
                slice_del();
                limit_backward = c16;
                if (cursor < I_p2)
                {
                    goto lab13;
                }
                int c18 = limit_backward;
                limit_backward = I_p2;
                ket = cursor;
                among_var = find_among_b(a_10, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c18;
                        goto lab13;
                    }
                }
                bra = cursor;
                limit_backward = c18;
                switch (among_var) {
                    case 1: {
                        if (!(eq_s_b("po")))
                        {
                            goto lab14;
                        }
                        goto lab13;
                    lab14: ;
                        break;
                    }
                }
                slice_del();
            lab13: ;
                cursor = limit - c15;
            }
        lab10: ;
            {
                int c19 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab15;
                }
                int c20 = limit_backward;
                limit_backward = I_p1;
                {
                    int c21 = limit - cursor;
                    int c22 = limit - cursor;
                    if (!r_LV())
                        goto lab16;
                    cursor = limit - c22;
                    ket = cursor;
                    if (cursor <= limit_backward)
                    {
                        goto lab16;
                    }
                    cursor--;
                    bra = cursor;
                    slice_del();
                lab16: ;
                    cursor = limit - c21;
                }
                {
                    int c23 = limit - cursor;
                    ket = cursor;
                    if (in_grouping_b(g_AEI, 97, 228, false) != 0)
                    {
                        goto lab17;
                    }
                    bra = cursor;
                    if (in_grouping_b(g_C, 98, 122, false) != 0)
                    {
                        goto lab17;
                    }
                    slice_del();
                lab17: ;
                    cursor = limit - c23;
                }
                {
                    int c24 = limit - cursor;
                    ket = cursor;
                    if (!(eq_s_b("j")))
                    {
                        goto lab18;
                    }
                    bra = cursor;
                    if (!(eq_s_b("o")))
                    {
                        goto lab20;
                    }
                    goto lab19;
                lab20: ;
                    if (!(eq_s_b("u")))
                    {
                        goto lab18;
                    }
                lab19: ;
                    slice_del();
                lab18: ;
                    cursor = limit - c24;
                }
                {
                    int c25 = limit - cursor;
                    ket = cursor;
                    if (!(eq_s_b("o")))
                    {
                        goto lab21;
                    }
                    bra = cursor;
                    if (!(eq_s_b("j")))
                    {
                        goto lab21;
                    }
                    slice_del();
                lab21: ;
                    cursor = limit - c25;
                }
                limit_backward = c20;
                {
                    int c26 = limit - cursor;
                    if (in_grouping_b(g_v, 97, 246, true) < 0)
                    {
                        goto lab22;
                    }

                    ket = cursor;
                    if (in_grouping_b(g_C, 98, 122, false) != 0)
                    {
                        goto lab22;
                    }
                    bra = cursor;
                    slice_to(S_x);
                    if (!(eq_s_b(S_x)))
                    {
                        goto lab22;
                    }
                    slice_del();
                lab22: ;
                    cursor = limit - c26;
                }
                ket = cursor;
                if (!(eq_s_b("'")))
                {
                    goto lab15;
                }
                bra = cursor;
                slice_del();
            lab15: ;
                cursor = limit - c19;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

