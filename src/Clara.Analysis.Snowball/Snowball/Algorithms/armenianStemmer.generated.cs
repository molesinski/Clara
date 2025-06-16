// Generated from armenian.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from armenian.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class ArmenianStemmer : Stemmer
    {
        private int I_p2;
        private int I_pV;

        private const string g_v = "աէիօւեոը";

        private static readonly Among[] a_0 = new[]
        {
            new Among("րորդ", -1, 1),
            new Among("երորդ", 0, 1),
            new Among("ալի", -1, 1),
            new Among("ակի", -1, 1),
            new Among("որակ", -1, 1),
            new Among("եղ", -1, 1),
            new Among("ական", -1, 1),
            new Among("արան", -1, 1),
            new Among("են", -1, 1),
            new Among("եկեն", 8, 1),
            new Among("երեն", 8, 1),
            new Among("որէն", -1, 1),
            new Among("ին", -1, 1),
            new Among("գին", 12, 1),
            new Among("ովին", 12, 1),
            new Among("լայն", -1, 1),
            new Among("վուն", -1, 1),
            new Among("պես", -1, 1),
            new Among("իվ", -1, 1),
            new Among("ատ", -1, 1),
            new Among("ավետ", -1, 1),
            new Among("կոտ", -1, 1),
            new Among("բար", -1, 1)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("ա", -1, 1),
            new Among("ացա", 0, 1),
            new Among("եցա", 0, 1),
            new Among("վե", -1, 1),
            new Among("ացրի", -1, 1),
            new Among("ացի", -1, 1),
            new Among("եցի", -1, 1),
            new Among("վեցի", 6, 1),
            new Among("ալ", -1, 1),
            new Among("ըալ", 8, 1),
            new Among("անալ", 8, 1),
            new Among("ենալ", 8, 1),
            new Among("ացնալ", 8, 1),
            new Among("ել", -1, 1),
            new Among("ըել", 13, 1),
            new Among("նել", 13, 1),
            new Among("ցնել", 15, 1),
            new Among("եցնել", 16, 1),
            new Among("չել", 13, 1),
            new Among("վել", 13, 1),
            new Among("ացվել", 19, 1),
            new Among("եցվել", 19, 1),
            new Among("տել", 13, 1),
            new Among("ատել", 22, 1),
            new Among("ոտել", 22, 1),
            new Among("կոտել", 24, 1),
            new Among("ված", -1, 1),
            new Among("ում", -1, 1),
            new Among("վում", 27, 1),
            new Among("ան", -1, 1),
            new Among("ցան", 29, 1),
            new Among("ացան", 30, 1),
            new Among("ացրին", -1, 1),
            new Among("ացին", -1, 1),
            new Among("եցին", -1, 1),
            new Among("վեցին", 34, 1),
            new Among("ալիս", -1, 1),
            new Among("ելիս", -1, 1),
            new Among("ավ", -1, 1),
            new Among("ացավ", 38, 1),
            new Among("եցավ", 38, 1),
            new Among("ալով", -1, 1),
            new Among("ելով", -1, 1),
            new Among("ար", -1, 1),
            new Among("ացար", 43, 1),
            new Among("եցար", 43, 1),
            new Among("ացրիր", -1, 1),
            new Among("ացիր", -1, 1),
            new Among("եցիր", -1, 1),
            new Among("վեցիր", 48, 1),
            new Among("աց", -1, 1),
            new Among("եց", -1, 1),
            new Among("ացրեց", 51, 1),
            new Among("ալուց", -1, 1),
            new Among("ելուց", -1, 1),
            new Among("ալու", -1, 1),
            new Among("ելու", -1, 1),
            new Among("աք", -1, 1),
            new Among("ցաք", 57, 1),
            new Among("ացաք", 58, 1),
            new Among("ացրիք", -1, 1),
            new Among("ացիք", -1, 1),
            new Among("եցիք", -1, 1),
            new Among("վեցիք", 62, 1),
            new Among("անք", -1, 1),
            new Among("ցանք", 64, 1),
            new Among("ացանք", 65, 1),
            new Among("ացրինք", -1, 1),
            new Among("ացինք", -1, 1),
            new Among("եցինք", -1, 1),
            new Among("վեցինք", 69, 1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("որդ", -1, 1),
            new Among("ույթ", -1, 1),
            new Among("ուհի", -1, 1),
            new Among("ցի", -1, 1),
            new Among("իլ", -1, 1),
            new Among("ակ", -1, 1),
            new Among("յակ", 5, 1),
            new Among("անակ", 5, 1),
            new Among("իկ", -1, 1),
            new Among("ուկ", -1, 1),
            new Among("ան", -1, 1),
            new Among("պան", 10, 1),
            new Among("ստան", 10, 1),
            new Among("արան", 10, 1),
            new Among("եղէն", -1, 1),
            new Among("յուն", -1, 1),
            new Among("ություն", 15, 1),
            new Among("ածո", -1, 1),
            new Among("իչ", -1, 1),
            new Among("ուս", -1, 1),
            new Among("ուստ", -1, 1),
            new Among("գար", -1, 1),
            new Among("վոր", -1, 1),
            new Among("ավոր", 22, 1),
            new Among("ոց", -1, 1),
            new Among("անօց", -1, 1),
            new Among("ու", -1, 1),
            new Among("ք", -1, 1),
            new Among("չեք", 27, 1),
            new Among("իք", 27, 1),
            new Among("ալիք", 29, 1),
            new Among("անիք", 29, 1),
            new Among("վածք", 27, 1),
            new Among("ույք", 27, 1),
            new Among("ենք", 27, 1),
            new Among("ոնք", 27, 1),
            new Among("ունք", 27, 1),
            new Among("մունք", 36, 1),
            new Among("իչք", 27, 1),
            new Among("արք", 27, 1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("սա", -1, 1),
            new Among("վա", -1, 1),
            new Among("ամբ", -1, 1),
            new Among("դ", -1, 1),
            new Among("անդ", 3, 1),
            new Among("ությանդ", 4, 1),
            new Among("վանդ", 4, 1),
            new Among("ոջդ", 3, 1),
            new Among("երդ", 3, 1),
            new Among("ներդ", 8, 1),
            new Among("ուդ", 3, 1),
            new Among("ը", -1, 1),
            new Among("անը", 11, 1),
            new Among("ությանը", 12, 1),
            new Among("վանը", 12, 1),
            new Among("ոջը", 11, 1),
            new Among("երը", 11, 1),
            new Among("ները", 16, 1),
            new Among("ի", -1, 1),
            new Among("վի", 18, 1),
            new Among("երի", 18, 1),
            new Among("ների", 20, 1),
            new Among("անում", -1, 1),
            new Among("երում", -1, 1),
            new Among("ներում", 23, 1),
            new Among("ն", -1, 1),
            new Among("ան", 25, 1),
            new Among("ության", 26, 1),
            new Among("վան", 26, 1),
            new Among("ին", 25, 1),
            new Among("երին", 29, 1),
            new Among("ներին", 30, 1),
            new Among("ությանն", 25, 1),
            new Among("երն", 25, 1),
            new Among("ներն", 33, 1),
            new Among("ուն", 25, 1),
            new Among("ոջ", -1, 1),
            new Among("ությանս", -1, 1),
            new Among("վանս", -1, 1),
            new Among("ոջս", -1, 1),
            new Among("ով", -1, 1),
            new Among("անով", 40, 1),
            new Among("վով", 40, 1),
            new Among("երով", 40, 1),
            new Among("ներով", 43, 1),
            new Among("եր", -1, 1),
            new Among("ներ", 45, 1),
            new Among("ց", -1, 1),
            new Among("ից", 47, 1),
            new Among("վանից", 48, 1),
            new Among("ոջից", 48, 1),
            new Among("վից", 48, 1),
            new Among("երից", 48, 1),
            new Among("ներից", 52, 1),
            new Among("ցից", 48, 1),
            new Among("ոց", 47, 1),
            new Among("ուց", 47, 1)
        };


        private bool r_mark_regions()
        {
            I_pV = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {

                    int ret = out_grouping(g_v, 1377, 1413, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_pV = cursor;
                {

                    int ret = in_grouping(g_v, 1377, 1413, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = out_grouping(g_v, 1377, 1413, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 1377, 1413, true);
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

        private bool r_adjective()
        {
            ket = cursor;
            if (find_among_b(a_0) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_verb()
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

        private bool r_noun()
        {
            ket = cursor;
            if (find_among_b(a_2) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_ending()
        {
            ket = cursor;
            if (find_among_b(a_3) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R2())
                return false;
            slice_del();
            return true;
        }

        protected override bool stem()
        {
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            if (cursor < I_pV)
            {
                return false;
            }
            int c1 = limit_backward;
            limit_backward = I_pV;
            {
                int c2 = limit - cursor;
                r_ending();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_verb();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_adjective();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                r_noun();
                cursor = limit - c5;
            }
            limit_backward = c1;
            cursor = limit_backward;
            return true;
        }

    }
}

