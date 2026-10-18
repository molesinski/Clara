// Generated from turkish.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from turkish.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class TurkishStemmer : Stemmer
    {
        private const string g_vowel = "aeiouöüı";
        private const string g_U = "iuüı";
        private const string g_vowel1 = "aouı";
        private const string g_vowel2 = "eiöü";
        private const string g_vowel3 = "aı";
        private const string g_vowel4 = "ei";
        private const string g_vowel5 = "ou";
        private const string g_vowel6 = "öü";

        private static readonly Among[] a_0 = new[]
        {
            new Among("m", -1, -1, 0),
            new Among("n", -1, -1, 0),
            new Among("miz", -1, -1, 0),
            new Among("niz", -1, -1, 0),
            new Among("muz", -1, -1, 0),
            new Among("nuz", -1, -1, 0),
            new Among("müz", -1, -1, 0),
            new Among("nüz", -1, -1, 0),
            new Among("mız", -1, -1, 0),
            new Among("nız", -1, -1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("leri", -1, -1, 0),
            new Among("ları", -1, -1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ni", -1, -1, 0),
            new Among("nu", -1, -1, 0),
            new Among("nü", -1, -1, 0),
            new Among("nı", -1, -1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("in", -1, -1, 0),
            new Among("un", -1, -1, 0),
            new Among("ün", -1, -1, 0),
            new Among("ın", -1, -1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("a", -1, -1, 0),
            new Among("e", -1, -1, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("na", -1, -1, 0),
            new Among("ne", -1, -1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("da", -1, -1, 0),
            new Among("ta", -1, -1, 0),
            new Among("de", -1, -1, 0),
            new Among("te", -1, -1, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("nda", -1, -1, 0),
            new Among("nde", -1, -1, 0)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("dan", -1, -1, 0),
            new Among("tan", -1, -1, 0),
            new Among("den", -1, -1, 0),
            new Among("ten", -1, -1, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("ndan", -1, -1, 0),
            new Among("nden", -1, -1, 0)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("la", -1, -1, 0),
            new Among("le", -1, -1, 0)
        };

        private static readonly Among[] a_11 = new[]
        {
            new Among("ca", -1, -1, 0),
            new Among("ce", -1, -1, 0)
        };

        private static readonly Among[] a_12 = new[]
        {
            new Among("im", -1, -1, 0),
            new Among("um", -1, -1, 0),
            new Among("üm", -1, -1, 0),
            new Among("ım", -1, -1, 0)
        };

        private static readonly Among[] a_13 = new[]
        {
            new Among("sin", -1, -1, 0),
            new Among("sun", -1, -1, 0),
            new Among("sün", -1, -1, 0),
            new Among("sın", -1, -1, 0)
        };

        private static readonly Among[] a_14 = new[]
        {
            new Among("iz", -1, -1, 0),
            new Among("uz", -1, -1, 0),
            new Among("üz", -1, -1, 0),
            new Among("ız", -1, -1, 0)
        };

        private static readonly Among[] a_15 = new[]
        {
            new Among("siniz", -1, -1, 0),
            new Among("sunuz", -1, -1, 0),
            new Among("sünüz", -1, -1, 0),
            new Among("sınız", -1, -1, 0)
        };

        private static readonly Among[] a_16 = new[]
        {
            new Among("lar", -1, -1, 0),
            new Among("ler", -1, -1, 0)
        };

        private static readonly Among[] a_17 = new[]
        {
            new Among("niz", -1, -1, 0),
            new Among("nuz", -1, -1, 0),
            new Among("nüz", -1, -1, 0),
            new Among("nız", -1, -1, 0)
        };

        private static readonly Among[] a_18 = new[]
        {
            new Among("dir", -1, -1, 0),
            new Among("tir", -1, -1, 0),
            new Among("dur", -1, -1, 0),
            new Among("tur", -1, -1, 0),
            new Among("dür", -1, -1, 0),
            new Among("tür", -1, -1, 0),
            new Among("dır", -1, -1, 0),
            new Among("tır", -1, -1, 0)
        };

        private static readonly Among[] a_19 = new[]
        {
            new Among("casına", -1, -1, 0),
            new Among("cesine", -1, -1, 0)
        };

        private static readonly Among[] a_20 = new[]
        {
            new Among("di", -1, -1, 0),
            new Among("ti", -1, -1, 0),
            new Among("dik", -1, -1, 0),
            new Among("tik", -1, -1, 0),
            new Among("duk", -1, -1, 0),
            new Among("tuk", -1, -1, 0),
            new Among("dük", -1, -1, 0),
            new Among("tük", -1, -1, 0),
            new Among("dık", -1, -1, 0),
            new Among("tık", -1, -1, 0),
            new Among("dim", -1, -1, 0),
            new Among("tim", -1, -1, 0),
            new Among("dum", -1, -1, 0),
            new Among("tum", -1, -1, 0),
            new Among("düm", -1, -1, 0),
            new Among("tüm", -1, -1, 0),
            new Among("dım", -1, -1, 0),
            new Among("tım", -1, -1, 0),
            new Among("din", -1, -1, 0),
            new Among("tin", -1, -1, 0),
            new Among("dun", -1, -1, 0),
            new Among("tun", -1, -1, 0),
            new Among("dün", -1, -1, 0),
            new Among("tün", -1, -1, 0),
            new Among("dın", -1, -1, 0),
            new Among("tın", -1, -1, 0),
            new Among("du", -1, -1, 0),
            new Among("tu", -1, -1, 0),
            new Among("dü", -1, -1, 0),
            new Among("tü", -1, -1, 0),
            new Among("dı", -1, -1, 0),
            new Among("tı", -1, -1, 0)
        };

        private static readonly Among[] a_21 = new[]
        {
            new Among("sa", -1, -1, 0),
            new Among("se", -1, -1, 0),
            new Among("sak", -1, -1, 0),
            new Among("sek", -1, -1, 0),
            new Among("sam", -1, -1, 0),
            new Among("sem", -1, -1, 0),
            new Among("san", -1, -1, 0),
            new Among("sen", -1, -1, 0)
        };

        private static readonly Among[] a_22 = new[]
        {
            new Among("miş", -1, -1, 0),
            new Among("muş", -1, -1, 0),
            new Among("müş", -1, -1, 0),
            new Among("mış", -1, -1, 0)
        };

        private static readonly Among[] a_23 = new[]
        {
            new Among("b", -1, 1, 0),
            new Among("c", -1, 2, 0),
            new Among("d", -1, 3, 0),
            new Among("ğ", -1, 4, 0)
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
                if (!(eq_s_b("n")))
                {
                    goto lab2;
                }
                return false;
            lab2: ;
                {
                    int c3 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c3;
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
                if (!(eq_s_b("y")))
                {
                    goto lab2;
                }
                return false;
            lab2: ;
                {
                    int c3 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c3;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_mark_possessives()
        {
            if (find_among_b(a_0, null) == 0)
            {
                return false;
            }
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
                if (in_grouping_b(g_U, 105, 305, false) != 0)
                {
                    goto lab2;
                }
                return false;
            lab2: ;
                {
                    int c3 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (out_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c3;
                }
            }
        lab0: ;
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
                if (!(eq_s_b("s")))
                {
                    goto lab2;
                }
                return false;
            lab2: ;
                {
                    int c3 = limit - cursor;
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    if (in_grouping_b(g_vowel, 97, 305, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c3;
                }
            }
        lab0: ;
            return true;
        }

        private bool r_mark_lArI()
        {
            return find_among_b(a_1, null) != 0;
        }

        private bool r_mark_nUn()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_3, null) == 0)
            {
                return false;
            }
            return r_mark_suffix_with_optional_n_consonant();
        }

        private bool r_mark_DA()
        {
            if (!r_check_vowel_harmony())
                return false;
            return find_among_b(a_6, null) != 0;
        }

        private bool r_mark_ndA()
        {
            if (!r_check_vowel_harmony())
                return false;
            return find_among_b(a_7, null) != 0;
        }

        private bool r_mark_yUm()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_12, null) == 0)
            {
                return false;
            }
            return r_mark_suffix_with_optional_y_consonant();
        }

        private bool r_mark_sUn()
        {
            if (!r_check_vowel_harmony())
                return false;
            return find_among_b(a_13, null) != 0;
        }

        private bool r_mark_yUz()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_14, null) == 0)
            {
                return false;
            }
            return r_mark_suffix_with_optional_y_consonant();
        }

        private bool r_mark_sUnUz()
        {
            return find_among_b(a_15, null) != 0;
        }

        private bool r_mark_lAr()
        {
            if (!r_check_vowel_harmony())
                return false;
            return find_among_b(a_16, null) != 0;
        }

        private bool r_mark_DUr()
        {
            if (!r_check_vowel_harmony())
                return false;
            return find_among_b(a_18, null) != 0;
        }

        private bool r_mark_yDU()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_20, null) == 0)
            {
                return false;
            }
            return r_mark_suffix_with_optional_y_consonant();
        }

        private bool r_mark_ysA()
        {
            if (find_among_b(a_21, null) == 0)
            {
                return false;
            }
            return r_mark_suffix_with_optional_y_consonant();
        }

        private bool r_mark_ymUs_()
        {
            if (!r_check_vowel_harmony())
                return false;
            if (find_among_b(a_22, null) == 0)
            {
                return false;
            }
            return r_mark_suffix_with_optional_y_consonant();
        }

        private bool r_stem_suffix_chain_before_ki()
        {
            ket = cursor;
            if (!(eq_s_b("ki")))
            {
                return false;
            }
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

        protected override bool stem()
        {
            int among_var;
            bool B_continue_stemming_noun_suffixes;
            {
                int c1 = cursor;
                bra = cursor;
                while (true)
                {
                    int c2 = cursor;
                    if (!(eq_s("'")))
                    {
                        goto lab3;
                    }
                    goto lab2;
                lab3: ;
                    cursor = c2;
                    break;
                lab2: ;
                    cursor = c2;
                    if (cursor >= limit)
                    {
                        goto lab1;
                    }
                    cursor++;
                }
                ket = cursor;
                slice_del();
            lab1: ;
                cursor = c1;
            }
            {
                int c3 = cursor;
                {
                    int c = cursor + 2;
                    if (c > limit)
                    {
                        goto lab4;
                    }
                    cursor = c;
                }
                while (true)
                {
                    int c4 = cursor;
                    if (!(eq_s("'")))
                    {
                        goto lab5;
                    }
                    cursor = c4;
                    break;
                lab5: ;
                    cursor = c4;
                    if (cursor >= limit)
                    {
                        goto lab4;
                    }
                    cursor++;
                }
                bra = cursor;
                cursor = limit;
                ket = cursor;
                slice_del();
            lab4: ;
                cursor = c3;
            }
            {
                int c5 = cursor;
                for (int c6 = 2; c6 > 0; c6--)
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
                cursor = c5;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c7 = limit - cursor;
                ket = cursor;
                B_continue_stemming_noun_suffixes = true;
                {
                    int c8 = limit - cursor;
                    {
                        int c9 = limit - cursor;
                        if (!r_mark_ymUs_())
                            goto lab10;
                        goto lab9;
                    lab10: ;
                        cursor = limit - c9;
                        if (!r_mark_yDU())
                            goto lab11;
                        goto lab9;
                    lab11: ;
                        cursor = limit - c9;
                        if (!r_mark_ysA())
                            goto lab12;
                        goto lab9;
                    lab12: ;
                        cursor = limit - c9;
                        if (!(eq_s_b("ken")))
                        {
                            goto lab8;
                        }
                        if (!r_mark_suffix_with_optional_y_consonant())
                            goto lab8;
                    }
                lab9: ;
                    goto lab7;
                lab8: ;
                    cursor = limit - c8;
                    if (find_among_b(a_19, null) == 0)
                    {
                        goto lab13;
                    }
                    {
                        int c10 = limit - cursor;
                        if (!r_mark_sUnUz())
                            goto lab15;
                        goto lab14;
                    lab15: ;
                        cursor = limit - c10;
                        if (!r_mark_lAr())
                            goto lab16;
                        goto lab14;
                    lab16: ;
                        cursor = limit - c10;
                        if (!r_mark_yUm())
                            goto lab17;
                        goto lab14;
                    lab17: ;
                        cursor = limit - c10;
                        if (!r_mark_sUn())
                            goto lab18;
                        goto lab14;
                    lab18: ;
                        cursor = limit - c10;
                        if (!r_mark_yUz())
                            goto lab19;
                        goto lab14;
                    lab19: ;
                        cursor = limit - c10;
                    }
                lab14: ;
                    if (!r_mark_ymUs_())
                        goto lab13;
                    goto lab7;
                lab13: ;
                    cursor = limit - c8;
                    if (!r_mark_lAr())
                        goto lab20;
                    bra = cursor;
                    slice_del();
                    {
                        int c11 = limit - cursor;
                        ket = cursor;
                        {
                            int c12 = limit - cursor;
                            if (!r_mark_DUr())
                                goto lab23;
                            goto lab22;
                        lab23: ;
                            cursor = limit - c12;
                            if (!r_mark_yDU())
                                goto lab24;
                            goto lab22;
                        lab24: ;
                            cursor = limit - c12;
                            if (!r_mark_ysA())
                                goto lab25;
                            goto lab22;
                        lab25: ;
                            cursor = limit - c12;
                            if (!r_mark_ymUs_())
                                {
                                    cursor = limit - c11;
                                    goto lab21;
                                }
                        }
                    lab22: ;
                    lab21: ;
                    }
                    B_continue_stemming_noun_suffixes = false;
                    goto lab7;
                lab20: ;
                    cursor = limit - c8;
                    if (!r_check_vowel_harmony())
                        goto lab26;
                    if (find_among_b(a_17, null) == 0)
                    {
                        goto lab26;
                    }
                    {
                        int c13 = limit - cursor;
                        if (!r_mark_yDU())
                            goto lab28;
                        goto lab27;
                    lab28: ;
                        cursor = limit - c13;
                        if (!r_mark_ysA())
                            goto lab26;
                    }
                lab27: ;
                    goto lab7;
                lab26: ;
                    cursor = limit - c8;
                    {
                        int c14 = limit - cursor;
                        if (!r_mark_sUnUz())
                            goto lab31;
                        goto lab30;
                    lab31: ;
                        cursor = limit - c14;
                        if (!r_mark_yUz())
                            goto lab32;
                        goto lab30;
                    lab32: ;
                        cursor = limit - c14;
                        if (!r_mark_sUn())
                            goto lab33;
                        goto lab30;
                    lab33: ;
                        cursor = limit - c14;
                        if (!r_mark_yUm())
                            goto lab29;
                    }
                lab30: ;
                    bra = cursor;
                    slice_del();
                    {
                        int c15 = limit - cursor;
                        ket = cursor;
                        if (!r_mark_ymUs_())
                            {
                                cursor = limit - c15;
                                goto lab34;
                            }
                    lab34: ;
                    }
                    goto lab7;
                lab29: ;
                    cursor = limit - c8;
                    if (!r_mark_DUr())
                        goto lab6;
                    bra = cursor;
                    slice_del();
                    {
                        int c16 = limit - cursor;
                        ket = cursor;
                        {
                            int c17 = limit - cursor;
                            if (!r_mark_sUnUz())
                                goto lab37;
                            goto lab36;
                        lab37: ;
                            cursor = limit - c17;
                            if (!r_mark_lAr())
                                goto lab38;
                            goto lab36;
                        lab38: ;
                            cursor = limit - c17;
                            if (!r_mark_yUm())
                                goto lab39;
                            goto lab36;
                        lab39: ;
                            cursor = limit - c17;
                            if (!r_mark_sUn())
                                goto lab40;
                            goto lab36;
                        lab40: ;
                            cursor = limit - c17;
                            if (!r_mark_yUz())
                                goto lab41;
                            goto lab36;
                        lab41: ;
                            cursor = limit - c17;
                        }
                    lab36: ;
                        if (!r_mark_ymUs_())
                            {
                                cursor = limit - c16;
                                goto lab35;
                            }
                    lab35: ;
                    }
                }
            lab7: ;
                bra = cursor;
                slice_del();
            lab6: ;
                cursor = limit - c7;
            }
            if (!B_continue_stemming_noun_suffixes)
            {
                return false;
            }
            {
                int c18 = limit - cursor;
                {
                    int c19 = limit - cursor;
                    ket = cursor;
                    if (!r_mark_lAr())
                        goto lab44;
                    bra = cursor;
                    slice_del();
                    {
                        int c20 = limit - cursor;
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c20;
                                goto lab45;
                            }
                    lab45: ;
                    }
                    goto lab43;
                lab44: ;
                    cursor = limit - c19;
                    ket = cursor;
                    if (!r_check_vowel_harmony())
                        goto lab46;
                    if (find_among_b(a_11, null) == 0)
                    {
                        goto lab46;
                    }
                    if (!r_mark_suffix_with_optional_n_consonant())
                        goto lab46;
                    bra = cursor;
                    slice_del();
                    {
                        int c21 = limit - cursor;
                        {
                            int c22 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lArI())
                                goto lab49;
                            bra = cursor;
                            slice_del();
                            goto lab48;
                        lab49: ;
                            cursor = limit - c22;
                            ket = cursor;
                            {
                                int c23 = limit - cursor;
                                if (!r_mark_possessives())
                                    goto lab52;
                                goto lab51;
                            lab52: ;
                                cursor = limit - c23;
                                if (!r_mark_sU())
                                    goto lab50;
                            }
                        lab51: ;
                            bra = cursor;
                            slice_del();
                            {
                                int c24 = limit - cursor;
                                ket = cursor;
                                if (!r_mark_lAr())
                                    {
                                        cursor = limit - c24;
                                        goto lab53;
                                    }
                                bra = cursor;
                                slice_del();
                                if (!r_stem_suffix_chain_before_ki())
                                    {
                                        cursor = limit - c24;
                                        goto lab53;
                                    }
                            lab53: ;
                            }
                            goto lab48;
                        lab50: ;
                            cursor = limit - c22;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c21;
                                    goto lab47;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c21;
                                    goto lab47;
                                }
                        }
                    lab48: ;
                    lab47: ;
                    }
                    goto lab43;
                lab46: ;
                    cursor = limit - c19;
                    ket = cursor;
                    {
                        int c25 = limit - cursor;
                        if (!r_mark_ndA())
                            goto lab56;
                        goto lab55;
                    lab56: ;
                        cursor = limit - c25;
                        if (!r_check_vowel_harmony())
                            goto lab54;
                        if (find_among_b(a_5, null) == 0)
                        {
                            goto lab54;
                        }
                    }
                lab55: ;
                    {
                        int c26 = limit - cursor;
                        if (!r_mark_lArI())
                            goto lab58;
                        bra = cursor;
                        slice_del();
                        goto lab57;
                    lab58: ;
                        cursor = limit - c26;
                        if (!r_mark_sU())
                            goto lab59;
                        bra = cursor;
                        slice_del();
                        {
                            int c27 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c27;
                                    goto lab60;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c27;
                                    goto lab60;
                                }
                        lab60: ;
                        }
                        goto lab57;
                    lab59: ;
                        cursor = limit - c26;
                        if (!r_stem_suffix_chain_before_ki())
                            goto lab54;
                    }
                lab57: ;
                    goto lab43;
                lab54: ;
                    cursor = limit - c19;
                    ket = cursor;
                    {
                        int c28 = limit - cursor;
                        if (!r_check_vowel_harmony())
                            goto lab63;
                        if (find_among_b(a_9, null) == 0)
                        {
                            goto lab63;
                        }
                        goto lab62;
                    lab63: ;
                        cursor = limit - c28;
                        if (!r_check_vowel_harmony())
                            goto lab61;
                        if (find_among_b(a_2, null) == 0)
                        {
                            goto lab61;
                        }
                    }
                lab62: ;
                    {
                        int c29 = limit - cursor;
                        if (!r_mark_sU())
                            goto lab65;
                        bra = cursor;
                        slice_del();
                        {
                            int c30 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c30;
                                    goto lab66;
                                }
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c30;
                                    goto lab66;
                                }
                        lab66: ;
                        }
                        goto lab64;
                    lab65: ;
                        cursor = limit - c29;
                        if (!r_mark_lArI())
                            goto lab61;
                    }
                lab64: ;
                    goto lab43;
                lab61: ;
                    cursor = limit - c19;
                    ket = cursor;
                    if (!r_check_vowel_harmony())
                        goto lab67;
                    if (find_among_b(a_8, null) == 0)
                    {
                        goto lab67;
                    }
                    bra = cursor;
                    slice_del();
                    {
                        int c31 = limit - cursor;
                        ket = cursor;
                        {
                            int c32 = limit - cursor;
                            if (!r_mark_possessives())
                                goto lab70;
                            bra = cursor;
                            slice_del();
                            {
                                int c33 = limit - cursor;
                                ket = cursor;
                                if (!r_mark_lAr())
                                    {
                                        cursor = limit - c33;
                                        goto lab71;
                                    }
                                bra = cursor;
                                slice_del();
                                if (!r_stem_suffix_chain_before_ki())
                                    {
                                        cursor = limit - c33;
                                        goto lab71;
                                    }
                            lab71: ;
                            }
                            goto lab69;
                        lab70: ;
                            cursor = limit - c32;
                            if (!r_mark_lAr())
                                goto lab72;
                            bra = cursor;
                            slice_del();
                            {
                                int c34 = limit - cursor;
                                if (!r_stem_suffix_chain_before_ki())
                                    {
                                        cursor = limit - c34;
                                        goto lab73;
                                    }
                            lab73: ;
                            }
                            goto lab69;
                        lab72: ;
                            cursor = limit - c32;
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c31;
                                    goto lab68;
                                }
                        }
                    lab69: ;
                    lab68: ;
                    }
                    goto lab43;
                lab67: ;
                    cursor = limit - c19;
                    ket = cursor;
                    {
                        int c35 = limit - cursor;
                        if (!r_mark_nUn())
                            goto lab76;
                        goto lab75;
                    lab76: ;
                        cursor = limit - c35;
                        if (!r_check_vowel_harmony())
                            goto lab74;
                        if (find_among_b(a_10, null) == 0)
                        {
                            goto lab74;
                        }
                        if (!r_mark_suffix_with_optional_y_consonant())
                            goto lab74;
                    }
                lab75: ;
                    bra = cursor;
                    slice_del();
                    {
                        int c36 = limit - cursor;
                        {
                            int c37 = limit - cursor;
                            ket = cursor;
                            if (!r_mark_lAr())
                                goto lab79;
                            bra = cursor;
                            slice_del();
                            if (!r_stem_suffix_chain_before_ki())
                                goto lab79;
                            goto lab78;
                        lab79: ;
                            cursor = limit - c37;
                            ket = cursor;
                            {
                                int c38 = limit - cursor;
                                if (!r_mark_possessives())
                                    goto lab82;
                                goto lab81;
                            lab82: ;
                                cursor = limit - c38;
                                if (!r_mark_sU())
                                    goto lab80;
                            }
                        lab81: ;
                            bra = cursor;
                            slice_del();
                            {
                                int c39 = limit - cursor;
                                ket = cursor;
                                if (!r_mark_lAr())
                                    {
                                        cursor = limit - c39;
                                        goto lab83;
                                    }
                                bra = cursor;
                                slice_del();
                                if (!r_stem_suffix_chain_before_ki())
                                    {
                                        cursor = limit - c39;
                                        goto lab83;
                                    }
                            lab83: ;
                            }
                            goto lab78;
                        lab80: ;
                            cursor = limit - c37;
                            if (!r_stem_suffix_chain_before_ki())
                                {
                                    cursor = limit - c36;
                                    goto lab77;
                                }
                        }
                    lab78: ;
                    lab77: ;
                    }
                    goto lab43;
                lab74: ;
                    cursor = limit - c19;
                    ket = cursor;
                    if (!r_mark_lArI())
                        goto lab84;
                    bra = cursor;
                    slice_del();
                    goto lab43;
                lab84: ;
                    cursor = limit - c19;
                    if (!r_stem_suffix_chain_before_ki())
                        goto lab85;
                    goto lab43;
                lab85: ;
                    cursor = limit - c19;
                    ket = cursor;
                    {
                        int c40 = limit - cursor;
                        if (!r_mark_DA())
                            goto lab88;
                        goto lab87;
                    lab88: ;
                        cursor = limit - c40;
                        if (!r_check_vowel_harmony())
                            goto lab89;
                        if (in_grouping_b(g_U, 105, 305, false) != 0)
                        {
                            goto lab89;
                        }
                        if (!r_mark_suffix_with_optional_y_consonant())
                            goto lab89;
                        goto lab87;
                    lab89: ;
                        cursor = limit - c40;
                        if (!r_check_vowel_harmony())
                            goto lab86;
                        if (find_among_b(a_4, null) == 0)
                        {
                            goto lab86;
                        }
                        if (!r_mark_suffix_with_optional_y_consonant())
                            goto lab86;
                    }
                lab87: ;
                    bra = cursor;
                    slice_del();
                    {
                        int c41 = limit - cursor;
                        ket = cursor;
                        {
                            int c42 = limit - cursor;
                            if (!r_mark_possessives())
                                goto lab92;
                            bra = cursor;
                            slice_del();
                            {
                                int c43 = limit - cursor;
                                ket = cursor;
                                if (!r_mark_lAr())
                                    {
                                        cursor = limit - c43;
                                        goto lab93;
                                    }
                            lab93: ;
                            }
                            goto lab91;
                        lab92: ;
                            cursor = limit - c42;
                            if (!r_mark_lAr())
                                {
                                    cursor = limit - c41;
                                    goto lab90;
                                }
                        }
                    lab91: ;
                        bra = cursor;
                        slice_del();
                        ket = cursor;
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c41;
                                goto lab90;
                            }
                    lab90: ;
                    }
                    goto lab43;
                lab86: ;
                    cursor = limit - c19;
                    ket = cursor;
                    {
                        int c44 = limit - cursor;
                        if (!r_mark_possessives())
                            goto lab95;
                        goto lab94;
                    lab95: ;
                        cursor = limit - c44;
                        if (!r_mark_sU())
                            goto lab42;
                    }
                lab94: ;
                    bra = cursor;
                    slice_del();
                    {
                        int c45 = limit - cursor;
                        ket = cursor;
                        if (!r_mark_lAr())
                            {
                                cursor = limit - c45;
                                goto lab96;
                            }
                        bra = cursor;
                        slice_del();
                        if (!r_stem_suffix_chain_before_ki())
                            {
                                cursor = limit - c45;
                                goto lab96;
                            }
                    lab96: ;
                    }
                }
            lab43: ;
            lab42: ;
                cursor = limit - c18;
            }
            cursor = limit_backward;
            limit_backward = cursor;
            cursor = limit;
            {
                int c46 = limit - cursor;
                if (!(eq_s_b("ad")))
                {
                    goto lab97;
                }
                {
                    int c47 = limit - cursor;
                    if (!(eq_s_b("soy")))
                    {
                        {
                            cursor = limit - c47;
                            goto lab98;
                        }
                    }
                lab98: ;
                }
                if (cursor > limit_backward)
                {
                    goto lab97;
                }
                return false;
            lab97: ;
                cursor = limit - c46;
            }
            {
                int c48 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (!(eq_s_b("d")))
                {
                    goto lab101;
                }
                goto lab100;
            lab101: ;
                if (!(eq_s_b("g")))
                {
                    goto lab99;
                }
            lab100: ;
                if (out_grouping_b(g_vowel, 97, 305, true) < 0)
                {
                    goto lab99;
                }

                {
                    int c49 = limit - cursor;
                    if (!(eq_s_b("a")))
                    {
                        goto lab105;
                    }
                    goto lab104;
                lab105: ;
                    if (!(eq_s_b("ı")))
                    {
                        goto lab103;
                    }
                lab104: ;
                    slice_from("ı");
                    goto lab102;
                lab103: ;
                    cursor = limit - c49;
                    if (!(eq_s_b("e")))
                    {
                        goto lab108;
                    }
                    goto lab107;
                lab108: ;
                    if (!(eq_s_b("i")))
                    {
                        goto lab106;
                    }
                lab107: ;
                    slice_from("i");
                    goto lab102;
                lab106: ;
                    cursor = limit - c49;
                    if (!(eq_s_b("o")))
                    {
                        goto lab111;
                    }
                    goto lab110;
                lab111: ;
                    if (!(eq_s_b("u")))
                    {
                        goto lab109;
                    }
                lab110: ;
                    slice_from("u");
                    goto lab102;
                lab109: ;
                    cursor = limit - c49;
                    if (!(eq_s_b("ö")))
                    {
                        goto lab113;
                    }
                    goto lab112;
                lab113: ;
                    if (!(eq_s_b("ü")))
                    {
                        goto lab99;
                    }
                lab112: ;
                    slice_from("ü");
                }
            lab102: ;
            lab99: ;
                cursor = limit - c48;
            }
            {
                int c50 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_23, null);
                if (among_var == 0)
                {
                    goto lab114;
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
            lab114: ;
                cursor = limit - c50;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

