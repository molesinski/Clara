// Generated from romanian.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from romanian.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class RomanianStemmer : Stemmer
    {
        private const string g_v = "aeiouâîă";

        private static readonly Among[] a_0 = new[]
        {
            new Among("ş", -1, 1, 0),
            new Among("ţ", -1, 2, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("", -1, 3, 0),
            new Among("I", 0, 1, 0),
            new Among("U", 0, 2, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ea", -1, 3, 0),
            new Among("ația", -1, 7, 0),
            new Among("aua", -1, 2, 0),
            new Among("iua", -1, 4, 0),
            new Among("ație", -1, 7, 0),
            new Among("ele", -1, 3, 0),
            new Among("ile", -1, 5, 0),
            new Among("iile", 6, 4, 0),
            new Among("iei", -1, 4, 0),
            new Among("atei", -1, 6, 0),
            new Among("ii", -1, 4, 0),
            new Among("ului", -1, 1, 0),
            new Among("ul", -1, 1, 0),
            new Among("elor", -1, 3, 0),
            new Among("ilor", -1, 4, 0),
            new Among("iilor", 14, 4, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("icala", -1, 4, 0),
            new Among("iciva", -1, 4, 0),
            new Among("ativa", -1, 5, 0),
            new Among("itiva", -1, 6, 0),
            new Among("icale", -1, 4, 0),
            new Among("ațiune", -1, 5, 0),
            new Among("ițiune", -1, 6, 0),
            new Among("atoare", -1, 5, 0),
            new Among("itoare", -1, 6, 0),
            new Among("ătoare", -1, 5, 0),
            new Among("icitate", -1, 4, 0),
            new Among("abilitate", -1, 1, 0),
            new Among("ibilitate", -1, 2, 0),
            new Among("ivitate", -1, 3, 0),
            new Among("icive", -1, 4, 0),
            new Among("ative", -1, 5, 0),
            new Among("itive", -1, 6, 0),
            new Among("icali", -1, 4, 0),
            new Among("atori", -1, 5, 0),
            new Among("icatori", 18, 4, 0),
            new Among("itori", -1, 6, 0),
            new Among("ători", -1, 5, 0),
            new Among("icitati", -1, 4, 0),
            new Among("abilitati", -1, 1, 0),
            new Among("ivitati", -1, 3, 0),
            new Among("icivi", -1, 4, 0),
            new Among("ativi", -1, 5, 0),
            new Among("itivi", -1, 6, 0),
            new Among("icităi", -1, 4, 0),
            new Among("abilităi", -1, 1, 0),
            new Among("ivităi", -1, 3, 0),
            new Among("icități", -1, 4, 0),
            new Among("abilități", -1, 1, 0),
            new Among("ivități", -1, 3, 0),
            new Among("ical", -1, 4, 0),
            new Among("ator", -1, 5, 0),
            new Among("icator", 35, 4, 0),
            new Among("itor", -1, 6, 0),
            new Among("ător", -1, 5, 0),
            new Among("iciv", -1, 4, 0),
            new Among("ativ", -1, 5, 0),
            new Among("itiv", -1, 6, 0),
            new Among("icală", -1, 4, 0),
            new Among("icivă", -1, 4, 0),
            new Among("ativă", -1, 5, 0),
            new Among("itivă", -1, 6, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("ica", -1, 1, 0),
            new Among("abila", -1, 1, 0),
            new Among("ibila", -1, 1, 0),
            new Among("oasa", -1, 1, 0),
            new Among("ata", -1, 1, 0),
            new Among("ita", -1, 1, 0),
            new Among("anta", -1, 1, 0),
            new Among("ista", -1, 3, 0),
            new Among("uta", -1, 1, 0),
            new Among("iva", -1, 1, 0),
            new Among("ic", -1, 1, 0),
            new Among("ice", -1, 1, 0),
            new Among("abile", -1, 1, 0),
            new Among("ibile", -1, 1, 0),
            new Among("isme", -1, 3, 0),
            new Among("iune", -1, 2, 0),
            new Among("oase", -1, 1, 0),
            new Among("ate", -1, 1, 0),
            new Among("itate", 17, 1, 0),
            new Among("ite", -1, 1, 0),
            new Among("ante", -1, 1, 0),
            new Among("iste", -1, 3, 0),
            new Among("ute", -1, 1, 0),
            new Among("ive", -1, 1, 0),
            new Among("ici", -1, 1, 0),
            new Among("abili", -1, 1, 0),
            new Among("ibili", -1, 1, 0),
            new Among("iuni", -1, 2, 0),
            new Among("atori", -1, 1, 0),
            new Among("osi", -1, 1, 0),
            new Among("ati", -1, 1, 0),
            new Among("itati", 30, 1, 0),
            new Among("iti", -1, 1, 0),
            new Among("anti", -1, 1, 0),
            new Among("isti", -1, 3, 0),
            new Among("uti", -1, 1, 0),
            new Among("iști", -1, 3, 0),
            new Among("ivi", -1, 1, 0),
            new Among("ităi", -1, 1, 0),
            new Among("oși", -1, 1, 0),
            new Among("ități", -1, 1, 0),
            new Among("abil", -1, 1, 0),
            new Among("ibil", -1, 1, 0),
            new Among("ism", -1, 3, 0),
            new Among("ator", -1, 1, 0),
            new Among("os", -1, 1, 0),
            new Among("at", -1, 1, 0),
            new Among("it", -1, 1, 0),
            new Among("ant", -1, 1, 0),
            new Among("ist", -1, 3, 0),
            new Among("ut", -1, 1, 0),
            new Among("iv", -1, 1, 0),
            new Among("ică", -1, 1, 0),
            new Among("abilă", -1, 1, 0),
            new Among("ibilă", -1, 1, 0),
            new Among("oasă", -1, 1, 0),
            new Among("ată", -1, 1, 0),
            new Among("ită", -1, 1, 0),
            new Among("antă", -1, 1, 0),
            new Among("istă", -1, 3, 0),
            new Among("ută", -1, 1, 0),
            new Among("ivă", -1, 1, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ea", -1, 1, 0),
            new Among("ia", -1, 1, 0),
            new Among("esc", -1, 1, 0),
            new Among("ăsc", -1, 1, 0),
            new Among("ind", -1, 1, 0),
            new Among("ând", -1, 1, 0),
            new Among("are", -1, 1, 0),
            new Among("ere", -1, 1, 0),
            new Among("ire", -1, 1, 0),
            new Among("âre", -1, 1, 0),
            new Among("se", -1, 2, 0),
            new Among("ase", 10, 1, 0),
            new Among("sese", 10, 2, 0),
            new Among("ise", 10, 1, 0),
            new Among("use", 10, 1, 0),
            new Among("âse", 10, 1, 0),
            new Among("ește", -1, 1, 0),
            new Among("ăște", -1, 1, 0),
            new Among("eze", -1, 1, 0),
            new Among("ai", -1, 1, 0),
            new Among("eai", 19, 1, 0),
            new Among("iai", 19, 1, 0),
            new Among("sei", -1, 2, 0),
            new Among("ești", -1, 1, 0),
            new Among("ăști", -1, 1, 0),
            new Among("ui", -1, 1, 0),
            new Among("ezi", -1, 1, 0),
            new Among("âi", -1, 1, 0),
            new Among("ași", -1, 1, 0),
            new Among("seși", -1, 2, 0),
            new Among("aseși", 29, 1, 0),
            new Among("seseși", 29, 2, 0),
            new Among("iseși", 29, 1, 0),
            new Among("useși", 29, 1, 0),
            new Among("âseși", 29, 1, 0),
            new Among("iși", -1, 1, 0),
            new Among("uși", -1, 1, 0),
            new Among("âși", -1, 1, 0),
            new Among("ați", -1, 2, 0),
            new Among("eați", 38, 1, 0),
            new Among("iați", 38, 1, 0),
            new Among("eți", -1, 2, 0),
            new Among("iți", -1, 2, 0),
            new Among("âți", -1, 2, 0),
            new Among("arăți", -1, 1, 0),
            new Among("serăți", -1, 2, 0),
            new Among("aserăți", 45, 1, 0),
            new Among("seserăți", 45, 2, 0),
            new Among("iserăți", 45, 1, 0),
            new Among("userăți", 45, 1, 0),
            new Among("âserăți", 45, 1, 0),
            new Among("irăți", -1, 1, 0),
            new Among("urăți", -1, 1, 0),
            new Among("ârăți", -1, 1, 0),
            new Among("am", -1, 1, 0),
            new Among("eam", 54, 1, 0),
            new Among("iam", 54, 1, 0),
            new Among("em", -1, 2, 0),
            new Among("asem", 57, 1, 0),
            new Among("sesem", 57, 2, 0),
            new Among("isem", 57, 1, 0),
            new Among("usem", 57, 1, 0),
            new Among("âsem", 57, 1, 0),
            new Among("im", -1, 2, 0),
            new Among("âm", -1, 2, 0),
            new Among("ăm", -1, 2, 0),
            new Among("arăm", 65, 1, 0),
            new Among("serăm", 65, 2, 0),
            new Among("aserăm", 67, 1, 0),
            new Among("seserăm", 67, 2, 0),
            new Among("iserăm", 67, 1, 0),
            new Among("userăm", 67, 1, 0),
            new Among("âserăm", 67, 1, 0),
            new Among("irăm", 65, 1, 0),
            new Among("urăm", 65, 1, 0),
            new Among("ârăm", 65, 1, 0),
            new Among("au", -1, 1, 0),
            new Among("eau", 76, 1, 0),
            new Among("iau", 76, 1, 0),
            new Among("indu", -1, 1, 0),
            new Among("ându", -1, 1, 0),
            new Among("ez", -1, 1, 0),
            new Among("ească", -1, 1, 0),
            new Among("ară", -1, 1, 0),
            new Among("seră", -1, 2, 0),
            new Among("aseră", 84, 1, 0),
            new Among("seseră", 84, 2, 0),
            new Among("iseră", 84, 1, 0),
            new Among("useră", 84, 1, 0),
            new Among("âseră", 84, 1, 0),
            new Among("iră", -1, 1, 0),
            new Among("ură", -1, 1, 0),
            new Among("âră", -1, 1, 0),
            new Among("ează", -1, 1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("a", -1, 1, 0),
            new Among("e", -1, 1, 0),
            new Among("ie", 1, 1, 0),
            new Among("i", -1, 1, 0),
            new Among("ă", -1, 1, 0)
        };


        protected override bool stem()
        {
            int among_var;
            bool B_standard_suffix_removed;
            int I_p2;
            int I_p1;
            int I_pV;
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    while (true)
                    {
                        int c3 = cursor;
                        bra = cursor;
                        among_var = find_among(a_0, null);
                        if (among_var == 0)
                        {
                            goto lab3;
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
                    lab3: ;
                        cursor = c3;
                        if (cursor >= limit)
                        {
                            goto lab2;
                        }
                        cursor++;
                    }
                    continue;
                lab2: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            {
                int c4 = cursor;
                while (true)
                {
                    int c5 = cursor;
                    while (true)
                    {
                        int c6 = cursor;
                        if (in_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab6;
                        }
                        bra = cursor;
                        {
                            int c7 = cursor;
                            if (!(eq_s("u")))
                            {
                                goto lab8;
                            }
                            ket = cursor;
                            if (in_grouping(g_v, 97, 259, false) != 0)
                            {
                                goto lab8;
                            }
                            slice_from("U");
                            goto lab7;
                        lab8: ;
                            cursor = c7;
                            if (!(eq_s("i")))
                            {
                                goto lab6;
                            }
                            ket = cursor;
                            if (in_grouping(g_v, 97, 259, false) != 0)
                            {
                                goto lab6;
                            }
                            slice_from("I");
                        }
                    lab7: ;
                        cursor = c6;
                        break;
                    lab6: ;
                        cursor = c6;
                        if (cursor >= limit)
                        {
                            goto lab5;
                        }
                        cursor++;
                    }
                    continue;
                lab5: ;
                    cursor = c5;
                    break;
                }
                cursor = c4;
            }
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            {
                int c8 = cursor;
                {
                    int c9 = cursor;
                    if (in_grouping(g_v, 97, 259, false) != 0)
                    {
                        goto lab12;
                    }
                    {
                        int c10 = cursor;
                        if (out_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab14;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 259, true);
                            if (ret < 0)
                            {
                                goto lab14;
                            }

                            cursor += ret;
                        }
                        goto lab13;
                    lab14: ;
                        cursor = c10;
                        if (in_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab12;
                        }
                        {

                            int ret = in_grouping(g_v, 97, 259, true);
                            if (ret < 0)
                            {
                                goto lab12;
                            }

                            cursor += ret;
                        }
                    }
                lab13: ;
                    goto lab11;
                lab12: ;
                    cursor = c9;
                    if (out_grouping(g_v, 97, 259, false) != 0)
                    {
                        goto lab10;
                    }
                    {
                        int c11 = cursor;
                        if (out_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab16;
                        }
                        {

                            int ret = out_grouping(g_v, 97, 259, true);
                            if (ret < 0)
                            {
                                goto lab16;
                            }

                            cursor += ret;
                        }
                        goto lab15;
                    lab16: ;
                        cursor = c11;
                        if (in_grouping(g_v, 97, 259, false) != 0)
                        {
                            goto lab10;
                        }
                        if (cursor >= limit)
                        {
                            goto lab10;
                        }
                        cursor++;
                    }
                lab15: ;
                }
            lab11: ;
                I_pV = cursor;
            lab10: ;
                cursor = c8;
            }
            {
                int c12 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab17;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab17;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab17;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 259, true);
                    if (ret < 0)
                    {
                        goto lab17;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab17: ;
                cursor = c12;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c13 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_2, null);
                if (among_var == 0)
                {
                    goto lab18;
                }
                bra = cursor;
                if (I_p1 > cursor)
                {
                    goto lab18;
                }
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
                        if (!(eq_s_b("ab")))
                        {
                            goto lab19;
                        }
                        goto lab18;
                    lab19: ;
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
            lab18: ;
                cursor = limit - c13;
            }
            {
                int c14 = limit - cursor;
                B_standard_suffix_removed = false;
                while (true)
                {
                    int c15 = limit - cursor;
                    {
                        int c16 = limit - cursor;
                        ket = cursor;
                        among_var = find_among_b(a_3, null);
                        if (among_var == 0)
                        {
                            goto lab21;
                        }
                        bra = cursor;
                        if (I_p1 > cursor)
                        {
                            goto lab21;
                        }
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
                        cursor = limit - c16;
                    }
                    continue;
                lab21: ;
                    cursor = limit - c15;
                    break;
                }
                ket = cursor;
                among_var = find_among_b(a_4, null);
                if (among_var == 0)
                {
                    goto lab20;
                }
                bra = cursor;
                if (I_p2 > cursor)
                {
                    goto lab20;
                }
                switch (among_var) {
                    case 1: {
                        slice_del();
                        break;
                    }
                    case 2: {
                        if (!(eq_s_b("ț")))
                        {
                            goto lab20;
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
            lab20: ;
                cursor = limit - c14;
            }
            {
                int c17 = limit - cursor;
                if (!B_standard_suffix_removed)
                {
                    goto lab24;
                }
                goto lab23;
            lab24: ;
                if (cursor < I_pV)
                {
                    goto lab22;
                }
                int c18 = limit_backward;
                limit_backward = I_pV;
                ket = cursor;
                among_var = find_among_b(a_5, null);
                if (among_var == 0)
                {
                    {
                        limit_backward = c18;
                        goto lab22;
                    }
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        if (out_grouping_b(g_v, 97, 259, false) != 0)
                        {
                            goto lab26;
                        }
                        goto lab25;
                    lab26: ;
                        if (!(eq_s_b("u")))
                        {
                            {
                                limit_backward = c18;
                                goto lab22;
                            }
                        }
                    lab25: ;
                        slice_del();
                        break;
                    }
                    case 2: {
                        slice_del();
                        break;
                    }
                }
                limit_backward = c18;
            lab23: ;
            lab22: ;
                cursor = limit - c17;
            }
            {
                int c19 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_6, null) == 0)
                {
                    goto lab27;
                }
                bra = cursor;
                if (I_pV > cursor)
                {
                    goto lab27;
                }
                slice_del();
            lab27: ;
                cursor = limit - c19;
            }
            cursor = limit_backward;
            {
                int c20 = cursor;
                while (true)
                {
                    int c21 = cursor;
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
                            if (cursor >= limit)
                            {
                                goto lab29;
                            }
                            cursor++;
                            break;
                        }
                    }
                    continue;
                lab29: ;
                    cursor = c21;
                    break;
                }
                cursor = c20;
            }
            return true;
        }

    }
}

