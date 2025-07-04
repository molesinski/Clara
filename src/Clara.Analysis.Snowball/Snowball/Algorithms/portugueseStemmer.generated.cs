﻿// Generated from portuguese.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from portuguese.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class PortugueseStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;
        private int I_pV;

        private const string g_v = "aeiouáéíóúâêô";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 3),
            new Among("ã", 0, 1),
            new Among("õ", 0, 2)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 3),
            new Among("a~", 0, 1),
            new Among("o~", 0, 2)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ic", -1, -1),
            new Among("ad", -1, -1),
            new Among("os", -1, -1),
            new Among("iv", -1, 1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ante", -1, 1),
            new Among("avel", -1, 1),
            new Among("ível", -1, 1)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("ic", -1, 1),
            new Among("abil", -1, 1),
            new Among("iv", -1, 1)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ica", -1, 1),
            new Among("ância", -1, 1),
            new Among("ência", -1, 4),
            new Among("logia", -1, 2),
            new Among("ira", -1, 9),
            new Among("adora", -1, 1),
            new Among("osa", -1, 1),
            new Among("ista", -1, 1),
            new Among("iva", -1, 8),
            new Among("eza", -1, 1),
            new Among("idade", -1, 7),
            new Among("ante", -1, 1),
            new Among("mente", -1, 6),
            new Among("amente", 12, 5),
            new Among("ável", -1, 1),
            new Among("ível", -1, 1),
            new Among("ico", -1, 1),
            new Among("ismo", -1, 1),
            new Among("oso", -1, 1),
            new Among("amento", -1, 1),
            new Among("imento", -1, 1),
            new Among("ivo", -1, 8),
            new Among("aça~o", -1, 1),
            new Among("uça~o", -1, 3),
            new Among("ador", -1, 1),
            new Among("icas", -1, 1),
            new Among("ências", -1, 4),
            new Among("logias", -1, 2),
            new Among("iras", -1, 9),
            new Among("adoras", -1, 1),
            new Among("osas", -1, 1),
            new Among("istas", -1, 1),
            new Among("ivas", -1, 8),
            new Among("ezas", -1, 1),
            new Among("idades", -1, 7),
            new Among("adores", -1, 1),
            new Among("antes", -1, 1),
            new Among("aço~es", -1, 1),
            new Among("uço~es", -1, 3),
            new Among("icos", -1, 1),
            new Among("ismos", -1, 1),
            new Among("osos", -1, 1),
            new Among("amentos", -1, 1),
            new Among("imentos", -1, 1),
            new Among("ivos", -1, 8)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("ada", -1, 1),
            new Among("ida", -1, 1),
            new Among("ia", -1, 1),
            new Among("aria", 2, 1),
            new Among("eria", 2, 1),
            new Among("iria", 2, 1),
            new Among("ara", -1, 1),
            new Among("era", -1, 1),
            new Among("ira", -1, 1),
            new Among("ava", -1, 1),
            new Among("asse", -1, 1),
            new Among("esse", -1, 1),
            new Among("isse", -1, 1),
            new Among("aste", -1, 1),
            new Among("este", -1, 1),
            new Among("iste", -1, 1),
            new Among("ei", -1, 1),
            new Among("arei", 16, 1),
            new Among("erei", 16, 1),
            new Among("irei", 16, 1),
            new Among("am", -1, 1),
            new Among("iam", 20, 1),
            new Among("ariam", 21, 1),
            new Among("eriam", 21, 1),
            new Among("iriam", 21, 1),
            new Among("aram", 20, 1),
            new Among("eram", 20, 1),
            new Among("iram", 20, 1),
            new Among("avam", 20, 1),
            new Among("em", -1, 1),
            new Among("arem", 29, 1),
            new Among("erem", 29, 1),
            new Among("irem", 29, 1),
            new Among("assem", 29, 1),
            new Among("essem", 29, 1),
            new Among("issem", 29, 1),
            new Among("ado", -1, 1),
            new Among("ido", -1, 1),
            new Among("ando", -1, 1),
            new Among("endo", -1, 1),
            new Among("indo", -1, 1),
            new Among("ara~o", -1, 1),
            new Among("era~o", -1, 1),
            new Among("ira~o", -1, 1),
            new Among("ar", -1, 1),
            new Among("er", -1, 1),
            new Among("ir", -1, 1),
            new Among("as", -1, 1),
            new Among("adas", 47, 1),
            new Among("idas", 47, 1),
            new Among("ias", 47, 1),
            new Among("arias", 50, 1),
            new Among("erias", 50, 1),
            new Among("irias", 50, 1),
            new Among("aras", 47, 1),
            new Among("eras", 47, 1),
            new Among("iras", 47, 1),
            new Among("avas", 47, 1),
            new Among("es", -1, 1),
            new Among("ardes", 58, 1),
            new Among("erdes", 58, 1),
            new Among("irdes", 58, 1),
            new Among("ares", 58, 1),
            new Among("eres", 58, 1),
            new Among("ires", 58, 1),
            new Among("asses", 58, 1),
            new Among("esses", 58, 1),
            new Among("isses", 58, 1),
            new Among("astes", 58, 1),
            new Among("estes", 58, 1),
            new Among("istes", 58, 1),
            new Among("is", -1, 1),
            new Among("ais", 71, 1),
            new Among("eis", 71, 1),
            new Among("areis", 73, 1),
            new Among("ereis", 73, 1),
            new Among("ireis", 73, 1),
            new Among("áreis", 73, 1),
            new Among("éreis", 73, 1),
            new Among("íreis", 73, 1),
            new Among("ásseis", 73, 1),
            new Among("ésseis", 73, 1),
            new Among("ísseis", 73, 1),
            new Among("áveis", 73, 1),
            new Among("íeis", 73, 1),
            new Among("aríeis", 84, 1),
            new Among("eríeis", 84, 1),
            new Among("iríeis", 84, 1),
            new Among("ados", -1, 1),
            new Among("idos", -1, 1),
            new Among("amos", -1, 1),
            new Among("áramos", 90, 1),
            new Among("éramos", 90, 1),
            new Among("íramos", 90, 1),
            new Among("ávamos", 90, 1),
            new Among("íamos", 90, 1),
            new Among("aríamos", 95, 1),
            new Among("eríamos", 95, 1),
            new Among("iríamos", 95, 1),
            new Among("emos", -1, 1),
            new Among("aremos", 99, 1),
            new Among("eremos", 99, 1),
            new Among("iremos", 99, 1),
            new Among("ássemos", 99, 1),
            new Among("êssemos", 99, 1),
            new Among("íssemos", 99, 1),
            new Among("imos", -1, 1),
            new Among("armos", -1, 1),
            new Among("ermos", -1, 1),
            new Among("irmos", -1, 1),
            new Among("ámos", -1, 1),
            new Among("arás", -1, 1),
            new Among("erás", -1, 1),
            new Among("irás", -1, 1),
            new Among("eu", -1, 1),
            new Among("iu", -1, 1),
            new Among("ou", -1, 1),
            new Among("ará", -1, 1),
            new Among("erá", -1, 1),
            new Among("irá", -1, 1)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("a", -1, 1),
            new Among("i", -1, 1),
            new Among("o", -1, 1),
            new Among("os", -1, 1),
            new Among("á", -1, 1),
            new Among("í", -1, 1),
            new Among("ó", -1, 1)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("e", -1, 1),
            new Among("ç", -1, 2),
            new Among("é", -1, 1),
            new Among("ê", -1, 1)
        };


        private bool r_prelude()
        {
            int among_var;
            while (true)
            {
                int c1 = cursor;
                bra = cursor;
                among_var = find_among(a_0);
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("a~");
                        break;
                    }
                    case 2: {
                        slice_from("o~");
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

        private bool r_mark_regions()
        {
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    if (in_grouping(g_v, 97, 250, false) != 0)
                    {
                        goto lab2;
                    }
                    {
                        int c3 = cursor;
                        if (out_grouping(g_v, 97, 250, false) != 0)
                        {
                            goto lab4;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 250, true);
                            if (ret < 0)
                            {
                                goto lab4;
                            }

                            cursor += ret;
                        }
                        goto lab3;
                    lab4: ;
                        cursor = c3;
                        if (in_grouping(g_v, 97, 250, false) != 0)
                        {
                            goto lab2;
                        }
                        {

                            int ret = in_grouping(g_v, 97, 250, true);
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
                    if (out_grouping(g_v, 97, 250, false) != 0)
                    {
                        goto lab0;
                    }
                    {
                        int c4 = cursor;
                        if (out_grouping(g_v, 97, 250, false) != 0)
                        {
                            goto lab6;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 250, true);
                            if (ret < 0)
                            {
                                goto lab6;
                            }

                            cursor += ret;
                        }
                        goto lab5;
                    lab6: ;
                        cursor = c4;
                        if (in_grouping(g_v, 97, 250, false) != 0)
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

                    int ret = out_grouping(g_v, 97, 250, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 250, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 250, true);
                    if (ret < 0)
                    {
                        goto lab7;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 250, true);
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
                        slice_from("ã");
                        break;
                    }
                    case 2: {
                        slice_from("õ");
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

        private bool r_standard_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_5);
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
                    slice_from("log");
                    break;
                }
                case 3: {
                    if (!r_R2())
                        return false;
                    slice_from("u");
                    break;
                }
                case 4: {
                    if (!r_R2())
                        return false;
                    slice_from("ente");
                    break;
                }
                case 5: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    {
                        int c1 = limit - cursor;
                        ket = cursor;
                        among_var = find_among_b(a_2);
                        if (among_var == 0)
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
                        switch (among_var) {
                            case 1: {
                                ket = cursor;
                                if (!(eq_s_b("at")))
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
                                break;
                            }
                        }
                    lab0: ;
                    }
                    break;
                }
                case 6: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    {
                        int c2 = limit - cursor;
                        ket = cursor;
                        if (find_among_b(a_3) == 0)
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
                    lab1: ;
                    }
                    break;
                }
                case 7: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    {
                        int c3 = limit - cursor;
                        ket = cursor;
                        if (find_among_b(a_4) == 0)
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
                case 8: {
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
                    lab3: ;
                    }
                    break;
                }
                case 9: {
                    if (!r_RV())
                        return false;
                    if (!(eq_s_b("e")))
                    {
                        return false;
                    }
                    slice_from("ir");
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
            if (find_among_b(a_6) == 0)
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

        private bool r_residual_suffix()
        {
            ket = cursor;
            if (find_among_b(a_7) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_RV())
                return false;
            slice_del();
            return true;
        }

        private bool r_residual_form()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_8);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_RV())
                        return false;
                    slice_del();
                    ket = cursor;
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("u")))
                        {
                            goto lab1;
                        }
                        bra = cursor;
                        {
                            int c2 = limit - cursor;
                            if (!(eq_s_b("g")))
                            {
                                goto lab1;
                            }
                            cursor = limit - c2;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("i")))
                        {
                            return false;
                        }
                        bra = cursor;
                        {
                            int c3 = limit - cursor;
                            if (!(eq_s_b("c")))
                            {
                                return false;
                            }
                            cursor = limit - c3;
                        }
                    }
                lab0: ;
                    if (!r_RV())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    slice_from("c");
                    break;
                }
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
                {
                    int c3 = limit - cursor;
                    int c4 = limit - cursor;
                    {
                        int c5 = limit - cursor;
                        if (!r_standard_suffix())
                            goto lab4;
                        goto lab3;
                    lab4: ;
                        cursor = limit - c5;
                        if (!r_verb_suffix())
                            goto lab2;
                    }
                lab3: ;
                    cursor = limit - c4;
                    {
                        int c6 = limit - cursor;
                        ket = cursor;
                        if (!(eq_s_b("i")))
                        {
                            goto lab5;
                        }
                        bra = cursor;
                        {
                            int c7 = limit - cursor;
                            if (!(eq_s_b("c")))
                            {
                                goto lab5;
                            }
                            cursor = limit - c7;
                        }
                        if (!r_RV())
                            goto lab5;
                        slice_del();
                    lab5: ;
                        cursor = limit - c6;
                    }
                    goto lab1;
                lab2: ;
                    cursor = limit - c3;
                    if (!r_residual_suffix())
                        goto lab0;
                }
            lab1: ;
            lab0: ;
                cursor = limit - c2;
            }
            {
                int c8 = limit - cursor;
                r_residual_form();
                cursor = limit - c8;
            }
            cursor = limit_backward;
            {
                int c9 = cursor;
                r_postlude();
                cursor = c9;
            }
            return true;
        }

    }
}

