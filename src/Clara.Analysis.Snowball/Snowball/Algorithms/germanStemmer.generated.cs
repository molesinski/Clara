// Generated from german.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from german.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class GermanStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;

        private const string g_v = "aeiouyäöü";
        private const string g_et_ending = "Udfgklmnrstzä";
        private const string g_s_ending = "bdfghklmnrt";
        private const string g_st_ending = "bdfghklmnt";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 5, 0),
            new Among("ae", 0, 2, 0),
            new Among("oe", 0, 3, 0),
            new Among("qu", 0, -1, 0),
            new Among("ue", 0, 4, 0),
            new Among("ß", 0, 1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 5, 0),
            new Among("U", 0, 2, 0),
            new Among("Y", 0, 1, 0),
            new Among("ä", 0, 3, 0),
            new Among("ö", 0, 4, 0),
            new Among("ü", 0, 2, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("e", -1, 3, 0),
            new Among("em", -1, 1, 0),
            new Among("en", -1, 3, 0),
            new Among("erinnen", 2, 2, 0),
            new Among("erin", -1, 2, 0),
            new Among("ln", -1, 5, 0),
            new Among("ern", -1, 2, 0),
            new Among("er", -1, 2, 0),
            new Among("s", -1, 4, 0),
            new Among("es", 8, 3, 0),
            new Among("lns", 8, 5, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("tick", -1, -1, 0),
            new Among("plan", -1, -1, 0),
            new Among("geordn", -1, -1, 0),
            new Among("intern", -1, -1, 0),
            new Among("tr", -1, -1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("en", -1, 1, 0),
            new Among("er", -1, 1, 0),
            new Among("et", -1, 3, 0),
            new Among("st", -1, 2, 0),
            new Among("est", 3, 1, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ig", -1, 1, 0),
            new Among("lich", -1, 1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("end", -1, 1, 0),
            new Among("ig", -1, 2, 0),
            new Among("ung", -1, 1, 0),
            new Among("lich", -1, 3, 0),
            new Among("isch", -1, 2, 0),
            new Among("ik", -1, 2, 0),
            new Among("heit", -1, 3, 0),
            new Among("keit", -1, 4, 0)
        };


        private bool r_prelude()
        {
            int among_var;
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    while (true)
                    {
                        int c3 = cursor;
                        if (in_grouping(g_v, 97, 252, false) != 0)
                        {
                            goto lab1;
                        }
                        bra = cursor;
                        {
                            int c4 = cursor;
                            if (!(eq_s("u")))
                            {
                                goto lab3;
                            }
                            ket = cursor;
                            if (in_grouping(g_v, 97, 252, false) != 0)
                            {
                                goto lab3;
                            }
                            slice_from("U");
                            goto lab2;
                        lab3: ;
                            cursor = c4;
                            if (!(eq_s("y")))
                            {
                                goto lab1;
                            }
                            ket = cursor;
                            if (in_grouping(g_v, 97, 252, false) != 0)
                            {
                                goto lab1;
                            }
                            slice_from("Y");
                        }
                    lab2: ;
                        cursor = c3;
                        break;
                    lab1: ;
                        cursor = c3;
                        if (cursor >= limit)
                        {
                            goto lab0;
                        }
                        cursor++;
                    }
                    continue;
                lab0: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            while (true)
            {
                int c5 = cursor;
                bra = cursor;
                among_var = find_among(a_0, null);
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("ss");
                        break;
                    }
                    case 2: {
                        slice_from("ä");
                        break;
                    }
                    case 3: {
                        slice_from("ö");
                        break;
                    }
                    case 4: {
                        slice_from("ü");
                        break;
                    }
                    case 5: {
                        if (cursor >= limit)
                        {
                            goto lab4;
                        }
                        cursor++;
                        break;
                    }
                }
                continue;
            lab4: ;
                cursor = c5;
                break;
            }
            return true;
        }

        private bool r_mark_regions()
        {
            int I_x;
            I_p1 = limit;
            I_p2 = limit;
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

                int ret = out_grouping(g_v, 97, 252, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_v, 97, 252, true);
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
            {

                int ret = out_grouping(g_v, 97, 252, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_v, 97, 252, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            I_p2 = cursor;
            return true;
        }

        private bool r_postlude()
        {
            int among_var;
            while (true)
            {
                int c1 = cursor;
                bra = cursor;
                among_var = find_among(a_1, null);
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("y");
                        break;
                    }
                    case 2: {
                        slice_from("u");
                        break;
                    }
                    case 3: {
                        slice_from("a");
                        break;
                    }
                    case 4: {
                        slice_from("o");
                        break;
                    }
                    case 5: {
                        if (cursor >= limit)
                        {
                            goto lab0;
                        }
                        cursor++;
                        break;
                    }
                }
                continue;
            lab0: ;
                cursor = c1;
                break;
            }
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

        private bool r_standard_suffix()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_2, null);
                if (among_var == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                if (!r_R1())
                    goto lab0;
                switch (among_var) {
                    case 1: {
                        {
                            int c2 = limit - cursor;
                            if (!(eq_s_b("syst")))
                            {
                                goto lab1;
                            }
                            goto lab0;
                        lab1: ;
                            cursor = limit - c2;
                        }
                        slice_del();
                        break;
                    }
                    case 2: {
                        slice_del();
                        break;
                    }
                    case 3: {
                        slice_del();
                        {
                            int c3 = limit - cursor;
                            ket = cursor;
                            if (!(eq_s_b("s")))
                            {
                                {
                                    cursor = limit - c3;
                                    goto lab2;
                                }
                            }
                            bra = cursor;
                            if (!(eq_s_b("nis")))
                            {
                                {
                                    cursor = limit - c3;
                                    goto lab2;
                                }
                            }
                            slice_del();
                        lab2: ;
                        }
                        break;
                    }
                    case 4: {
                        if (in_grouping_b(g_s_ending, 98, 116, false) != 0)
                        {
                            goto lab0;
                        }
                        slice_del();
                        break;
                    }
                    case 5: {
                        slice_from("l");
                        break;
                    }
                }
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c4 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_4, null);
                if (among_var == 0)
                {
                    goto lab3;
                }
                bra = cursor;
                if (!r_R1())
                    goto lab3;
                switch (among_var) {
                    case 1: {
                        slice_del();
                        break;
                    }
                    case 2: {
                        if (in_grouping_b(g_st_ending, 98, 116, false) != 0)
                        {
                            goto lab3;
                        }
                        {
                            int c = cursor - 3;
                            if (c < limit_backward)
                            {
                                goto lab3;
                            }
                            cursor = c;
                        }
                        slice_del();
                        break;
                    }
                    case 3: {
                        {
                            int c5 = limit - cursor;
                            if (in_grouping_b(g_et_ending, 85, 228, false) != 0)
                            {
                                goto lab3;
                            }
                            cursor = limit - c5;
                        }
                        {
                            int c6 = limit - cursor;
                            if (find_among_b(a_3, null) == 0)
                            {
                                goto lab4;
                            }
                            goto lab3;
                        lab4: ;
                            cursor = limit - c6;
                        }
                        slice_del();
                        break;
                    }
                }
            lab3: ;
                cursor = limit - c4;
            }
            {
                int c7 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_6, null);
                if (among_var == 0)
                {
                    goto lab5;
                }
                bra = cursor;
                if (!r_R2())
                    goto lab5;
                switch (among_var) {
                    case 1: {
                        slice_del();
                        {
                            int c8 = limit - cursor;
                            ket = cursor;
                            if (!(eq_s_b("ig")))
                            {
                                {
                                    cursor = limit - c8;
                                    goto lab6;
                                }
                            }
                            bra = cursor;
                            {
                                int c9 = limit - cursor;
                                if (!(eq_s_b("e")))
                                {
                                    goto lab7;
                                }
                                {
                                    cursor = limit - c8;
                                    goto lab6;
                                }
                            lab7: ;
                                cursor = limit - c9;
                            }
                            if (!r_R2())
                                {
                                    cursor = limit - c8;
                                    goto lab6;
                                }
                            slice_del();
                        lab6: ;
                        }
                        break;
                    }
                    case 2: {
                        {
                            int c10 = limit - cursor;
                            if (!(eq_s_b("e")))
                            {
                                goto lab8;
                            }
                            goto lab5;
                        lab8: ;
                            cursor = limit - c10;
                        }
                        slice_del();
                        break;
                    }
                    case 3: {
                        slice_del();
                        {
                            int c11 = limit - cursor;
                            ket = cursor;
                            {
                                int c12 = limit - cursor;
                                if (!(eq_s_b("er")))
                                {
                                    goto lab11;
                                }
                                goto lab10;
                            lab11: ;
                                cursor = limit - c12;
                                if (!(eq_s_b("en")))
                                {
                                    {
                                        cursor = limit - c11;
                                        goto lab9;
                                    }
                                }
                            }
                        lab10: ;
                            bra = cursor;
                            if (!r_R1())
                                {
                                    cursor = limit - c11;
                                    goto lab9;
                                }
                            slice_del();
                        lab9: ;
                        }
                        break;
                    }
                    case 4: {
                        slice_del();
                        {
                            int c13 = limit - cursor;
                            ket = cursor;
                            if (find_among_b(a_5, null) == 0)
                            {
                                {
                                    cursor = limit - c13;
                                    goto lab12;
                                }
                            }
                            bra = cursor;
                            if (!r_R2())
                                {
                                    cursor = limit - c13;
                                    goto lab12;
                                }
                            slice_del();
                        lab12: ;
                        }
                        break;
                    }
                }
            lab5: ;
                cursor = limit - c7;
            }
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                r_prelude();
                cursor = c1;
            }
            {
                int c2 = cursor;
                r_mark_regions();
                cursor = c2;
            }
            limit_backward = cursor;
            cursor = limit;
            r_standard_suffix();
            cursor = limit_backward;
            {
                int c3 = cursor;
                r_postlude();
                cursor = c3;
            }
            return true;
        }

    }
}

