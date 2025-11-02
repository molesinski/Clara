// Generated from hungarian.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from hungarian.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class HungarianStemmer : Stemmer
    {
        private int I_p1;

        private const string g_v = "aeiouáéíóöúüőű";

        private static readonly Among[] a_0 = new[]
        {
            new Among("á", -1, 1, 0),
            new Among("é", -1, 2, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("bb", -1, -1, 0),
            new Among("cc", -1, -1, 0),
            new Among("dd", -1, -1, 0),
            new Among("ff", -1, -1, 0),
            new Among("gg", -1, -1, 0),
            new Among("jj", -1, -1, 0),
            new Among("kk", -1, -1, 0),
            new Among("ll", -1, -1, 0),
            new Among("mm", -1, -1, 0),
            new Among("nn", -1, -1, 0),
            new Among("pp", -1, -1, 0),
            new Among("rr", -1, -1, 0),
            new Among("ccs", -1, -1, 0),
            new Among("ss", -1, -1, 0),
            new Among("zzs", -1, -1, 0),
            new Among("tt", -1, -1, 0),
            new Among("vv", -1, -1, 0),
            new Among("ggy", -1, -1, 0),
            new Among("lly", -1, -1, 0),
            new Among("nny", -1, -1, 0),
            new Among("tty", -1, -1, 0),
            new Among("ssz", -1, -1, 0),
            new Among("zz", -1, -1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("al", -1, 1, 0),
            new Among("el", -1, 1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ba", -1, -1, 0),
            new Among("ra", -1, -1, 0),
            new Among("be", -1, -1, 0),
            new Among("re", -1, -1, 0),
            new Among("ig", -1, -1, 0),
            new Among("nak", -1, -1, 0),
            new Among("nek", -1, -1, 0),
            new Among("val", -1, -1, 0),
            new Among("vel", -1, -1, 0),
            new Among("ul", -1, -1, 0),
            new Among("nál", -1, -1, 0),
            new Among("nél", -1, -1, 0),
            new Among("ból", -1, -1, 0),
            new Among("ról", -1, -1, 0),
            new Among("tól", -1, -1, 0),
            new Among("ül", -1, -1, 0),
            new Among("ből", -1, -1, 0),
            new Among("ről", -1, -1, 0),
            new Among("től", -1, -1, 0),
            new Among("n", -1, -1, 0),
            new Among("an", 19, -1, 0),
            new Among("ban", 20, -1, 0),
            new Among("en", 19, -1, 0),
            new Among("ben", 22, -1, 0),
            new Among("képpen", 22, -1, 0),
            new Among("on", 19, -1, 0),
            new Among("ön", 19, -1, 0),
            new Among("képp", -1, -1, 0),
            new Among("kor", -1, -1, 0),
            new Among("t", -1, -1, 0),
            new Among("at", 29, -1, 0),
            new Among("et", 29, -1, 0),
            new Among("ként", 29, -1, 0),
            new Among("anként", 32, -1, 0),
            new Among("enként", 32, -1, 0),
            new Among("onként", 32, -1, 0),
            new Among("ot", 29, -1, 0),
            new Among("ért", 29, -1, 0),
            new Among("öt", 29, -1, 0),
            new Among("hez", -1, -1, 0),
            new Among("hoz", -1, -1, 0),
            new Among("höz", -1, -1, 0),
            new Among("vá", -1, -1, 0),
            new Among("vé", -1, -1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("án", -1, 2, 0),
            new Among("én", -1, 1, 0),
            new Among("ánként", -1, 2, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("stul", -1, 1, 0),
            new Among("astul", 0, 1, 0),
            new Among("ástul", 0, 2, 0),
            new Among("stül", -1, 1, 0),
            new Among("estül", 3, 1, 0),
            new Among("éstül", 3, 3, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("á", -1, 1, 0),
            new Among("é", -1, 1, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("k", -1, 3, 0),
            new Among("ak", 0, 3, 0),
            new Among("ek", 0, 3, 0),
            new Among("ok", 0, 3, 0),
            new Among("ák", 0, 1, 0),
            new Among("ék", 0, 2, 0),
            new Among("ök", 0, 3, 0)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("éi", -1, 1, 0),
            new Among("áéi", 0, 3, 0),
            new Among("ééi", 0, 2, 0),
            new Among("é", -1, 1, 0),
            new Among("ké", 3, 1, 0),
            new Among("aké", 4, 1, 0),
            new Among("eké", 4, 1, 0),
            new Among("oké", 4, 1, 0),
            new Among("áké", 4, 3, 0),
            new Among("éké", 4, 2, 0),
            new Among("öké", 4, 1, 0),
            new Among("éé", 3, 2, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("a", -1, 1, 0),
            new Among("ja", 0, 1, 0),
            new Among("d", -1, 1, 0),
            new Among("ad", 2, 1, 0),
            new Among("ed", 2, 1, 0),
            new Among("od", 2, 1, 0),
            new Among("ád", 2, 2, 0),
            new Among("éd", 2, 3, 0),
            new Among("öd", 2, 1, 0),
            new Among("e", -1, 1, 0),
            new Among("je", 9, 1, 0),
            new Among("nk", -1, 1, 0),
            new Among("unk", 11, 1, 0),
            new Among("ánk", 11, 2, 0),
            new Among("énk", 11, 3, 0),
            new Among("ünk", 11, 1, 0),
            new Among("uk", -1, 1, 0),
            new Among("juk", 16, 1, 0),
            new Among("ájuk", 17, 2, 0),
            new Among("ük", -1, 1, 0),
            new Among("jük", 19, 1, 0),
            new Among("éjük", 20, 3, 0),
            new Among("m", -1, 1, 0),
            new Among("am", 22, 1, 0),
            new Among("em", 22, 1, 0),
            new Among("om", 22, 1, 0),
            new Among("ám", 22, 2, 0),
            new Among("ém", 22, 3, 0),
            new Among("o", -1, 1, 0),
            new Among("á", -1, 2, 0),
            new Among("é", -1, 3, 0)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("id", -1, 1, 0),
            new Among("aid", 0, 1, 0),
            new Among("jaid", 1, 1, 0),
            new Among("eid", 0, 1, 0),
            new Among("jeid", 3, 1, 0),
            new Among("áid", 0, 2, 0),
            new Among("éid", 0, 3, 0),
            new Among("i", -1, 1, 0),
            new Among("ai", 7, 1, 0),
            new Among("jai", 8, 1, 0),
            new Among("ei", 7, 1, 0),
            new Among("jei", 10, 1, 0),
            new Among("ái", 7, 2, 0),
            new Among("éi", 7, 3, 0),
            new Among("itek", -1, 1, 0),
            new Among("eitek", 14, 1, 0),
            new Among("jeitek", 15, 1, 0),
            new Among("éitek", 14, 3, 0),
            new Among("ik", -1, 1, 0),
            new Among("aik", 18, 1, 0),
            new Among("jaik", 19, 1, 0),
            new Among("eik", 18, 1, 0),
            new Among("jeik", 21, 1, 0),
            new Among("áik", 18, 2, 0),
            new Among("éik", 18, 3, 0),
            new Among("ink", -1, 1, 0),
            new Among("aink", 25, 1, 0),
            new Among("jaink", 26, 1, 0),
            new Among("eink", 25, 1, 0),
            new Among("jeink", 28, 1, 0),
            new Among("áink", 25, 2, 0),
            new Among("éink", 25, 3, 0),
            new Among("aitok", -1, 1, 0),
            new Among("jaitok", 32, 1, 0),
            new Among("áitok", -1, 2, 0),
            new Among("im", -1, 1, 0),
            new Among("aim", 35, 1, 0),
            new Among("jaim", 36, 1, 0),
            new Among("eim", 35, 1, 0),
            new Among("jeim", 38, 1, 0),
            new Among("áim", 35, 2, 0),
            new Among("éim", 35, 3, 0)
        };


        private bool r_mark_regions()
        {
            I_p1 = limit;
            {
                int c1 = cursor;
                if (in_grouping(g_v, 97, 369, false) != 0)
                {
                    goto lab1;
                }
                {
                    int c2 = cursor;
                    {

                        int ret = in_grouping(g_v, 97, 369, true);
                        if (ret < 0)
                        {
                            goto lab2;
                        }

                        cursor += ret;
                    }
                    I_p1 = cursor;
                lab2: ;
                    cursor = c2;
                }
                goto lab0;
            lab1: ;
                cursor = c1;
                {

                    int ret = out_grouping(g_v, 97, 369, true);
                    if (ret < 0)
                    {
                        return false;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
            }
        lab0: ;
            return true;
        }

        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_v_ending()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_0, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            switch (among_var) {
                case 1: {
                    slice_from("a");
                    break;
                }
                case 2: {
                    slice_from("e");
                    break;
                }
            }
            return true;
        }

        private bool r_double()
        {
            {
                int c1 = limit - cursor;
                if (find_among_b(a_1, null) == 0)
                {
                    return false;
                }
                cursor = limit - c1;
            }
            return true;
        }

        private bool r_undouble()
        {
            if (cursor <= limit_backward)
            {
                return false;
            }
            cursor--;
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

        private bool r_instrum()
        {
            ket = cursor;
            if (find_among_b(a_2, null) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            if (!r_double())
                return false;
            slice_del();
            return r_undouble();
        }

        private bool r_case()
        {
            ket = cursor;
            if (find_among_b(a_3, null) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            slice_del();
            return r_v_ending();
        }

        private bool r_case_special()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_4, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            switch (among_var) {
                case 1: {
                    slice_from("e");
                    break;
                }
                case 2: {
                    slice_from("a");
                    break;
                }
            }
            return true;
        }

        private bool r_case_other()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_5, null);
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
            }
            return true;
        }

        private bool r_factive()
        {
            ket = cursor;
            if (find_among_b(a_6, null) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            if (!r_double())
                return false;
            slice_del();
            return r_undouble();
        }

        private bool r_plural()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_7, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            switch (among_var) {
                case 1: {
                    slice_from("a");
                    break;
                }
                case 2: {
                    slice_from("e");
                    break;
                }
                case 3: {
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_owned()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_8, null);
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
                    slice_from("e");
                    break;
                }
                case 3: {
                    slice_from("a");
                    break;
                }
            }
            return true;
        }

        private bool r_sing_owner()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_9, null);
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
            }
            return true;
        }

        private bool r_plur_owner()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_10, null);
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
            }
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                r_mark_regions();
                cursor = c1;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                r_instrum();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_case();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_case_special();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                r_case_other();
                cursor = limit - c5;
            }
            {
                int c6 = limit - cursor;
                r_factive();
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                r_owned();
                cursor = limit - c7;
            }
            {
                int c8 = limit - cursor;
                r_sing_owner();
                cursor = limit - c8;
            }
            {
                int c9 = limit - cursor;
                r_plur_owner();
                cursor = limit - c9;
            }
            {
                int c10 = limit - cursor;
                r_plural();
                cursor = limit - c10;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

