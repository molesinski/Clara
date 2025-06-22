﻿// Generated from russian.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from russian.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class RussianStemmer : Stemmer
    {
        private int I_p2;
        private int I_pV;

        private const string g_v = "аеиоуыэюя";

        private static readonly Among[] a_0 = new[]
        {
            new Among("в", -1, 1),
            new Among("ив", 0, 2),
            new Among("ыв", 0, 2),
            new Among("вши", -1, 1),
            new Among("ивши", 3, 2),
            new Among("ывши", 3, 2),
            new Among("вшись", -1, 1),
            new Among("ившись", 6, 2),
            new Among("ывшись", 6, 2)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("ее", -1, 1),
            new Among("ие", -1, 1),
            new Among("ое", -1, 1),
            new Among("ые", -1, 1),
            new Among("ими", -1, 1),
            new Among("ыми", -1, 1),
            new Among("ей", -1, 1),
            new Among("ий", -1, 1),
            new Among("ой", -1, 1),
            new Among("ый", -1, 1),
            new Among("ем", -1, 1),
            new Among("им", -1, 1),
            new Among("ом", -1, 1),
            new Among("ым", -1, 1),
            new Among("его", -1, 1),
            new Among("ого", -1, 1),
            new Among("ему", -1, 1),
            new Among("ому", -1, 1),
            new Among("их", -1, 1),
            new Among("ых", -1, 1),
            new Among("ею", -1, 1),
            new Among("ою", -1, 1),
            new Among("ую", -1, 1),
            new Among("юю", -1, 1),
            new Among("ая", -1, 1),
            new Among("яя", -1, 1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ем", -1, 1),
            new Among("нн", -1, 1),
            new Among("вш", -1, 1),
            new Among("ивш", 2, 2),
            new Among("ывш", 2, 2),
            new Among("щ", -1, 1),
            new Among("ющ", 5, 1),
            new Among("ующ", 6, 2)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("сь", -1, 1),
            new Among("ся", -1, 1)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("ла", -1, 1),
            new Among("ила", 0, 2),
            new Among("ыла", 0, 2),
            new Among("на", -1, 1),
            new Among("ена", 3, 2),
            new Among("ете", -1, 1),
            new Among("ите", -1, 2),
            new Among("йте", -1, 1),
            new Among("ейте", 7, 2),
            new Among("уйте", 7, 2),
            new Among("ли", -1, 1),
            new Among("или", 10, 2),
            new Among("ыли", 10, 2),
            new Among("й", -1, 1),
            new Among("ей", 13, 2),
            new Among("уй", 13, 2),
            new Among("л", -1, 1),
            new Among("ил", 16, 2),
            new Among("ыл", 16, 2),
            new Among("ем", -1, 1),
            new Among("им", -1, 2),
            new Among("ым", -1, 2),
            new Among("н", -1, 1),
            new Among("ен", 22, 2),
            new Among("ло", -1, 1),
            new Among("ило", 24, 2),
            new Among("ыло", 24, 2),
            new Among("но", -1, 1),
            new Among("ено", 27, 2),
            new Among("нно", 27, 1),
            new Among("ет", -1, 1),
            new Among("ует", 30, 2),
            new Among("ит", -1, 2),
            new Among("ыт", -1, 2),
            new Among("ют", -1, 1),
            new Among("уют", 34, 2),
            new Among("ят", -1, 2),
            new Among("ны", -1, 1),
            new Among("ены", 37, 2),
            new Among("ть", -1, 1),
            new Among("ить", 39, 2),
            new Among("ыть", 39, 2),
            new Among("ешь", -1, 1),
            new Among("ишь", -1, 2),
            new Among("ю", -1, 2),
            new Among("ую", 44, 2)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("а", -1, 1),
            new Among("ев", -1, 1),
            new Among("ов", -1, 1),
            new Among("е", -1, 1),
            new Among("ие", 3, 1),
            new Among("ье", 3, 1),
            new Among("и", -1, 1),
            new Among("еи", 6, 1),
            new Among("ии", 6, 1),
            new Among("ами", 6, 1),
            new Among("ями", 6, 1),
            new Among("иями", 10, 1),
            new Among("й", -1, 1),
            new Among("ей", 12, 1),
            new Among("ией", 13, 1),
            new Among("ий", 12, 1),
            new Among("ой", 12, 1),
            new Among("ам", -1, 1),
            new Among("ем", -1, 1),
            new Among("ием", 18, 1),
            new Among("ом", -1, 1),
            new Among("ям", -1, 1),
            new Among("иям", 21, 1),
            new Among("о", -1, 1),
            new Among("у", -1, 1),
            new Among("ах", -1, 1),
            new Among("ях", -1, 1),
            new Among("иях", 26, 1),
            new Among("ы", -1, 1),
            new Among("ь", -1, 1),
            new Among("ю", -1, 1),
            new Among("ию", 30, 1),
            new Among("ью", 30, 1),
            new Among("я", -1, 1),
            new Among("ия", 33, 1),
            new Among("ья", 33, 1)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("ост", -1, 1),
            new Among("ость", -1, 1)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("ейше", -1, 1),
            new Among("н", -1, 2),
            new Among("ейш", -1, 1),
            new Among("ь", -1, 3)
        };


        private bool r_mark_regions()
        {
            I_pV = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {

                    int ret = out_grouping(g_v, 1072, 1103, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_pV = cursor;
                {

                    int ret = in_grouping(g_v, 1072, 1103, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = out_grouping(g_v, 1072, 1103, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 1072, 1103, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab0: ;
                cursor = c1;
            }
            return true;
        }

        private bool r_R2()
        {
            return I_p2 <= cursor;
        }

        private bool r_perfective_gerund()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_0);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("а")))
                        {
                            goto lab1;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("я")))
                        {
                            return false;
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
            return true;
        }

        private bool r_adjective()
        {
            ket = cursor;
            if (find_among_b(a_1) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_adjectival()
        {
            int among_var;
            if (!r_adjective())
                return false;
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
                switch (among_var) {
                    case 1: {
                        {
                            int c2 = limit - cursor;
                            if (!(eq_s_b("а")))
                            {
                                goto lab2;
                            }
                            goto lab1;
                        lab2: ;
                            cursor = limit - c2;
                            if (!(eq_s_b("я")))
                            {
                                {
                                    cursor = limit - c1;
                                    goto lab0;
                                }
                            }
                        }
                    lab1: ;
                        slice_del();
                        break;
                    }
                    case 2: {
                        slice_del();
                        break;
                    }
                }
            lab0: ;
            }
            return true;
        }

        private bool r_reflexive()
        {
            ket = cursor;
            if (find_among_b(a_3) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_verb()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_4);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("а")))
                        {
                            goto lab1;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("я")))
                        {
                            return false;
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
            return true;
        }

        private bool r_noun()
        {
            ket = cursor;
            if (find_among_b(a_5) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_derivational()
        {
            ket = cursor;
            if (find_among_b(a_6) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R2())
                return false;
            slice_del();
            return true;
        }

        private bool r_tidy_up()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_7);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_del();
                    ket = cursor;
                    if (!(eq_s_b("н")))
                    {
                        return false;
                    }
                    bra = cursor;
                    if (!(eq_s_b("н")))
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
                case 2: {
                    if (!(eq_s_b("н")))
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
                case 3: {
                    slice_del();
                    break;
                }
            }
            return true;
        }

        protected override bool stem()
        {
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    while (true)
                    {
                        int c3 = cursor;
                        bra = cursor;
                        if (!(eq_s("ё")))
                        {
                            goto lab2;
                        }
                        ket = cursor;
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
                    slice_from("е");
                    continue;
                lab1: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            if (cursor < I_pV)
            {
                return false;
            }
            int c4 = limit_backward;
            limit_backward = I_pV;
            {
                int c5 = limit - cursor;
                {
                    int c6 = limit - cursor;
                    if (!r_perfective_gerund())
                        goto lab5;
                    goto lab4;
                lab5: ;
                    cursor = limit - c6;
                    {
                        int c7 = limit - cursor;
                        if (!r_reflexive())
                            {
                                cursor = limit - c7;
                                goto lab6;
                            }
                    lab6: ;
                    }
                    {
                        int c8 = limit - cursor;
                        if (!r_adjectival())
                            goto lab8;
                        goto lab7;
                    lab8: ;
                        cursor = limit - c8;
                        if (!r_verb())
                            goto lab9;
                        goto lab7;
                    lab9: ;
                        cursor = limit - c8;
                        if (!r_noun())
                            goto lab3;
                    }
                lab7: ;
                }
            lab4: ;
            lab3: ;
                cursor = limit - c5;
            }
            {
                int c9 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("и")))
                {
                    {
                        cursor = limit - c9;
                        goto lab10;
                    }
                }
                bra = cursor;
                slice_del();
            lab10: ;
            }
            {
                int c10 = limit - cursor;
                r_derivational();
                cursor = limit - c10;
            }
            {
                int c11 = limit - cursor;
                r_tidy_up();
                cursor = limit - c11;
            }
            limit_backward = c4;
            cursor = limit_backward;
            return true;
        }

    }
}

