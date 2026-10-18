// Generated from lithuanian.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from lithuanian.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class LithuanianStemmer : Stemmer
    {
        private const string g_v = "aeiouyąėęįūų";

        private static readonly Among[] a_0 = new[]
        {
            new Among("a", -1, -1, 0),
            new Among("ia", 0, -1, 0),
            new Among("osna", 0, -1, 0),
            new Among("iosna", 2, -1, 0),
            new Among("uosna", 2, -1, 0),
            new Among("iuosna", 4, -1, 0),
            new Among("ysna", 0, -1, 0),
            new Among("ėsna", 0, -1, 0),
            new Among("e", -1, -1, 0),
            new Among("ie", 8, -1, 0),
            new Among("enie", 9, -1, 0),
            new Among("oje", 8, -1, 0),
            new Among("ioje", 11, -1, 0),
            new Among("uje", 8, -1, 0),
            new Among("iuje", 13, -1, 0),
            new Among("yje", 8, -1, 0),
            new Among("enyje", 15, -1, 0),
            new Among("ėje", 8, -1, 0),
            new Among("ame", 8, -1, 0),
            new Among("iame", 18, -1, 0),
            new Among("sime", 8, -1, 0),
            new Among("ome", 8, -1, 0),
            new Among("ėme", 8, -1, 0),
            new Among("tumėme", 22, -1, 0),
            new Among("ose", 8, -1, 0),
            new Among("iose", 24, -1, 0),
            new Among("uose", 24, -1, 0),
            new Among("iuose", 26, -1, 0),
            new Among("yse", 8, -1, 0),
            new Among("enyse", 28, -1, 0),
            new Among("ėse", 8, -1, 0),
            new Among("ate", 8, -1, 0),
            new Among("iate", 31, -1, 0),
            new Among("ite", 8, -1, 0),
            new Among("kite", 33, -1, 0),
            new Among("site", 33, -1, 0),
            new Among("ote", 8, -1, 0),
            new Among("tute", 8, -1, 0),
            new Among("ėte", 8, -1, 0),
            new Among("tumėte", 38, -1, 0),
            new Among("i", -1, -1, 0),
            new Among("ai", 40, -1, 0),
            new Among("iai", 41, -1, 0),
            new Among("ei", 40, -1, 0),
            new Among("tumei", 43, -1, 0),
            new Among("ki", 40, -1, 0),
            new Among("imi", 40, -1, 0),
            new Among("umi", 40, -1, 0),
            new Among("iumi", 47, -1, 0),
            new Among("si", 40, -1, 0),
            new Among("asi", 49, -1, 0),
            new Among("iasi", 50, -1, 0),
            new Among("esi", 49, -1, 0),
            new Among("iesi", 52, -1, 0),
            new Among("siesi", 53, -1, 0),
            new Among("isi", 49, -1, 0),
            new Among("aisi", 55, -1, 0),
            new Among("eisi", 55, -1, 0),
            new Among("tumeisi", 57, -1, 0),
            new Among("uisi", 55, -1, 0),
            new Among("osi", 49, -1, 0),
            new Among("ėjosi", 60, -1, 0),
            new Among("uosi", 60, -1, 0),
            new Among("iuosi", 62, -1, 0),
            new Among("siuosi", 63, -1, 0),
            new Among("usi", 49, -1, 0),
            new Among("ausi", 65, -1, 0),
            new Among("čiausi", 66, -1, 0),
            new Among("ąsi", 49, -1, 0),
            new Among("ėsi", 49, -1, 0),
            new Among("ųsi", 49, -1, 0),
            new Among("tųsi", 70, -1, 0),
            new Among("ti", 40, -1, 0),
            new Among("enti", 72, -1, 0),
            new Among("inti", 72, -1, 0),
            new Among("oti", 72, -1, 0),
            new Among("ioti", 75, -1, 0),
            new Among("uoti", 75, -1, 0),
            new Among("iuoti", 77, -1, 0),
            new Among("auti", 72, -1, 0),
            new Among("iauti", 79, -1, 0),
            new Among("yti", 72, -1, 0),
            new Among("ėti", 72, -1, 0),
            new Among("telėti", 82, -1, 0),
            new Among("inėti", 82, -1, 0),
            new Among("terėti", 82, -1, 0),
            new Among("ui", 40, -1, 0),
            new Among("iui", 86, -1, 0),
            new Among("eniui", 87, -1, 0),
            new Among("oj", -1, -1, 0),
            new Among("ėj", -1, -1, 0),
            new Among("k", -1, -1, 0),
            new Among("am", -1, -1, 0),
            new Among("iam", 92, -1, 0),
            new Among("iem", -1, -1, 0),
            new Among("im", -1, -1, 0),
            new Among("sim", 95, -1, 0),
            new Among("om", -1, -1, 0),
            new Among("tum", -1, -1, 0),
            new Among("ėm", -1, -1, 0),
            new Among("tumėm", 99, -1, 0),
            new Among("an", -1, -1, 0),
            new Among("on", -1, -1, 0),
            new Among("ion", 102, -1, 0),
            new Among("un", -1, -1, 0),
            new Among("iun", 104, -1, 0),
            new Among("ėn", -1, -1, 0),
            new Among("o", -1, -1, 0),
            new Among("io", 107, -1, 0),
            new Among("enio", 108, -1, 0),
            new Among("ėjo", 107, -1, 0),
            new Among("uo", 107, -1, 0),
            new Among("s", -1, -1, 0),
            new Among("as", 112, -1, 0),
            new Among("ias", 113, -1, 0),
            new Among("es", 112, -1, 0),
            new Among("ies", 115, -1, 0),
            new Among("is", 112, -1, 0),
            new Among("ais", 117, -1, 0),
            new Among("iais", 118, -1, 0),
            new Among("tumeis", 117, -1, 0),
            new Among("imis", 117, -1, 0),
            new Among("enimis", 121, -1, 0),
            new Among("omis", 117, -1, 0),
            new Among("iomis", 123, -1, 0),
            new Among("umis", 117, -1, 0),
            new Among("ėmis", 117, -1, 0),
            new Among("enis", 117, -1, 0),
            new Among("asis", 117, -1, 0),
            new Among("ysis", 117, -1, 0),
            new Among("ams", 112, -1, 0),
            new Among("iams", 130, -1, 0),
            new Among("iems", 112, -1, 0),
            new Among("ims", 112, -1, 0),
            new Among("enims", 133, -1, 0),
            new Among("oms", 112, -1, 0),
            new Among("ioms", 135, -1, 0),
            new Among("ums", 112, -1, 0),
            new Among("ėms", 112, -1, 0),
            new Among("ens", 112, -1, 0),
            new Among("os", 112, -1, 0),
            new Among("ios", 140, -1, 0),
            new Among("uos", 140, -1, 0),
            new Among("iuos", 142, -1, 0),
            new Among("us", 112, -1, 0),
            new Among("aus", 144, -1, 0),
            new Among("iaus", 145, -1, 0),
            new Among("ius", 144, -1, 0),
            new Among("ys", 112, -1, 0),
            new Among("enys", 148, -1, 0),
            new Among("ąs", 112, -1, 0),
            new Among("iąs", 150, -1, 0),
            new Among("ės", 112, -1, 0),
            new Among("amės", 152, -1, 0),
            new Among("iamės", 153, -1, 0),
            new Among("imės", 152, -1, 0),
            new Among("kimės", 155, -1, 0),
            new Among("simės", 155, -1, 0),
            new Among("omės", 152, -1, 0),
            new Among("ėmės", 152, -1, 0),
            new Among("tumėmės", 159, -1, 0),
            new Among("atės", 152, -1, 0),
            new Among("iatės", 161, -1, 0),
            new Among("sitės", 152, -1, 0),
            new Among("otės", 152, -1, 0),
            new Among("ėtės", 152, -1, 0),
            new Among("tumėtės", 165, -1, 0),
            new Among("įs", 112, -1, 0),
            new Among("ūs", 112, -1, 0),
            new Among("tųs", 112, -1, 0),
            new Among("at", -1, -1, 0),
            new Among("iat", 170, -1, 0),
            new Among("it", -1, -1, 0),
            new Among("sit", 172, -1, 0),
            new Among("ot", -1, -1, 0),
            new Among("ėt", -1, -1, 0),
            new Among("tumėt", 175, -1, 0),
            new Among("u", -1, -1, 0),
            new Among("au", 177, -1, 0),
            new Among("iau", 178, -1, 0),
            new Among("čiau", 179, -1, 0),
            new Among("iu", 177, -1, 0),
            new Among("eniu", 181, -1, 0),
            new Among("siu", 181, -1, 0),
            new Among("y", -1, -1, 0),
            new Among("ą", -1, -1, 0),
            new Among("ią", 185, -1, 0),
            new Among("ė", -1, -1, 0),
            new Among("ę", -1, -1, 0),
            new Among("į", -1, -1, 0),
            new Among("enį", 189, -1, 0),
            new Among("ų", -1, -1, 0),
            new Among("ių", 191, -1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("ing", -1, -1, 0),
            new Among("aj", -1, -1, 0),
            new Among("iaj", 1, -1, 0),
            new Among("iej", -1, -1, 0),
            new Among("oj", -1, -1, 0),
            new Among("ioj", 4, -1, 0),
            new Among("uoj", 4, -1, 0),
            new Among("iuoj", 6, -1, 0),
            new Among("auj", -1, -1, 0),
            new Among("ąj", -1, -1, 0),
            new Among("iąj", 9, -1, 0),
            new Among("ėj", -1, -1, 0),
            new Among("ųj", -1, -1, 0),
            new Among("iųj", 12, -1, 0),
            new Among("ok", -1, -1, 0),
            new Among("iok", 14, -1, 0),
            new Among("iuk", -1, -1, 0),
            new Among("uliuk", 16, -1, 0),
            new Among("učiuk", 16, -1, 0),
            new Among("išk", -1, -1, 0),
            new Among("iul", -1, -1, 0),
            new Among("yl", -1, -1, 0),
            new Among("ėl", -1, -1, 0),
            new Among("am", -1, -1, 0),
            new Among("dam", 23, -1, 0),
            new Among("jam", 23, -1, 0),
            new Among("zgan", -1, -1, 0),
            new Among("ain", -1, -1, 0),
            new Among("esn", -1, -1, 0),
            new Among("op", -1, -1, 0),
            new Among("iop", 29, -1, 0),
            new Among("ias", -1, -1, 0),
            new Among("ies", -1, -1, 0),
            new Among("ais", -1, -1, 0),
            new Among("iais", 33, -1, 0),
            new Among("os", -1, -1, 0),
            new Among("ios", 35, -1, 0),
            new Among("uos", 35, -1, 0),
            new Among("iuos", 37, -1, 0),
            new Among("aus", -1, -1, 0),
            new Among("iaus", 39, -1, 0),
            new Among("ąs", -1, -1, 0),
            new Among("iąs", 41, -1, 0),
            new Among("ęs", -1, -1, 0),
            new Among("utėait", -1, -1, 0),
            new Among("ant", -1, -1, 0),
            new Among("iant", 45, -1, 0),
            new Among("siant", 46, -1, 0),
            new Among("int", -1, -1, 0),
            new Among("ot", -1, -1, 0),
            new Among("uot", 49, -1, 0),
            new Among("iuot", 50, -1, 0),
            new Among("yt", -1, -1, 0),
            new Among("ėt", -1, -1, 0),
            new Among("ykšt", -1, -1, 0),
            new Among("iau", -1, -1, 0),
            new Among("dav", -1, -1, 0),
            new Among("sv", -1, -1, 0),
            new Among("šv", -1, -1, 0),
            new Among("ykšč", -1, -1, 0),
            new Among("ę", -1, -1, 0),
            new Among("ėję", 60, -1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ojime", -1, 7, 0),
            new Among("ėjime", -1, 3, 0),
            new Among("avime", -1, 6, 0),
            new Among("okate", -1, 8, 0),
            new Among("aite", -1, 1, 0),
            new Among("uote", -1, 2, 0),
            new Among("asius", -1, 5, 0),
            new Among("okatės", -1, 8, 0),
            new Among("aitės", -1, 1, 0),
            new Among("uotės", -1, 2, 0),
            new Among("esiu", -1, 4, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("č", -1, 1, 0),
            new Among("dž", -1, 2, 0)
        };


        private bool r_fix_chdz()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_3, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_from("t");
                    break;
                }
                case 2: {
                    slice_from("d");
                    break;
                }
            }
            return true;
        }

        protected override bool stem()
        {
            int among_var;
            int I_p1;
            I_p1 = limit;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    if (!(eq_s("a")))
                    {
                        {
                            cursor = c2;
                            goto lab1;
                        }
                    }
                    if (current.Length < 7)
                    {
                        {
                            cursor = c2;
                            goto lab1;
                        }
                    }
                lab1: ;
                }
                {

                    int ret = out_grouping(g_v, 97, 371, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 371, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
            lab0: ;
                cursor = c1;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c3 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_2, null);
                if (among_var == 0)
                {
                    goto lab2;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("aitė");
                        break;
                    }
                    case 2: {
                        slice_from("uotė");
                        break;
                    }
                    case 3: {
                        slice_from("ėjimas");
                        break;
                    }
                    case 4: {
                        slice_from("esys");
                        break;
                    }
                    case 5: {
                        slice_from("asys");
                        break;
                    }
                    case 6: {
                        slice_from("avimas");
                        break;
                    }
                    case 7: {
                        slice_from("ojimas");
                        break;
                    }
                    case 8: {
                        slice_from("okatė");
                        break;
                    }
                }
            lab2: ;
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab3;
                }
                int c5 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                if (find_among_b(a_0, null) == 0)
                {
                    {
                        limit_backward = c5;
                        goto lab3;
                    }
                }
                bra = cursor;
                limit_backward = c5;
                slice_del();
            lab3: ;
                cursor = limit - c4;
            }
            {
                int c6 = limit - cursor;
                r_fix_chdz();
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                while (true)
                {
                    int c8 = limit - cursor;
                    if (cursor < I_p1)
                    {
                        goto lab5;
                    }
                    int c9 = limit_backward;
                    limit_backward = I_p1;
                    ket = cursor;
                    if (find_among_b(a_1, null) == 0)
                    {
                        {
                            limit_backward = c9;
                            goto lab5;
                        }
                    }
                    bra = cursor;
                    limit_backward = c9;
                    slice_del();
                    continue;
                lab5: ;
                    cursor = limit - c8;
                    break;
                }
                cursor = limit - c7;
            }
            {
                int c10 = limit - cursor;
                r_fix_chdz();
                cursor = limit - c10;
            }
            {
                int c11 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("gd")))
                {
                    goto lab6;
                }
                bra = cursor;
                slice_from("g");
            lab6: ;
                cursor = limit - c11;
            }
            ket = cursor;
            if (!(eq_s_b("'")))
            {
                return false;
            }
            bra = cursor;
            slice_del();
            cursor = limit_backward;
            return true;
        }

    }
}

