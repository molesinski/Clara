// Generated from dutch_porter.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from dutch_porter.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class Dutch_porterStemmer : Stemmer
    {
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
            if (I_p1 > cursor)
            {
                return false;
            }
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
            if (I_p1 > cursor)
            {
                return false;
            }
            int c1 = limit - cursor;
            if (out_grouping_b(g_v, 97, 232, false) != 0)
            {
                return false;
            }
            cursor = limit - c1;
            if (!(eq_s_b("gem")))
            {
                goto lab0;
            }
            return false;
        lab0: ;
            slice_del();
            return r_undouble();
        }

        protected override bool stem()
        {
            int among_var;
            int I_x;
            int I_p2;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    while (true)
                    {
                        int c3 = cursor;
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
                                    goto lab1;
                                }
                                cursor++;
                                break;
                            }
                        }
                        continue;
                    lab1: ;
                        cursor = c3;
                        break;
                    }
                    cursor = c2;
                }
                {
                    int c4 = cursor;
                    bra = cursor;
                    if (!(eq_s("y")))
                    {
                        {
                            cursor = c4;
                            goto lab2;
                        }
                    }
                    ket = cursor;
                    slice_from("Y");
                lab2: ;
                }
                while (true)
                {
                    int c5 = cursor;
                    {

                        int ret = out_grouping(g_v, 97, 232, true);
                        if (ret < 0)
                        {
                            goto lab3;
                        }

                        cursor += ret;
                    }
                    {
                        int c6 = cursor;
                        bra = cursor;
                        {
                            int c7 = cursor;
                            if (!(eq_s("i")))
                            {
                                goto lab6;
                            }
                            ket = cursor;
                            {
                                int c8 = cursor;
                                if (in_grouping(g_v, 97, 232, false) != 0)
                                {
                                    goto lab7;
                                }
                                slice_from("I");
                            lab7: ;
                                cursor = c8;
                            }
                            goto lab5;
                        lab6: ;
                            cursor = c7;
                            if (!(eq_s("y")))
                            {
                                {
                                    cursor = c6;
                                    goto lab4;
                                }
                            }
                            ket = cursor;
                            slice_from("Y");
                        }
                    lab5: ;
                    lab4: ;
                    }
                    continue;
                lab3: ;
                    cursor = c5;
                    break;
                }
                cursor = c1;
            }
            {
                int c9 = cursor;
                I_p1 = limit;
                I_p2 = limit;
                {
                    int c10 = cursor;
                    {
                        int c = cursor + 3;
                        if (c > limit)
                        {
                            goto lab8;
                        }
                        cursor = c;
                    }
                    I_x = cursor;
                    cursor = c10;
                }
                {

                    int ret = out_grouping(g_v, 97, 232, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 232, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                if (I_p1 >= I_x)
                {
                    goto lab9;
                }
                I_p1 = I_x;
            lab9: ;
                {

                    int ret = out_grouping(g_v, 97, 232, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 232, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab8: ;
                cursor = c9;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c11 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_3, null);
                if (among_var == 0)
                {
                    goto lab11;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (I_p1 > cursor)
                        {
                            goto lab11;
                        }
                        slice_from("heid");
                        break;
                    }
                    case 2: {
                        if (!r_en_ending())
                            goto lab11;
                        break;
                    }
                    case 3: {
                        if (I_p1 > cursor)
                        {
                            goto lab11;
                        }
                        if (out_grouping_b(g_v_j, 97, 232, false) != 0)
                        {
                            goto lab11;
                        }
                        slice_del();
                        break;
                    }
                }
            lab11: ;
                cursor = limit - c11;
            }
            {
                int c12 = limit - cursor;
                r_e_ending();
                cursor = limit - c12;
            }
            {
                int c13 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("heid")))
                {
                    goto lab12;
                }
                bra = cursor;
                if (I_p2 > cursor)
                {
                    goto lab12;
                }
                if (!(eq_s_b("c")))
                {
                    goto lab13;
                }
                goto lab12;
            lab13: ;
                slice_del();
                ket = cursor;
                if (!(eq_s_b("en")))
                {
                    goto lab12;
                }
                bra = cursor;
                if (!r_en_ending())
                    goto lab12;
            lab12: ;
                cursor = limit - c13;
            }
            {
                int c14 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_4, null);
                if (among_var == 0)
                {
                    goto lab14;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (I_p2 > cursor)
                        {
                            goto lab14;
                        }
                        slice_del();
                        {
                            int c15 = limit - cursor;
                            ket = cursor;
                            if (!(eq_s_b("ig")))
                            {
                                goto lab16;
                            }
                            bra = cursor;
                            if (I_p2 > cursor)
                            {
                                goto lab16;
                            }
                            if (!(eq_s_b("e")))
                            {
                                goto lab17;
                            }
                            goto lab16;
                        lab17: ;
                            slice_del();
                            goto lab15;
                        lab16: ;
                            cursor = limit - c15;
                            if (!r_undouble())
                                goto lab14;
                        }
                    lab15: ;
                        break;
                    }
                    case 2: {
                        if (I_p2 > cursor)
                        {
                            goto lab14;
                        }
                        if (!(eq_s_b("e")))
                        {
                            goto lab18;
                        }
                        goto lab14;
                    lab18: ;
                        slice_del();
                        break;
                    }
                    case 3: {
                        if (I_p2 > cursor)
                        {
                            goto lab14;
                        }
                        slice_del();
                        if (!r_e_ending())
                            goto lab14;
                        break;
                    }
                    case 4: {
                        if (I_p2 > cursor)
                        {
                            goto lab14;
                        }
                        slice_del();
                        break;
                    }
                    case 5: {
                        if (I_p2 > cursor)
                        {
                            goto lab14;
                        }
                        if (!B_e_found)
                        {
                            goto lab14;
                        }
                        slice_del();
                        break;
                    }
                }
            lab14: ;
                cursor = limit - c14;
            }
            {
                int c16 = limit - cursor;
                if (out_grouping_b(g_v_I, 73, 232, false) != 0)
                {
                    goto lab19;
                }
                {
                    int c17 = limit - cursor;
                    if (find_among_b(a_5, null) == 0)
                    {
                        goto lab19;
                    }
                    if (out_grouping_b(g_v, 97, 232, false) != 0)
                    {
                        goto lab19;
                    }
                    cursor = limit - c17;
                }
                ket = cursor;
                if (cursor <= limit_backward)
                {
                    goto lab19;
                }
                cursor--;
                bra = cursor;
                slice_del();
            lab19: ;
                cursor = limit - c16;
            }
            cursor = limit_backward;
            {
                int c18 = cursor;
                while (true)
                {
                    int c19 = cursor;
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
                                goto lab21;
                            }
                            cursor++;
                            break;
                        }
                    }
                    continue;
                lab21: ;
                    cursor = c19;
                    break;
                }
                cursor = c18;
            }
            return true;
        }

    }
}

