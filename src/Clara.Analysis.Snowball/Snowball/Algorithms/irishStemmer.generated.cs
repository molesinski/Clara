// Generated from irish.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from irish.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class IrishStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;
        private int I_pV;

        private const string g_v = "aeiouáéíóú";

        private static readonly Among[] a_0 = new[]
        {
            new Among("b'", -1, 1, 0),
            new Among("bh", -1, 4, 0),
            new Among("bhf", 1, 2, 0),
            new Among("bp", -1, 8, 0),
            new Among("ch", -1, 5, 0),
            new Among("d'", -1, 1, 0),
            new Among("d'fh", 5, 2, 0),
            new Among("dh", -1, 6, 0),
            new Among("dt", -1, 9, 0),
            new Among("fh", -1, 2, 0),
            new Among("gc", -1, 5, 0),
            new Among("gh", -1, 7, 0),
            new Among("h-", -1, 1, 0),
            new Among("m'", -1, 1, 0),
            new Among("mb", -1, 4, 0),
            new Among("mh", -1, 10, 0),
            new Among("n-", -1, 1, 0),
            new Among("nd", -1, 6, 0),
            new Among("ng", -1, 7, 0),
            new Among("ph", -1, 8, 0),
            new Among("sh", -1, 3, 0),
            new Among("t-", -1, 1, 0),
            new Among("th", -1, 9, 0),
            new Among("ts", -1, 3, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("íochta", -1, 1, 0),
            new Among("aíochta", 0, 1, 0),
            new Among("ire", -1, 2, 0),
            new Among("aire", 2, 2, 0),
            new Among("abh", -1, 1, 0),
            new Among("eabh", 4, 1, 0),
            new Among("ibh", -1, 1, 0),
            new Among("aibh", 6, 1, 0),
            new Among("amh", -1, 1, 0),
            new Among("eamh", 8, 1, 0),
            new Among("imh", -1, 1, 0),
            new Among("aimh", 10, 1, 0),
            new Among("íocht", -1, 1, 0),
            new Among("aíocht", 12, 1, 0),
            new Among("irí", -1, 2, 0),
            new Among("airí", 14, 2, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("óideacha", -1, 6, 0),
            new Among("patacha", -1, 5, 0),
            new Among("achta", -1, 1, 0),
            new Among("arcachta", 2, 2, 0),
            new Among("eachta", 2, 1, 0),
            new Among("grafaíochta", -1, 4, 0),
            new Among("paite", -1, 5, 0),
            new Among("ach", -1, 1, 0),
            new Among("each", 7, 1, 0),
            new Among("óideach", 8, 6, 0),
            new Among("gineach", 8, 3, 0),
            new Among("patach", 7, 5, 0),
            new Among("grafaíoch", -1, 4, 0),
            new Among("pataigh", -1, 5, 0),
            new Among("óidigh", -1, 6, 0),
            new Among("achtúil", -1, 1, 0),
            new Among("eachtúil", 15, 1, 0),
            new Among("gineas", -1, 3, 0),
            new Among("ginis", -1, 3, 0),
            new Among("acht", -1, 1, 0),
            new Among("arcacht", 19, 2, 0),
            new Among("eacht", 19, 1, 0),
            new Among("grafaíocht", -1, 4, 0),
            new Among("arcachtaí", -1, 2, 0),
            new Among("grafaíochtaí", -1, 4, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("imid", -1, 1, 0),
            new Among("aimid", 0, 1, 0),
            new Among("ímid", -1, 1, 0),
            new Among("aímid", 2, 1, 0),
            new Among("adh", -1, 2, 0),
            new Among("eadh", 4, 2, 0),
            new Among("faidh", -1, 1, 0),
            new Among("fidh", -1, 1, 0),
            new Among("áil", -1, 2, 0),
            new Among("ain", -1, 2, 0),
            new Among("tear", -1, 2, 0),
            new Among("tar", -1, 2, 0)
        };


        private bool r_mark_regions()
        {
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 250, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_pV = cursor;
                {

                    int ret = in_grouping(g_v, 97, 250, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 250, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 250, true);
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

        private bool r_initial_morph()
        {
            int among_var;
            bra = cursor;
            among_var = find_among(a_0, null);
            if (among_var == 0)
            {
                return false;
            }
            ket = cursor;
            switch (among_var) {
                case 1: {
                    slice_del();
                    break;
                }
                case 2: {
                    slice_from("f");
                    break;
                }
                case 3: {
                    slice_from("s");
                    break;
                }
                case 4: {
                    slice_from("b");
                    break;
                }
                case 5: {
                    slice_from("c");
                    break;
                }
                case 6: {
                    slice_from("d");
                    break;
                }
                case 7: {
                    slice_from("g");
                    break;
                }
                case 8: {
                    slice_from("p");
                    break;
                }
                case 9: {
                    slice_from("t");
                    break;
                }
                case 10: {
                    slice_from("m");
                    break;
                }
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

        private bool r_noun_sfx()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_1, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_deriv()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_2, null);
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
                    slice_from("arc");
                    break;
                }
                case 3: {
                    slice_from("gin");
                    break;
                }
                case 4: {
                    slice_from("graf");
                    break;
                }
                case 5: {
                    slice_from("paite");
                    break;
                }
                case 6: {
                    slice_from("óid");
                    break;
                }
            }
            return true;
        }

        private bool r_verb_sfx()
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
                    if (!r_RV())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    if (!r_R1())
                        return false;
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
                r_initial_morph();
                cursor = c1;
            }
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            {
                int c2 = limit - cursor;
                r_noun_sfx();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_deriv();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_verb_sfx();
                cursor = limit - c4;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

