﻿// Generated from yiddish.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from yiddish.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class YiddishStemmer : Stemmer
    {
        private int I_x;
        private int I_p1;

        private const string g_niked = "\u05B0\u05B4\u05B5\u05B6\u05B1\u05B7\u05B2\u05B8\u05B3\u05C2\u05C1\u05B9\u05BC\u05BB\u05BF";
        private const string g_vowel = "\u05D0\u05D5\u05D9\u05E2\u05F1\u05F2";
        private const string g_consonant = "\u05D1\u05D2\u05D3\u05D4\u05D6\u05D7\u05D8\u05DA\u05DB\u05DC\u05DD\u05DE\u05DF\u05E0\u05E1\u05E3\u05E4\u05E5\u05E6\u05E7\u05E8\u05E9\u05EA\u05F0";

        private static readonly Among[] a_0 = new[]
        {
            new Among("\u05D5\u05D5", -1, 1),
            new Among("\u05D5\u05D9", -1, 2),
            new Among("\u05D9\u05D9", -1, 3),
            new Among("\u05DA", -1, 4),
            new Among("\u05DD", -1, 5),
            new Among("\u05DF", -1, 6),
            new Among("\u05E3", -1, 7),
            new Among("\u05E5", -1, 8)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("\u05D0\u05D3\u05D5\u05E8\u05DB", -1, 1),
            new Among("\u05D0\u05D4\u05D9\u05E0", -1, 1),
            new Among("\u05D0\u05D4\u05E2\u05E8", -1, 1),
            new Among("\u05D0\u05D4\u05F2\u05DE", -1, 1),
            new Among("\u05D0\u05D5\u05DE", -1, 1),
            new Among("\u05D0\u05D5\u05E0\u05D8\u05E2\u05E8", -1, 1),
            new Among("\u05D0\u05D9\u05D1\u05E2\u05E8", -1, 1),
            new Among("\u05D0\u05E0", -1, 1),
            new Among("\u05D0\u05E0\u05D8", 7, 1),
            new Among("\u05D0\u05E0\u05D8\u05E7\u05E2\u05D2\u05E0", 8, 1),
            new Among("\u05D0\u05E0\u05D9\u05D3\u05E2\u05E8", 7, 1),
            new Among("\u05D0\u05E4", -1, 1),
            new Among("\u05D0\u05E4\u05D9\u05E8", 11, 1),
            new Among("\u05D0\u05E7\u05E2\u05D2\u05E0", -1, 1),
            new Among("\u05D0\u05E8\u05D0\u05E4", -1, 1),
            new Among("\u05D0\u05E8\u05D5\u05DE", -1, 1),
            new Among("\u05D0\u05E8\u05D5\u05E0\u05D8\u05E2\u05E8", -1, 1),
            new Among("\u05D0\u05E8\u05D9\u05D1\u05E2\u05E8", -1, 1),
            new Among("\u05D0\u05E8\u05F1\u05E1", -1, 1),
            new Among("\u05D0\u05E8\u05F1\u05E4", -1, 1),
            new Among("\u05D0\u05E8\u05F2\u05E0", -1, 1),
            new Among("\u05D0\u05F0\u05E2\u05E7", -1, 1),
            new Among("\u05D0\u05F1\u05E1", -1, 1),
            new Among("\u05D0\u05F1\u05E4", -1, 1),
            new Among("\u05D0\u05F2\u05E0", -1, 1),
            new Among("\u05D1\u05D0", -1, 1),
            new Among("\u05D1\u05F2", -1, 1),
            new Among("\u05D3\u05D5\u05E8\u05DB", -1, 1),
            new Among("\u05D3\u05E2\u05E8", -1, 1),
            new Among("\u05DE\u05D9\u05D8", -1, 1),
            new Among("\u05E0\u05D0\u05DB", -1, 1),
            new Among("\u05E4\u05D0\u05E8", -1, 1),
            new Among("\u05E4\u05D0\u05E8\u05D1\u05F2", 31, 1),
            new Among("\u05E4\u05D0\u05E8\u05F1\u05E1", 31, 1),
            new Among("\u05E4\u05D5\u05E0\u05D0\u05E0\u05D3\u05E2\u05E8", -1, 1),
            new Among("\u05E6\u05D5", -1, 1),
            new Among("\u05E6\u05D5\u05D6\u05D0\u05DE\u05E2\u05E0", 35, 1),
            new Among("\u05E6\u05D5\u05E0\u05F1\u05E4", 35, 1),
            new Among("\u05E6\u05D5\u05E8\u05D9\u05E7", 35, 1),
            new Among("\u05E6\u05E2", -1, 1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("\u05D3\u05D6\u05E9", -1, -1),
            new Among("\u05E9\u05D8\u05E8", -1, -1),
            new Among("\u05E9\u05D8\u05E9", -1, -1),
            new Among("\u05E9\u05E4\u05E8", -1, -1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("\u05E7\u05DC\u05D9\u05D1", -1, 9),
            new Among("\u05E8\u05D9\u05D1", -1, 10),
            new Among("\u05D8\u05E8\u05D9\u05D1", 1, 7),
            new Among("\u05E9\u05E8\u05D9\u05D1", 1, 15),
            new Among("\u05D4\u05F1\u05D1", -1, 23),
            new Among("\u05E9\u05F0\u05D9\u05D2", -1, 12),
            new Among("\u05D2\u05D0\u05E0\u05D2", -1, 1),
            new Among("\u05D6\u05D5\u05E0\u05D2", -1, 18),
            new Among("\u05E9\u05DC\u05D5\u05E0\u05D2", -1, 21),
            new Among("\u05E6\u05F0\u05D5\u05E0\u05D2", -1, 20),
            new Among("\u05D1\u05F1\u05D2", -1, 22),
            new Among("\u05D1\u05D5\u05E0\u05D3", -1, 16),
            new Among("\u05F0\u05D9\u05D6", -1, 6),
            new Among("\u05D1\u05D9\u05D8", -1, 4),
            new Among("\u05DC\u05D9\u05D8", -1, 8),
            new Among("\u05DE\u05D9\u05D8", -1, 3),
            new Among("\u05E9\u05E0\u05D9\u05D8", -1, 14),
            new Among("\u05E0\u05D5\u05DE", -1, 2),
            new Among("\u05E9\u05D8\u05D0\u05E0", -1, 25),
            new Among("\u05D1\u05D9\u05E1", -1, 5),
            new Among("\u05E9\u05DE\u05D9\u05E1", -1, 13),
            new Among("\u05E8\u05D9\u05E1", -1, 11),
            new Among("\u05D8\u05E8\u05D5\u05E0\u05E7", -1, 19),
            new Among("\u05E4\u05D0\u05E8\u05DC\u05F1\u05E8", -1, 24),
            new Among("\u05E9\u05F0\u05F1\u05E8", -1, 26),
            new Among("\u05F0\u05D5\u05D8\u05E9", -1, 17)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("\u05D5\u05E0\u05D2", -1, 1),
            new Among("\u05E1\u05D8\u05D5", -1, 1),
            new Among("\u05D8", -1, 1),
            new Among("\u05D1\u05E8\u05D0\u05DB\u05D8", 2, 31),
            new Among("\u05E1\u05D8", 2, 1),
            new Among("\u05D9\u05E1\u05D8", 4, 33),
            new Among("\u05E2\u05D8", 2, 1),
            new Among("\u05E9\u05D0\u05E4\u05D8", 2, 1),
            new Among("\u05D4\u05F2\u05D8", 2, 1),
            new Among("\u05E7\u05F2\u05D8", 2, 1),
            new Among("\u05D9\u05E7\u05F2\u05D8", 9, 1),
            new Among("\u05DC\u05E2\u05DB", -1, 1),
            new Among("\u05E2\u05DC\u05E2\u05DB", 11, 1),
            new Among("\u05D9\u05D6\u05DE", -1, 1),
            new Among("\u05D9\u05DE", -1, 1),
            new Among("\u05E2\u05DE", -1, 1),
            new Among("\u05E2\u05E0\u05E2\u05DE", 15, 3),
            new Among("\u05D8\u05E2\u05E0\u05E2\u05DE", 16, 4),
            new Among("\u05E0", -1, 1),
            new Among("\u05E7\u05DC\u05D9\u05D1\u05E0", 18, 14),
            new Among("\u05E8\u05D9\u05D1\u05E0", 18, 15),
            new Among("\u05D8\u05E8\u05D9\u05D1\u05E0", 20, 12),
            new Among("\u05E9\u05E8\u05D9\u05D1\u05E0", 20, 7),
            new Among("\u05D4\u05F1\u05D1\u05E0", 18, 27),
            new Among("\u05E9\u05F0\u05D9\u05D2\u05E0", 18, 17),
            new Among("\u05D6\u05D5\u05E0\u05D2\u05E0", 18, 22),
            new Among("\u05E9\u05DC\u05D5\u05E0\u05D2\u05E0", 18, 25),
            new Among("\u05E6\u05F0\u05D5\u05E0\u05D2\u05E0", 18, 24),
            new Among("\u05D1\u05F1\u05D2\u05E0", 18, 26),
            new Among("\u05D1\u05D5\u05E0\u05D3\u05E0", 18, 20),
            new Among("\u05F0\u05D9\u05D6\u05E0", 18, 11),
            new Among("\u05D8\u05E0", 18, 4),
            new Among("GE\u05D1\u05D9\u05D8\u05E0", 31, 9),
            new Among("GE\u05DC\u05D9\u05D8\u05E0", 31, 13),
            new Among("GE\u05DE\u05D9\u05D8\u05E0", 31, 8),
            new Among("\u05E9\u05E0\u05D9\u05D8\u05E0", 31, 19),
            new Among("\u05E1\u05D8\u05E0", 31, 1),
            new Among("\u05D9\u05E1\u05D8\u05E0", 36, 1),
            new Among("\u05E2\u05D8\u05E0", 31, 1),
            new Among("GE\u05D1\u05D9\u05E1\u05E0", 18, 10),
            new Among("\u05E9\u05DE\u05D9\u05E1\u05E0", 18, 18),
            new Among("GE\u05E8\u05D9\u05E1\u05E0", 18, 16),
            new Among("\u05E2\u05E0", 18, 1),
            new Among("\u05D2\u05D0\u05E0\u05D2\u05E2\u05E0", 42, 5),
            new Among("\u05E2\u05DC\u05E2\u05E0", 42, 1),
            new Among("\u05E0\u05D5\u05DE\u05E2\u05E0", 42, 6),
            new Among("\u05D9\u05D6\u05DE\u05E2\u05E0", 42, 1),
            new Among("\u05E9\u05D8\u05D0\u05E0\u05E2\u05E0", 42, 29),
            new Among("\u05D8\u05E8\u05D5\u05E0\u05E7\u05E0", 18, 23),
            new Among("\u05E4\u05D0\u05E8\u05DC\u05F1\u05E8\u05E0", 18, 28),
            new Among("\u05E9\u05F0\u05F1\u05E8\u05E0", 18, 30),
            new Among("\u05F0\u05D5\u05D8\u05E9\u05E0", 18, 21),
            new Among("\u05D2\u05F2\u05E0", 18, 5),
            new Among("\u05E1", -1, 1),
            new Among("\u05D8\u05E1", 53, 4),
            new Among("\u05E2\u05D8\u05E1", 54, 1),
            new Among("\u05E0\u05E1", 53, 1),
            new Among("\u05D8\u05E0\u05E1", 56, 4),
            new Among("\u05E2\u05E0\u05E1", 56, 3),
            new Among("\u05E2\u05E1", 53, 1),
            new Among("\u05D9\u05E2\u05E1", 59, 2),
            new Among("\u05E2\u05DC\u05E2\u05E1", 59, 1),
            new Among("\u05E2\u05E8\u05E1", 53, 1),
            new Among("\u05E2\u05E0\u05E2\u05E8\u05E1", 62, 1),
            new Among("\u05E2", -1, 1),
            new Among("\u05D8\u05E2", 64, 4),
            new Among("\u05E1\u05D8\u05E2", 65, 1),
            new Among("\u05E2\u05D8\u05E2", 65, 1),
            new Among("\u05D9\u05E2", 64, -1),
            new Among("\u05E2\u05DC\u05E2", 64, 1),
            new Among("\u05E2\u05E0\u05E2", 64, 3),
            new Among("\u05D8\u05E2\u05E0\u05E2", 70, 4),
            new Among("\u05E2\u05E8", -1, 1),
            new Among("\u05D8\u05E2\u05E8", 72, 4),
            new Among("\u05E1\u05D8\u05E2\u05E8", 73, 1),
            new Among("\u05E2\u05D8\u05E2\u05E8", 73, 1),
            new Among("\u05E2\u05E0\u05E2\u05E8", 72, 3),
            new Among("\u05D8\u05E2\u05E0\u05E2\u05E8", 76, 4),
            new Among("\u05D5\u05EA", -1, 32)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("\u05D5\u05E0\u05D2", -1, 1),
            new Among("\u05E9\u05D0\u05E4\u05D8", -1, 1),
            new Among("\u05D4\u05F2\u05D8", -1, 1),
            new Among("\u05E7\u05F2\u05D8", -1, 1),
            new Among("\u05D9\u05E7\u05F2\u05D8", 3, 1),
            new Among("\u05DC", -1, 2)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("\u05D9\u05D2", -1, 1),
            new Among("\u05D9\u05E7", -1, 1),
            new Among("\u05D3\u05D9\u05E7", 1, 1),
            new Among("\u05E0\u05D3\u05D9\u05E7", 2, 1),
            new Among("\u05E2\u05E0\u05D3\u05D9\u05E7", 3, 1),
            new Among("\u05D1\u05DC\u05D9\u05E7", 1, -1),
            new Among("\u05D2\u05DC\u05D9\u05E7", 1, -1),
            new Among("\u05E0\u05D9\u05E7", 1, 1),
            new Among("\u05D9\u05E9", -1, 1)
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
                        bra = cursor;
                        among_var = find_among(a_0);
                        if (among_var == 0)
                        {
                            goto lab2;
                        }
                        ket = cursor;
                        switch (among_var) {
                            case 1: {
                                {
                                    int c4 = cursor;
                                    if (!(eq_s("\u05BC")))
                                    {
                                        goto lab3;
                                    }
                                    goto lab2;
                                lab3: ;
                                    cursor = c4;
                                }
                                slice_from("\u05F0");
                                break;
                            }
                            case 2: {
                                {
                                    int c5 = cursor;
                                    if (!(eq_s("\u05B4")))
                                    {
                                        goto lab4;
                                    }
                                    goto lab2;
                                lab4: ;
                                    cursor = c5;
                                }
                                slice_from("\u05F1");
                                break;
                            }
                            case 3: {
                                {
                                    int c6 = cursor;
                                    if (!(eq_s("\u05B4")))
                                    {
                                        goto lab5;
                                    }
                                    goto lab2;
                                lab5: ;
                                    cursor = c6;
                                }
                                slice_from("\u05F2");
                                break;
                            }
                            case 4: {
                                slice_from("\u05DB");
                                break;
                            }
                            case 5: {
                                slice_from("\u05DE");
                                break;
                            }
                            case 6: {
                                slice_from("\u05E0");
                                break;
                            }
                            case 7: {
                                slice_from("\u05E4");
                                break;
                            }
                            case 8: {
                                slice_from("\u05E6");
                                break;
                            }
                        }
                        cursor = c3;
                        break;
                    lab2: ;
                        cursor = c3;
                        if (cursor >= limit)
                        {
                            goto lab1;
                        }
                        cursor++;
                    }
                    continue;
                lab1: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            {
                int c7 = cursor;
                while (true)
                {
                    int c8 = cursor;
                    while (true)
                    {
                        int c9 = cursor;
                        bra = cursor;
                        if (in_grouping(g_niked, 1456, 1474, false) != 0)
                        {
                            goto lab8;
                        }
                        ket = cursor;
                        slice_del();
                        cursor = c9;
                        break;
                    lab8: ;
                        cursor = c9;
                        if (cursor >= limit)
                        {
                            goto lab7;
                        }
                        cursor++;
                    }
                    continue;
                lab7: ;
                    cursor = c8;
                    break;
                }
                cursor = c7;
            }
            return true;
        }

        private bool r_mark_regions()
        {
            I_p1 = limit;
            {
                int c1 = cursor;
                bra = cursor;
                if (!(eq_s("\u05D2\u05E2")))
                {
                    {
                        cursor = c1;
                        goto lab0;
                    }
                }
                ket = cursor;
                {
                    int c2 = cursor;
                    {
                        int c3 = cursor;
                        if (!(eq_s("\u05DC\u05D8")))
                        {
                            goto lab3;
                        }
                        goto lab2;
                    lab3: ;
                        cursor = c3;
                        if (!(eq_s("\u05D1\u05E0")))
                        {
                            goto lab4;
                        }
                        goto lab2;
                    lab4: ;
                        cursor = c3;
                        if (cursor < limit)
                        {
                            goto lab1;
                        }
                    }
                lab2: ;
                    {
                        cursor = c1;
                        goto lab0;
                    }
                lab1: ;
                    cursor = c2;
                }
                slice_from("GE");
            lab0: ;
            }
            {
                int c4 = cursor;
                if (find_among(a_1) == 0)
                {
                    {
                        cursor = c4;
                        goto lab5;
                    }
                }
                {
                    int c5 = cursor;
                    {
                        int c6 = cursor;
                        {
                            int c7 = cursor;
                            if (!(eq_s("\u05E6\u05D5\u05D2\u05E0")))
                            {
                                goto lab9;
                            }
                            goto lab8;
                        lab9: ;
                            cursor = c7;
                            if (!(eq_s("\u05E6\u05D5\u05E7\u05D8")))
                            {
                                goto lab10;
                            }
                            goto lab8;
                        lab10: ;
                            cursor = c7;
                            if (!(eq_s("\u05E6\u05D5\u05E7\u05E0")))
                            {
                                goto lab7;
                            }
                        }
                    lab8: ;
                        if (cursor < limit)
                        {
                            goto lab7;
                        }
                        cursor = c6;
                    }
                    goto lab6;
                lab7: ;
                    cursor = c5;
                    {
                        int c8 = cursor;
                        if (!(eq_s("\u05D2\u05E2\u05D1\u05E0")))
                        {
                            goto lab11;
                        }
                        cursor = c8;
                    }
                    goto lab6;
                lab11: ;
                    cursor = c5;
                    bra = cursor;
                    if (!(eq_s("\u05D2\u05E2")))
                    {
                        goto lab12;
                    }
                    ket = cursor;
                    slice_from("GE");
                    goto lab6;
                lab12: ;
                    cursor = c5;
                    bra = cursor;
                    if (!(eq_s("\u05E6\u05D5")))
                    {
                        {
                            cursor = c4;
                            goto lab5;
                        }
                    }
                    ket = cursor;
                    slice_from("TSU");
                }
            lab6: ;
            lab5: ;
            }
            {
                int c9 = cursor;
                {
                    int c = cursor + 3;
                    if (c > limit)
                    {
                        return false;
                    }
                    cursor = c;
                }
                I_x = cursor;
                cursor = c9;
            }
            {
                int c10 = cursor;
                if (find_among(a_2) == 0)
                {
                    {
                        cursor = c10;
                        goto lab13;
                    }
                }
            lab13: ;
            }
            {
                int c11 = cursor;
                if (in_grouping(g_consonant, 1489, 1520, false) != 0)
                {
                    goto lab14;
                }
                if (in_grouping(g_consonant, 1489, 1520, false) != 0)
                {
                    goto lab14;
                }
                if (in_grouping(g_consonant, 1489, 1520, false) != 0)
                {
                    goto lab14;
                }
                I_p1 = cursor;
                return false;
            lab14: ;
                cursor = c11;
            }
            {

                int ret = out_grouping(g_vowel, 1488, 1522, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            if (in_grouping(g_vowel, 1488, 1522, true) < 0)
            {
                return false;
            }

            I_p1 = cursor;
            if (I_p1 >= I_x)
            {
                goto lab15;
            }
            I_p1 = I_x;
        lab15: ;
            return true;
        }

        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_R1plus3()
        {
            return I_p1 <= (cursor + 3);
        }

        private bool r_standard_suffix()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_4);
                if (among_var == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (!r_R1())
                            goto lab0;
                        slice_del();
                        break;
                    }
                    case 2: {
                        if (!r_R1())
                            goto lab0;
                        slice_from("\u05D9\u05E2");
                        break;
                    }
                    case 3: {
                        if (!r_R1())
                            goto lab0;
                        slice_del();
                        ket = cursor;
                        among_var = find_among_b(a_3);
                        if (among_var == 0)
                        {
                            goto lab0;
                        }
                        bra = cursor;
                        switch (among_var) {
                            case 1: {
                                slice_from("\u05D2\u05F2");
                                break;
                            }
                            case 2: {
                                slice_from("\u05E0\u05E2\u05DE");
                                break;
                            }
                            case 3: {
                                slice_from("\u05DE\u05F2\u05D3");
                                break;
                            }
                            case 4: {
                                slice_from("\u05D1\u05F2\u05D8");
                                break;
                            }
                            case 5: {
                                slice_from("\u05D1\u05F2\u05E1");
                                break;
                            }
                            case 6: {
                                slice_from("\u05F0\u05F2\u05D6");
                                break;
                            }
                            case 7: {
                                slice_from("\u05D8\u05E8\u05F2\u05D1");
                                break;
                            }
                            case 8: {
                                slice_from("\u05DC\u05F2\u05D8");
                                break;
                            }
                            case 9: {
                                slice_from("\u05E7\u05DC\u05F2\u05D1");
                                break;
                            }
                            case 10: {
                                slice_from("\u05E8\u05F2\u05D1");
                                break;
                            }
                            case 11: {
                                slice_from("\u05E8\u05F2\u05E1");
                                break;
                            }
                            case 12: {
                                slice_from("\u05E9\u05F0\u05F2\u05D2");
                                break;
                            }
                            case 13: {
                                slice_from("\u05E9\u05DE\u05F2\u05E1");
                                break;
                            }
                            case 14: {
                                slice_from("\u05E9\u05E0\u05F2\u05D3");
                                break;
                            }
                            case 15: {
                                slice_from("\u05E9\u05E8\u05F2\u05D1");
                                break;
                            }
                            case 16: {
                                slice_from("\u05D1\u05D9\u05E0\u05D3");
                                break;
                            }
                            case 17: {
                                slice_from("\u05F0\u05D9\u05D8\u05E9");
                                break;
                            }
                            case 18: {
                                slice_from("\u05D6\u05D9\u05E0\u05D2");
                                break;
                            }
                            case 19: {
                                slice_from("\u05D8\u05E8\u05D9\u05E0\u05E7");
                                break;
                            }
                            case 20: {
                                slice_from("\u05E6\u05F0\u05D9\u05E0\u05D2");
                                break;
                            }
                            case 21: {
                                slice_from("\u05E9\u05DC\u05D9\u05E0\u05D2");
                                break;
                            }
                            case 22: {
                                slice_from("\u05D1\u05F2\u05D2");
                                break;
                            }
                            case 23: {
                                slice_from("\u05D4\u05F2\u05D1");
                                break;
                            }
                            case 24: {
                                slice_from("\u05E4\u05D0\u05E8\u05DC\u05D9\u05E8");
                                break;
                            }
                            case 25: {
                                slice_from("\u05E9\u05D8\u05F2");
                                break;
                            }
                            case 26: {
                                slice_from("\u05E9\u05F0\u05E2\u05E8");
                                break;
                            }
                        }
                        break;
                    }
                    case 4: {
                        {
                            int c2 = limit - cursor;
                            if (!r_R1())
                                goto lab2;
                            slice_del();
                            goto lab1;
                        lab2: ;
                            cursor = limit - c2;
                            slice_from("\u05D8");
                        }
                    lab1: ;
                        ket = cursor;
                        if (!(eq_s_b("\u05D1\u05E8\u05D0\u05DB")))
                        {
                            goto lab0;
                        }
                        {
                            int c3 = limit - cursor;
                            if (!(eq_s_b("\u05D2\u05E2")))
                            {
                                {
                                    cursor = limit - c3;
                                    goto lab3;
                                }
                            }
                        lab3: ;
                        }
                        bra = cursor;
                        slice_from("\u05D1\u05E8\u05E2\u05E0\u05D2");
                        break;
                    }
                    case 5: {
                        slice_from("\u05D2\u05F2");
                        break;
                    }
                    case 6: {
                        slice_from("\u05E0\u05E2\u05DE");
                        break;
                    }
                    case 7: {
                        slice_from("\u05E9\u05E8\u05F2\u05D1");
                        break;
                    }
                    case 8: {
                        slice_from("\u05DE\u05F2\u05D3");
                        break;
                    }
                    case 9: {
                        slice_from("\u05D1\u05F2\u05D8");
                        break;
                    }
                    case 10: {
                        slice_from("\u05D1\u05F2\u05E1");
                        break;
                    }
                    case 11: {
                        slice_from("\u05F0\u05F2\u05D6");
                        break;
                    }
                    case 12: {
                        slice_from("\u05D8\u05E8\u05F2\u05D1");
                        break;
                    }
                    case 13: {
                        slice_from("\u05DC\u05F2\u05D8");
                        break;
                    }
                    case 14: {
                        slice_from("\u05E7\u05DC\u05F2\u05D1");
                        break;
                    }
                    case 15: {
                        slice_from("\u05E8\u05F2\u05D1");
                        break;
                    }
                    case 16: {
                        slice_from("\u05E8\u05F2\u05E1");
                        break;
                    }
                    case 17: {
                        slice_from("\u05E9\u05F0\u05F2\u05D2");
                        break;
                    }
                    case 18: {
                        slice_from("\u05E9\u05DE\u05F2\u05E1");
                        break;
                    }
                    case 19: {
                        slice_from("\u05E9\u05E0\u05F2\u05D3");
                        break;
                    }
                    case 20: {
                        slice_from("\u05D1\u05D9\u05E0\u05D3");
                        break;
                    }
                    case 21: {
                        slice_from("\u05F0\u05D9\u05D8\u05E9");
                        break;
                    }
                    case 22: {
                        slice_from("\u05D6\u05D9\u05E0\u05D2");
                        break;
                    }
                    case 23: {
                        slice_from("\u05D8\u05E8\u05D9\u05E0\u05E7");
                        break;
                    }
                    case 24: {
                        slice_from("\u05E6\u05F0\u05D9\u05E0\u05D2");
                        break;
                    }
                    case 25: {
                        slice_from("\u05E9\u05DC\u05D9\u05E0\u05D2");
                        break;
                    }
                    case 26: {
                        slice_from("\u05D1\u05F2\u05D2");
                        break;
                    }
                    case 27: {
                        slice_from("\u05D4\u05F2\u05D1");
                        break;
                    }
                    case 28: {
                        slice_from("\u05E4\u05D0\u05E8\u05DC\u05D9\u05E8");
                        break;
                    }
                    case 29: {
                        slice_from("\u05E9\u05D8\u05F2");
                        break;
                    }
                    case 30: {
                        slice_from("\u05E9\u05F0\u05E2\u05E8");
                        break;
                    }
                    case 31: {
                        slice_from("\u05D1\u05E8\u05E2\u05E0\u05D2");
                        break;
                    }
                    case 32: {
                        if (!r_R1())
                            goto lab0;
                        slice_from("\u05D4");
                        break;
                    }
                    case 33: {
                        {
                            int c4 = limit - cursor;
                            {
                                int c5 = limit - cursor;
                                if (!(eq_s_b("\u05D2")))
                                {
                                    goto lab7;
                                }
                                goto lab6;
                            lab7: ;
                                cursor = limit - c5;
                                if (!(eq_s_b("\u05E9")))
                                {
                                    goto lab5;
                                }
                            }
                        lab6: ;
                            {
                                int c6 = limit - cursor;
                                if (!r_R1plus3())
                                    {
                                        cursor = limit - c6;
                                        goto lab8;
                                    }
                                slice_from("\u05D9\u05E1");
                            lab8: ;
                            }
                            goto lab4;
                        lab5: ;
                            cursor = limit - c4;
                            if (!r_R1())
                                goto lab0;
                            slice_del();
                        }
                    lab4: ;
                        break;
                    }
                }
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c7 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_5);
                if (among_var == 0)
                {
                    goto lab9;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (!r_R1())
                            goto lab9;
                        slice_del();
                        break;
                    }
                    case 2: {
                        if (!r_R1())
                            goto lab9;
                        if (in_grouping_b(g_consonant, 1489, 1520, false) != 0)
                        {
                            goto lab9;
                        }
                        slice_del();
                        break;
                    }
                }
            lab9: ;
                cursor = limit - c7;
            }
            {
                int c8 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_6);
                if (among_var == 0)
                {
                    goto lab10;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (!r_R1())
                            goto lab10;
                        slice_del();
                        break;
                    }
                }
            lab10: ;
                cursor = limit - c8;
            }
            {
                int c9 = limit - cursor;
                while (true)
                {
                    int c10 = limit - cursor;
                    while (true)
                    {
                        int c11 = limit - cursor;
                        ket = cursor;
                        {
                            int c12 = limit - cursor;
                            if (!(eq_s_b("GE")))
                            {
                                goto lab15;
                            }
                            goto lab14;
                        lab15: ;
                            cursor = limit - c12;
                            if (!(eq_s_b("TSU")))
                            {
                                goto lab13;
                            }
                        }
                    lab14: ;
                        bra = cursor;
                        slice_del();
                        cursor = limit - c11;
                        break;
                    lab13: ;
                        cursor = limit - c11;
                        if (cursor <= limit_backward)
                        {
                            goto lab12;
                        }
                        cursor--;
                    }
                    continue;
                lab12: ;
                    cursor = limit - c10;
                    break;
                }
                cursor = limit - c9;
            }
            return true;
        }

        protected override bool stem()
        {
            r_prelude();
            {
                int c1 = cursor;
                r_mark_regions();
                cursor = c1;
            }
            limit_backward = cursor;
            cursor = limit;
            r_standard_suffix();
            cursor = limit_backward;
            return true;
        }

    }
}

