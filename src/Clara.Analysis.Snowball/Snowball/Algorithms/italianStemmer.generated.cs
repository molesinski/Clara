﻿// Generated from italian.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from italian.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class ItalianStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;
        private int I_pV;

        private const string g_v = "aeiouàèìòù";
        private const string g_AEIO = "aeioàèìò";
        private const string g_CG = "cg";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 7),
            new Among("qu", 0, 6),
            new Among("á", 0, 1),
            new Among("é", 0, 2),
            new Among("í", 0, 3),
            new Among("ó", 0, 4),
            new Among("ú", 0, 5)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 3),
            new Among("I", 0, 1),
            new Among("U", 0, 2)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("la", -1, -1),
            new Among("cela", 0, -1),
            new Among("gliela", 0, -1),
            new Among("mela", 0, -1),
            new Among("tela", 0, -1),
            new Among("vela", 0, -1),
            new Among("le", -1, -1),
            new Among("cele", 6, -1),
            new Among("gliele", 6, -1),
            new Among("mele", 6, -1),
            new Among("tele", 6, -1),
            new Among("vele", 6, -1),
            new Among("ne", -1, -1),
            new Among("cene", 12, -1),
            new Among("gliene", 12, -1),
            new Among("mene", 12, -1),
            new Among("sene", 12, -1),
            new Among("tene", 12, -1),
            new Among("vene", 12, -1),
            new Among("ci", -1, -1),
            new Among("li", -1, -1),
            new Among("celi", 20, -1),
            new Among("glieli", 20, -1),
            new Among("meli", 20, -1),
            new Among("teli", 20, -1),
            new Among("veli", 20, -1),
            new Among("gli", 20, -1),
            new Among("mi", -1, -1),
            new Among("si", -1, -1),
            new Among("ti", -1, -1),
            new Among("vi", -1, -1),
            new Among("lo", -1, -1),
            new Among("celo", 31, -1),
            new Among("glielo", 31, -1),
            new Among("melo", 31, -1),
            new Among("telo", 31, -1),
            new Among("velo", 31, -1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ando", -1, 1),
            new Among("endo", -1, 1),
            new Among("ar", -1, 2),
            new Among("er", -1, 2),
            new Among("ir", -1, 2)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("ic", -1, -1),
            new Among("abil", -1, -1),
            new Among("os", -1, -1),
            new Among("iv", -1, 1)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ic", -1, 1),
            new Among("abil", -1, 1),
            new Among("iv", -1, 1)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("ica", -1, 1),
            new Among("logia", -1, 3),
            new Among("osa", -1, 1),
            new Among("ista", -1, 1),
            new Among("iva", -1, 9),
            new Among("anza", -1, 1),
            new Among("enza", -1, 5),
            new Among("ice", -1, 1),
            new Among("atrice", 7, 1),
            new Among("iche", -1, 1),
            new Among("logie", -1, 3),
            new Among("abile", -1, 1),
            new Among("ibile", -1, 1),
            new Among("usione", -1, 4),
            new Among("azione", -1, 2),
            new Among("uzione", -1, 4),
            new Among("atore", -1, 2),
            new Among("ose", -1, 1),
            new Among("ante", -1, 1),
            new Among("mente", -1, 1),
            new Among("amente", 19, 7),
            new Among("iste", -1, 1),
            new Among("ive", -1, 9),
            new Among("anze", -1, 1),
            new Among("enze", -1, 5),
            new Among("ici", -1, 1),
            new Among("atrici", 25, 1),
            new Among("ichi", -1, 1),
            new Among("abili", -1, 1),
            new Among("ibili", -1, 1),
            new Among("ismi", -1, 1),
            new Among("usioni", -1, 4),
            new Among("azioni", -1, 2),
            new Among("uzioni", -1, 4),
            new Among("atori", -1, 2),
            new Among("osi", -1, 1),
            new Among("anti", -1, 1),
            new Among("amenti", -1, 6),
            new Among("imenti", -1, 6),
            new Among("isti", -1, 1),
            new Among("ivi", -1, 9),
            new Among("ico", -1, 1),
            new Among("ismo", -1, 1),
            new Among("oso", -1, 1),
            new Among("amento", -1, 6),
            new Among("imento", -1, 6),
            new Among("ivo", -1, 9),
            new Among("ità", -1, 8),
            new Among("istà", -1, 1),
            new Among("istè", -1, 1),
            new Among("istì", -1, 1)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("isca", -1, 1),
            new Among("enda", -1, 1),
            new Among("ata", -1, 1),
            new Among("ita", -1, 1),
            new Among("uta", -1, 1),
            new Among("ava", -1, 1),
            new Among("eva", -1, 1),
            new Among("iva", -1, 1),
            new Among("erebbe", -1, 1),
            new Among("irebbe", -1, 1),
            new Among("isce", -1, 1),
            new Among("ende", -1, 1),
            new Among("are", -1, 1),
            new Among("ere", -1, 1),
            new Among("ire", -1, 1),
            new Among("asse", -1, 1),
            new Among("ate", -1, 1),
            new Among("avate", 16, 1),
            new Among("evate", 16, 1),
            new Among("ivate", 16, 1),
            new Among("ete", -1, 1),
            new Among("erete", 20, 1),
            new Among("irete", 20, 1),
            new Among("ite", -1, 1),
            new Among("ereste", -1, 1),
            new Among("ireste", -1, 1),
            new Among("ute", -1, 1),
            new Among("erai", -1, 1),
            new Among("irai", -1, 1),
            new Among("isci", -1, 1),
            new Among("endi", -1, 1),
            new Among("erei", -1, 1),
            new Among("irei", -1, 1),
            new Among("assi", -1, 1),
            new Among("ati", -1, 1),
            new Among("iti", -1, 1),
            new Among("eresti", -1, 1),
            new Among("iresti", -1, 1),
            new Among("uti", -1, 1),
            new Among("avi", -1, 1),
            new Among("evi", -1, 1),
            new Among("ivi", -1, 1),
            new Among("isco", -1, 1),
            new Among("ando", -1, 1),
            new Among("endo", -1, 1),
            new Among("Yamo", -1, 1),
            new Among("iamo", -1, 1),
            new Among("avamo", -1, 1),
            new Among("evamo", -1, 1),
            new Among("ivamo", -1, 1),
            new Among("eremo", -1, 1),
            new Among("iremo", -1, 1),
            new Among("assimo", -1, 1),
            new Among("ammo", -1, 1),
            new Among("emmo", -1, 1),
            new Among("eremmo", 54, 1),
            new Among("iremmo", 54, 1),
            new Among("immo", -1, 1),
            new Among("ano", -1, 1),
            new Among("iscano", 58, 1),
            new Among("avano", 58, 1),
            new Among("evano", 58, 1),
            new Among("ivano", 58, 1),
            new Among("eranno", -1, 1),
            new Among("iranno", -1, 1),
            new Among("ono", -1, 1),
            new Among("iscono", 65, 1),
            new Among("arono", 65, 1),
            new Among("erono", 65, 1),
            new Among("irono", 65, 1),
            new Among("erebbero", -1, 1),
            new Among("irebbero", -1, 1),
            new Among("assero", -1, 1),
            new Among("essero", -1, 1),
            new Among("issero", -1, 1),
            new Among("ato", -1, 1),
            new Among("ito", -1, 1),
            new Among("uto", -1, 1),
            new Among("avo", -1, 1),
            new Among("evo", -1, 1),
            new Among("ivo", -1, 1),
            new Among("ar", -1, 1),
            new Among("ir", -1, 1),
            new Among("erà", -1, 1),
            new Among("irà", -1, 1),
            new Among("erò", -1, 1),
            new Among("irò", -1, 1)
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
                    among_var = find_among(a_0);
                    ket = cursor;
                    switch (among_var) {
                        case 1: {
                            slice_from("à");
                            break;
                        }
                        case 2: {
                            slice_from("è");
                            break;
                        }
                        case 3: {
                            slice_from("ì");
                            break;
                        }
                        case 4: {
                            slice_from("ò");
                            break;
                        }
                        case 5: {
                            slice_from("ù");
                            break;
                        }
                        case 6: {
                            slice_from("qU");
                            break;
                        }
                        case 7: {
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
            while (true)
            {
                int c3 = cursor;
                while (true)
                {
                    int c4 = cursor;
                    if (in_grouping(g_v, 97, 249, false) != 0)
                    {
                        goto lab2;
                    }
                    bra = cursor;
                    {
                        int c5 = cursor;
                        if (!(eq_s("u")))
                        {
                            goto lab4;
                        }
                        ket = cursor;
                        if (in_grouping(g_v, 97, 249, false) != 0)
                        {
                            goto lab4;
                        }
                        slice_from("U");
                        goto lab3;
                    lab4: ;
                        cursor = c5;
                        if (!(eq_s("i")))
                        {
                            goto lab2;
                        }
                        ket = cursor;
                        if (in_grouping(g_v, 97, 249, false) != 0)
                        {
                            goto lab2;
                        }
                        slice_from("I");
                    }
                lab3: ;
                    cursor = c4;
                    break;
                lab2: ;
                    cursor = c4;
                    if (cursor >= limit)
                    {
                        goto lab1;
                    }
                    cursor++;
                }
                continue;
            lab1: ;
                cursor = c3;
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
                    if (in_grouping(g_v, 97, 249, false) != 0)
                    {
                        goto lab2;
                    }
                    {
                        int c3 = cursor;
                        if (out_grouping(g_v, 97, 249, false) != 0)
                        {
                            goto lab4;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 249, true);
                            if (ret < 0)
                            {
                                goto lab4;
                            }

                            cursor += ret;
                        }
                        goto lab3;
                    lab4: ;
                        cursor = c3;
                        if (in_grouping(g_v, 97, 249, false) != 0)
                        {
                            goto lab2;
                        }
                        {

                            int ret = in_grouping(g_v, 97, 249, true);
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
                    if (!(eq_s("divan")))
                    {
                        goto lab5;
                    }
                    goto lab1;
                lab5: ;
                    cursor = c2;
                    if (out_grouping(g_v, 97, 249, false) != 0)
                    {
                        goto lab0;
                    }
                    {
                        int c4 = cursor;
                        if (out_grouping(g_v, 97, 249, false) != 0)
                        {
                            goto lab7;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 249, true);
                            if (ret < 0)
                            {
                                goto lab7;
                            }

                            cursor += ret;
                        }
                        goto lab6;
                    lab7: ;
                        cursor = c4;
                        if (in_grouping(g_v, 97, 249, false) != 0)
                        {
                            goto lab0;
                        }
                        if (cursor >= limit)
                        {
                            goto lab0;
                        }
                        cursor++;
                    }
                lab6: ;
                }
            lab1: ;
                I_pV = cursor;
            lab0: ;
                cursor = c1;
            }
            {
                int c5 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 249, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 249, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 249, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 249, true);
                    if (ret < 0)
                    {
                        goto lab8;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab8: ;
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

        private bool r_attached_pronoun()
        {
            int among_var;
            ket = cursor;
            if (find_among_b(a_2) == 0)
            {
                return false;
            }
            bra = cursor;
            among_var = find_among_b(a_3);
            if (among_var == 0)
            {
                return false;
            }
            if (!r_RV())
                return false;
            switch (among_var) {
                case 1: {
                    slice_del();
                    break;
                }
                case 2: {
                    slice_from("e");
                    break;
                }
            }
            return true;
        }

        private bool r_standard_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_6);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    {
                        int c1 = limit - cursor;
                        ket = cursor;
                        if (!(eq_s_b("ic")))
                        {
                            {
                                cursor = limit - c1;
                                goto lab0;
                            }
                        }
                        bra = cursor;
                        if (!r_R2())
                            {
                                cursor = limit - c1;
                                goto lab0;
                            }
                        slice_del();
                    lab0: ;
                    }
                    break;
                }
                case 3: {
                    if (!r_R2())
                        return false;
                    slice_from("log");
                    break;
                }
                case 4: {
                    if (!r_R2())
                        return false;
                    slice_from("u");
                    break;
                }
                case 5: {
                    if (!r_R2())
                        return false;
                    slice_from("ente");
                    break;
                }
                case 6: {
                    if (!r_RV())
                        return false;
                    slice_del();
                    break;
                }
                case 7: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    {
                        int c2 = limit - cursor;
                        ket = cursor;
                        among_var = find_among_b(a_4);
                        if (among_var == 0)
                        {
                            {
                                cursor = limit - c2;
                                goto lab1;
                            }
                        }
                        bra = cursor;
                        if (!r_R2())
                            {
                                cursor = limit - c2;
                                goto lab1;
                            }
                        slice_del();
                        switch (among_var) {
                            case 1: {
                                ket = cursor;
                                if (!(eq_s_b("at")))
                                {
                                    {
                                        cursor = limit - c2;
                                        goto lab1;
                                    }
                                }
                                bra = cursor;
                                if (!r_R2())
                                    {
                                        cursor = limit - c2;
                                        goto lab1;
                                    }
                                slice_del();
                                break;
                            }
                        }
                    lab1: ;
                    }
                    break;
                }
                case 8: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    {
                        int c3 = limit - cursor;
                        ket = cursor;
                        if (find_among_b(a_5) == 0)
                        {
                            {
                                cursor = limit - c3;
                                goto lab2;
                            }
                        }
                        bra = cursor;
                        if (!r_R2())
                            {
                                cursor = limit - c3;
                                goto lab2;
                            }
                        slice_del();
                    lab2: ;
                    }
                    break;
                }
                case 9: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    {
                        int c4 = limit - cursor;
                        ket = cursor;
                        if (!(eq_s_b("at")))
                        {
                            {
                                cursor = limit - c4;
                                goto lab3;
                            }
                        }
                        bra = cursor;
                        if (!r_R2())
                            {
                                cursor = limit - c4;
                                goto lab3;
                            }
                        slice_del();
                        ket = cursor;
                        if (!(eq_s_b("ic")))
                        {
                            {
                                cursor = limit - c4;
                                goto lab3;
                            }
                        }
                        bra = cursor;
                        if (!r_R2())
                            {
                                cursor = limit - c4;
                                goto lab3;
                            }
                        slice_del();
                    lab3: ;
                    }
                    break;
                }
            }
            return true;
        }

        private bool r_verb_suffix()
        {
            if (cursor < I_pV)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_pV;
            ket = cursor;
            if (find_among_b(a_7) == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            slice_del();
            limit_backward = c1;
            return true;
        }

        private bool r_vowel_suffix()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (in_grouping_b(g_AEIO, 97, 242, false) != 0)
                {
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                }
                bra = cursor;
                if (!r_RV())
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                slice_del();
                ket = cursor;
                if (!(eq_s_b("i")))
                {
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                }
                bra = cursor;
                if (!r_RV())
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                slice_del();
            lab0: ;
            }
            {
                int c2 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("h")))
                {
                    {
                        cursor = limit - c2;
                        goto lab1;
                    }
                }
                bra = cursor;
                if (in_grouping_b(g_CG, 99, 103, false) != 0)
                {
                    {
                        cursor = limit - c2;
                        goto lab1;
                    }
                }
                if (!r_RV())
                    {
                        cursor = limit - c2;
                        goto lab1;
                    }
                slice_del();
            lab1: ;
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
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                r_attached_pronoun();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                {
                    int c4 = limit - cursor;
                    if (!r_standard_suffix())
                        goto lab2;
                    goto lab1;
                lab2: ;
                    cursor = limit - c4;
                    if (!r_verb_suffix())
                        goto lab0;
                }
            lab1: ;
            lab0: ;
                cursor = limit - c3;
            }
            {
                int c5 = limit - cursor;
                r_vowel_suffix();
                cursor = limit - c5;
            }
            cursor = limit_backward;
            {
                int c6 = cursor;
                r_postlude();
                cursor = c6;
            }
            return true;
        }

    }
}

