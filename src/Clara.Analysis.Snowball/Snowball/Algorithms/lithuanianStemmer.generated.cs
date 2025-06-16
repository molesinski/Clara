// Generated from lithuanian.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from lithuanian.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class LithuanianStemmer : Stemmer
    {
        private int I_p1;

        private const string g_v = "aeiyouąęįųėū";

        private static readonly Among[] a_0 = new[]
        {
            new Among("a", -1, -1),
            new Among("ia", 0, -1),
            new Among("eria", 1, -1),
            new Among("osna", 0, -1),
            new Among("iosna", 3, -1),
            new Among("uosna", 3, -1),
            new Among("iuosna", 5, -1),
            new Among("ysna", 0, -1),
            new Among("ėsna", 0, -1),
            new Among("e", -1, -1),
            new Among("ie", 9, -1),
            new Among("enie", 10, -1),
            new Among("erie", 10, -1),
            new Among("oje", 9, -1),
            new Among("ioje", 13, -1),
            new Among("uje", 9, -1),
            new Among("iuje", 15, -1),
            new Among("yje", 9, -1),
            new Among("enyje", 17, -1),
            new Among("eryje", 17, -1),
            new Among("ėje", 9, -1),
            new Among("ame", 9, -1),
            new Among("iame", 21, -1),
            new Among("sime", 9, -1),
            new Among("ome", 9, -1),
            new Among("ėme", 9, -1),
            new Among("tumėme", 25, -1),
            new Among("ose", 9, -1),
            new Among("iose", 27, -1),
            new Among("uose", 27, -1),
            new Among("iuose", 29, -1),
            new Among("yse", 9, -1),
            new Among("enyse", 31, -1),
            new Among("eryse", 31, -1),
            new Among("ėse", 9, -1),
            new Among("ate", 9, -1),
            new Among("iate", 35, -1),
            new Among("ite", 9, -1),
            new Among("kite", 37, -1),
            new Among("site", 37, -1),
            new Among("ote", 9, -1),
            new Among("tute", 9, -1),
            new Among("ėte", 9, -1),
            new Among("tumėte", 42, -1),
            new Among("i", -1, -1),
            new Among("ai", 44, -1),
            new Among("iai", 45, -1),
            new Among("eriai", 46, -1),
            new Among("ei", 44, -1),
            new Among("tumei", 48, -1),
            new Among("ki", 44, -1),
            new Among("imi", 44, -1),
            new Among("erimi", 51, -1),
            new Among("umi", 44, -1),
            new Among("iumi", 53, -1),
            new Among("si", 44, -1),
            new Among("asi", 55, -1),
            new Among("iasi", 56, -1),
            new Among("esi", 55, -1),
            new Among("iesi", 58, -1),
            new Among("siesi", 59, -1),
            new Among("isi", 55, -1),
            new Among("aisi", 61, -1),
            new Among("eisi", 61, -1),
            new Among("tumeisi", 63, -1),
            new Among("uisi", 61, -1),
            new Among("osi", 55, -1),
            new Among("ėjosi", 66, -1),
            new Among("uosi", 66, -1),
            new Among("iuosi", 68, -1),
            new Among("siuosi", 69, -1),
            new Among("usi", 55, -1),
            new Among("ausi", 71, -1),
            new Among("čiausi", 72, -1),
            new Among("ąsi", 55, -1),
            new Among("ėsi", 55, -1),
            new Among("ųsi", 55, -1),
            new Among("tųsi", 76, -1),
            new Among("ti", 44, -1),
            new Among("enti", 78, -1),
            new Among("inti", 78, -1),
            new Among("oti", 78, -1),
            new Among("ioti", 81, -1),
            new Among("uoti", 81, -1),
            new Among("iuoti", 83, -1),
            new Among("auti", 78, -1),
            new Among("iauti", 85, -1),
            new Among("yti", 78, -1),
            new Among("ėti", 78, -1),
            new Among("telėti", 88, -1),
            new Among("inėti", 88, -1),
            new Among("terėti", 88, -1),
            new Among("ui", 44, -1),
            new Among("iui", 92, -1),
            new Among("eniui", 93, -1),
            new Among("oj", -1, -1),
            new Among("ėj", -1, -1),
            new Among("k", -1, -1),
            new Among("am", -1, -1),
            new Among("iam", 98, -1),
            new Among("iem", -1, -1),
            new Among("im", -1, -1),
            new Among("sim", 101, -1),
            new Among("om", -1, -1),
            new Among("tum", -1, -1),
            new Among("ėm", -1, -1),
            new Among("tumėm", 105, -1),
            new Among("an", -1, -1),
            new Among("on", -1, -1),
            new Among("ion", 108, -1),
            new Among("un", -1, -1),
            new Among("iun", 110, -1),
            new Among("ėn", -1, -1),
            new Among("o", -1, -1),
            new Among("io", 113, -1),
            new Among("enio", 114, -1),
            new Among("ėjo", 113, -1),
            new Among("uo", 113, -1),
            new Among("s", -1, -1),
            new Among("as", 118, -1),
            new Among("ias", 119, -1),
            new Among("es", 118, -1),
            new Among("ies", 121, -1),
            new Among("is", 118, -1),
            new Among("ais", 123, -1),
            new Among("iais", 124, -1),
            new Among("tumeis", 123, -1),
            new Among("imis", 123, -1),
            new Among("enimis", 127, -1),
            new Among("omis", 123, -1),
            new Among("iomis", 129, -1),
            new Among("umis", 123, -1),
            new Among("ėmis", 123, -1),
            new Among("enis", 123, -1),
            new Among("asis", 123, -1),
            new Among("ysis", 123, -1),
            new Among("ams", 118, -1),
            new Among("iams", 136, -1),
            new Among("iems", 118, -1),
            new Among("ims", 118, -1),
            new Among("enims", 139, -1),
            new Among("erims", 139, -1),
            new Among("oms", 118, -1),
            new Among("ioms", 142, -1),
            new Among("ums", 118, -1),
            new Among("ėms", 118, -1),
            new Among("ens", 118, -1),
            new Among("os", 118, -1),
            new Among("ios", 147, -1),
            new Among("uos", 147, -1),
            new Among("iuos", 149, -1),
            new Among("ers", 118, -1),
            new Among("us", 118, -1),
            new Among("aus", 152, -1),
            new Among("iaus", 153, -1),
            new Among("ius", 152, -1),
            new Among("ys", 118, -1),
            new Among("enys", 156, -1),
            new Among("erys", 156, -1),
            new Among("ąs", 118, -1),
            new Among("iąs", 159, -1),
            new Among("ės", 118, -1),
            new Among("amės", 161, -1),
            new Among("iamės", 162, -1),
            new Among("imės", 161, -1),
            new Among("kimės", 164, -1),
            new Among("simės", 164, -1),
            new Among("omės", 161, -1),
            new Among("ėmės", 161, -1),
            new Among("tumėmės", 168, -1),
            new Among("atės", 161, -1),
            new Among("iatės", 170, -1),
            new Among("sitės", 161, -1),
            new Among("otės", 161, -1),
            new Among("ėtės", 161, -1),
            new Among("tumėtės", 174, -1),
            new Among("įs", 118, -1),
            new Among("ūs", 118, -1),
            new Among("tųs", 118, -1),
            new Among("at", -1, -1),
            new Among("iat", 179, -1),
            new Among("it", -1, -1),
            new Among("sit", 181, -1),
            new Among("ot", -1, -1),
            new Among("ėt", -1, -1),
            new Among("tumėt", 184, -1),
            new Among("u", -1, -1),
            new Among("au", 186, -1),
            new Among("iau", 187, -1),
            new Among("čiau", 188, -1),
            new Among("iu", 186, -1),
            new Among("eniu", 190, -1),
            new Among("siu", 190, -1),
            new Among("y", -1, -1),
            new Among("ą", -1, -1),
            new Among("ią", 194, -1),
            new Among("ė", -1, -1),
            new Among("ę", -1, -1),
            new Among("į", -1, -1),
            new Among("enį", 198, -1),
            new Among("erį", 198, -1),
            new Among("ų", -1, -1),
            new Among("ių", 201, -1),
            new Among("erų", 201, -1)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("ing", -1, -1),
            new Among("aj", -1, -1),
            new Among("iaj", 1, -1),
            new Among("iej", -1, -1),
            new Among("oj", -1, -1),
            new Among("ioj", 4, -1),
            new Among("uoj", 4, -1),
            new Among("iuoj", 6, -1),
            new Among("auj", -1, -1),
            new Among("ąj", -1, -1),
            new Among("iąj", 9, -1),
            new Among("ėj", -1, -1),
            new Among("ųj", -1, -1),
            new Among("iųj", 12, -1),
            new Among("ok", -1, -1),
            new Among("iok", 14, -1),
            new Among("iuk", -1, -1),
            new Among("uliuk", 16, -1),
            new Among("učiuk", 16, -1),
            new Among("išk", -1, -1),
            new Among("iul", -1, -1),
            new Among("yl", -1, -1),
            new Among("ėl", -1, -1),
            new Among("am", -1, -1),
            new Among("dam", 23, -1),
            new Among("jam", 23, -1),
            new Among("zgan", -1, -1),
            new Among("ain", -1, -1),
            new Among("esn", -1, -1),
            new Among("op", -1, -1),
            new Among("iop", 29, -1),
            new Among("ias", -1, -1),
            new Among("ies", -1, -1),
            new Among("ais", -1, -1),
            new Among("iais", 33, -1),
            new Among("os", -1, -1),
            new Among("ios", 35, -1),
            new Among("uos", 35, -1),
            new Among("iuos", 37, -1),
            new Among("aus", -1, -1),
            new Among("iaus", 39, -1),
            new Among("ąs", -1, -1),
            new Among("iąs", 41, -1),
            new Among("ęs", -1, -1),
            new Among("utėait", -1, -1),
            new Among("ant", -1, -1),
            new Among("iant", 45, -1),
            new Among("siant", 46, -1),
            new Among("int", -1, -1),
            new Among("ot", -1, -1),
            new Among("uot", 49, -1),
            new Among("iuot", 50, -1),
            new Among("yt", -1, -1),
            new Among("ėt", -1, -1),
            new Among("ykšt", -1, -1),
            new Among("iau", -1, -1),
            new Among("dav", -1, -1),
            new Among("sv", -1, -1),
            new Among("šv", -1, -1),
            new Among("ykšč", -1, -1),
            new Among("ę", -1, -1),
            new Among("ėję", 60, -1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ojime", -1, 7),
            new Among("ėjime", -1, 3),
            new Among("avime", -1, 6),
            new Among("okate", -1, 8),
            new Among("aite", -1, 1),
            new Among("uote", -1, 2),
            new Among("asius", -1, 5),
            new Among("okatės", -1, 8),
            new Among("aitės", -1, 1),
            new Among("uotės", -1, 2),
            new Among("esiu", -1, 4)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("č", -1, 1),
            new Among("dž", -1, 2)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("gd", -1, 1)
        };


        private bool r_step1()
        {
            if (cursor < I_p1)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_p1;
            ket = cursor;
            if (find_among_b(a_0) == 0)
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

        private bool r_step2()
        {
            while (true)
            {
                int c1 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab0;
                }
                int c2 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                if (find_among_b(a_1) == 0)
                {
                    {
                        limit_backward = c2;
                        goto lab0;
                    }
                }
                bra = cursor;
                limit_backward = c2;
                slice_del();
                continue;
            lab0: ;
                cursor = limit - c1;
                break;
            }
            return true;
        }

        private bool r_fix_conflicts()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_2);
            if (among_var == 0)
            {
                return false;
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
            return true;
        }

        private bool r_fix_chdz()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_3);
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

        private bool r_fix_gd()
        {
            ket = cursor;
            if (find_among_b(a_4) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_from("g");
            return true;
        }

        protected override bool stem()
        {
            I_p1 = limit;
            {
                int c1 = cursor;
                {
                    int c2 = cursor;
                    {
                        int c3 = cursor;
                        if (!(eq_s("a")))
                        {
                            {
                                cursor = c2;
                                goto lab1;
                            }
                        }
                        cursor = c3;
                    }
                    if (current.Length <= 6)
                    {
                        {
                            cursor = c2;
                            goto lab1;
                        }
                    }
                    if (cursor >= limit)
                    {
                        {
                            cursor = c2;
                            goto lab1;
                        }
                    }
                    cursor++;
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
                int c4 = limit - cursor;
                r_fix_conflicts();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                r_step1();
                cursor = limit - c5;
            }
            {
                int c6 = limit - cursor;
                r_fix_chdz();
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                r_step2();
                cursor = limit - c7;
            }
            {
                int c8 = limit - cursor;
                r_fix_chdz();
                cursor = limit - c8;
            }
            {
                int c9 = limit - cursor;
                r_fix_gd();
                cursor = limit - c9;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

