﻿// Generated from turkish.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from turkish.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class TurkishStemmer : Stemmer
    {
        private bool B_continue_stemming_noun_suffixes;

        private const string g_vowel = "aeıioöuü";
        private const string g_U = "ıiuü";
        private const string g_vowel1 = "aıou";
        private const string g_vowel2 = "eiöü";
        private const string g_vowel3 = "aı";
        private const string g_vowel4 = "ei";
        private const string g_vowel5 = "ou";
        private const string g_vowel6 = "öü";

        private static readonly Among[] a_0 = new[]
        {
            new Among("m", -1, -1),
            new Among("n", -1, -1),
            new Among("miz", -1, -1),
            new Among("niz", -1, -1),
            new Among("muz", -1, -1),
            new Among("nuz", -1, -1),
            new Among("müz", -1, -1),
            new Among("nüz", -1, -1),
            new Among("mız", -1, -1),
            new Among("nız", -1, -1)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("leri", -1, -1),
            new Among("ları", -1, -1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ni", -1, -1),
            new Among("nu", -1, -1),
            new Among("nü", -1, -1),
            new Among("nı", -1, -1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("in", -1, -1),
            new Among("un", -1, -1),
            new Among("ün", -1, -1),
            new Among("ın", -1, -1)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("a", -1, -1),
            new Among("e", -1, -1)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("na", -1, -1),
            new Among("ne", -1, -1)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("da", -1, -1),
            new Among("ta", -1, -1),
            new Among("de", -1, -1),
            new Among("te", -1, -1)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("nda", -1, -1),
            new Among("nde", -1, -1)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("dan", -1, -1),
            new Among("tan", -1, -1),
            new Among("den", -1, -1),
            new Among("ten", -1, -1)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("ndan", -1, -1),
            new Among("nden", -1, -1)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("la", -1, -1),
            new Among("le", -1, -1)
        };

        private static readonly Among[] a_11 = new[]
        {
            new Among("ca", -1, -1),
            new Among("ce", -1, -1)
        };

        private static readonly Among[] a_12 = new[]
        {
            new Among("im", -1, -1),
            new Among("um", -1, -1),
            new Among("üm", -1, -1),
            new Among("ım", -1, -1)
        };

        private static readonly Among[] a_13 = new[]
        {
            new Among("sin", -1, -1),
            new Among("sun", -1, -1),
            new Among("sün", -1, -1),
            new Among("sın", -1, -1)
        };

        private static readonly Among[] a_14 = new[]
        {
            new Among("iz", -1, -1),
            new Among("uz", -1, -1),
            new Among("üz", -1, -1),
            new Among("ız", -1, -1)
        };

        private static readonly Among[] a_15 = new[]
        {
            new Among("siniz", -1, -1),
            new Among("sunuz", -1, -1),
            new Among("sünüz", -1, -1),
            new Among("sınız", -1, -1)
        };

        private static readonly Among[] a_16 = new[]
        {
            new Among("lar", -1, -1),
            new Among("ler", -1, -1)
        };

        private static readonly Among[] a_17 = new[]
        {
            new Among("niz", -1, -1),
            new Among("nuz", -1, -1),
            new Among("nüz", -1, -1),
            new Among("nız", -1, -1)
        };

        private static readonly Among[] a_18 = new[]
        {
            new Among("dir", -1, -1),
            new Among("tir", -1, -1),
            new Among("dur", -1, -1),
            new Among("tur", -1, -1),
            new Among("dür", -1, -1),
            new Among("tür", -1, -1),
            new Among("dır", -1, -1),
            new Among("tır", -1, -1)
        };

        private static readonly Among[] a_19 = new[]
        {
            new Among("casına", -1, -1),
            new Among("cesine", -1, -1)
        };

        private static readonly Among[] a_20 = new[]
        {
            new Among("di", -1, -1),
            new Among("ti", -1, -1),
            new Among("dik", -1, -1),
            new Among("tik", -1, -1),
            new Among("duk", -1, -1),
            new Among("tuk", -1, -1),
            new Among("dük", -1, -1),
            new Among("tük", -1, -1),
            new Among("dık", -1, -1),
            new Among("tık", -1, -1),
            new Among("dim", -1, -1),
            new Among("tim", -1, -1),
            new Among("dum", -1, -1),
            new Among("tum", -1, -1),
            new Among("düm", -1, -1),
            new Among("tüm", -1, -1),
            new Among("dım", -1, -1),
            new Among("tım", -1, -1),
            new Among("din", -1, -1),
            new Among("tin", -1, -1),
            new Among("dun", -1, -1),
            new Among("tun", -1, -1),
            new Among("dün", -1, -1),
            new Among("tün", -1, -1),
            new Among("dın", -1, -1),
            new Among("tın", -1, -1),
            new Among("du", -1, -1),
            new Among("tu", -1, -1),
            new Among("dü", -1, -1),
            new Among("tü", -1, -1),
            new Among("dı", -1, -1),
            new Among("tı", -1, -1)
        };

        private static readonly Among[] a_21 = new[]
        {
            new Among("sa", -1, -1),
            new Among("se", -1, -1),
            new Among("sak", -1, -1),
            new Among("sek", -1, -1),
            new Among("sam", -1, -1),
            new Among("sem", -1, -1),
            new Among("san", -1, -1),
            new Among("sen", -1, -1)
        };

        private static readonly Among[] a_22 = new[]
        {
            new Among("miş", -1, -1),
            new Among("muş", -1, -1),
            new Among("müş", -1, -1),
            new Among("mış", -1, -1)
        };

        private static readonly Among[] a_23 = new[]
        {
            new Among("b", -1, 1),
            new Among("c", -1, 2),
            new Among("d", -1, 3),
            new Among("ğ", -1, 4)
        };


        private bool r_check_vowel_harmony()
        {
            {
                int c1 = limit - cursor;
                if (out_grouping_b(g_vowel, 97, 305, true) < 0)
                {
                    return false;
                }

                {
                    int c2 = limit - cursor;
                    if (!(eq_s_b("a")))
                    {
                        goto lab1;
                    }
                    if (out_grouping_b(g_vowel1, 97, 305, true) < 0)
                    {
                        goto lab1;
                    }

                    goto lab0;
                lab1: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("e")))
                    {
                        goto lab2;
                    }
                    if (out_grouping_b(g_vowel2, 101, 252, true) < 0)
                    {
                        goto lab2;
                    }

                    goto lab0;
                lab2: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("ı")))
                    {
                        goto lab3;
                    }
                    if (out_grouping_b(g_vowel3, 97, 305, true) < 0)
                    {
                        goto lab3;
                    }

                    goto lab0;
                lab3: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("i")))
                    {
                        goto lab4;
                    }
                    if (out_grouping_b(g_vowel4, 101, 105, true) < 0)
                    {
                        goto lab4;
                    }

                    goto lab0;
                lab4: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("o")))
                    {
                        goto lab5;
                    }
                    if (out_grouping_b(g_vowel5, 111, 117, true) < 0)
                    {
                        goto lab5;
                    }

                    goto lab0;
                lab5: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("ö")))
                    {
                        goto lab6;
                    }
                    if (out_grouping_b(g_vowel6, 246, 252, true) < 0)
                    {
                        goto lab6;
                    }

                    goto lab0;
                lab6: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("u")))
                    {
                        goto lab7;
                    }
                    if (out_grouping_b(g_vowel5, 111, 117, true) < 0)
                    {
                        goto lab7;
                    }

                    goto lab0;
                lab7: ;
                    cursor = limit - c2;
                    if (!(eq_s_b("ü")))
                    {
                        return false;
                    }
                    if (out_grouping_b(g_vowel6, 246, 252, true) < 0)
                    {
                        return false;
                    }

                }
            lab0: ;
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_mark_suffix_with_optional_n_consonant()
        {
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("n")))
                {
                    goto lab1;
                }
                {
                    int c2 = limit - cursor;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        goto lab1;
                    }
                    cursor = limit - c2;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                {
                    int c3 = limit - cursor;
                    {
                        int c4 = limit - cursor;
                        if (!(eq_s_b("n")))
                        {
                            goto lab2;
                        }
                        cursor = limit - c4;
                    }
                    return false;
                lab2: ;
                    cursor = limit - c3;
                }
                {
                    int c5 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c5;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_mark_suffix_with_optional_s_consonant()
        {
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("s")))
                {
                    goto lab1;
                }
                {
                    int c2 = limit - cursor;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        goto lab1;
                    }
                    cursor = limit - c2;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                {
                    int c3 = limit - cursor;
                    {
                        int c4 = limit - cursor;
                        if (!(eq_s_b("s")))
                        {
                            goto lab2;
                        }
                        cursor = limit - c4;
                    }
                    return false;
                lab2: ;
                    cursor = limit - c3;
                }
                {
                    int c5 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c5;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_mark_suffix_with_optional_y_consonant()
        {
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("y")))
                {
                    goto lab1;
                }
                {
                    int c2 = limit - cursor;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        goto lab1;
                    }
                    cursor = limit - c2;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                {
                    int c3 = limit - cursor;
                    {
                        int c4 = limit - cursor;
                        if (!(eq_s_b("y")))
                        {
                            goto lab2;
                        }
                        cursor = limit - c4;
                    }
                    return false;
                lab2: ;
                    cursor = limit - c3;
                }
                {
                    int c5 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c5;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_mark_suffix_with_optional_U_vowel()
        {
            {
                int c1 = limit - cursor;
                if (in_grouping_b(g_U, 105, 305, false) != 0)
                {
                    goto lab1;
                }
                {
                    int c2 = limit - cursor;
                    if (out_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        goto lab1;
                    }
                    cursor = limit - c2;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                {
                    int c3 = limit - cursor;
                    {
                        int c4 = limit - cursor;
                        if (in_grouping_b(g_U, 105, 305, false) != 0)
                        {
                            goto lab2;
                        }
                        cursor = limit - c4;
                    }
                    return false;
                lab2: ;
                    cursor = limit - c3;
                }
                {
                    int c5 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (out_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c5;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_mark_possessives()
        {
            if (find_among_b(a_0) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_U_vowel())
                return false;
            return true;
        }

        private bool r_mark_sU()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (in_grouping_b(g_U, 105, 305, false) != 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_s_consonant())
                return false;
            return true;
        }

        private bool r_mark_lArI()
        {
            if (find_among_b(a_1) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_yU()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (in_grouping_b(g_U, 105, 305, false) != 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_nU()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_2) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_nUn()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_3) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_n_consonant())
                return false;
            return true;
        }

        private bool r_mark_yA()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_4) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_nA()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_5) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_DA()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_6) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_ndA()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_7) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_DAn()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_8) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_ndAn()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_9) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_ylA()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_10) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_ki()
        {
            if (!(eq_s_b("ki")))
            {
                return false;
            }
            return true;
        }

        private bool r_mark_ncA()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_11) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_n_consonant())
                return false;
            return true;
        }

        private bool r_mark_yUm()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_12) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_sUn()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_13) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_yUz()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_14) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_sUnUz()
        {
            if (find_among_b(a_15) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_lAr()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_16) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_nUz()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_17) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_DUr()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_18) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_cAsInA()
        {
            if (find_among_b(a_19) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_mark_yDU()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_20) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_ysA()
        {
            if (find_among_b(a_21) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_ymUs_()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_22) == 0)
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_mark_yken()
        {
            if (!(eq_s_b("ken")))
            {
                return false;
            }
            if (!r_mark_suffix_with_optional_y_consonant())
                return false;
            return true;
        }

        private bool r_stem_nominal_verb_suffixes()
        {
            ket = cursor;
            B_continue_stemming_noun_suffixes = true;
            {
                int c1 = limit - cursor;
                {
                    int c2 = limit - cursor;
                    if (!r_mark_ymUs_())
                        goto lab3;
                    goto lab2;
                lab3: ;
                    cursor = limit - c2;
                    if (!r_mark_yDU())
                        goto lab4;
                    goto lab2;
                lab4: ;
                    cursor = limit - c2;
                    if (!r_mark_ysA())
                        goto lab5;
                    goto lab2;
                lab5: ;
                    cursor = limit - c2;
                    if (!r_mark_yken())
                        goto lab1;
                }
            lab2: ;
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                if (!r_mark_cAsInA())
                    goto lab6;
                {
                    int c3 = limit - cursor;
                    if (!r_mark_sUnUz())
                        goto lab8;
                    goto lab7;
                lab8: ;
                    cursor = limit - c3;
                    if (!r_mark_lAr())
                        goto lab9;
                    goto lab7;
                lab9: ;
                    cursor = limit - c3;
                    if (!r_mark_yUm())
                        goto lab10;
                    goto lab7;
                lab10: ;
                    cursor = limit - c3;
                    if (!r_mark_sUn())
                        goto lab11;
                    goto lab7;
                lab11: ;
                    cursor = limit - c3;
                    if (!r_mark_yUz())
                        goto lab12;
                    goto lab7;
                lab12: ;
                    cursor = limit - c3;
                }
            lab7: ;
                if (!r_mark_ymUs_())
                    goto lab6;
                goto lab0;
            lab6: ;
                cursor = limit - c1;
                if (!r_mark_lAr())
                    goto lab13;
                bra = cursor;
                slice_del();
                {
                    int c4 = limit - cursor;
                    ket = cursor;
                    {
                        int c5 = limit - cursor;
                        if (!r_mark_DUr())
                            goto lab16;
                        goto lab15;
                    lab16: ;
                        cursor = limit - c5;
                        if (!r_mark_yDU())
                            goto lab17;
                        goto lab15;
                    lab17: ;
                        cursor = limit - c5;
                        if (!r_mark_ysA())
                            goto lab18;
                        goto lab15;
                    lab18: ;
                        cursor = limit - c5;
                        if (!r_mark_ymUs_())
                            {
                                cursor = limit - c4;
                                goto lab14;
                            }
                    }
                lab15: ;
                lab14: ;
                }
                B_continue_stemming_noun_suffixes = false;
                goto lab0;
            lab13: ;
                cursor = limit - c1;
                if (!r_mark_nUz())
                    goto lab19;
                {
                    int c6 = limit - cursor;
                    if (!r_mark_yDU())
                        goto lab21;
                    goto lab20;
                lab21: ;
                    cursor = limit - c6;
                    if (!r_mark_ysA())
                        goto lab19;
                }
            lab20: ;
                goto lab0;
            lab19: ;
                cursor = limit - c1;
                {
                    int c7 = limit - cursor;
                    if (!r_mark_sUnUz())
                        goto lab24;
                    goto lab23;
                lab24: ;
                    cursor = limit - c7;
                    if (!r_mark_yUz())
                        goto lab25;
                    goto lab23;
                lab25: ;
                    cursor = limit - c7;
                    if (!r_mark_sUn())
                        goto lab26;
                    goto lab23;
                lab26: ;
                    cursor = limit - c7;
                    if (!r_mark_yUm())
                        goto lab22;
                }
            lab23: ;
                bra = cursor;
                slice_del();
                {
                    int c8 = limit - cursor;
                    ket = cursor;
                    if (!r_mark_ymUs_())
                        {
                            cursor = limit - c8;
                            goto lab27;
                        }
                lab27: ;
                }
                goto lab0;
            lab22: ;
                cursor = limit - c1;
                if (!r_mark_DUr())
                    return false;
                bra = cursor;
                slice_del();
                {
                    int c9 = limit - cursor;
                    ket = cursor;
                    {
                        int c10 = limit - cursor;
                        if (!r_mark_sUnUz())
                            goto lab30;
                        goto lab29;
                    lab30: ;
                        cursor = limit - c10;
                        if (!r_mark_lAr())
                            goto lab31;
                        goto lab29;
                    lab31: ;
                        cursor = limit - c10;
                        if (!r_mark_yUm())
                            goto lab32;
                        goto lab29;
                    lab32: ;
                        cursor = limit - c10;
                        if (!r_mark_sUn())
                            goto lab33;
                        goto lab29;
                    lab33: ;
                        cursor = limit - c10;
                        if (!r_mark_yUz())
                            goto lab34;
                        goto lab29;
                    lab34: ;
                        cursor = limit - c10;
                    }
                lab29: ;
                    if (!r_mark_ymUs_())
                        {
                            cursor = limit - c9;
                            goto lab28;
                        }
                lab28: ;
                }
            }
        lab0: ;
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_stem_suffix_chain_before_ki()
        {
            ket = cursor;
            if (!r_mark_ki())
                return false;
            {
                int c1 = limit - cursor;
                if (!r_mark_DA())
                    goto lab1;
                bra = cursor;
                slice_del();
                {
                    int c2 = limit - cursor;
                    ket = cursor;
                    {
                        int c3 = limit - cursor;
                        if (!r_mark_lAr())
                            goto lab4;
                        bra = cursor;
                        slice_del();
                        {
                            int c4 = limit - cursor;
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c4;
                                    goto lab5;
                                }
                        lab5: ;
                        }
                        goto lab3;
                    lab4: ;
                        cursor = limit - c3;
                        if (!r_mark_possessives())
                            {
                                cursor = limit - c2;
                                goto lab2;
                            }
                        bra = cursor;
                        slice_del();
                        {
                            int c5 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c5;
                                    goto lab6;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c5;
                                    goto lab6;
                                }
                        lab6: ;
                        }
                    }
                lab3: ;
                lab2: ;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                if (!r_mark_nUn())
                    goto lab7;
                bra = cursor;
                slice_del();
                {
                    int c6 = limit - cursor;
                    ket = cursor;
                    {
                        int c7 = limit - cursor;
                        if (!r_mark_lArI())
                            goto lab10;
                        bra = cursor;
                        slice_del();
                        goto lab9;
                    lab10: ;
                        cursor = limit - c7;
                        ket = cursor;
                        {
                            int c8 = limit - cursor;
                            if (!r_mark_possessives())
                                goto lab13;
                            goto lab12;
                        lab13: ;
                            cursor = limit - c8;
                            if (!r_mark_sU())
                                goto lab11;
                        }
                    lab12: ;
                        bra = cursor;
                        slice_del();
                        {
                            int c9 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c9;
                                    goto lab14;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c9;
                                    goto lab14;
                                }
                        lab14: ;
                        }
                        goto lab9;
                    lab11: ;
                        cursor = limit - c7;
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c6;
                                goto lab8;
                            }
                    }
                lab9: ;
                lab8: ;
                }
                goto lab0;
            lab7: ;
                cursor = limit - c1;
                if (!r_mark_ndA())
                    return false;
                {
                    int c10 = limit - cursor;
                    if (!r_mark_lArI())
                        goto lab16;
                    bra = cursor;
                    slice_del();
                    goto lab15;
                lab16: ;
                    cursor = limit - c10;
                    if (!r_mark_sU())
                        goto lab17;
                    bra = cursor;
                    slice_del();
                    {
                        int c11 = limit - cursor;
                        ket = cursor;
                        if (!r_mark_lAr())
                            {
                                cursor = limit - c11;
                                goto lab18;
                            }
                        bra = cursor;
                        slice_del();
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c11;
                                goto lab18;
                            }
                    lab18: ;
                    }
                    goto lab15;
                lab17: ;
                    cursor = limit - c10;
                    if (!r_stem_suffix_chain_before_ki())
                        return false;
                }
            lab15: ;
            }
        lab0: ;
            return true;
        }

        private bool r_stem_noun_suffixes()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (!r_mark_lAr())
                    goto lab1;
                bra = cursor;
                slice_del();
                {
                    int c2 = limit - cursor;
                    if (!r_stem_suffix_chain_before_ki())
                        {
                            cursor = limit - c2;
                            goto lab2;
                        }
                lab2: ;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                if (!r_mark_ncA())
                    goto lab3;
                bra = cursor;
                slice_del();
                {
                    int c3 = limit - cursor;
                    {
                        int c4 = limit - cursor;
                        ket = cursor;
                        if (!r_mark_lArI())
                            goto lab6;
                        bra = cursor;
                        slice_del();
                        goto lab5;
                    lab6: ;
                        cursor = limit - c4;
                        ket = cursor;
                        {
                            int c5 = limit - cursor;
                            if (!r_mark_possessives())
                                goto lab9;
                            goto lab8;
                        lab9: ;
                            cursor = limit - c5;
                            if (!r_mark_sU())
                                goto lab7;
                        }
                    lab8: ;
                        bra = cursor;
                        slice_del();
                        {
                            int c6 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c6;
                                    goto lab10;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c6;
                                    goto lab10;
                                }
                        lab10: ;
                        }
                        goto lab5;
                    lab7: ;
                        cursor = limit - c4;
                        ket = cursor;
                        if (!r_mark_lAr())
                            {
                                cursor = limit - c3;
                                goto lab4;
                            }
                        bra = cursor;
                        slice_del();
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c3;
                                goto lab4;
                            }
                    }
                lab5: ;
                lab4: ;
                }
                goto lab0;
            lab3: ;
                cursor = limit - c1;
                ket = cursor;
                {
                    int c7 = limit - cursor;
                    if (!r_mark_ndA())
                        goto lab13;
                    goto lab12;
                lab13: ;
                    cursor = limit - c7;
                    if (!r_mark_nA())
                        goto lab11;
                }
            lab12: ;
                {
                    int c8 = limit - cursor;
                    if (!r_mark_lArI())
                        goto lab15;
                    bra = cursor;
                    slice_del();
                    goto lab14;
                lab15: ;
                    cursor = limit - c8;
                    if (!r_mark_sU())
                        goto lab16;
                    bra = cursor;
                    slice_del();
                    {
                        int c9 = limit - cursor;
                        ket = cursor;
                        if (!r_mark_lAr())
                            {
                                cursor = limit - c9;
                                goto lab17;
                            }
                        bra = cursor;
                        slice_del();
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c9;
                                goto lab17;
                            }
                    lab17: ;
                    }
                    goto lab14;
                lab16: ;
                    cursor = limit - c8;
                    if (!r_stem_suffix_chain_before_ki())
                        goto lab11;
                }
            lab14: ;
                goto lab0;
            lab11: ;
                cursor = limit - c1;
                ket = cursor;
                {
                    int c10 = limit - cursor;
                    if (!r_mark_ndAn())
                        goto lab20;
                    goto lab19;
                lab20: ;
                    cursor = limit - c10;
                    if (!r_mark_nU())
                        goto lab18;
                }
            lab19: ;
                {
                    int c11 = limit - cursor;
                    if (!r_mark_sU())
                        goto lab22;
                    bra = cursor;
                    slice_del();
                    {
                        int c12 = limit - cursor;
                        ket = cursor;
                        if (!r_mark_lAr())
                            {
                                cursor = limit - c12;
                                goto lab23;
                            }
                        bra = cursor;
                        slice_del();
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c12;
                                goto lab23;
                            }
                    lab23: ;
                    }
                    goto lab21;
                lab22: ;
                    cursor = limit - c11;
                    if (!r_mark_lArI())
                        goto lab18;
                }
            lab21: ;
                goto lab0;
            lab18: ;
                cursor = limit - c1;
                ket = cursor;
                if (!r_mark_DAn())
                    goto lab24;
                bra = cursor;
                slice_del();
                {
                    int c13 = limit - cursor;
                    ket = cursor;
                    {
                        int c14 = limit - cursor;
                        if (!r_mark_possessives())
                            goto lab27;
                        bra = cursor;
                        slice_del();
                        {
                            int c15 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c15;
                                    goto lab28;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c15;
                                    goto lab28;
                                }
                        lab28: ;
                        }
                        goto lab26;
                    lab27: ;
                        cursor = limit - c14;
                        if (!r_mark_lAr())
                            goto lab29;
                        bra = cursor;
                        slice_del();
                        {
                            int c16 = limit - cursor;
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c16;
                                    goto lab30;
                                }
                        lab30: ;
                        }
                        goto lab26;
                    lab29: ;
                        cursor = limit - c14;
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c13;
                                goto lab25;
                            }
                    }
                lab26: ;
                lab25: ;
                }
                goto lab0;
            lab24: ;
                cursor = limit - c1;
                ket = cursor;
                {
                    int c17 = limit - cursor;
                    if (!r_mark_nUn())
                        goto lab33;
                    goto lab32;
                lab33: ;
                    cursor = limit - c17;
                    if (!r_mark_ylA())
                        goto lab31;
                }
            lab32: ;
                bra = cursor;
                slice_del();
                {
                    int c18 = limit - cursor;
                    {
                        int c19 = limit - cursor;
                        ket = cursor;
                        if (!r_mark_lAr())
                            goto lab36;
                        bra = cursor;
                        slice_del();
                        if (!r_stem_suffix_chain_before_ki())
                            goto lab36;
                        goto lab35;
                    lab36: ;
                        cursor = limit - c19;
                        ket = cursor;
                        {
                            int c20 = limit - cursor;
                            if (!r_mark_possessives())
                                goto lab39;
                            goto lab38;
                        lab39: ;
                            cursor = limit - c20;
                            if (!r_mark_sU())
                                goto lab37;
                        }
                    lab38: ;
                        bra = cursor;
                        slice_del();
                        {
                            int c21 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c21;
                                    goto lab40;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c21;
                                    goto lab40;
                                }
                        lab40: ;
                        }
                        goto lab35;
                    lab37: ;
                        cursor = limit - c19;
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c18;
                                goto lab34;
                            }
                    }
                lab35: ;
                lab34: ;
                }
                goto lab0;
            lab31: ;
                cursor = limit - c1;
                ket = cursor;
                if (!r_mark_lArI())
                    goto lab41;
                bra = cursor;
                slice_del();
                goto lab0;
            lab41: ;
                cursor = limit - c1;
                if (!r_stem_suffix_chain_before_ki())
                    goto lab42;
                goto lab0;
            lab42: ;
                cursor = limit - c1;
                ket = cursor;
                {
                    int c22 = limit - cursor;
                    if (!r_mark_DA())
                        goto lab45;
                    goto lab44;
                lab45: ;
                    cursor = limit - c22;
                    if (!r_mark_yU())
                        goto lab46;
                    goto lab44;
                lab46: ;
                    cursor = limit - c22;
                    if (!r_mark_yA())
                        goto lab43;
                }
            lab44: ;
                bra = cursor;
                slice_del();
                {
                    int c23 = limit - cursor;
                    ket = cursor;
                    {
                        int c24 = limit - cursor;
                        if (!r_mark_possessives())
                            goto lab49;
                        bra = cursor;
                        slice_del();
                        {
                            int c25 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c25;
                                    goto lab50;
                                }
                        lab50: ;
                        }
                        goto lab48;
                    lab49: ;
                        cursor = limit - c24;
                        if (!r_mark_lAr())
                            {
                                cursor = limit - c23;
                                goto lab47;
                            }
                    }
                lab48: ;
                    bra = cursor;
                    slice_del();
                    ket = cursor;
                    if (!r_stem_suffix_chain_before_ki())
                        {
                            cursor = limit - c23;
                            goto lab47;
                        }
                lab47: ;
                }
                goto lab0;
            lab43: ;
                cursor = limit - c1;
                ket = cursor;
                {
                    int c26 = limit - cursor;
                    if (!r_mark_possessives())
                        goto lab52;
                    goto lab51;
                lab52: ;
                    cursor = limit - c26;
                    if (!r_mark_sU())
                        return false;
                }
            lab51: ;
                bra = cursor;
                slice_del();
                {
                    int c27 = limit - cursor;
                    ket = cursor;
                    if (!r_mark_lAr())
                        {
                            cursor = limit - c27;
                            goto lab53;
                        }
                    bra = cursor;
                    slice_del();
                    if (!r_stem_suffix_chain_before_ki())
                        {
                            cursor = limit - c27;
                            goto lab53;
                        }
                lab53: ;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_post_process_last_consonants()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_23);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_from("p");
                    break;
                }
                case 2: {
                    slice_from("ç");
                    break;
                }
                case 3: {
                    slice_from("t");
                    break;
                }
                case 4: {
                    slice_from("k");
                    break;
                }
            }
            return true;
        }

        private bool r_append_U_to_stems_ending_with_d_or_g()
        {
            ket = cursor;
            bra = cursor;
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("d")))
                {
                    goto lab1;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                if (!(eq_s_b("g")))
                {
                    return false;
                }
            }
        lab0: ;
            if (out_grouping_b(g_vowel, 97, 305, true) < 0)
            {
                return false;
            }

            {
                int c2 = limit - cursor;
                {
                    int c3 = limit - cursor;
                    if (!(eq_s_b("a")))
                    {
                        goto lab5;
                    }
                    goto lab4;
                lab5: ;
                    cursor = limit - c3;
                    if (!(eq_s_b("ı")))
                    {
                        goto lab3;
                    }
                }
            lab4: ;
                slice_from("ı");
                goto lab2;
            lab3: ;
                cursor = limit - c2;
                {
                    int c4 = limit - cursor;
                    if (!(eq_s_b("e")))
                    {
                        goto lab8;
                    }
                    goto lab7;
                lab8: ;
                    cursor = limit - c4;
                    if (!(eq_s_b("i")))
                    {
                        goto lab6;
                    }
                }
            lab7: ;
                slice_from("i");
                goto lab2;
            lab6: ;
                cursor = limit - c2;
                {
                    int c5 = limit - cursor;
                    if (!(eq_s_b("o")))
                    {
                        goto lab11;
                    }
                    goto lab10;
                lab11: ;
                    cursor = limit - c5;
                    if (!(eq_s_b("u")))
                    {
                        goto lab9;
                    }
                }
            lab10: ;
                slice_from("u");
                goto lab2;
            lab9: ;
                cursor = limit - c2;
                {
                    int c6 = limit - cursor;
                    if (!(eq_s_b("ö")))
                    {
                        goto lab13;
                    }
                    goto lab12;
                lab13: ;
                    cursor = limit - c6;
                    if (!(eq_s_b("ü")))
                    {
                        return false;
                    }
                }
            lab12: ;
                slice_from("ü");
            }
        lab2: ;
            return true;
        }

        private bool r_is_reserved_word()
        {
            if (!(eq_s_b("ad")))
            {
                return false;
            }
            {
                int c1 = limit - cursor;
                if (!(eq_s_b("soy")))
                {
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                }
            lab0: ;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            return true;
        }

        private bool r_remove_proper_noun_suffix()
        {
            {
                int c1 = cursor;
                bra = cursor;
                while (true)
                {
                    int c2 = cursor;
                    {
                        int c3 = cursor;
                        if (!(eq_s("'")))
                        {
                            goto lab2;
                        }
                        goto lab1;
                    lab2: ;
                        cursor = c3;
                    }
                    cursor = c2;
                    break;
                lab1: ;
                    cursor = c2;
                    if (cursor >= limit)
                    {
                        goto lab0;
                    }
                    cursor++;
                }
                ket = cursor;
                slice_del();
            lab0: ;
                cursor = c1;
            }
            {
                int c4 = cursor;
                {
                    int c = cursor + 2;
                    if (c > limit)
                    {
                        goto lab3;
                    }
                    cursor = c;
                }
                while (true)
                {
                    int c5 = cursor;
                    if (!(eq_s("'")))
                    {
                        goto lab4;
                    }
                    cursor = c5;
                    break;
                lab4: ;
                    cursor = c5;
                    if (cursor >= limit)
                    {
                        goto lab3;
                    }
                    cursor++;
                }
                bra = cursor;
                cursor = limit;
                ket = cursor;
                slice_del();
            lab3: ;
                cursor = c4;
            }
            return true;
        }

        private bool r_more_than_one_syllable_word()
        {
            {
                int c1 = cursor;
                for (int c2 = 2; c2 > 0; c2--)
                {
                    {

                        int ret = out_grouping(g_vowel, 97, 305, true);
                        if (ret < 0)
                        {
                            return false;
                        }

                        cursor += ret;
                    }
                }
                cursor = c1;
            }
            return true;
        }

        private bool r_postlude()
        {
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                if (!r_is_reserved_word())
                    goto lab0;
                return false;
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c2 = limit - cursor;
                r_append_U_to_stems_ending_with_d_or_g();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_post_process_last_consonants();
                cursor = limit - c3;
            }
            cursor = limit_backward;
            return true;
        }

        protected override bool stem()
        {
            r_remove_proper_noun_suffix();
            if (!r_more_than_one_syllable_word())
                return false;
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                r_stem_nominal_verb_suffixes();
                cursor = limit - c1;
            }
            if (!(B_continue_stemming_noun_suffixes))
            {
                return false;
            }
            {
                int c2 = limit - cursor;
                r_stem_noun_suffixes();
                cursor = limit - c2;
            }
            cursor = limit_backward;
            if (!r_postlude())
                return false;
            return true;
        }

    }
}

