// Generated from dutch_porter.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from dutch_porter.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class Dutch_porterStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;
        private bool B_e_found;

        private const string g_v = "aeiouyè";
        private const string g_v_I = "Iaeiouyè";
        private const string g_v_j = "aeijouyè";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 6, 0),
            new Among("á", 0, 1, 0),
            new Among("ä", 0, 1, 0),
            new Among("é", 0, 2, 0),
            new Among("ë", 0, 2, 0),
            new Among("í", 0, 3, 0),
            new Among("ï", 0, 3, 0),
            new Among("ó", 0, 4, 0),
            new Among("ö", 0, 4, 0),
            new Among("ú", 0, 5, 0),
            new Among("ü", 0, 5, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 3, 0),
            new Among("I", 0, 2, 0),
            new Among("Y", 0, 1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("dd", -1, -1, 0),
            new Among("kk", -1, -1, 0),
            new Among("tt", -1, -1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ene", -1, 2, 0),
            new Among("se", -1, 3, 0),
            new Among("en", -1, 2, 0),
            new Among("heden", 2, 1, 0),
            new Among("s", -1, 3, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("end", -1, 1, 0),
            new Among("ig", -1, 2, 0),
            new Among("ing", -1, 1, 0),
            new Among("lijk", -1, 3, 0),
            new Among("baar", -1, 4, 0),
            new Among("bar", -1, 5, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("aa", -1, -1, 0),
            new Among("ee", -1, -1, 0),
            new Among("oo", -1, -1, 0),
            new Among("uu", -1, -1, 0)
        };


        private bool r_prelude()
        {
            int among_var;
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    bra = cursor;
                    among_var = find_among(a_0, null);
                    ket = cursor;
                    switch (among_var) {
                        case 1: {
                            slice_from("a");
                            break;
                        }
                        case 2: {
                            slice_from("e");
                            break;
                        }
                        case 3: {
                            slice_from("i");
                            break;
                        }
                        case 4: {
                            slice_from("o");
                            break;
                        }
                        case 5: {
                            slice_from("u");
                            break;
                        }
                        case 6: {
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
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            {
                int c3 = cursor;
                bra = cursor;
                if (!(eq_s("y")))
                {
                    {
                        cursor = c3;
                        goto lab1;
                    }
                }
                ket = cursor;
                slice_from("Y");
            lab1: ;
            }
            while (true)
            {
                int c4 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 232, true);
                    if (ret < 0)
                    {
                        goto lab2;
                    }

                    cursor += ret;
                }
                {
                    int c5 = cursor;
                    bra = cursor;
                    {
                        int c6 = cursor;
                        if (!(eq_s("i")))
                        {
                            goto lab5;
                        }
                        ket = cursor;
                        {
                            int c7 = cursor;
                            if (in_grouping(g_v, 97, 232, false) != 0)
                            {
                                goto lab6;
                            }
                            slice_from("I");
                        lab6: ;
                            cursor = c7;
                        }
                        goto lab4;
                    lab5: ;
                        cursor = c6;
                        if (!(eq_s("y")))
                        {
                            {
                                cursor = c5;
                                goto lab3;
                            }
                        }
                        ket = cursor;
                        slice_from("Y");
                    }
                lab4: ;
                lab3: ;
                }
                continue;
            lab2: ;
                cursor = c4;
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

                int ret = out_grouping(g_v, 97, 232, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_v, 97, 232, true);
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

                int ret = out_grouping(g_v, 97, 232, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_v, 97, 232, true);
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
                        slice_from("i");
                        break;
                    }
                    case 3: {
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

        private bool r_undouble()
        {
            {
                int c1 = limit - cursor;
                if (find_among_b(a_2, null) == 0)
                {
                    return false;
                }
                cursor = limit - c1;
            }
            ket = cursor;
            if (cursor <= limit_backward)
            {
                return false;
            }
            cursor--;
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_e_ending()
        {
            B_e_found = false;
            ket = cursor;
            if (!(eq_s_b("e")))
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            {
                int c1 = limit - cursor;
                if (out_grouping_b(g_v, 97, 232, false) != 0)
                {
                    return false;
                }
                cursor = limit - c1;
            }
            slice_del();
            B_e_found = true;
            return r_undouble();
        }

        private bool r_en_ending()
        {
            if (!r_R1())
                return false;
            int c1 = limit - cursor;
            if (out_grouping_b(g_v, 97, 232, false) != 0)
            {
                return false;
            }
            cursor = limit - c1;
            {
                int c2 = limit - cursor;
                if (!(eq_s_b("gem")))
                {
                    goto lab0;
                }
                return false;
            lab0: ;
                cursor = limit - c2;
            }
            slice_del();
            return r_undouble();
        }

        private bool r_standard_suffix()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_3, null);
                if (among_var == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (!r_R1())
                            goto lab0;
                        slice_from("heid");
                        break;
                    }
                    case 2: {
                        if (!r_en_ending())
                            goto lab0;
                        break;
                    }
                    case 3: {
                        if (!r_R1())
                            goto lab0;
                        if (out_grouping_b(g_v_j, 97, 232, false) != 0)
                        {
                            goto lab0;
                        }
                        slice_del();
                        break;
                    }
                }
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c2 = limit - cursor;
                r_e_ending();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("heid")))
                {
                    goto lab1;
                }
                bra = cursor;
                if (!r_R2())
                    goto lab1;
                {
                    int c4 = limit - cursor;
                    if (!(eq_s_b("c")))
                    {
                        goto lab2;
                    }
                    goto lab1;
                lab2: ;
                    cursor = limit - c4;
                }
                slice_del();
                ket = cursor;
                if (!(eq_s_b("en")))
                {
                    goto lab1;
                }
                bra = cursor;
                if (!r_en_ending())
                    goto lab1;
            lab1: ;
                cursor = limit - c3;
            }
            {
                int c5 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_4, null);
                if (among_var == 0)
                {
                    goto lab3;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (!r_R2())
                            goto lab3;
                        slice_del();
                        {
                            int c6 = limit - cursor;
                            ket = cursor;
                            if (!(eq_s_b("ig")))
                            {
                                goto lab5;
                            }
                            bra = cursor;
                            if (!r_R2())
                                goto lab5;
                            {
                                int c7 = limit - cursor;
                                if (!(eq_s_b("e")))
                                {
                                    goto lab6;
                                }
                                goto lab5;
                            lab6: ;
                                cursor = limit - c7;
                            }
                            slice_del();
                            goto lab4;
                        lab5: ;
                            cursor = limit - c6;
                            if (!r_undouble())
                                goto lab3;
                        }
                    lab4: ;
                        break;
                    }
                    case 2: {
                        if (!r_R2())
                            goto lab3;
                        {
                            int c8 = limit - cursor;
                            if (!(eq_s_b("e")))
                            {
                                goto lab7;
                            }
                            goto lab3;
                        lab7: ;
                            cursor = limit - c8;
                        }
                        slice_del();
                        break;
                    }
                    case 3: {
                        if (!r_R2())
                            goto lab3;
                        slice_del();
                        if (!r_e_ending())
                            goto lab3;
                        break;
                    }
                    case 4: {
                        if (!r_R2())
                            goto lab3;
                        slice_del();
                        break;
                    }
                    case 5: {
                        if (!r_R2())
                            goto lab3;
                        if (!B_e_found)
                        {
                            goto lab3;
                        }
                        slice_del();
                        break;
                    }
                }
            lab3: ;
                cursor = limit - c5;
            }
            {
                int c9 = limit - cursor;
                if (out_grouping_b(g_v_I, 73, 232, false) != 0)
                {
                    goto lab8;
                }
                {
                    int c10 = limit - cursor;
                    if (find_among_b(a_5, null) == 0)
                    {
                        goto lab8;
                    }
                    if (out_grouping_b(g_v, 97, 232, false) != 0)
                    {
                        goto lab8;
                    }
                    cursor = limit - c10;
                }
                ket = cursor;
                if (cursor <= limit_backward)
                {
                    goto lab8;
                }
                cursor--;
                bra = cursor;
                slice_del();
            lab8: ;
                cursor = limit - c9;
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

