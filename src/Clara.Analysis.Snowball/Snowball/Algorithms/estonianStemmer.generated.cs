// Generated from estonian.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from estonian.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class EstonianStemmer : Stemmer
    {
        private int I_p1;

        private const string g_V1 = "aeiouõäöü";
        private const string g_RV = "aeiuo";
        private const string g_KI = "kptgbdshfšzž";
        private const string g_GI = "cjlmnqrvwxaeiouõäöü";

        private static readonly Among[] a_0 = new[]
        {
            new Among("gi", -1, 1),
            new Among("ki", -1, 2)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("da", -1, 3),
            new Among("mata", -1, 1),
            new Among("b", -1, 3),
            new Among("ksid", -1, 1),
            new Among("nuksid", 3, 1),
            new Among("me", -1, 3),
            new Among("sime", 5, 1),
            new Among("ksime", 6, 1),
            new Among("nuksime", 7, 1),
            new Among("akse", -1, 2),
            new Among("dakse", 9, 1),
            new Among("takse", 9, 1),
            new Among("site", -1, 1),
            new Among("ksite", 12, 1),
            new Among("nuksite", 13, 1),
            new Among("n", -1, 3),
            new Among("sin", 15, 1),
            new Among("ksin", 16, 1),
            new Among("nuksin", 17, 1),
            new Among("daks", -1, 1),
            new Among("taks", -1, 1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("aa", -1, -1),
            new Among("ee", -1, -1),
            new Among("ii", -1, -1),
            new Among("oo", -1, -1),
            new Among("uu", -1, -1),
            new Among("ää", -1, -1),
            new Among("õõ", -1, -1),
            new Among("öö", -1, -1),
            new Among("üü", -1, -1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("i", -1, 1)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("lane", -1, 1),
            new Among("line", -1, 3),
            new Among("mine", -1, 2),
            new Among("lasse", -1, 1),
            new Among("lisse", -1, 3),
            new Among("misse", -1, 2),
            new Among("lasi", -1, 1),
            new Among("lisi", -1, 3),
            new Among("misi", -1, 2),
            new Among("last", -1, 1),
            new Among("list", -1, 3),
            new Among("mist", -1, 2)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ga", -1, 1),
            new Among("ta", -1, 1),
            new Among("le", -1, 1),
            new Among("sse", -1, 1),
            new Among("l", -1, 1),
            new Among("s", -1, 1),
            new Among("ks", 5, 1),
            new Among("t", -1, 2),
            new Among("lt", 7, 1),
            new Among("st", 7, 1)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("", -1, 2),
            new Among("las", 0, 1),
            new Among("lis", 0, 1),
            new Among("mis", 0, 1),
            new Among("t", 0, -1)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("d", -1, 4),
            new Among("sid", 0, 2),
            new Among("de", -1, 4),
            new Among("ikkude", 2, 1),
            new Among("ike", -1, 1),
            new Among("ikke", -1, 1),
            new Among("te", -1, 3)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("va", -1, -1),
            new Among("du", -1, -1),
            new Among("nu", -1, -1),
            new Among("tu", -1, -1)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("kk", -1, 1),
            new Among("pp", -1, 2),
            new Among("tt", -1, 3)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("ma", -1, 2),
            new Among("mai", -1, 1),
            new Among("m", -1, 1)
        };

        private static readonly Among[] a_11 = new[]
        {
            new Among("joob", -1, 1),
            new Among("jood", -1, 1),
            new Among("joodakse", 1, 1),
            new Among("jooma", -1, 1),
            new Among("joomata", 3, 1),
            new Among("joome", -1, 1),
            new Among("joon", -1, 1),
            new Among("joote", -1, 1),
            new Among("joovad", -1, 1),
            new Among("juua", -1, 1),
            new Among("juuakse", 9, 1),
            new Among("jäi", -1, 12),
            new Among("jäid", 11, 12),
            new Among("jäime", 11, 12),
            new Among("jäin", 11, 12),
            new Among("jäite", 11, 12),
            new Among("jääb", -1, 12),
            new Among("jääd", -1, 12),
            new Among("jääda", 17, 12),
            new Among("jäädakse", 18, 12),
            new Among("jäädi", 17, 12),
            new Among("jääks", -1, 12),
            new Among("jääksid", 21, 12),
            new Among("jääksime", 21, 12),
            new Among("jääksin", 21, 12),
            new Among("jääksite", 21, 12),
            new Among("jääma", -1, 12),
            new Among("jäämata", 26, 12),
            new Among("jääme", -1, 12),
            new Among("jään", -1, 12),
            new Among("jääte", -1, 12),
            new Among("jäävad", -1, 12),
            new Among("jõi", -1, 1),
            new Among("jõid", 32, 1),
            new Among("jõime", 32, 1),
            new Among("jõin", 32, 1),
            new Among("jõite", 32, 1),
            new Among("keeb", -1, 4),
            new Among("keed", -1, 4),
            new Among("keedakse", 38, 4),
            new Among("keeks", -1, 4),
            new Among("keeksid", 40, 4),
            new Among("keeksime", 40, 4),
            new Among("keeksin", 40, 4),
            new Among("keeksite", 40, 4),
            new Among("keema", -1, 4),
            new Among("keemata", 45, 4),
            new Among("keeme", -1, 4),
            new Among("keen", -1, 4),
            new Among("kees", -1, 4),
            new Among("keeta", -1, 4),
            new Among("keete", -1, 4),
            new Among("keevad", -1, 4),
            new Among("käia", -1, 8),
            new Among("käiakse", 53, 8),
            new Among("käib", -1, 8),
            new Among("käid", -1, 8),
            new Among("käidi", 56, 8),
            new Among("käiks", -1, 8),
            new Among("käiksid", 58, 8),
            new Among("käiksime", 58, 8),
            new Among("käiksin", 58, 8),
            new Among("käiksite", 58, 8),
            new Among("käima", -1, 8),
            new Among("käimata", 63, 8),
            new Among("käime", -1, 8),
            new Among("käin", -1, 8),
            new Among("käis", -1, 8),
            new Among("käite", -1, 8),
            new Among("käivad", -1, 8),
            new Among("laob", -1, 16),
            new Among("laod", -1, 16),
            new Among("laoks", -1, 16),
            new Among("laoksid", 72, 16),
            new Among("laoksime", 72, 16),
            new Among("laoksin", 72, 16),
            new Among("laoksite", 72, 16),
            new Among("laome", -1, 16),
            new Among("laon", -1, 16),
            new Among("laote", -1, 16),
            new Among("laovad", -1, 16),
            new Among("loeb", -1, 14),
            new Among("loed", -1, 14),
            new Among("loeks", -1, 14),
            new Among("loeksid", 83, 14),
            new Among("loeksime", 83, 14),
            new Among("loeksin", 83, 14),
            new Among("loeksite", 83, 14),
            new Among("loeme", -1, 14),
            new Among("loen", -1, 14),
            new Among("loete", -1, 14),
            new Among("loevad", -1, 14),
            new Among("loob", -1, 7),
            new Among("lood", -1, 7),
            new Among("loodi", 93, 7),
            new Among("looks", -1, 7),
            new Among("looksid", 95, 7),
            new Among("looksime", 95, 7),
            new Among("looksin", 95, 7),
            new Among("looksite", 95, 7),
            new Among("looma", -1, 7),
            new Among("loomata", 100, 7),
            new Among("loome", -1, 7),
            new Among("loon", -1, 7),
            new Among("loote", -1, 7),
            new Among("loovad", -1, 7),
            new Among("luua", -1, 7),
            new Among("luuakse", 106, 7),
            new Among("lõi", -1, 6),
            new Among("lõid", 108, 6),
            new Among("lõime", 108, 6),
            new Among("lõin", 108, 6),
            new Among("lõite", 108, 6),
            new Among("lööb", -1, 5),
            new Among("lööd", -1, 5),
            new Among("löödakse", 114, 5),
            new Among("löödi", 114, 5),
            new Among("lööks", -1, 5),
            new Among("lööksid", 117, 5),
            new Among("lööksime", 117, 5),
            new Among("lööksin", 117, 5),
            new Among("lööksite", 117, 5),
            new Among("lööma", -1, 5),
            new Among("löömata", 122, 5),
            new Among("lööme", -1, 5),
            new Among("löön", -1, 5),
            new Among("lööte", -1, 5),
            new Among("löövad", -1, 5),
            new Among("lüüa", -1, 5),
            new Among("lüüakse", 128, 5),
            new Among("müüa", -1, 13),
            new Among("müüakse", 130, 13),
            new Among("müüb", -1, 13),
            new Among("müüd", -1, 13),
            new Among("müüdi", 133, 13),
            new Among("müüks", -1, 13),
            new Among("müüksid", 135, 13),
            new Among("müüksime", 135, 13),
            new Among("müüksin", 135, 13),
            new Among("müüksite", 135, 13),
            new Among("müüma", -1, 13),
            new Among("müümata", 140, 13),
            new Among("müüme", -1, 13),
            new Among("müün", -1, 13),
            new Among("müüs", -1, 13),
            new Among("müüte", -1, 13),
            new Among("müüvad", -1, 13),
            new Among("näeb", -1, 18),
            new Among("näed", -1, 18),
            new Among("näeks", -1, 18),
            new Among("näeksid", 149, 18),
            new Among("näeksime", 149, 18),
            new Among("näeksin", 149, 18),
            new Among("näeksite", 149, 18),
            new Among("näeme", -1, 18),
            new Among("näen", -1, 18),
            new Among("näete", -1, 18),
            new Among("näevad", -1, 18),
            new Among("nägema", -1, 18),
            new Among("nägemata", 158, 18),
            new Among("näha", -1, 18),
            new Among("nähakse", 160, 18),
            new Among("nähti", -1, 18),
            new Among("põeb", -1, 15),
            new Among("põed", -1, 15),
            new Among("põeks", -1, 15),
            new Among("põeksid", 165, 15),
            new Among("põeksime", 165, 15),
            new Among("põeksin", 165, 15),
            new Among("põeksite", 165, 15),
            new Among("põeme", -1, 15),
            new Among("põen", -1, 15),
            new Among("põete", -1, 15),
            new Among("põevad", -1, 15),
            new Among("saab", -1, 2),
            new Among("saad", -1, 2),
            new Among("saada", 175, 2),
            new Among("saadakse", 176, 2),
            new Among("saadi", 175, 2),
            new Among("saaks", -1, 2),
            new Among("saaksid", 179, 2),
            new Among("saaksime", 179, 2),
            new Among("saaksin", 179, 2),
            new Among("saaksite", 179, 2),
            new Among("saama", -1, 2),
            new Among("saamata", 184, 2),
            new Among("saame", -1, 2),
            new Among("saan", -1, 2),
            new Among("saate", -1, 2),
            new Among("saavad", -1, 2),
            new Among("sai", -1, 2),
            new Among("said", 190, 2),
            new Among("saime", 190, 2),
            new Among("sain", 190, 2),
            new Among("saite", 190, 2),
            new Among("sõi", -1, 9),
            new Among("sõid", 195, 9),
            new Among("sõime", 195, 9),
            new Among("sõin", 195, 9),
            new Among("sõite", 195, 9),
            new Among("sööb", -1, 9),
            new Among("sööd", -1, 9),
            new Among("söödakse", 201, 9),
            new Among("söödi", 201, 9),
            new Among("sööks", -1, 9),
            new Among("sööksid", 204, 9),
            new Among("sööksime", 204, 9),
            new Among("sööksin", 204, 9),
            new Among("sööksite", 204, 9),
            new Among("sööma", -1, 9),
            new Among("söömata", 209, 9),
            new Among("sööme", -1, 9),
            new Among("söön", -1, 9),
            new Among("sööte", -1, 9),
            new Among("söövad", -1, 9),
            new Among("süüa", -1, 9),
            new Among("süüakse", 215, 9),
            new Among("teeb", -1, 17),
            new Among("teed", -1, 17),
            new Among("teeks", -1, 17),
            new Among("teeksid", 219, 17),
            new Among("teeksime", 219, 17),
            new Among("teeksin", 219, 17),
            new Among("teeksite", 219, 17),
            new Among("teeme", -1, 17),
            new Among("teen", -1, 17),
            new Among("teete", -1, 17),
            new Among("teevad", -1, 17),
            new Among("tegema", -1, 17),
            new Among("tegemata", 228, 17),
            new Among("teha", -1, 17),
            new Among("tehakse", 230, 17),
            new Among("tehti", -1, 17),
            new Among("toob", -1, 10),
            new Among("tood", -1, 10),
            new Among("toodi", 234, 10),
            new Among("tooks", -1, 10),
            new Among("tooksid", 236, 10),
            new Among("tooksime", 236, 10),
            new Among("tooksin", 236, 10),
            new Among("tooksite", 236, 10),
            new Among("tooma", -1, 10),
            new Among("toomata", 241, 10),
            new Among("toome", -1, 10),
            new Among("toon", -1, 10),
            new Among("toote", -1, 10),
            new Among("toovad", -1, 10),
            new Among("tuua", -1, 10),
            new Among("tuuakse", 247, 10),
            new Among("tõi", -1, 10),
            new Among("tõid", 249, 10),
            new Among("tõime", 249, 10),
            new Among("tõin", 249, 10),
            new Among("tõite", 249, 10),
            new Among("viia", -1, 3),
            new Among("viiakse", 254, 3),
            new Among("viib", -1, 3),
            new Among("viid", -1, 3),
            new Among("viidi", 257, 3),
            new Among("viiks", -1, 3),
            new Among("viiksid", 259, 3),
            new Among("viiksime", 259, 3),
            new Among("viiksin", 259, 3),
            new Among("viiksite", 259, 3),
            new Among("viima", -1, 3),
            new Among("viimata", 264, 3),
            new Among("viime", -1, 3),
            new Among("viin", -1, 3),
            new Among("viisime", -1, 3),
            new Among("viisin", -1, 3),
            new Among("viisite", -1, 3),
            new Among("viite", -1, 3),
            new Among("viivad", -1, 3),
            new Among("võib", -1, 11),
            new Among("võid", -1, 11),
            new Among("võida", 274, 11),
            new Among("võidakse", 275, 11),
            new Among("võidi", 274, 11),
            new Among("võiks", -1, 11),
            new Among("võiksid", 278, 11),
            new Among("võiksime", 278, 11),
            new Among("võiksin", 278, 11),
            new Among("võiksite", 278, 11),
            new Among("võima", -1, 11),
            new Among("võimata", 283, 11),
            new Among("võime", -1, 11),
            new Among("võin", -1, 11),
            new Among("võis", -1, 11),
            new Among("võite", -1, 11),
            new Among("võivad", -1, 11)
        };


        private bool r_mark_regions()
        {
            I_p1 = limit;
            {

                int ret = out_grouping(g_V1, 97, 252, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_V1, 97, 252, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            I_p1 = cursor;
            return true;
        }

        private bool r_emphasis()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_0);
            if (among_var == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            {
                int c2 = limit - cursor;
                {
                    int c = cursor - 4;
                    if (c < limit_backward)
                    {
                        return false;
                    }
                    cursor = c;
                }
                cursor = limit - c2;
            }
            switch (among_var) {
                case 1: {
                    int c3 = limit - cursor;
                    if (in_grouping_b(g_GI, 97, 252, false) != 0)
                    {
                        return false;
                    }
                    cursor = limit - c3;
                    {
                        int c4 = limit - cursor;
                        if (!r_LONGV())
                            goto lab0;
                        return false;
                    lab0: ;
                        cursor = limit - c4;
                    }
                    slice_del();
                    break;
                }
                case 2: {
                    if (in_grouping_b(g_KI, 98, 382, false) != 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_verb()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_1);
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
                    slice_del();
                    break;
                }
                case 2: {
                    slice_from("a");
                    break;
                }
                case 3: {
                    if (in_grouping_b(g_V1, 97, 252, false) != 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_LONGV()
        {
            if (find_among_b(a_2) == 0)
            {
                return false;
            }
            return true;
        }

        private bool r_i_plural()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            if (find_among_b(a_3) == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            if (in_grouping_b(g_RV, 97, 117, false) != 0)
            {
                return false;
            }
            slice_del();
            return true;
        }

        private bool r_special_noun_endings()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_4);
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
                    slice_from("lase");
                    break;
                }
                case 2: {
                    slice_from("mise");
                    break;
                }
                case 3: {
                    slice_from("lise");
                    break;
                }
            }
            return true;
        }

        private bool r_case_ending()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
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
            limit_backward = c1;
            switch (among_var) {
                case 1: {
                    {
                        int c2 = limit - cursor;
                        if (in_grouping_b(g_RV, 97, 117, false) != 0)
                        {
                            goto lab1;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c2;
                        if (!r_LONGV())
                            return false;
                    }
                lab0: ;
                    break;
                }
                case 2: {
                    {
                        int c3 = limit - cursor;
                        {
                            int c = cursor - 4;
                            if (c < limit_backward)
                            {
                                return false;
                            }
                            cursor = c;
                        }
                        cursor = limit - c3;
                    }
                    break;
                }
            }
            slice_del();
            return true;
        }

        private bool r_plural_three_first_cases()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_7);
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
                    slice_from("iku");
                    break;
                }
                case 2: {
                    {
                        int c2 = limit - cursor;
                        if (!r_LONGV())
                            goto lab0;
                        return false;
                    lab0: ;
                        cursor = limit - c2;
                    }
                    slice_del();
                    break;
                }
                case 3: {
                    {
                        int c3 = limit - cursor;
                        {
                            int c4 = limit - cursor;
                            {
                                int c = cursor - 4;
                                if (c < limit_backward)
                                {
                                    goto lab2;
                                }
                                cursor = c;
                            }
                            cursor = limit - c4;
                        }
                        among_var = find_among_b(a_6);
                        switch (among_var) {
                            case 1: {
                                slice_from("e");
                                break;
                            }
                            case 2: {
                                slice_del();
                                break;
                            }
                        }
                        goto lab1;
                    lab2: ;
                        cursor = limit - c3;
                        slice_from("t");
                    }
                lab1: ;
                    break;
                }
                case 4: {
                    {
                        int c5 = limit - cursor;
                        if (in_grouping_b(g_RV, 97, 117, false) != 0)
                        {
                            goto lab4;
                        }
                        goto lab3;
                    lab4: ;
                        cursor = limit - c5;
                        if (!r_LONGV())
                            return false;
                    }
                lab3: ;
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_nu()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            if (find_among_b(a_8) == 0)
            {
                {
                    limit_backward = c1;
                    return false;
                }
            }
            bra = cursor;
            limit_backward = c1;
            slice_del();
            return true;
        }

        private bool r_undouble_kpt()
        {
            int among_var;
            if (in_grouping_b(g_V1, 97, 252, false) != 0)
            {
                return false;
            }
            if (I_p1 > cursor)
            {
                return false;
            }
            ket = cursor;
            among_var = find_among_b(a_9);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_from("k");
                    break;
                }
                case 2: {
                    slice_from("p");
                    break;
                }
                case 3: {
                    slice_from("t");
                    break;
                }
            }
            return true;
        }

        private bool r_degrees()
        {
            int among_var;
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            among_var = find_among_b(a_10);
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
                    if (in_grouping_b(g_RV, 97, 117, false) != 0)
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
                case 2: {
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_substantive()
        {
            {
                int c1 = limit - cursor;
                r_special_noun_endings();
                cursor = limit - c1;
            }
            {
                int c2 = limit - cursor;
                r_case_ending();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_plural_three_first_cases();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_degrees();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                r_i_plural();
                cursor = limit - c5;
            }
            {
                int c6 = limit - cursor;
                r_nu();
                cursor = limit - c6;
            }
            return true;
        }

        private bool r_verb_exceptions()
        {
            int among_var;
            bra = cursor;
            among_var = find_among(a_11);
            if (among_var == 0)
            {
                return false;
            }
            ket = cursor;
            if (cursor < limit)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    slice_from("joo");
                    break;
                }
                case 2: {
                    slice_from("saa");
                    break;
                }
                case 3: {
                    slice_from("viima");
                    break;
                }
                case 4: {
                    slice_from("keesi");
                    break;
                }
                case 5: {
                    slice_from("löö");
                    break;
                }
                case 6: {
                    slice_from("lõi");
                    break;
                }
                case 7: {
                    slice_from("loo");
                    break;
                }
                case 8: {
                    slice_from("käisi");
                    break;
                }
                case 9: {
                    slice_from("söö");
                    break;
                }
                case 10: {
                    slice_from("too");
                    break;
                }
                case 11: {
                    slice_from("võisi");
                    break;
                }
                case 12: {
                    slice_from("jääma");
                    break;
                }
                case 13: {
                    slice_from("müüsi");
                    break;
                }
                case 14: {
                    slice_from("luge");
                    break;
                }
                case 15: {
                    slice_from("põde");
                    break;
                }
                case 16: {
                    slice_from("ladu");
                    break;
                }
                case 17: {
                    slice_from("tegi");
                    break;
                }
                case 18: {
                    slice_from("nägi");
                    break;
                }
            }
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                if (!r_verb_exceptions())
                    goto lab0;
                return false;
            lab0: ;
                cursor = c1;
            }
            {
                int c2 = cursor;
                r_mark_regions();
                cursor = c2;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c3 = limit - cursor;
                r_emphasis();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                {
                    int c5 = limit - cursor;
                    if (!r_verb())
                        goto lab3;
                    goto lab2;
                lab3: ;
                    cursor = limit - c5;
                    r_substantive();
                }
            lab2: ;
                cursor = limit - c4;
            }
            {
                int c6 = limit - cursor;
                r_undouble_kpt();
                cursor = limit - c6;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

