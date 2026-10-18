// Generated from danish.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from danish.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class DanishStemmer : Stemmer
    {
        private int I_p1;
        private StringBuilder S_ch = new StringBuilder();

        private const string g_undouble_c = "bdfgklmnprst";
        private const string g_v = "aeiouyåæø";
        private const string g_s_ending = "'abcdfghjklmnoprtvyzå";

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

        protected override bool stem()
        {
            int among_var;
            I_p1 = limit;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    while (true)
                    {
                        if (!(eq_s("'")))
                        {
                            goto lab3;
                        }
                        break;
                    lab3: ;
                        if (cursor >= limit)
                        {
                            goto lab2;
                        }
                        cursor++;
                    }
                    goto lab1;
                lab2: ;
                    cursor = c2;
                    {

                        int ret = out_grouping(g_v, 97, 248, true);
                        if (ret < 0)
                        {
                            goto lab0;
                        }

                        cursor += ret;
                    }
                    {

                        int ret = in_grouping(g_v, 97, 248, true);
                        if (ret < 0)
                        {
                            goto lab0;
                        }

                        cursor += ret;
                    }
                }
            lab1: ;
                I_p1 = cursor;
            lab0: ;
                cursor = c1;
            }
            {
                int c3 = cursor;
                {
                    int c = cursor + 3;
                    if (c > limit)
                    {
                        return false;
                    }
                    cursor = c;
                }
                if (I_p1 >= cursor)
                {
                    goto lab4;
                }
                I_p1 = cursor;
            lab4: ;
                cursor = c3;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c4 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab5;
                }
                int c5 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                among_var = find_among_b(a_0, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c5;
                        goto lab5;
                    }
                }
                bra = cursor;
                limit_backward = c5;
                switch (among_var) {
                    case 1: {
                        slice_del();
                        break;
                    }
                    case 2: {
                        if (in_grouping_b(g_s_ending, 39, 229, false) != 0)
                        {
                            goto lab5;
                        }
                        slice_del();
                        break;
                    }
                }
            lab5: ;
                cursor = limit - c4;
            }
            {
                int c6 = limit - cursor;
                r_consonant_pair();
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                {
                    int c8 = limit - cursor;
                    ket = cursor;
                    if (!(eq_s_b("st")))
                    {
                        goto lab7;
                    }
                    bra = cursor;
                    if (!(eq_s_b("ig")))
                    {
                        goto lab7;
                    }
                    slice_del();
                lab7: ;
                    cursor = limit - c8;
                }
                if (cursor < I_p1)
                {
                    goto lab6;
                }
                int c9 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                among_var = find_among_b(a_2, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c9;
                        goto lab6;
                    }
                }
                bra = cursor;
                limit_backward = c9;
                switch (among_var) {
                    case 1: {
                        slice_del();
                        {
                            int c10 = limit - cursor;
                            r_consonant_pair();
                            cursor = limit - c10;
                        }
                        break;
                    }
                    case 2: {
                        slice_from("løs");
                        break;
                    }
                }
            lab6: ;
                cursor = limit - c7;
            }
            {
                int c11 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab8;
                }
                int c12 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                if (in_grouping_b(g_undouble_c, 98, 116, false) != 0)
                {
                    {
                        limit_backward = c12;
                        goto lab8;
                    }
                }
                bra = cursor;
                slice_to(S_ch);
                limit_backward = c12;
                if (!(eq_s_b(S_ch)))
                {
                    goto lab8;
                }
                slice_del();
            lab8: ;
                cursor = limit - c11;
            }
            ket = cursor;
            if (!(eq_s_b("'")))
            {
                return false;
            }
            bra = cursor;
            slice_del();
            cursor = limit_backward;
            return true;
        }

    }
}

