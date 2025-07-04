﻿// Generated from nepali.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from nepali.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class NepaliStemmer : Stemmer
    {

        private static readonly Among[] a_0 = new[]
        {
            new Among("\u0932\u093E\u0907", -1, 1),
            new Among("\u0932\u093E\u0908", -1, 1),
            new Among("\u0938\u0901\u0917", -1, 1),
            new Among("\u0938\u0902\u0917", -1, 1),
            new Among("\u092E\u093E\u0930\u094D\u092B\u0924", -1, 1),
            new Among("\u0930\u0924", -1, 1),
            new Among("\u0915\u093E", -1, 2),
            new Among("\u092E\u093E", -1, 1),
            new Among("\u0926\u094D\u0935\u093E\u0930\u093E", -1, 1),
            new Among("\u0915\u093F", -1, 2),
            new Among("\u092A\u091B\u093F", -1, 1),
            new Among("\u0915\u0940", -1, 2),
            new Among("\u0932\u0947", -1, 1),
            new Among("\u0915\u0948", -1, 2),
            new Among("\u0938\u0901\u0917\u0948", -1, 1),
            new Among("\u092E\u0948", -1, 1),
            new Among("\u0915\u094B", -1, 2)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("\u0901", -1, 1),
            new Among("\u0902", -1, 1),
            new Among("\u0948", -1, 2)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("\u0925\u093F\u090F", -1, 1),
            new Among("\u091B", -1, 1),
            new Among("\u0907\u091B", 1, 1),
            new Among("\u090F\u091B", 1, 1),
            new Among("\u093F\u091B", 1, 1),
            new Among("\u0947\u091B", 1, 1),
            new Among("\u0928\u0947\u091B", 5, 1),
            new Among("\u0939\u0941\u0928\u0947\u091B", 6, 1),
            new Among("\u0907\u0928\u094D\u091B", 1, 1),
            new Among("\u093F\u0928\u094D\u091B", 1, 1),
            new Among("\u0939\u0941\u0928\u094D\u091B", 1, 1),
            new Among("\u090F\u0915\u093E", -1, 1),
            new Among("\u0907\u090F\u0915\u093E", 11, 1),
            new Among("\u093F\u090F\u0915\u093E", 11, 1),
            new Among("\u0947\u0915\u093E", -1, 1),
            new Among("\u0928\u0947\u0915\u093E", 14, 1),
            new Among("\u0926\u093E", -1, 1),
            new Among("\u0907\u0926\u093E", 16, 1),
            new Among("\u093F\u0926\u093E", 16, 1),
            new Among("\u0926\u0947\u0916\u093F", -1, 1),
            new Among("\u092E\u093E\u0925\u093F", -1, 1),
            new Among("\u090F\u0915\u0940", -1, 1),
            new Among("\u0907\u090F\u0915\u0940", 21, 1),
            new Among("\u093F\u090F\u0915\u0940", 21, 1),
            new Among("\u0947\u0915\u0940", -1, 1),
            new Among("\u0926\u0947\u0916\u0940", -1, 1),
            new Among("\u0925\u0940", -1, 1),
            new Among("\u0926\u0940", -1, 1),
            new Among("\u091B\u0941", -1, 1),
            new Among("\u090F\u091B\u0941", 28, 1),
            new Among("\u0947\u091B\u0941", 28, 1),
            new Among("\u0928\u0947\u091B\u0941", 30, 1),
            new Among("\u0928\u0941", -1, 1),
            new Among("\u0939\u0930\u0941", -1, 1),
            new Among("\u0939\u0930\u0942", -1, 1),
            new Among("\u091B\u0947", -1, 1),
            new Among("\u0925\u0947", -1, 1),
            new Among("\u0928\u0947", -1, 1),
            new Among("\u090F\u0915\u0948", -1, 1),
            new Among("\u0947\u0915\u0948", -1, 1),
            new Among("\u0928\u0947\u0915\u0948", 39, 1),
            new Among("\u0926\u0948", -1, 1),
            new Among("\u0907\u0926\u0948", 41, 1),
            new Among("\u093F\u0926\u0948", 41, 1),
            new Among("\u090F\u0915\u094B", -1, 1),
            new Among("\u0907\u090F\u0915\u094B", 44, 1),
            new Among("\u093F\u090F\u0915\u094B", 44, 1),
            new Among("\u0947\u0915\u094B", -1, 1),
            new Among("\u0928\u0947\u0915\u094B", 47, 1),
            new Among("\u0926\u094B", -1, 1),
            new Among("\u0907\u0926\u094B", 49, 1),
            new Among("\u093F\u0926\u094B", 49, 1),
            new Among("\u092F\u094B", -1, 1),
            new Among("\u0907\u092F\u094B", 52, 1),
            new Among("\u092D\u092F\u094B", 52, 1),
            new Among("\u093F\u092F\u094B", 52, 1),
            new Among("\u0925\u093F\u092F\u094B", 55, 1),
            new Among("\u0926\u093F\u092F\u094B", 55, 1),
            new Among("\u0925\u094D\u092F\u094B", 52, 1),
            new Among("\u091B\u094C", -1, 1),
            new Among("\u0907\u091B\u094C", 59, 1),
            new Among("\u090F\u091B\u094C", 59, 1),
            new Among("\u093F\u091B\u094C", 59, 1),
            new Among("\u0947\u091B\u094C", 59, 1),
            new Among("\u0928\u0947\u091B\u094C", 63, 1),
            new Among("\u092F\u094C", -1, 1),
            new Among("\u0925\u093F\u092F\u094C", 65, 1),
            new Among("\u091B\u094D\u092F\u094C", 65, 1),
            new Among("\u0925\u094D\u092F\u094C", 65, 1),
            new Among("\u091B\u0928\u094D", -1, 1),
            new Among("\u0907\u091B\u0928\u094D", 69, 1),
            new Among("\u090F\u091B\u0928\u094D", 69, 1),
            new Among("\u093F\u091B\u0928\u094D", 69, 1),
            new Among("\u0947\u091B\u0928\u094D", 69, 1),
            new Among("\u0928\u0947\u091B\u0928\u094D", 73, 1),
            new Among("\u0932\u093E\u0928\u094D", -1, 1),
            new Among("\u091B\u093F\u0928\u094D", -1, 1),
            new Among("\u0925\u093F\u0928\u094D", -1, 1),
            new Among("\u092A\u0930\u094D", -1, 1),
            new Among("\u0907\u0938\u094D", -1, 1),
            new Among("\u0925\u093F\u0907\u0938\u094D", 79, 1),
            new Among("\u091B\u0938\u094D", -1, 1),
            new Among("\u0907\u091B\u0938\u094D", 81, 1),
            new Among("\u090F\u091B\u0938\u094D", 81, 1),
            new Among("\u093F\u091B\u0938\u094D", 81, 1),
            new Among("\u0947\u091B\u0938\u094D", 81, 1),
            new Among("\u0928\u0947\u091B\u0938\u094D", 85, 1),
            new Among("\u093F\u0938\u094D", -1, 1),
            new Among("\u0925\u093F\u0938\u094D", 87, 1),
            new Among("\u091B\u0947\u0938\u094D", -1, 1),
            new Among("\u0939\u094B\u0938\u094D", -1, 1)
        };


        private bool r_remove_category_1()
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
                    slice_del();
                    break;
                }
                case 2: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("\u090F")))
                        {
                            goto lab1;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("\u0947")))
                        {
                            goto lab2;
                        }
                        goto lab0;
                    lab2: ;
                        cursor = limit - c1;
                        slice_del();
                    }
                lab0: ;
                    break;
                }
            }
            return true;
        }

        private bool r_remove_category_2()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_1);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    {
                        int c1 = limit - cursor;
                        if (!(eq_s_b("\u092F\u094C")))
                        {
                            goto lab1;
                        }
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("\u091B\u094C")))
                        {
                            goto lab2;
                        }
                        goto lab0;
                    lab2: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("\u0928\u094C")))
                        {
                            goto lab3;
                        }
                        goto lab0;
                    lab3: ;
                        cursor = limit - c1;
                        if (!(eq_s_b("\u0925\u0947")))
                        {
                            return false;
                        }
                    }
                lab0: ;
                    slice_del();
                    break;
                }
                case 2: {
                    if (!(eq_s_b("\u0924\u094D\u0930")))
                    {
                        return false;
                    }
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_remove_category_3()
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

        protected override bool stem()
        {
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                r_remove_category_1();
                cursor = limit - c1;
            }
            while (true)
            {
                int c2 = limit - cursor;
                {
                    int c3 = limit - cursor;
                    r_remove_category_2();
                    cursor = limit - c3;
                }
                if (!r_remove_category_3())
                    goto lab0;
                continue;
            lab0: ;
                cursor = limit - c2;
                break;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

