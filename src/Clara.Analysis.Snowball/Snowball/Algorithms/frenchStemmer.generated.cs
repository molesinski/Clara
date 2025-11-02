// Generated from french.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from french.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class FrenchStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;
        private int I_pV;

        private const string g_v = "aeiouyàâèéêëîïôùû";
        private const string g_oux_ending = "bhjlnp";
        private const string g_elision_char = "cdjlmnst";
        private const string g_keep_with_s = "aiosuè";

        private static readonly Among[] a_0 = new[]
        {
            new Among("col", -1, -1, 0),
            new Among("ni", -1, 1, 0),
            new Among("par", -1, -1, 0),
            new Among("tap", -1, -1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 7, 0),
            new Among("H", 0, 6, 0),
            new Among("He", 1, 4, 0),
            new Among("Hi", 1, 5, 0),
            new Among("I", 0, 1, 0),
            new Among("U", 0, 2, 0),
            new Among("Y", 0, 3, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("iqU", -1, 3, 0),
            new Among("abl", -1, 3, 0),
            new Among("Ièr", -1, 4, 0),
            new Among("ièr", -1, 4, 0),
            new Among("eus", -1, 2, 0),
            new Among("iv", -1, 1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ic", -1, 2, 0),
            new Among("abil", -1, 1, 0),
            new Among("iv", -1, 3, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("iqUe", -1, 1, 0),
            new Among("atrice", -1, 2, 0),
            new Among("ance", -1, 1, 0),
            new Among("ence", -1, 5, 0),
            new Among("logie", -1, 3, 0),
            new Among("able", -1, 1, 0),
            new Among("isme", -1, 1, 0),
            new Among("euse", -1, 12, 0),
            new Among("iste", -1, 1, 0),
            new Among("ive", -1, 8, 0),
            new Among("if", -1, 8, 0),
            new Among("usion", -1, 4, 0),
            new Among("ation", -1, 2, 0),
            new Among("ution", -1, 4, 0),
            new Among("ateur", -1, 2, 0),
            new Among("iqUes", -1, 1, 0),
            new Among("atrices", -1, 2, 0),
            new Among("ances", -1, 1, 0),
            new Among("ences", -1, 5, 0),
            new Among("logies", -1, 3, 0),
            new Among("ables", -1, 1, 0),
            new Among("ismes", -1, 1, 0),
            new Among("euses", -1, 12, 0),
            new Among("istes", -1, 1, 0),
            new Among("ives", -1, 8, 0),
            new Among("ifs", -1, 8, 0),
            new Among("usions", -1, 4, 0),
            new Among("ations", -1, 2, 0),
            new Among("utions", -1, 4, 0),
            new Among("ateurs", -1, 2, 0),
            new Among("ments", -1, 16, 0),
            new Among("ements", 30, 6, 0),
            new Among("issements", 31, 13, 0),
            new Among("ités", -1, 7, 0),
            new Among("ment", -1, 16, 0),
            new Among("ement", 34, 6, 0),
            new Among("issement", 35, 13, 0),
            new Among("amment", 34, 14, 0),
            new Among("emment", 34, 15, 0),
            new Among("aux", -1, 10, 0),
            new Among("eaux", 39, 9, 0),
            new Among("eux", -1, 1, 0),
            new Among("oux", -1, 11, 0),
            new Among("ité", -1, 7, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ira", -1, 1, 0),
            new Among("ie", -1, 1, 0),
            new Among("isse", -1, 1, 0),
            new Among("issante", -1, 1, 0),
            new Among("i", -1, 1, 0),
            new Among("irai", 4, 1, 0),
            new Among("ir", -1, 1, 0),
            new Among("iras", -1, 1, 0),
            new Among("ies", -1, 1, 0),
            new Among("îmes", -1, 1, 0),
            new Among("isses", -1, 1, 0),
            new Among("issantes", -1, 1, 0),
            new Among("îtes", -1, 1, 0),
            new Among("is", -1, 1, 0),
            new Among("irais", 13, 1, 0),
            new Among("issais", 13, 1, 0),
            new Among("irions", -1, 1, 0),
            new Among("issions", -1, 1, 0),
            new Among("irons", -1, 1, 0),
            new Among("issons", -1, 1, 0),
            new Among("issants", -1, 1, 0),
            new Among("it", -1, 1, 0),
            new Among("irait", 21, 1, 0),
            new Among("issait", 21, 1, 0),
            new Among("issant", -1, 1, 0),
            new Among("iraIent", -1, 1, 0),
            new Among("issaIent", -1, 1, 0),
            new Among("irent", -1, 1, 0),
            new Among("issent", -1, 1, 0),
            new Among("iront", -1, 1, 0),
            new Among("ît", -1, 1, 0),
            new Among("iriez", -1, 1, 0),
            new Among("issiez", -1, 1, 0),
            new Among("irez", -1, 1, 0),
            new Among("issez", -1, 1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("al", -1, 1, 0),
            new Among("épl", -1, -1, 0),
            new Among("auv", -1, -1, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("a", -1, 3, 0),
            new Among("era", 0, 2, 0),
            new Among("aise", -1, 4, 0),
            new Among("asse", -1, 3, 0),
            new Among("ante", -1, 3, 0),
            new Among("ée", -1, 2, 0),
            new Among("ai", -1, 3, 0),
            new Among("erai", 6, 2, 0),
            new Among("er", -1, 2, 0),
            new Among("as", -1, 3, 0),
            new Among("eras", 9, 2, 0),
            new Among("âmes", -1, 3, 0),
            new Among("aises", -1, 4, 0),
            new Among("asses", -1, 3, 0),
            new Among("antes", -1, 3, 0),
            new Among("âtes", -1, 3, 0),
            new Among("ées", -1, 2, 0),
            new Among("ais", -1, 4, 0),
            new Among("eais", 17, 2, 0),
            new Among("erais", 17, 2, 0),
            new Among("ions", -1, 1, 0),
            new Among("erions", 20, 2, 0),
            new Among("assions", 20, 3, 0),
            new Among("erons", -1, 2, 0),
            new Among("ants", -1, 3, 0),
            new Among("és", -1, 2, 0),
            new Among("ait", -1, 3, 0),
            new Among("erait", 26, 2, 0),
            new Among("ant", -1, 3, 0),
            new Among("aIent", -1, 3, 0),
            new Among("eraIent", 29, 2, 0),
            new Among("èrent", -1, 2, 0),
            new Among("assent", -1, 3, 0),
            new Among("eront", -1, 2, 0),
            new Among("ât", -1, 3, 0),
            new Among("ez", -1, 2, 0),
            new Among("iez", 35, 2, 0),
            new Among("eriez", 36, 2, 0),
            new Among("assiez", 36, 3, 0),
            new Among("erez", 35, 2, 0),
            new Among("é", -1, 2, 0)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("e", -1, 3, 0),
            new Among("Ière", 0, 2, 0),
            new Among("ière", 0, 2, 0),
            new Among("ion", -1, 1, 0),
            new Among("Ier", -1, 2, 0),
            new Among("ier", -1, 2, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("ell", -1, -1, 0),
            new Among("eill", -1, -1, 0),
            new Among("enn", -1, -1, 0),
            new Among("onn", -1, -1, 0),
            new Among("ett", -1, -1, 0)
        };


        private bool r_elisions()
        {
            bra = cursor;
            {
                int c1 = cursor;
                if (in_grouping(g_elision_char, 99, 116, false) != 0)
                {
                    goto lab1;
                }
                goto lab0;
            lab1: ;
                cursor = c1;
                if (!(eq_s("qu")))
                {
                    return false;
                }
            }
        lab0: ;
            if (!(eq_s("'")))
            {
                return false;
            }
            ket = cursor;
            if (cursor < limit)
            {
                goto lab2;
            }
            return false;
        lab2: ;
            slice_del();
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
                    {
                        int c3 = cursor;
                        if (in_grouping(g_v, 97, 251, false) != 0)
                        {
                            goto lab3;
                        }
                        bra = cursor;
                        {
                            int c4 = cursor;
                            if (!(eq_s("u")))
                            {
                                goto lab5;
                            }
                            ket = cursor;
                            if (in_grouping(g_v, 97, 251, false) != 0)
                            {
                                goto lab5;
                            }
                            slice_from("U");
                            goto lab4;
                        lab5: ;
                            cursor = c4;
                            if (!(eq_s("i")))
                            {
                                goto lab6;
                            }
                            ket = cursor;
                            if (in_grouping(g_v, 97, 251, false) != 0)
                            {
                                goto lab6;
                            }
                            slice_from("I");
                            goto lab4;
                        lab6: ;
                            cursor = c4;
                            if (!(eq_s("y")))
                            {
                                goto lab3;
                            }
                            ket = cursor;
                            slice_from("Y");
                        }
                    lab4: ;
                        goto lab2;
                    lab3: ;
                        cursor = c3;
                        bra = cursor;
                        if (!(eq_s("ë")))
                        {
                            goto lab7;
                        }
                        ket = cursor;
                        slice_from("He");
                        goto lab2;
                    lab7: ;
                        cursor = c3;
                        bra = cursor;
                        if (!(eq_s("ï")))
                        {
                            goto lab8;
                        }
                        ket = cursor;
                        slice_from("Hi");
                        goto lab2;
                    lab8: ;
                        cursor = c3;
                        bra = cursor;
                        if (!(eq_s("y")))
                        {
                            goto lab9;
                        }
                        ket = cursor;
                        if (in_grouping(g_v, 97, 251, false) != 0)
                        {
                            goto lab9;
                        }
                        slice_from("Y");
                        goto lab2;
                    lab9: ;
                        cursor = c3;
                        if (!(eq_s("q")))
                        {
                            goto lab1;
                        }
                        bra = cursor;
                        if (!(eq_s("u")))
                        {
                            goto lab1;
                        }
                        ket = cursor;
                        slice_from("U");
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
            int among_var;
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    if (in_grouping(g_v, 97, 251, false) != 0)
                    {
                        goto lab2;
                    }
                    if (in_grouping(g_v, 97, 251, false) != 0)
                    {
                        goto lab2;
                    }
                    if (cursor >= limit)
                    {
                        goto lab2;
                    }
                    cursor++;
                    goto lab1;
                lab2: ;
                    cursor = c2;
                    among_var = find_among(a_0, null);
                    if (among_var == 0)
                    {
                        goto lab3;
                    }
                    switch (among_var) {
                        case 1: {
                            if (in_grouping(g_v, 97, 251, false) != 0)
                            {
                                goto lab3;
                            }
                            break;
                        }
                    }
                    goto lab1;
                lab3: ;
                    cursor = c2;
                    if (cursor >= limit)
                    {
                        goto lab0;
                    }
                    cursor++;
                    {

                        int ret = out_grouping(g_v, 97, 251, true);
                        if (ret < 0)
                        {
                            goto lab0;
                        }

                        cursor += ret;
                    }
                }
            lab1: ;
                I_pV = cursor;
            lab0: ;
                cursor = c1;
            }
            {
                int c3 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 251, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 251, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 251, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 251, true);
                    if (ret < 0)
                    {
                        goto lab4;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab4: ;
                cursor = c3;
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
                among_var = find_among(a_1, null);
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
                        slice_from("y");
                        break;
                    }
                    case 4: {
                        slice_from("ë");
                        break;
                    }
                    case 5: {
                        slice_from("ï");
                        break;
                    }
                    case 6: {
                        slice_del();
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

        private bool r_standard_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_4, null);
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
                        {
                            int c2 = limit - cursor;
                            if (!r_R2())
                                goto lab2;
                            slice_del();
                            goto lab1;
                        lab2: ;
                            cursor = limit - c2;
                            slice_from("iqU");
                        }
                    lab1: ;
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
                    slice_from("ent");
                    break;
                }
                case 6: {
                    if (!r_RV())
                        return false;
                    slice_del();
                    {
                        int c3 = limit - cursor;
                        ket = cursor;
                        among_var = find_among_b(a_2, null);
                        if (among_var == 0)
                        {
                            {
                                cursor = limit - c3;
                                goto lab3;
                            }
                        }
                        bra = cursor;
                        switch (among_var) {
                            case 1: {
                                if (!r_R2())
                                    {
                                        cursor = limit - c3;
                                        goto lab3;
                                    }
                                slice_del();
                                ket = cursor;
                                if (!(eq_s_b("at")))
                                {
                                    {
                                        cursor = limit - c3;
                                        goto lab3;
                                    }
                                }
                                bra = cursor;
                                if (!r_R2())
                                    {
                                        cursor = limit - c3;
                                        goto lab3;
                                    }
                                slice_del();
                                break;
                            }
                            case 2: {
                                {
                                    int c4 = limit - cursor;
                                    if (!r_R2())
                                        goto lab5;
                                    slice_del();
                                    goto lab4;
                                lab5: ;
                                    cursor = limit - c4;
                                    if (!r_R1())
                                        {
                                            cursor = limit - c3;
                                            goto lab3;
                                        }
                                    slice_from("eux");
                                }
                            lab4: ;
                                break;
                            }
                            case 3: {
                                if (!r_R2())
                                    {
                                        cursor = limit - c3;
                                        goto lab3;
                                    }
                                slice_del();
                                break;
                            }
                            case 4: {
                                if (!r_RV())
                                    {
                                        cursor = limit - c3;
                                        goto lab3;
                                    }
                                slice_from("i");
                                break;
                            }
                        }
                    lab3: ;
                    }
                    break;
                }
                case 7: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    {
                        int c5 = limit - cursor;
                        ket = cursor;
                        among_var = find_among_b(a_3, null);
                        if (among_var == 0)
                        {
                            {
                                cursor = limit - c5;
                                goto lab6;
                            }
                        }
                        bra = cursor;
                        switch (among_var) {
                            case 1: {
                                {
                                    int c6 = limit - cursor;
                                    if (!r_R2())
                                        goto lab8;
                                    slice_del();
                                    goto lab7;
                                lab8: ;
                                    cursor = limit - c6;
                                    slice_from("abl");
                                }
                            lab7: ;
                                break;
                            }
                            case 2: {
                                {
                                    int c7 = limit - cursor;
                                    if (!r_R2())
                                        goto lab10;
                                    slice_del();
                                    goto lab9;
                                lab10: ;
                                    cursor = limit - c7;
                                    slice_from("iqU");
                                }
                            lab9: ;
                                break;
                            }
                            case 3: {
                                if (!r_R2())
                                    {
                                        cursor = limit - c5;
                                        goto lab6;
                                    }
                                slice_del();
                                break;
                            }
                        }
                    lab6: ;
                    }
                    break;
                }
                case 8: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    {
                        int c8 = limit - cursor;
                        ket = cursor;
                        if (!(eq_s_b("at")))
                        {
                            {
                                cursor = limit - c8;
                                goto lab11;
                            }
                        }
                        bra = cursor;
                        if (!r_R2())
                            {
                                cursor = limit - c8;
                                goto lab11;
                            }
                        slice_del();
                        ket = cursor;
                        if (!(eq_s_b("ic")))
                        {
                            {
                                cursor = limit - c8;
                                goto lab11;
                            }
                        }
                        bra = cursor;
                        {
                            int c9 = limit - cursor;
                            if (!r_R2())
                                goto lab13;
                            slice_del();
                            goto lab12;
                        lab13: ;
                            cursor = limit - c9;
                            slice_from("iqU");
                        }
                    lab12: ;
                    lab11: ;
                    }
                    break;
                }
                case 9: {
                    slice_from("eau");
                    break;
                }
                case 10: {
                    if (!r_R1())
                        return false;
                    slice_from("al");
                    break;
                }
                case 11: {
                    if (in_grouping_b(g_oux_ending, 98, 112, false) != 0)
                    {
                        return false;
                    }
                    slice_from("ou");
                    break;
                }
                case 12: {
                    {
                        int c10 = limit - cursor;
                        if (!r_R2())
                            goto lab15;
                        slice_del();
                        goto lab14;
                    lab15: ;
                        cursor = limit - c10;
                        if (!r_R1())
                            return false;
                        slice_from("eux");
                    }
                lab14: ;
                    break;
                }
                case 13: {
                    if (!r_R1())
                        return false;
                    if (out_grouping_b(g_v, 97, 251, false) != 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
                case 14: {
                    if (!r_RV())
                        return false;
                    slice_from("ant");
                    return false;
                    break;
                }
                case 15: {
                    if (!r_RV())
                        return false;
                    slice_from("ent");
                    return false;
                    break;
                }
                case 16: {
                    {
                        int c11 = limit - cursor;
                        if (in_grouping_b(g_v, 97, 251, false) != 0)
                        {
                            return false;
                        }
                        if (!r_RV())
                            return false;
                        cursor = limit - c11;
                    }
                    slice_del();
                    return false;
                    break;
                }
            }
            return true;
        }

        private bool r_i_verb_suffix()
        {
            if (cursor < I_pV)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_pV;
            ket = cursor;
            if (find_among_b(a_5, null) == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            {
                int c2 = limit - cursor;
                if (!(eq_s_b("H")))
                {
                    goto lab0;
                }
                {
                    limit_backward = c1;
                    return false;
                }
            lab0: ;
                cursor = limit - c2;
            }
            if (out_grouping_b(g_v, 97, 251, false) != 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            slice_del();
            limit_backward = c1;
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
            among_var = find_among_b(a_7, null);
            if (among_var == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            switch (among_var) {
                case 1: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    slice_del();
                    break;
                }
                case 3: {
                    {
                        int c2 = limit - cursor;
                        if (!(eq_s_b("e")))
                        {
                            {
                                cursor = limit - c2;
                                goto lab0;
                            }
                        }
                        if (!r_RV())
                            {
                                cursor = limit - c2;
                                goto lab0;
                            }
                        bra = cursor;
                    lab0: ;
                    }
                    slice_del();
                    break;
                }
                case 4: {
                    {
                        int c3 = limit - cursor;
                        among_var = find_among_b(a_6, null);
                        if (among_var == 0)
                        {
                            goto lab1;
                        }
                        switch (among_var) {
                            case 1: {
                                if (cursor <= limit_backward)
                                {
                                    goto lab1;
                                }
                                cursor--;
                                if (cursor > limit_backward)
                                {
                                    goto lab1;
                                }
                                break;
                            }
                        }
                        return false;
                    lab1: ;
                        cursor = limit - c3;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_residual_suffix()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("s")))
                {
                    {
                        cursor = limit - c1;
                        goto lab0;
                    }
                }
                bra = cursor;
                {
                    int c2 = limit - cursor;
                    {
                        int c3 = limit - cursor;
                        if (!(eq_s_b("Hi")))
                        {
                            goto lab2;
                        }
                        goto lab1;
                    lab2: ;
                        cursor = limit - c3;
                        if (out_grouping_b(g_keep_with_s, 97, 232, false) != 0)
                        {
                            {
                                cursor = limit - c1;
                                goto lab0;
                            }
                        }
                    }
                lab1: ;
                    cursor = limit - c2;
                }
                slice_del();
            lab0: ;
            }
            if (cursor < I_pV)
            {
                return false;
            }
            int c4 = limit_backward;
            limit_backward = I_pV;
            ket = cursor;
            among_var = find_among_b(a_8, null);
            if (among_var == 0)
            {
                {
                    limit_backward = c4;
                    return false;
                }
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R2())
                        {
                            limit_backward = c4;
                            return false;
                        }
                    {
                        int c5 = limit - cursor;
                        if (!(eq_s_b("s")))
                        {
                            goto lab4;
                        }
                        goto lab3;
                    lab4: ;
                        cursor = limit - c5;
                        if (!(eq_s_b("t")))
                        {
                            {
                                limit_backward = c4;
                                return false;
                            }
                        }
                    }
                lab3: ;
                    slice_del();
                    break;
                }
                case 2: {
                    slice_from("i");
                    break;
                }
                case 3: {
                    slice_del();
                    break;
                }
            }
            limit_backward = c4;
            return true;
        }

        private bool r_un_double()
        {
            {
                int c1 = limit - cursor;
                if (find_among_b(a_9, null) == 0)
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

        private bool r_un_accent()
        {
            {
                int c1 = 1;
                while (true)
                {
                    if (out_grouping_b(g_v, 97, 251, false) != 0)
                    {
                        goto lab0;
                    }
                    c1--;
                    continue;
                lab0: ;
                    break;
                }
                if (c1 > 0)
                {
                    return false;
                }
            }
            ket = cursor;
            {
                int c2 = limit - cursor;
                if (!(eq_s_b("é")))
                {
                    goto lab2;
                }
                goto lab1;
            lab2: ;
                cursor = limit - c2;
                if (!(eq_s_b("è")))
                {
                    return false;
                }
            }
        lab1: ;
            bra = cursor;
            slice_from("e");
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                r_elisions();
                cursor = c1;
            }
            {
                int c2 = cursor;
                r_prelude();
                cursor = c2;
            }
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            {
                int c3 = limit - cursor;
                {
                    int c4 = limit - cursor;
                    int c5 = limit - cursor;
                    {
                        int c6 = limit - cursor;
                        if (!r_standard_suffix())
                            goto lab4;
                        goto lab3;
                    lab4: ;
                        cursor = limit - c6;
                        if (!r_i_verb_suffix())
                            goto lab5;
                        goto lab3;
                    lab5: ;
                        cursor = limit - c6;
                        if (!r_verb_suffix())
                            goto lab2;
                    }
                lab3: ;
                    cursor = limit - c5;
                    {
                        int c7 = limit - cursor;
                        ket = cursor;
                        {
                            int c8 = limit - cursor;
                            if (!(eq_s_b("Y")))
                            {
                                goto lab8;
                            }
                            bra = cursor;
                            slice_from("i");
                            goto lab7;
                        lab8: ;
                            cursor = limit - c8;
                            if (!(eq_s_b("ç")))
                            {
                                {
                                    cursor = limit - c7;
                                    goto lab6;
                                }
                            }
                            bra = cursor;
                            slice_from("c");
                        }
                    lab7: ;
                    lab6: ;
                    }
                    goto lab1;
                lab2: ;
                    cursor = limit - c4;
                    if (!r_residual_suffix())
                        goto lab0;
                }
            lab1: ;
            lab0: ;
                cursor = limit - c3;
            }
            {
                int c9 = limit - cursor;
                r_un_double();
                cursor = limit - c9;
            }
            {
                int c10 = limit - cursor;
                r_un_accent();
                cursor = limit - c10;
            }
            cursor = limit_backward;
            {
                int c11 = cursor;
                r_postlude();
                cursor = c11;
            }
            return true;
        }

    }
}

