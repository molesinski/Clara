// Generated from swedish.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from swedish.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class SwedishStemmer : Stemmer
    {
        private int I_x;
        private int I_p1;

        private const string g_v = "aeiouyäåö";
        private const string g_s_ending = "bcdfghjklmnoprtvy";
        private const string g_ost_ending = "iklnprtuv";

        private static readonly Among[] a_0 = new[]
        {
            new Among("fab", -1, -1),
            new Among("h", -1, -1),
            new Among("pak", -1, -1),
            new Among("rak", -1, -1),
            new Among("stak", -1, -1),
            new Among("kom", -1, -1),
            new Among("iet", -1, -1),
            new Among("cit", -1, -1),
            new Among("dit", -1, -1),
            new Among("alit", -1, -1),
            new Among("ilit", -1, -1),
            new Among("mit", -1, -1),
            new Among("nit", -1, -1),
            new Among("pit", -1, -1),
            new Among("rit", -1, -1),
            new Among("sit", -1, -1),
            new Among("tit", -1, -1),
            new Among("uit", -1, -1),
            new Among("ivit", -1, -1),
            new Among("kvit", -1, -1),
            new Among("xit", -1, -1)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("a", -1, 1),
            new Among("arna", 0, 1),
            new Among("erna", 0, 1),
            new Among("heterna", 2, 1),
            new Among("orna", 0, 1),
            new Among("ad", -1, 1),
            new Among("e", -1, 1),
            new Among("ade", 6, 1),
            new Among("ande", 6, 1),
            new Among("arne", 6, 1),
            new Among("are", 6, 1),
            new Among("aste", 6, 1),
            new Among("en", -1, 1),
            new Among("anden", 12, 1),
            new Among("aren", 12, 1),
            new Among("heten", 12, 1),
            new Among("ern", -1, 1),
            new Among("ar", -1, 1),
            new Among("er", -1, 1),
            new Among("heter", 18, 1),
            new Among("or", -1, 1),
            new Among("s", -1, 2),
            new Among("as", 21, 1),
            new Among("arnas", 22, 1),
            new Among("ernas", 22, 1),
            new Among("ornas", 22, 1),
            new Among("es", 21, 1),
            new Among("ades", 26, 1),
            new Among("andes", 26, 1),
            new Among("ens", 21, 1),
            new Among("arens", 29, 1),
            new Among("hetens", 29, 1),
            new Among("erns", 21, 1),
            new Among("at", -1, 1),
            new Among("et", -1, 3),
            new Among("andet", 34, 1),
            new Among("het", 34, 1),
            new Among("ast", -1, 1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("dd", -1, -1),
            new Among("gd", -1, -1),
            new Among("nn", -1, -1),
            new Among("dt", -1, -1),
            new Among("gt", -1, -1),
            new Among("kt", -1, -1),
            new Among("tt", -1, -1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ig", -1, 1),
            new Among("lig", 0, 1),
            new Among("els", -1, 1),
            new Among("fullt", -1, 3),
            new Among("öst", -1, 2)
        };


        private bool r_mark_regions()
        {
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

                int ret = out_grouping(g_v, 97, 246, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_v, 97, 246, true);
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

        private bool r_et_condition()
        {
            int c1 = limit - cursor;
            if (out_grouping_b(g_v, 97, 246, false) != 0)
            {
                return false;
            }
            if (in_grouping_b(g_v, 97, 246, false) != 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                goto lab0;
            }
            return false;
        lab0: ;
            cursor = limit - c1;
            {
                int c2 = limit - cursor;
                if (find_among_b(a_0) == 0)
                {
                    goto lab1;
                }
                return false;
            lab1: ;
                cursor = limit - c2;
            }
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
            among_var = find_among_b(a_1);
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
                    {
                        int c2 = limit - cursor;
                        if (!(eq_s_b("et")))
                        {
                            goto lab1;
                        }
                        if (!r_et_condition())
                            goto lab1;
                        bra = cursor;
                        goto lab0;
                    lab1: ;
                        cursor = limit - c2;
                        if (in_grouping_b(g_s_ending, 98, 121, false) != 0)
                        {
                            return false;
                        }
                    }
                lab0: ;
                    slice_del();
                    break;
                }
                case 3: {
                    if (!r_et_condition())
                        return false;
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_consonant_pair()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            int c2 = limit - cursor;
            if (find_among_b(a_2) == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            cursor = limit - c2;
            ket = cursor;
            if (cursor <= limit_backward)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            cursor--;
            bra = cursor;
            slice_del();
            limit_backward = c1;
            return true;
        }

        private bool r_other_suffix()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_3);
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
                    if (in_grouping_b(g_ost_ending, 105, 118, false) != 0)
                    {
                        return false;
                    }
                    slice_from("ös");
                    break;
                }
                case 3: {
                    slice_from("full");
                    break;
                }
            }
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
            cursor = limit_backward;
            return true;
        }

    }
}

