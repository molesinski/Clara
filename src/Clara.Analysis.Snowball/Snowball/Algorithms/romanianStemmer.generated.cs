﻿// Generated from romanian.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from romanian.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class RomanianStemmer : Stemmer
    {
        private bool B_standard_suffix_removed;
        private int I_p2;
        private int I_p1;
        private int I_pV;

        private const string g_v = "aeiouâîă";

        private static readonly Among[] a_0 = new[]
        {
            new Among("ş", -1, 1),
            new Among("ţ", -1, 2)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 3),
            new Among("I", 0, 1),
            new Among("U", 0, 2)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ea", -1, 3),
            new Among("ația", -1, 7),
            new Among("aua", -1, 2),
            new Among("iua", -1, 4),
            new Among("ație", -1, 7),
            new Among("ele", -1, 3),
            new Among("ile", -1, 5),
            new Among("iile", 6, 4),
            new Among("iei", -1, 4),
            new Among("atei", -1, 6),
            new Among("ii", -1, 4),
            new Among("ului", -1, 1),
            new Among("ul", -1, 1),
            new Among("elor", -1, 3),
            new Among("ilor", -1, 4),
            new Among("iilor", 14, 4)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("icala", -1, 4),
            new Among("iciva", -1, 4),
            new Among("ativa", -1, 5),
            new Among("itiva", -1, 6),
            new Among("icale", -1, 4),
            new Among("ațiune", -1, 5),
            new Among("ițiune", -1, 6),
            new Among("atoare", -1, 5),
            new Among("itoare", -1, 6),
            new Among("ătoare", -1, 5),
            new Among("icitate", -1, 4),
            new Among("abilitate", -1, 1),
            new Among("ibilitate", -1, 2),
            new Among("ivitate", -1, 3),
            new Among("icive", -1, 4),
            new Among("ative", -1, 5),
            new Among("itive", -1, 6),
            new Among("icali", -1, 4),
            new Among("atori", -1, 5),
            new Among("icatori", 18, 4),
            new Among("itori", -1, 6),
            new Among("ători", -1, 5),
            new Among("icitati", -1, 4),
            new Among("abilitati", -1, 1),
            new Among("ivitati", -1, 3),
            new Among("icivi", -1, 4),
            new Among("ativi", -1, 5),
            new Among("itivi", -1, 6),
            new Among("icităi", -1, 4),
            new Among("abilităi", -1, 1),
            new Among("ivităi", -1, 3),
            new Among("icități", -1, 4),
            new Among("abilități", -1, 1),
            new Among("ivități", -1, 3),
            new Among("ical", -1, 4),
            new Among("ator", -1, 5),
            new Among("icator", 35, 4),
            new Among("itor", -1, 6),
            new Among("ător", -1, 5),
            new Among("iciv", -1, 4),
            new Among("ativ", -1, 5),
            new Among("itiv", -1, 6),
            new Among("icală", -1, 4),
            new Among("icivă", -1, 4),
            new Among("ativă", -1, 5),
            new Among("itivă", -1, 6)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("ica", -1, 1),
            new Among("abila", -1, 1),
            new Among("ibila", -1, 1),
            new Among("oasa", -1, 1),
            new Among("ata", -1, 1),
            new Among("ita", -1, 1),
            new Among("anta", -1, 1),
            new Among("ista", -1, 3),
            new Among("uta", -1, 1),
            new Among("iva", -1, 1),
            new Among("ic", -1, 1),
            new Among("ice", -1, 1),
            new Among("abile", -1, 1),
            new Among("ibile", -1, 1),
            new Among("isme", -1, 3),
            new Among("iune", -1, 2),
            new Among("oase", -1, 1),
            new Among("ate", -1, 1),
            new Among("itate", 17, 1),
            new Among("ite", -1, 1),
            new Among("ante", -1, 1),
            new Among("iste", -1, 3),
            new Among("ute", -1, 1),
            new Among("ive", -1, 1),
            new Among("ici", -1, 1),
            new Among("abili", -1, 1),
            new Among("ibili", -1, 1),
            new Among("iuni", -1, 2),
            new Among("atori", -1, 1),
            new Among("osi", -1, 1),
            new Among("ati", -1, 1),
            new Among("itati", 30, 1),
            new Among("iti", -1, 1),
            new Among("anti", -1, 1),
            new Among("isti", -1, 3),
            new Among("uti", -1, 1),
            new Among("iști", -1, 3),
            new Among("ivi", -1, 1),
            new Among("ităi", -1, 1),
            new Among("oși", -1, 1),
            new Among("ități", -1, 1),
            new Among("abil", -1, 1),
            new Among("ibil", -1, 1),
            new Among("ism", -1, 3),
            new Among("ator", -1, 1),
            new Among("os", -1, 1),
            new Among("at", -1, 1),
            new Among("it", -1, 1),
            new Among("ant", -1, 1),
            new Among("ist", -1, 3),
            new Among("ut", -1, 1),
            new Among("iv", -1, 1),
            new Among("ică", -1, 1),
            new Among("abilă", -1, 1),
            new Among("ibilă", -1, 1),
            new Among("oasă", -1, 1),
            new Among("ată", -1, 1),
            new Among("ită", -1, 1),
            new Among("antă", -1, 1),
            new Among("istă", -1, 3),
            new Among("ută", -1, 1),
            new Among("ivă", -1, 1)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ea", -1, 1),
            new Among("ia", -1, 1),
            new Among("esc", -1, 1),
            new Among("ăsc", -1, 1),
            new Among("ind", -1, 1),
            new Among("ând", -1, 1),
            new Among("are", -1, 1),
            new Among("ere", -1, 1),
            new Among("ire", -1, 1),
            new Among("âre", -1, 1),
            new Among("se", -1, 2),
            new Among("ase", 10, 1),
            new Among("sese", 10, 2),
            new Among("ise", 10, 1),
            new Among("use", 10, 1),
            new Among("âse", 10, 1),
            new Among("ește", -1, 1),
            new Among("ăște", -1, 1),
            new Among("eze", -1, 1),
            new Among("ai", -1, 1),
            new Among("eai", 19, 1),
            new Among("iai", 19, 1),
            new Among("sei", -1, 2),
            new Among("ești", -1, 1),
            new Among("ăști", -1, 1),
            new Among("ui", -1, 1),
            new Among("ezi", -1, 1),
            new Among("âi", -1, 1),
            new Among("ași", -1, 1),
            new Among("seși", -1, 2),
            new Among("aseși", 29, 1),
            new Among("seseși", 29, 2),
            new Among("iseși", 29, 1),
            new Among("useși", 29, 1),
            new Among("âseși", 29, 1),
            new Among("iși", -1, 1),
            new Among("uși", -1, 1),
            new Among("âși", -1, 1),
            new Among("ați", -1, 2),
            new Among("eați", 38, 1),
            new Among("iați", 38, 1),
            new Among("eți", -1, 2),
            new Among("iți", -1, 2),
            new Among("âți", -1, 2),
            new Among("arăți", -1, 1),
            new Among("serăți", -1, 2),
            new Among("aserăți", 45, 1),
            new Among("seserăți", 45, 2),
            new Among("iserăți", 45, 1),
            new Among("userăți", 45, 1),
            new Among("âserăți", 45, 1),
            new Among("irăți", -1, 1),
            new Among("urăți", -1, 1),
            new Among("ârăți", -1, 1),
            new Among("am", -1, 1),
            new Among("eam", 54, 1),
            new Among("iam", 54, 1),
            new Among("em", -1, 2),
            new Among("asem", 57, 1),
            new Among("sesem", 57, 2),
            new Among("isem", 57, 1),
            new Among("usem", 57, 1),
            new Among("âsem", 57, 1),
            new Among("im", -1, 2),
            new Among("âm", -1, 2),
            new Among("ăm", -1, 2),
            new Among("arăm", 65, 1),
            new Among("serăm", 65, 2),
            new Among("aserăm", 67, 1),
            new Among("seserăm", 67, 2),
            new Among("iserăm", 67, 1),
            new Among("userăm", 67, 1),
            new Among("âserăm", 67, 1),
            new Among("irăm", 65, 1),
            new Among("urăm", 65, 1),
            new Among("ârăm", 65, 1),
            new Among("au", -1, 1),
            new Among("eau", 76, 1),
            new Among("iau", 76, 1),
            new Among("indu", -1, 1),
            new Among("ându", -1, 1),
            new Among("ez", -1, 1),
            new Among("ească", -1, 1),
            new Among("ară", -1, 1),
            new Among("seră", -1, 2),
            new Among("aseră", 84, 1),
            new Among("seseră", 84, 2),
            new Among("iseră", 84, 1),
            new Among("useră", 84, 1),
            new Among("âseră", 84, 1),
            new Among("iră", -1, 1),
            new Among("ură", -1, 1),
            new Among("âră", -1, 1),
            new Among("ează", -1, 1)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("a", -1, 1),
            new Among("e", -1, 1),
            new Among("ie", 1, 1),
            new Among("i", -1, 1),
            new Among("ă", -1, 1)
        };


        private bool r_norm()
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
                                slice_from("ș");
                                break;
                            }
                            case 2: {
                                slice_from("ț");
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
            return true;
        }

        private bool r_prelude()
        {
            while (true)
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    if (in_grouping(g_v, 97, 259, false) != 0)
                    {
                        goto lab1;
                    }
                    bra = cursor;
                    {
                        int c3 = cursor;
                        if (!(eq_s("u")))
                        {
                            goto lab3;
                        }
                        ket = cursor;
                        if (in_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab3;
                        }
                        slice_from("U");
                        goto lab2;
                    lab3: ;
                        cursor = c3;
                        if (!(eq_s("i")))
                        {
                            goto lab1;
                        }
                        ket = cursor;
                        if (in_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab1;
                        }
                        slice_from("I");
                    }
                lab2: ;
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
                continue;
            lab0: ;
                cursor = c1;
                break;
            }
            return true;
        }

        private bool r_mark_regions()
        {
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    if (in_grouping(g_v, 97, 259, false) != 0)
                    {
                        goto lab2;
                    }
                    {
                        int c3 = cursor;
                        if (out_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab4;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 259, true);
                            if (ret < 0)
                            {
                                goto lab4;
                            }

                            cursor += ret;
                        }
                        goto lab3;
                    lab4: ;
                        cursor = c3;
                        if (in_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab2;
                        }
                        {

                            int ret = in_grouping(g_v, 97, 259, true);
                            if (ret < 0)
                            {
                                goto lab2;
                            }

                            cursor += ret;
                        }
                    }
                lab3: ;
                    goto lab1;
                lab2: ;
                    cursor = c2;
                    if (out_grouping(g_v, 97, 259, false) != 0)
                    {
                        goto lab0;
                    }
                    {
                        int c4 = cursor;
                        if (out_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab6;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 259, true);
                            if (ret < 0)
                            {
                                goto lab6;
                            }

                            cursor += ret;
                        }
                        goto lab5;
                    lab6: ;
                        cursor = c4;
                        if (in_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab0;
                        }
                        if (cursor >= limit)
                        {
                            goto lab0;
                        }
                        cursor++;
                    }
                lab5: ;
                }
            lab1: ;
                I_pV = cursor;
            lab0: ;
                cursor = c1;
            }
            {
                int c5 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab7: ;
                cursor = c5;
            }
            return true;
        }

        private bool r_postlude()
        {
            int among_var;
            while (true)
            {
                int c1 = cursor;
                bra = cursor;
                among_var = find_among(a_1);
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("i");
                        break;
                    }
                    case 2: {
                        slice_from("u");
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

        private bool r_RV()
        {
            return I_pV <= cursor;
        }

        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_R2()
        {
            return I_p2 <= cursor;
        }

        private bool r_step_0()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_2);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            switch (among_var) {
                case 1: {
                    slice_del();
                    break;
                }
                case 2: {
                    slice_from("a");
                    break;
                }
                case 3: {
                    slice_from("e");
                    break;
                }
                case 4: {
                    slice_from("i");
                    break;
                }
                case 5: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("ab")))
                        {
                            goto lab0;
                        }
                        return false;
                    lab0: ;
                        cursor = limit - c1;
                    }
                    slice_from("i");
                    break;
                }
                case 6: {
                    slice_from("at");
                    break;
                }
                case 7: {
                    slice_from("ați");
                    break;
                }
            }
            return true;
        }

        private bool r_combo_suffix()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_3);
                if (among_var == 0)
                {
                    return false;
                }
                bra = cursor;
                if (!r_R1())
                    return false;
                switch (among_var) {
                    case 1: {
                        slice_from("abil");
                        break;
                    }
                    case 2: {
                        slice_from("ibil");
                        break;
                    }
                    case 3: {
                        slice_from("iv");
                        break;
                    }
                    case 4: {
                        slice_from("ic");
                        break;
                    }
                    case 5: {
                        slice_from("at");
                        break;
                    }
                    case 6: {
                        slice_from("it");
                        break;
                    }
                }
                B_standard_suffix_removed = true;
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_standard_suffix()
        {
            int among_var;
            B_standard_suffix_removed = false;
            while (true)
            {
                int c1 = limit - cursor;
                if (!r_combo_suffix())
                    goto lab0;
                continue;
            lab0: ;
                cursor = limit - c1;
                break;
            }
            ket = cursor;
            among_var = find_among_b(a_4);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R2())
                return false;
            switch (among_var) {
                case 1: {
                    slice_del();
                    break;
                }
                case 2: {
                    if (!(eq_s_b("ț")))
                    {
                        return false;
                    }
                    bra = cursor;
                    slice_from("t");
                    break;
                }
                case 3: {
                    slice_from("ist");
                    break;
                }
            }
            B_standard_suffix_removed = true;
            return true;
        }

        private bool r_verb_suffix()
        {
            int among_var;
            if (cursor < I_pV)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_pV;
            ket = cursor;
            among_var = find_among_b(a_5);
            if (among_var == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    {
                        int c2 = limit - cursor;
                        if (out_grouping_b(g_v, 97, 259, false) != 0)
                        {
                            goto lab1;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c2;
                        if (!(eq_s_b("u")))
                        {
                            {
                                limit_backward = c1;
                                return false;
                            }
                        }
                    }
                lab0: ;
                    slice_del();
                    break;
                }
                case 2: {
                    slice_del();
                    break;
                }
            }
            limit_backward = c1;
            return true;
        }

        private bool r_vowel_suffix()
        {
            ket = cursor;
            if (find_among_b(a_6) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_RV())
                return false;
            slice_del();
            return true;
        }

        protected override bool stem()
        {
            r_norm();
            {
                int c1 = cursor;
                r_prelude();
                cursor = c1;
            }
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                r_step_0();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_standard_suffix();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                {
                    int c5 = limit - cursor;
                    if (!(B_standard_suffix_removed))
                    {
                        goto lab2;
                    }
                    goto lab1;
                lab2: ;
                    cursor = limit - c5;
                    if (!r_verb_suffix())
                        goto lab0;
                }
            lab1: ;
            lab0: ;
                cursor = limit - c4;
            }
            {
                int c6 = limit - cursor;
                r_vowel_suffix();
                cursor = limit - c6;
            }
            cursor = limit_backward;
            {
                int c7 = cursor;
                r_postlude();
                cursor = c7;
            }
            return true;
        }

    }
}

