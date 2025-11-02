// Generated from finnish.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from finnish.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class FinnishStemmer : Stemmer
    {
        private bool B_ending_removed;
        private StringBuilder S_x = new StringBuilder();
        private int I_p2;
        private int I_p1;

        private const string g_AEI = "aeiä";
        private const string g_C = "bcdfghjklmnpqrstvwxz";
        private const string g_V1 = "aeiouyäö";
        private const string g_V2 = "aeiouäö";
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
            new Among("a", -1, 8, 0),
            new Among("lla", 0, -1, 0),
            new Among("na", 0, -1, 0),
            new Among("ssa", 0, -1, 0),
            new Among("ta", 0, -1, 0),
            new Among("lta", 4, -1, 0),
            new Among("sta", 4, -1, 0),
            new Among("tta", 4, 2, 0),
            new Among("lle", -1, -1, 0),
            new Among("ine", -1, -1, 0),
            new Among("ksi", -1, -1, 0),
            new Among("n", -1, 7, 0),
            new Among("han", 11, 1, 0),
            new Among("den", 11, -1, 1),
            new Among("seen", 11, -1, 2),
            new Among("hen", 11, 2, 0),
            new Among("tten", 11, -1, 1),
            new Among("hin", 11, 3, 0),
            new Among("siin", 11, -1, 1),
            new Among("hon", 11, 4, 0),
            new Among("hän", 11, 5, 0),
            new Among("hön", 11, 6, 0),
            new Among("ä", -1, 8, 0),
            new Among("llä", 22, -1, 0),
            new Among("nä", 22, -1, 0),
            new Among("ssä", 22, -1, 0),
            new Among("tä", 22, -1, 0),
            new Among("ltä", 26, -1, 0),
            new Among("stä", 26, -1, 0),
            new Among("ttä", 26, 2, 0)
        };

        private bool af_6() {
            switch (af) {
                case 1: return r_VI();
                case 2: return r_LONG();
            }
            return false;
        }

        private static readonly Among[] a_7 = new[]
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

        private static readonly Among[] a_8 = new[]
        {
            new Among("i", -1, -1, 0),
            new Among("j", -1, -1, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("mma", -1, 1, 0),
            new Among("imma", 0, -1, 0)
        };


        private bool r_mark_regions()
        {
            I_p1 = limit;
            I_p2 = limit;
            {

                int ret = out_grouping(g_V1, 97, 246, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_V1, 97, 246, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            I_p1 = cursor;
            {

                int ret = out_grouping(g_V1, 97, 246, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_V1, 97, 246, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            I_p2 = cursor;
            return true;
        }

        private bool r_R2()
        {
            return I_p2 <= cursor;
        }

        private bool r_particle_etc()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_0, null);
            if (among_var == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            switch (among_var) {
                case 1: {
                    if (in_grouping_b(g_particle_end, 97, 246, false) != 0)
                    {
                        return false;
                    }
                    break;
                }
                case 2: {
                    if (!r_R2())
                        return false;
                    break;
                }
            }
            slice_del();
            return true;
        }

        private bool r_possessive()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_4, null);
            if (among_var == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            switch (among_var) {
                case 1: {
                    {
                        int c2 = limit - cursor;
                        if (!(eq_s_b("k")))
                        {
                            goto lab0;
                        }
                        return false;
                    lab0: ;
                        cursor = limit - c2;
                    }
                    slice_del();
                    break;
                }
                case 2: {
                    slice_del();
                    ket = cursor;
                    if (!(eq_s_b("kse")))
                    {
                        return false;
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
                        return false;
                    }
                    slice_del();
                    break;
                }
                case 5: {
                    if (find_among_b(a_2, null) == 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
                case 6: {
                    if (find_among_b(a_3, null) == 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_LONG()
        {
            return find_among_b(a_5, null) != 0;
        }

        private bool r_VI()
        {
            if (!(eq_s_b("i")))
            {
                return false;
            }
            return (in_grouping_b(g_V2, 97, 246, false) == 0);
        }

        private bool r_case_ending()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_6, af_6);
            if (among_var == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            switch (among_var) {
                case 1: {
                    if (!(eq_s_b("a")))
                    {
                        return false;
                    }
                    break;
                }
                case 2: {
                    if (!(eq_s_b("e")))
                    {
                        return false;
                    }
                    break;
                }
                case 3: {
                    if (!(eq_s_b("i")))
                    {
                        return false;
                    }
                    break;
                }
                case 4: {
                    if (!(eq_s_b("o")))
                    {
                        return false;
                    }
                    break;
                }
                case 5: {
                    if (!(eq_s_b("ä")))
                    {
                        return false;
                    }
                    break;
                }
                case 6: {
                    if (!(eq_s_b("ö")))
                    {
                        return false;
                    }
                    break;
                }
                case 7: {
                    {
                        int c2 = limit - cursor;
                        int c3 = limit - cursor;
                        {
                            int c4 = limit - cursor;
                            if (!r_LONG())
                                goto lab2;
                            goto lab1;
                        lab2: ;
                            cursor = limit - c4;
                            if (!(eq_s_b("ie")))
                            {
                                {
                                    cursor = limit - c2;
                                    goto lab0;
                                }
                            }
                        }
                    lab1: ;
                        cursor = limit - c3;
                        if (cursor <= limit_backward)
                        {
                            {
                                cursor = limit - c2;
                                goto lab0;
                            }
                        }
                        cursor--;
                        bra = cursor;
                    lab0: ;
                    }
                    break;
                }
                case 8: {
                    if (in_grouping_b(g_V1, 97, 246, false) != 0)
                    {
                        return false;
                    }
                    if (in_grouping_b(g_C, 98, 122, false) != 0)
                    {
                        return false;
                    }
                    break;
                }
            }
            slice_del();
            B_ending_removed = true;
            return true;
        }

        private bool r_other_endings()
        {
            int among_var;
            if (cursor < I_p2)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p2;
            ket = cursor;
            among_var = find_among_b(a_7, null);
            if (among_var == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            switch (among_var) {
                case 1: {
                    {
                        int c2 = limit - cursor;
                        if (!(eq_s_b("po")))
                        {
                            goto lab0;
                        }
                        return false;
                    lab0: ;
                        cursor = limit - c2;
                    }
                    break;
                }
            }
            slice_del();
            return true;
        }

        private bool r_i_plural()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            if (find_among_b(a_8, null) == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            slice_del();
            return true;
        }

        private bool r_t_plural()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            if (!(eq_s_b("t")))
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            {
                int c2 = limit - cursor;
                if (in_grouping_b(g_V1, 97, 246, false) != 0)
                {
                    {
                        limit_backward = c1;
                        return false;
                    }
                }
                cursor = limit - c2;
            }
            slice_del();
            limit_backward = c1;
            if (cursor < I_p2)
            {
                return false;
            }
            int c3 = limit_backward;
            limit_backward = I_p2;
            ket = cursor;
            among_var = find_among_b(a_9, null);
            if (among_var == 0)
            {
                {
                    limit_backward = c3;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c3;
            switch (among_var) {
                case 1: {
                    {
                        int c4 = limit - cursor;
                        if (!(eq_s_b("po")))
                        {
                            goto lab0;
                        }
                        return false;
                    lab0: ;
                        cursor = limit - c4;
                    }
                    break;
                }
            }
            slice_del();
            return true;
        }

        private bool r_tidy()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            {
                int c2 = limit - cursor;
                int c3 = limit - cursor;
                if (!r_LONG())
                    goto lab0;
                cursor = limit - c3;
                ket = cursor;
                if (cursor <= limit_backward)
                {
                    goto lab0;
                }
                cursor--;
                bra = cursor;
                slice_del();
            lab0: ;
                cursor = limit - c2;
            }
            {
                int c4 = limit - cursor;
                ket = cursor;
                if (in_grouping_b(g_AEI, 97, 228, false) != 0)
                {
                    goto lab1;
                }
                bra = cursor;
                if (in_grouping_b(g_C, 98, 122, false) != 0)
                {
                    goto lab1;
                }
                slice_del();
            lab1: ;
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("j")))
                {
                    goto lab2;
                }
                bra = cursor;
                {
                    int c6 = limit - cursor;
                    if (!(eq_s_b("o")))
                    {
                        goto lab4;
                    }
                    goto lab3;
                lab4: ;
                    cursor = limit - c6;
                    if (!(eq_s_b("u")))
                    {
                        goto lab2;
                    }
                }
            lab3: ;
                slice_del();
            lab2: ;
                cursor = limit - c5;
            }
            {
                int c7 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("o")))
                {
                    goto lab5;
                }
                bra = cursor;
                if (!(eq_s_b("j")))
                {
                    goto lab5;
                }
                slice_del();
            lab5: ;
                cursor = limit - c7;
            }
            limit_backward = c1;
            if (in_grouping_b(g_V1, 97, 246, true) < 0)
            {
                return false;
            }

            ket = cursor;
            if (in_grouping_b(g_C, 98, 122, false) != 0)
            {
                return false;
            }
            bra = cursor;
            slice_to(S_x);
            if (!(eq_s_b(S_x)))
            {
                return false;
            }
            slice_del();
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                r_mark_regions();
                cursor = c1;
            }
            B_ending_removed = false;
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                r_particle_etc();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_possessive();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_case_ending();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                r_other_endings();
                cursor = limit - c5;
            }
            if (!B_ending_removed)
            {
                goto lab1;
            }
            {
                int c6 = limit - cursor;
                r_i_plural();
                cursor = limit - c6;
            }
            goto lab0;
        lab1: ;
            {
                int c7 = limit - cursor;
                r_t_plural();
                cursor = limit - c7;
            }
        lab0: ;
            {
                int c8 = limit - cursor;
                r_tidy();
                cursor = limit - c8;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

