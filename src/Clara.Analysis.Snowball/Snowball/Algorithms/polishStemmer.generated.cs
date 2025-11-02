// Generated from polish.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from polish.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class PolishStemmer : Stemmer
    {
        private int I_p1;

        private const string g_v = "aeiouyóąę";

        private static readonly Among[] a_0 = new[]
        {
            new Among("byście", -1, 1, 0),
            new Among("bym", -1, 1, 0),
            new Among("by", -1, 1, 0),
            new Among("byśmy", -1, 1, 0),
            new Among("byś", -1, 1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("ąc", -1, 1, 0),
            new Among("ając", 0, 1, 0),
            new Among("sząc", 0, 2, 0),
            new Among("sz", -1, 1, 0),
            new Among("iejsz", 3, 1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("a", -1, 1, 1),
            new Among("ąca", 0, 1, 0),
            new Among("ająca", 1, 1, 0),
            new Among("sząca", 1, 2, 0),
            new Among("ia", 0, 1, 1),
            new Among("sza", 0, 1, 0),
            new Among("iejsza", 5, 1, 0),
            new Among("ała", 0, 1, 0),
            new Among("iała", 7, 1, 0),
            new Among("iła", 0, 1, 0),
            new Among("ąc", -1, 1, 0),
            new Among("ając", 10, 1, 0),
            new Among("e", -1, 1, 1),
            new Among("ące", 12, 1, 0),
            new Among("ające", 13, 1, 0),
            new Among("szące", 13, 2, 0),
            new Among("ie", 12, 1, 1),
            new Among("cie", 16, 1, 0),
            new Among("acie", 17, 1, 0),
            new Among("ecie", 17, 1, 0),
            new Among("icie", 17, 1, 0),
            new Among("ajcie", 17, 1, 0),
            new Among("liście", 17, 4, 0),
            new Among("aliście", 22, 1, 0),
            new Among("ieliście", 22, 1, 0),
            new Among("iliście", 22, 1, 0),
            new Among("łyście", 17, 4, 0),
            new Among("ałyście", 26, 1, 0),
            new Among("iałyście", 27, 1, 0),
            new Among("iłyście", 26, 1, 0),
            new Among("sze", 12, 1, 0),
            new Among("iejsze", 30, 1, 0),
            new Among("ach", -1, 1, 1),
            new Among("iach", 32, 1, 1),
            new Among("ich", -1, 5, 0),
            new Among("ych", -1, 5, 0),
            new Among("i", -1, 1, 1),
            new Among("ali", 36, 1, 0),
            new Among("ieli", 36, 1, 0),
            new Among("ili", 36, 1, 0),
            new Among("ami", 36, 1, 1),
            new Among("iami", 40, 1, 1),
            new Among("imi", 36, 5, 0),
            new Among("ymi", 36, 5, 0),
            new Among("owi", 36, 1, 1),
            new Among("iowi", 44, 1, 1),
            new Among("aj", -1, 1, 0),
            new Among("ej", -1, 5, 0),
            new Among("iej", 47, 5, 0),
            new Among("am", -1, 1, 0),
            new Among("ałam", 49, 1, 0),
            new Among("iałam", 50, 1, 0),
            new Among("iłam", 49, 1, 0),
            new Among("em", -1, 1, 1),
            new Among("iem", 53, 1, 1),
            new Among("ałem", 53, 1, 0),
            new Among("iałem", 55, 1, 0),
            new Among("iłem", 53, 1, 0),
            new Among("im", -1, 5, 0),
            new Among("om", -1, 1, 1),
            new Among("iom", 59, 1, 1),
            new Among("ym", -1, 5, 0),
            new Among("o", -1, 1, 1),
            new Among("ego", 62, 5, 0),
            new Among("iego", 63, 5, 0),
            new Among("ało", 62, 1, 0),
            new Among("iało", 65, 1, 0),
            new Among("iło", 62, 1, 0),
            new Among("u", -1, 1, 1),
            new Among("iu", 68, 1, 1),
            new Among("emu", 68, 5, 0),
            new Among("iemu", 70, 5, 0),
            new Among("ów", -1, 1, 1),
            new Among("y", -1, 5, 0),
            new Among("amy", 73, 1, 0),
            new Among("emy", 73, 1, 0),
            new Among("imy", 73, 1, 0),
            new Among("liśmy", 73, 4, 0),
            new Among("aliśmy", 77, 1, 0),
            new Among("ieliśmy", 77, 1, 0),
            new Among("iliśmy", 77, 1, 0),
            new Among("łyśmy", 73, 4, 0),
            new Among("ałyśmy", 81, 1, 0),
            new Among("iałyśmy", 82, 1, 0),
            new Among("iłyśmy", 81, 1, 0),
            new Among("ały", 73, 1, 0),
            new Among("iały", 85, 1, 0),
            new Among("iły", 73, 1, 0),
            new Among("asz", -1, 1, 0),
            new Among("esz", -1, 1, 0),
            new Among("isz", -1, 1, 0),
            new Among("ą", -1, 1, 1),
            new Among("ącą", 91, 1, 0),
            new Among("ającą", 92, 1, 0),
            new Among("szącą", 92, 2, 0),
            new Among("ią", 91, 1, 1),
            new Among("ają", 91, 1, 0),
            new Among("szą", 91, 3, 0),
            new Among("iejszą", 97, 1, 0),
            new Among("ać", -1, 1, 0),
            new Among("ieć", -1, 1, 0),
            new Among("ić", -1, 1, 0),
            new Among("ąć", -1, 1, 0),
            new Among("aść", -1, 1, 0),
            new Among("eść", -1, 1, 0),
            new Among("ę", -1, 1, 0),
            new Among("szę", 105, 2, 0),
            new Among("ał", -1, 1, 0),
            new Among("iał", 107, 1, 0),
            new Among("ił", -1, 1, 0),
            new Among("łaś", -1, 4, 0),
            new Among("ałaś", 110, 1, 0),
            new Among("iałaś", 111, 1, 0),
            new Among("iłaś", 110, 1, 0),
            new Among("łeś", -1, 4, 0),
            new Among("ałeś", 114, 1, 0),
            new Among("iałeś", 115, 1, 0),
            new Among("iłeś", 114, 1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ć", -1, 1, 0),
            new Among("ń", -1, 2, 0),
            new Among("ś", -1, 3, 0),
            new Among("ź", -1, 4, 0)
        };


        private bool r_mark_regions()
        {
            I_p1 = limit;
            {

                int ret = out_grouping(g_v, 97, 281, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            {

                int ret = in_grouping(g_v, 97, 281, true);
                if (ret < 0)
                {
                    return false;
                }

                cursor += ret;
            }
            I_p1 = cursor;
            return true;
        }

        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_remove_endings()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                if (cursor < I_p1)
                {
                    goto lab0;
                }
                int c2 = limit_backward;
                limit_backward = I_p1;
                ket = cursor;
                if (find_among_b(a_0, null) == 0)
                {
                    {
                        limit_backward = c2;
                        goto lab0;
                    }
                }
                bra = cursor;
                limit_backward = c2;
                slice_del();
            lab0: ;
                cursor = limit - c1;
            }
            ket = cursor;
            among_var = find_among_b(a_2, r_R1);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_del();
                    break;
                }
                case 2: {
                    slice_from("s");
                    break;
                }
                case 3: {
                    {
                        int c3 = limit - cursor;
                        int c4 = limit - cursor;
                        if (!r_R1())
                            goto lab2;
                        cursor = limit - c4;
                        slice_del();
                        goto lab1;
                    lab2: ;
                        cursor = limit - c3;
                        slice_from("s");
                    }
                lab1: ;
                    break;
                }
                case 4: {
                    slice_from("ł");
                    break;
                }
                case 5: {
                    slice_del();
                    {
                        int c5 = limit - cursor;
                        ket = cursor;
                        among_var = find_among_b(a_1, null);
                        if (among_var == 0)
                        {
                            {
                                cursor = limit - c5;
                                goto lab3;
                            }
                        }
                        bra = cursor;
                        switch (among_var) {
                            case 1: {
                                slice_del();
                                break;
                            }
                            case 2: {
                                slice_from("s");
                                break;
                            }
                        }
                    lab3: ;
                    }
                    break;
                }
            }
            return true;
        }

        private bool r_normalize_consonant()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_3, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (cursor > limit_backward)
            {
                goto lab0;
            }
            return false;
        lab0: ;
            switch (among_var) {
                case 1: {
                    slice_from("c");
                    break;
                }
                case 2: {
                    slice_from("n");
                    break;
                }
                case 3: {
                    slice_from("s");
                    break;
                }
                case 4: {
                    slice_from("z");
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
            {
                int c2 = cursor;
                {
                    int c = cursor + 2;
                    if (c > limit)
                    {
                        goto lab1;
                    }
                    cursor = c;
                }
                limit_backward = cursor;
                cursor = limit;
                if (!r_remove_endings())
                    goto lab1;
                cursor = limit_backward;
                goto lab0;
            lab1: ;
                cursor = c2;
                limit_backward = cursor;
                cursor = limit;
                if (!r_normalize_consonant())
                    return false;
                cursor = limit_backward;
            }
        lab0: ;
            return true;
        }

    }
}

