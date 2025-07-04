﻿// Generated from indonesian.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from indonesian.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class IndonesianStemmer : Stemmer
    {
        private int I_prefix;
        private int I_measure;

        private const string g_vowel = "aeiou";

        private static readonly Among[] a_0 = new[]
        {
            new Among("kah", -1, 1),
            new Among("lah", -1, 1),
            new Among("pun", -1, 1)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("nya", -1, 1),
            new Among("ku", -1, 1),
            new Among("mu", -1, 1)
        };

        private readonly Among[] a_2;

        private readonly Among[] a_3;

        private readonly Among[] a_4;

        /// <summary>
        ///   Initializes a new instance of the <see cref="IndonesianStemmer"/> class.
        /// </summary>
        ///
        public IndonesianStemmer()
        {

            a_2 = new[]
            {
                new Among("i", -1, 1, r_SUFFIX_I_OK),
                new Among("an", -1, 1, r_SUFFIX_AN_OK),
                new Among("kan", 1, 1, r_SUFFIX_KAN_OK)
            };

            a_3 = new[]
            {
                new Among("di", -1, 1),
                new Among("ke", -1, 2),
                new Among("me", -1, 1),
                new Among("mem", 2, 5),
                new Among("men", 2, 1),
                new Among("meng", 4, 1),
                new Among("meny", 4, 3, r_VOWEL),
                new Among("pem", -1, 6),
                new Among("pen", -1, 2),
                new Among("peng", 8, 2),
                new Among("peny", 8, 4, r_VOWEL),
                new Among("ter", -1, 1)
            };

            a_4 = new[]
            {
                new Among("be", -1, 3, r_KER),
                new Among("belajar", 0, 4),
                new Among("ber", 0, 3),
                new Among("pe", -1, 1),
                new Among("pelajar", 3, 2),
                new Among("per", 3, 1)
            };
        }



        private bool r_remove_particle()
        {
            ket = cursor;
            if (find_among_b(a_0) == 0)
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
            if (find_among_b(a_1) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            I_measure -= 1;
            return true;
        }

        private bool r_SUFFIX_KAN_OK()
        {
            if (I_prefix == 3)
            {
                return false;
            }
            if (I_prefix == 2)
            {
                return false;
            }
            return true;
        }

        private bool r_SUFFIX_AN_OK()
        {
            return I_prefix != 1;
        }

        private bool r_SUFFIX_I_OK()
        {
            if (I_prefix > 2)
            {
                return false;
            }
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("s")))
                {
                    goto lab0;
                }
                return false;
            lab0: ;
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_remove_suffix()
        {
            ket = cursor;
            if (find_among_b(a_2) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            I_measure -= 1;
            return true;
        }

        private bool r_VOWEL()
        {
            if (in_grouping(g_vowel, 97, 117, false) != 0)
            {
                return false;
            }
            return true;
        }

        private bool r_KER()
        {
            if (out_grouping(g_vowel, 97, 117, false) != 0)
            {
                return false;
            }
            if (!(eq_s("er")))
            {
                return false;
            }
            return true;
        }

        private bool r_remove_first_order_prefix()
        {
            int among_var;
            bra = cursor;
            among_var = find_among(a_3);
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
                    slice_del();
                    I_prefix = 3;
                    I_measure -= 1;
                    break;
                }
                case 3: {
                    I_prefix = 1;
                    slice_from("s");
                    I_measure -= 1;
                    break;
                }
                case 4: {
                    I_prefix = 3;
                    slice_from("s");
                    I_measure -= 1;
                    break;
                }
                case 5: {
                    I_prefix = 1;
                    I_measure -= 1;
                    {
                        int c1 = cursor;
                        int c2 = cursor;
                        if (in_grouping(g_vowel, 97, 117, false) != 0)
                        {
                            goto lab1;
                        }
                        cursor = c2;
                        slice_from("p");
                        goto lab0;
                    lab1: ;
                        cursor = c1;
                        slice_del();
                    }
                lab0: ;
                    break;
                }
                case 6: {
                    I_prefix = 3;
                    I_measure -= 1;
                    {
                        int c3 = cursor;
                        int c4 = cursor;
                        if (in_grouping(g_vowel, 97, 117, false) != 0)
                        {
                            goto lab3;
                        }
                        cursor = c4;
                        slice_from("p");
                        goto lab2;
                    lab3: ;
                        cursor = c3;
                        slice_del();
                    }
                lab2: ;
                    break;
                }
            }
            return true;
        }

        private bool r_remove_second_order_prefix()
        {
            int among_var;
            bra = cursor;
            among_var = find_among(a_4);
            if (among_var == 0)
            {
                return false;
            }
            ket = cursor;
            switch (among_var) {
                case 1: {
                    slice_del();
                    I_prefix = 2;
                    I_measure -= 1;
                    break;
                }
                case 2: {
                    slice_from("ajar");
                    I_measure -= 1;
                    break;
                }
                case 3: {
                    slice_del();
                    I_prefix = 4;
                    I_measure -= 1;
                    break;
                }
                case 4: {
                    slice_from("ajar");
                    I_prefix = 4;
                    I_measure -= 1;
                    break;
                }
            }
            return true;
        }

        protected override bool stem()
        {
            I_measure = 0;
            {
                int c1 = cursor;
                while (true)
                {
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
                int c2 = limit - cursor;
                r_remove_particle();
                cursor = limit - c2;
            }
            if (I_measure <= 2)
            {
                return false;
            }
            {
                int c3 = limit - cursor;
                r_remove_possessive_pronoun();
                cursor = limit - c3;
            }
            cursor = limit_backward;
            if (I_measure <= 2)
            {
                return false;
            }
            {
                int c4 = cursor;
                {
                    int c5 = cursor;
                    if (!r_remove_first_order_prefix())
                        goto lab3;
                    {
                        int c6 = cursor;
                        {
                            int c7 = cursor;
                            if (I_measure <= 2)
                            {
                                goto lab4;
                            }
                            limit_backward = cursor;
                            cursor = limit;
                            if (!r_remove_suffix())
                                goto lab4;
                            cursor = limit_backward;
                            cursor = c7;
                        }
                        if (I_measure <= 2)
                        {
                            goto lab4;
                        }
                        if (!r_remove_second_order_prefix())
                            goto lab4;
                    lab4: ;
                        cursor = c6;
                    }
                    cursor = c5;
                }
                goto lab2;
            lab3: ;
                cursor = c4;
                {
                    int c8 = cursor;
                    r_remove_second_order_prefix();
                    cursor = c8;
                }
                {
                    int c9 = cursor;
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
                    cursor = c9;
                }
            }
        lab2: ;
            return true;
        }

    }
}

