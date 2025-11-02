// Generated from danish.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from danish.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class DanishStemmer : Stemmer
    {
        private int I_p1;
        private StringBuilder S_ch = new StringBuilder();

        private const string g_c = "bcdfghjklmnpqrstvwxz";
        private const string g_v = "aeiouyåæø";
        private const string g_s_ending = "abcdfghjklmnoprtvyzå";

        private static readonly Among[] a_0 = new[]
        {
            new Among("hed", -1, 1, 0),
            new Among("ethed", 0, 1, 0),
            new Among("ered", -1, 1, 0),
            new Among("e", -1, 1, 0),
            new Among("erede", 3, 1, 0),
            new Among("ende", 3, 1, 0),
            new Among("erende", 5, 1, 0),
            new Among("ene", 3, 1, 0),
            new Among("erne", 3, 1, 0),
            new Among("ere", 3, 1, 0),
            new Among("en", -1, 1, 0),
            new Among("heden", 10, 1, 0),
            new Among("eren", 10, 1, 0),
            new Among("er", -1, 1, 0),
            new Among("heder", 13, 1, 0),
            new Among("erer", 13, 1, 0),
            new Among("s", -1, 2, 0),
            new Among("heds", 16, 1, 0),
            new Among("es", 16, 1, 0),
            new Among("endes", 18, 1, 0),
            new Among("erendes", 19, 1, 0),
            new Among("enes", 18, 1, 0),
            new Among("ernes", 18, 1, 0),
            new Among("eres", 18, 1, 0),
            new Among("ens", 16, 1, 0),
            new Among("hedens", 24, 1, 0),
            new Among("erens", 24, 1, 0),
            new Among("ers", 16, 1, 0),
            new Among("ets", 16, 1, 0),
            new Among("erets", 28, 1, 0),
            new Among("et", -1, 1, 0),
            new Among("eret", 30, 1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("gd", -1, -1, 0),
            new Among("dt", -1, -1, 0),
            new Among("gt", -1, -1, 0),
            new Among("kt", -1, -1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ig", -1, 1, 0),
            new Among("lig", 0, 1, 0),
            new Among("elig", 1, 1, 0),
            new Among("els", -1, 1, 0),
            new Among("løst", -1, 2, 0)
        };


        private bool r_mark_regions()
        {
            int I_x;
            I_p1 = limit;
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
                I_x = cursor;
                cursor = c1;
            }
            {

                int ret = out_grouping(g_v, 97, 248, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_v, 97, 248, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            I_p1 = cursor;
            if (I_p1 >= I_x)
            {
                goto lab0;
            }
            I_p1 = I_x;
        lab0: ;
            return true;
        }

        private bool r_main_suffix()
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
                    slice_del();
                    break;
                }
                case 2: {
                    if (in_grouping_b(g_s_ending, 97, 229, false) != 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_consonant_pair()
        {
            {
                int c1 = limit - cursor;
                if (cursor < I_p1)
                {
                    return false;
                }
                int c2 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                if (find_among_b(a_1, null) == 0)
                {
                    {
                        limit_backward = c2;
                        return false;
                    }
                }
                bra = cursor;
                limit_backward = c2;
                cursor = limit - c1;
            }
            if (cursor <= limit_backward)
            {
                return false;
            }
            cursor--;
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_other_suffix()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("st")))
                {
                    goto lab0;
                }
                bra = cursor;
                if (!(eq_s_b("ig")))
                {
                    goto lab0;
                }
                slice_del();
            lab0: ;
                cursor = limit - c1;
            }
            if (cursor < I_p1)
            {
                return false;
            }
            int c2 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_2, null);
            if (among_var == 0)
            {
                {
                    limit_backward = c2;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c2;
            switch (among_var) {
                case 1: {
                    slice_del();
                    {
                        int c3 = limit - cursor;
                        r_consonant_pair();
                        cursor = limit - c3;
                    }
                    break;
                }
                case 2: {
                    slice_from("løs");
                    break;
                }
            }
            return true;
        }

        private bool r_undouble()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            if (in_grouping_b(g_c, 98, 122, false) != 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            slice_to(S_ch);
            limit_backward = c1;
            if (!(eq_s_b(S_ch)))
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
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                r_main_suffix();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_consonant_pair();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_other_suffix();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                r_undouble();
                cursor = limit - c5;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

