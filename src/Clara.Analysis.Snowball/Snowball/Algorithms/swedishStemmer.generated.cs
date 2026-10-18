// Generated from swedish.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from swedish.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class SwedishStemmer : Stemmer
    {
        private const string g_v = "aeiouyäåö";
        private const string g_s_ending = "bcdfghjklmnoprtvy";
        private const string g_ost_ending = "iklnprtuv";

        private static readonly Among[] a_0 = new[]
        {
            new Among("fab", -1, -1, 0),
            new Among("h", -1, -1, 0),
            new Among("pak", -1, -1, 0),
            new Among("rak", -1, -1, 0),
            new Among("stak", -1, -1, 0),
            new Among("kom", -1, -1, 0),
            new Among("iet", -1, -1, 0),
            new Among("cit", -1, -1, 0),
            new Among("dit", -1, -1, 0),
            new Among("alit", -1, -1, 0),
            new Among("ilit", -1, -1, 0),
            new Among("mit", -1, -1, 0),
            new Among("nit", -1, -1, 0),
            new Among("pit", -1, -1, 0),
            new Among("rit", -1, -1, 0),
            new Among("sit", -1, -1, 0),
            new Among("tit", -1, -1, 0),
            new Among("uit", -1, -1, 0),
            new Among("ivit", -1, -1, 0),
            new Among("kvit", -1, -1, 0),
            new Among("xit", -1, -1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("a", -1, 1, 0),
            new Among("arna", 0, 1, 0),
            new Among("erna", 0, 1, 0),
            new Among("heterna", 2, 1, 0),
            new Among("orna", 0, 1, 0),
            new Among("ad", -1, 1, 0),
            new Among("e", -1, 1, 0),
            new Among("ade", 6, 1, 0),
            new Among("ande", 6, 1, 0),
            new Among("arne", 6, 1, 0),
            new Among("are", 6, 1, 0),
            new Among("aste", 6, 1, 0),
            new Among("en", -1, 1, 0),
            new Among("anden", 12, 1, 0),
            new Among("aren", 12, 1, 0),
            new Among("heten", 12, 1, 0),
            new Among("ern", -1, 1, 0),
            new Among("ar", -1, 1, 0),
            new Among("er", -1, 1, 0),
            new Among("heter", 18, 1, 0),
            new Among("or", -1, 1, 0),
            new Among("s", -1, 2, 0),
            new Among("as", 21, 1, 0),
            new Among("arnas", 22, 1, 0),
            new Among("ernas", 22, 1, 0),
            new Among("ornas", 22, 1, 0),
            new Among("es", 21, 1, 0),
            new Among("ades", 26, 1, 0),
            new Among("andes", 26, 1, 0),
            new Among("ens", 21, 1, 0),
            new Among("arens", 29, 1, 0),
            new Among("hetens", 29, 1, 0),
            new Among("erns", 21, 1, 0),
            new Among("at", -1, 1, 0),
            new Among("et", -1, 3, 0),
            new Among("andet", 34, 1, 0),
            new Among("het", 34, 1, 0),
            new Among("ast", -1, 1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("dd", -1, -1, 0),
            new Among("gd", -1, -1, 0),
            new Among("nn", -1, -1, 0),
            new Among("dt", -1, -1, 0),
            new Among("gt", -1, -1, 0),
            new Among("kt", -1, -1, 0),
            new Among("tt", -1, -1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ig", -1, 1, 0),
            new Among("lig", 0, 1, 0),
            new Among("els", -1, 1, 0),
            new Among("fullt", -1, 3, 0),
            new Among("öst", -1, 2, 0)
        };


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
            if (cursor <= limit_backward)
            {
                return false;
            }
            cursor = limit - c1;
            {
                int c2 = limit - cursor;
                if (find_among_b(a_0, null) == 0)
                {
                    goto lab0;
                }
                return false;
            lab0: ;
                cursor = limit - c2;
            }
            return true;
        }

        protected override bool stem()
        {
            int among_var;
            int I_x;
            int I_p1;
            {
                int c1 = cursor;
                I_p1 = limit;
                {
                    int c2 = cursor;
                    {
                        int c = cursor + 3;
                        if (c > limit)
                        {
                            goto lab0;
                        }
                        cursor = c;
                    }
                    I_x = cursor;
                    cursor = c2;
                }
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
                if (I_p1 >= I_x)
                {
                    goto lab1;
                }
                I_p1 = I_x;
            lab1: ;
            lab0: ;
                cursor = c1;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c3 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab2;
                }
                int c4 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                among_var = find_among_b(a_1, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c4;
                        goto lab2;
                    }
                }
                bra = cursor;
                limit_backward = c4;
                switch (among_var) {
                    case 1: {
                        slice_del();
                        break;
                    }
                    case 2: {
                        {
                            int c5 = limit - cursor;
                            if (!(eq_s_b("et")))
                            {
                                goto lab4;
                            }
                            if (!r_et_condition())
                                goto lab4;
                            bra = cursor;
                            goto lab3;
                        lab4: ;
                            cursor = limit - c5;
                            if (in_grouping_b(g_s_ending, 98, 121, false) != 0)
                            {
                                goto lab2;
                            }
                        }
                    lab3: ;
                        slice_del();
                        break;
                    }
                    case 3: {
                        if (!r_et_condition())
                            goto lab2;
                        slice_del();
                        break;
                    }
                }
            lab2: ;
                cursor = limit - c3;
            }
            {
                int c6 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab5;
                }
                int c7 = limit_backward;
                limit_backward = I_p1;
                int c8 = limit - cursor;
                if (find_among_b(a_2, null) == 0)
                {
                    {
                        limit_backward = c7;
                        goto lab5;
                    }
                }
                cursor = limit - c8;
                ket = cursor;
                if (cursor <= limit_backward)
                {
                    {
                        limit_backward = c7;
                        goto lab5;
                    }
                }
                cursor--;
                bra = cursor;
                slice_del();
                limit_backward = c7;
            lab5: ;
                cursor = limit - c6;
            }
            {
                int c9 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab6;
                }
                int c10 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                among_var = find_among_b(a_3, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c10;
                        goto lab6;
                    }
                }
                bra = cursor;
                limit_backward = c10;
                switch (among_var) {
                    case 1: {
                        slice_del();
                        break;
                    }
                    case 2: {
                        if (in_grouping_b(g_ost_ending, 105, 118, false) != 0)
                        {
                            goto lab6;
                        }
                        slice_from("ös");
                        break;
                    }
                    case 3: {
                        slice_from("full");
                        break;
                    }
                }
            lab6: ;
                cursor = limit - c9;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

