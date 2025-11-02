// Generated from indonesian.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from indonesian.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class IndonesianStemmer : Stemmer
    {
        private int I_prefix;
        private int I_measure;

        private const string g_vowel = "aeiou";

        private static readonly Among[] a_0 = new[]
        {
            new Among("kah", -1, 1, 0),
            new Among("lah", -1, 1, 0),
            new Among("pun", -1, 1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("nya", -1, 1, 0),
            new Among("ku", -1, 1, 0),
            new Among("mu", -1, 1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("i", -1, 2, 0),
            new Among("an", -1, 1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("di", -1, 1, 0),
            new Among("ke", -1, 3, 0),
            new Among("me", -1, 1, 0),
            new Among("mem", 2, 5, 0),
            new Among("men", 2, 2, 0),
            new Among("meng", 4, 1, 0),
            new Among("pem", -1, 6, 0),
            new Among("pen", -1, 4, 0),
            new Among("peng", 7, 3, 0),
            new Among("ter", -1, 1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("be", -1, 2, 0),
            new Among("pe", -1, 1, 0)
        };


        private bool r_remove_particle()
        {
            ket = cursor;
            if (find_among_b(a_0, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            I_measure -= 1;
            return true;
        }

        private bool r_remove_possessive_pronoun()
        {
            ket = cursor;
            if (find_among_b(a_1, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            I_measure -= 1;
            return true;
        }

        private bool r_remove_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_2, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    {
                        int c1 = limit - cursor;
                        if (I_prefix == 3)
                        {
                            goto lab1;
                        }
                        if (I_prefix == 2)
                        {
                            goto lab1;
                        }
                        if (!(eq_s_b("k")))
                        {
                            goto lab1;
                        }
                        bra = cursor;
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (I_prefix == 1)
                        {
                            return false;
                        }
                    }
                lab0: ;
                    break;
                }
                case 2: {
                    if (I_prefix > 2)
                    {
                        return false;
                    }
                    {
                        int c2 = limit - cursor;
                        if (!(eq_s_b("s")))
                        {
                            goto lab2;
                        }
                        return false;
                    lab2: ;
                        cursor = limit - c2;
                    }
                    break;
                }
            }
            slice_del();
            I_measure -= 1;
            return true;
        }

        private bool r_remove_first_order_prefix()
        {
            int among_var;
            bra = cursor;
            among_var = find_among(a_3, null);
            if (among_var == 0)
            {
                return false;
            }
            ket = cursor;
            switch (among_var) {
                case 1: {
                    slice_del();
                    I_prefix = 1;
                    I_measure -= 1;
                    break;
                }
                case 2: {
                    {
                        int c1 = cursor;
                        if (!(eq_s("y")))
                        {
                            goto lab1;
                        }
                        {
                            int c2 = cursor;
                            if (in_grouping(g_vowel, 97, 117, false) != 0)
                            {
                                goto lab1;
                            }
                            cursor = c2;
                        }
                        ket = cursor;
                        slice_from("s");
                        I_prefix = 1;
                        I_measure -= 1;
                        goto lab0;
                    lab1: ;
                        cursor = c1;
                        slice_del();
                        I_prefix = 1;
                        I_measure -= 1;
                    }
                lab0: ;
                    break;
                }
                case 3: {
                    slice_del();
                    I_prefix = 3;
                    I_measure -= 1;
                    break;
                }
                case 4: {
                    {
                        int c3 = cursor;
                        if (!(eq_s("y")))
                        {
                            goto lab3;
                        }
                        {
                            int c4 = cursor;
                            if (in_grouping(g_vowel, 97, 117, false) != 0)
                            {
                                goto lab3;
                            }
                            cursor = c4;
                        }
                        ket = cursor;
                        slice_from("s");
                        I_prefix = 3;
                        I_measure -= 1;
                        goto lab2;
                    lab3: ;
                        cursor = c3;
                        slice_del();
                        I_prefix = 3;
                        I_measure -= 1;
                    }
                lab2: ;
                    break;
                }
                case 5: {
                    I_prefix = 1;
                    I_measure -= 1;
                    {
                        int c5 = cursor;
                        int c6 = cursor;
                        if (in_grouping(g_vowel, 97, 117, false) != 0)
                        {
                            goto lab5;
                        }
                        cursor = c6;
                        slice_from("p");
                        goto lab4;
                    lab5: ;
                        cursor = c5;
                        slice_del();
                    }
                lab4: ;
                    break;
                }
                case 6: {
                    I_prefix = 3;
                    I_measure -= 1;
                    {
                        int c7 = cursor;
                        int c8 = cursor;
                        if (in_grouping(g_vowel, 97, 117, false) != 0)
                        {
                            goto lab7;
                        }
                        cursor = c8;
                        slice_from("p");
                        goto lab6;
                    lab7: ;
                        cursor = c7;
                        slice_del();
                    }
                lab6: ;
                    break;
                }
            }
            return true;
        }

        private bool r_remove_second_order_prefix()
        {
            int among_var;
            bra = cursor;
            among_var = find_among(a_4, null);
            if (among_var == 0)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    {
                        int c1 = cursor;
                        if (!(eq_s("r")))
                        {
                            goto lab1;
                        }
                        ket = cursor;
                        I_prefix = 2;
                        goto lab0;
                    lab1: ;
                        cursor = c1;
                        if (!(eq_s("l")))
                        {
                            goto lab2;
                        }
                        ket = cursor;
                        if (!(eq_s("ajar")))
                        {
                            goto lab2;
                        }
                        goto lab0;
                    lab2: ;
                        cursor = c1;
                        ket = cursor;
                        I_prefix = 2;
                    }
                lab0: ;
                    break;
                }
                case 2: {
                    {
                        int c2 = cursor;
                        if (!(eq_s("r")))
                        {
                            goto lab4;
                        }
                        ket = cursor;
                        goto lab3;
                    lab4: ;
                        cursor = c2;
                        if (!(eq_s("l")))
                        {
                            goto lab5;
                        }
                        ket = cursor;
                        if (!(eq_s("ajar")))
                        {
                            goto lab5;
                        }
                        goto lab3;
                    lab5: ;
                        cursor = c2;
                        ket = cursor;
                        if (out_grouping(g_vowel, 97, 117, false) != 0)
                        {
                            return false;
                        }
                        if (!(eq_s("er")))
                        {
                            return false;
                        }
                    }
                lab3: ;
                    I_prefix = 4;
                    break;
                }
            }
            I_measure -= 1;
            slice_del();
            return true;
        }

        protected override bool stem()
        {
            I_measure = 0;
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    {

                        int ret = out_grouping(g_vowel, 97, 117, true);
                        if (ret < 0)
                        {
                            goto lab1;
                        }

                        cursor += ret;
                    }
                    I_measure += 1;
                    continue;
                lab1: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            if (I_measure <= 2)
            {
                return false;
            }
            I_prefix = 0;
            limit_backward = cursor;
            cursor = limit;
            {
                int c3 = limit - cursor;
                r_remove_particle();
                cursor = limit - c3;
            }
            if (I_measure <= 2)
            {
                return false;
            }
            {
                int c4 = limit - cursor;
                r_remove_possessive_pronoun();
                cursor = limit - c4;
            }
            cursor = limit_backward;
            if (I_measure <= 2)
            {
                return false;
            }
            {
                int c5 = cursor;
                {
                    int c6 = cursor;
                    if (!r_remove_first_order_prefix())
                        goto lab3;
                    {
                        int c7 = cursor;
                        {
                            int c8 = cursor;
                            if (I_measure <= 2)
                            {
                                goto lab4;
                            }
                            limit_backward = cursor;
                            cursor = limit;
                            if (!r_remove_suffix())
                                goto lab4;
                            cursor = limit_backward;
                            cursor = c8;
                        }
                        if (I_measure <= 2)
                        {
                            goto lab4;
                        }
                        if (!r_remove_second_order_prefix())
                            goto lab4;
                    lab4: ;
                        cursor = c7;
                    }
                    cursor = c6;
                }
                goto lab2;
            lab3: ;
                cursor = c5;
                {
                    int c9 = cursor;
                    r_remove_second_order_prefix();
                    cursor = c9;
                }
                {
                    int c10 = cursor;
                    if (I_measure <= 2)
                    {
                        goto lab5;
                    }
                    limit_backward = cursor;
                    cursor = limit;
                    if (!r_remove_suffix())
                        goto lab5;
                    cursor = limit_backward;
                lab5: ;
                    cursor = c10;
                }
            }
        lab2: ;
            return true;
        }

    }
}

