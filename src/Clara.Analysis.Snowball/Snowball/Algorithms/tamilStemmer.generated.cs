// Generated from tamil.sbl by Snowball 3.2.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from tamil.sbl by Snowball 3.2.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.2.0")]
    internal partial class TamilStemmer : Stemmer
    {
        private bool B_found_vetrumai_urupu;


        private static readonly Among[] a_0 = new[]
        {
            new Among("\u0BB5\u0BC1", -1, 3, 0),
            new Among("\u0BB5\u0BC2", -1, 4, 0),
            new Among("\u0BB5\u0BCA", -1, 2, 0),
            new Among("\u0BB5\u0BCB", -1, 1, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("\u0B95", -1, -1, 0),
            new Among("\u0B99", -1, -1, 0),
            new Among("\u0B9A", -1, -1, 0),
            new Among("\u0B9E", -1, -1, 0),
            new Among("\u0BA4", -1, -1, 0),
            new Among("\u0BA8", -1, -1, 0),
            new Among("\u0BAA", -1, -1, 0),
            new Among("\u0BAE", -1, -1, 0),
            new Among("\u0BAF", -1, -1, 0),
            new Among("\u0BB5", -1, -1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("\u0BBF", -1, -1, 0),
            new Among("\u0BC0", -1, -1, 0),
            new Among("\u0BC8", -1, -1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("\u0BBE", -1, -1, 0),
            new Among("\u0BBF", -1, -1, 0),
            new Among("\u0BC0", -1, -1, 0),
            new Among("\u0BC1", -1, -1, 0),
            new Among("\u0BC2", -1, -1, 0),
            new Among("\u0BC6", -1, -1, 0),
            new Among("\u0BC7", -1, -1, 0),
            new Among("\u0BC8", -1, -1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("", -1, 2, 0),
            new Among("\u0BC8", 0, 1, 0),
            new Among("\u0BCD", 0, 1, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("\u0BA8\u0BCD\u0BA4", -1, 1, 0),
            new Among("\u0BAF", -1, 1, 0),
            new Among("\u0BB5", -1, 1, 0),
            new Among("\u0BA9\u0BC1", -1, 8, 0),
            new Among("\u0BC1\u0B95\u0BCD", -1, 7, 0),
            new Among("\u0BC1\u0B95\u0BCD\u0B95\u0BCD", -1, 7, 0),
            new Among("\u0B9F\u0BCD\u0B95\u0BCD", -1, 3, 0),
            new Among("\u0BB1\u0BCD\u0B95\u0BCD", -1, 4, 0),
            new Among("\u0B99\u0BCD", -1, 9, 0),
            new Among("\u0B9F\u0BCD\u0B9F\u0BCD", -1, 5, 0),
            new Among("\u0BA4\u0BCD\u0BA4\u0BCD", -1, 6, 0),
            new Among("\u0BA8\u0BCD\u0BA4\u0BCD", -1, 1, 0),
            new Among("\u0BA8\u0BCD", -1, 1, 0),
            new Among("\u0B9F\u0BCD\u0BAA\u0BCD", -1, 3, 0),
            new Among("\u0BAF\u0BCD", -1, 2, 0),
            new Among("\u0BA9\u0BCD\u0BB1\u0BCD", -1, 4, 0),
            new Among("\u0BB5\u0BCD", -1, 1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("\u0B95", -1, -1, 0),
            new Among("\u0B9A", -1, -1, 0),
            new Among("\u0B9F", -1, -1, 0),
            new Among("\u0BA4", -1, -1, 0),
            new Among("\u0BAA", -1, -1, 0),
            new Among("\u0BB1", -1, -1, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("\u0B9E", -1, -1, 0),
            new Among("\u0BA3", -1, -1, 0),
            new Among("\u0BA8", -1, -1, 0),
            new Among("\u0BA9", -1, -1, 0),
            new Among("\u0BAE", -1, -1, 0),
            new Among("\u0BAF", -1, -1, 0),
            new Among("\u0BB0", -1, -1, 0),
            new Among("\u0BB2", -1, -1, 0),
            new Among("\u0BB3", -1, -1, 0),
            new Among("\u0BB4", -1, -1, 0),
            new Among("\u0BB5", -1, -1, 0)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("\u0BBE", -1, -1, 0),
            new Among("\u0BBF", -1, -1, 0),
            new Among("\u0BC0", -1, -1, 0),
            new Among("\u0BC1", -1, -1, 0),
            new Among("\u0BC2", -1, -1, 0),
            new Among("\u0BC6", -1, -1, 0),
            new Among("\u0BC7", -1, -1, 0),
            new Among("\u0BC8", -1, -1, 0),
            new Among("\u0BCD", -1, -1, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("\u0B85", -1, -1, 0),
            new Among("\u0B87", -1, -1, 0),
            new Among("\u0B89", -1, -1, 0)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("\u0B95\u0BB3\u0BCD", -1, 4, 0),
            new Among("\u0BC1\u0B99\u0BCD\u0B95\u0BB3\u0BCD", 0, 1, 0),
            new Among("\u0B9F\u0BCD\u0B95\u0BB3\u0BCD", 0, 3, 0),
            new Among("\u0BB1\u0BCD\u0B95\u0BB3\u0BCD", 0, 2, 0)
        };

        private static readonly Among[] a_11 = new[]
        {
            new Among("\u0BBE", -1, -1, 0),
            new Among("\u0BC7", -1, -1, 0),
            new Among("\u0BCB", -1, -1, 0)
        };

        private static readonly Among[] a_12 = new[]
        {
            new Among("\u0BAA\u0BBF", -1, -1, 0),
            new Among("\u0BB5\u0BBF", -1, -1, 0)
        };

        private static readonly Among[] a_13 = new[]
        {
            new Among("\u0BAA\u0B9F\u0BCD\u0B9F", -1, 3, 0),
            new Among("\u0BAA\u0B9F\u0BCD\u0B9F\u0BA3", -1, 3, 0),
            new Among("\u0BA4\u0BBE\u0BA9", -1, 3, 0),
            new Among("\u0BAA\u0B9F\u0BBF\u0BA4\u0BBE\u0BA9", 2, 3, 0),
            new Among("\u0BC6\u0BA9", -1, 1, 0),
            new Among("\u0BBE\u0B95\u0BBF\u0BAF", -1, 1, 0),
            new Among("\u0B95\u0BC1\u0BB0\u0BBF\u0BAF", -1, 3, 0),
            new Among("\u0BC1\u0B9F\u0BC8\u0BAF", -1, 1, 0),
            new Among("\u0BB2\u0BCD\u0BB2", -1, 2, 0),
            new Among("\u0BC1\u0BB3\u0BCD\u0BB3", -1, 1, 0),
            new Among("\u0BBE\u0B95\u0BBF", -1, 1, 0),
            new Among("\u0BAA\u0B9F\u0BBF", -1, 3, 0),
            new Among("\u0BBF\u0BA9\u0BCD\u0BB1\u0BBF", -1, 1, 0),
            new Among("\u0BAA\u0BB1\u0BCD\u0BB1\u0BBF", -1, 3, 0),
            new Among("\u0BAA\u0B9F\u0BC1", -1, 3, 0),
            new Among("\u0BB5\u0BBF\u0B9F\u0BC1", -1, 3, 0),
            new Among("\u0BAA\u0B9F\u0BCD\u0B9F\u0BC1", -1, 3, 0),
            new Among("\u0BB5\u0BBF\u0B9F\u0BCD\u0B9F\u0BC1", -1, 3, 0),
            new Among("\u0BAA\u0B9F\u0BCD\u0B9F\u0BA4\u0BC1", -1, 3, 0),
            new Among("\u0BC6\u0BA9\u0BCD\u0BB1\u0BC1", -1, 1, 0),
            new Among("\u0BC1\u0B9F\u0BC8", -1, 1, 0),
            new Among("\u0BBF\u0BB2\u0BCD\u0BB2\u0BC8", -1, 1, 0),
            new Among("\u0BC1\u0B9F\u0BA9\u0BCD", -1, 1, 0),
            new Among("\u0BBF\u0B9F\u0BAE\u0BCD", -1, 1, 0),
            new Among("\u0BC6\u0BB2\u0BCD\u0BB2\u0BBE\u0BAE\u0BCD", -1, 3, 0),
            new Among("\u0BC6\u0BA9\u0BC1\u0BAE\u0BCD", -1, 1, 0)
        };

        private static readonly Among[] a_14 = new[]
        {
            new Among("\u0BB5\u0BBF\u0B9F", -1, 2, 0),
            new Among("\u0BC0", -1, 7, 0),
            new Among("\u0BCA\u0B9F\u0BC1", -1, 2, 0),
            new Among("\u0BCB\u0B9F\u0BC1", -1, 2, 0),
            new Among("\u0BA4\u0BC1", -1, 6, 0),
            new Among("\u0BBF\u0BB0\u0BC1\u0BA8\u0BCD\u0BA4\u0BC1", 4, 2, 0),
            new Among("\u0BBF\u0BA9\u0BCD\u0BB1\u0BC1", -1, 2, 0),
            new Among("\u0BC1\u0B9F\u0BC8", -1, 2, 0),
            new Among("\u0BA9\u0BC8", -1, 1, 0),
            new Among("\u0B95\u0BA3\u0BCD", -1, 1, 0),
            new Among("\u0BBF\u0BA9\u0BCD", -1, 3, 0),
            new Among("\u0BAE\u0BC1\u0BA9\u0BCD", -1, 1, 0),
            new Among("\u0BBF\u0B9F\u0BAE\u0BCD", -1, 4, 0),
            new Among("\u0BBF\u0BB1\u0BCD", -1, 2, 0),
            new Among("\u0BAE\u0BC7\u0BB1\u0BCD", -1, 1, 0),
            new Among("\u0BB2\u0BCD", -1, 5, 0),
            new Among("\u0BBE\u0BAE\u0BB2\u0BCD", 15, 2, 0),
            new Among("\u0BBE\u0BB2\u0BCD", 15, 2, 0),
            new Among("\u0BBF\u0BB2\u0BCD", 15, 2, 0),
            new Among("\u0BAE\u0BC7\u0BB2\u0BCD", 15, 1, 0),
            new Among("\u0BC1\u0BB3\u0BCD", -1, 2, 0),
            new Among("\u0B95\u0BC0\u0BB4\u0BCD", -1, 1, 0)
        };

        private static readonly Among[] a_15 = new[]
        {
            new Among("\u0B85", -1, -1, 0),
            new Among("\u0B86", -1, -1, 0),
            new Among("\u0B87", -1, -1, 0),
            new Among("\u0B88", -1, -1, 0),
            new Among("\u0B89", -1, -1, 0),
            new Among("\u0B8A", -1, -1, 0),
            new Among("\u0B8E", -1, -1, 0),
            new Among("\u0B8F", -1, -1, 0),
            new Among("\u0B90", -1, -1, 0),
            new Among("\u0B92", -1, -1, 0),
            new Among("\u0B93", -1, -1, 0),
            new Among("\u0B94", -1, -1, 0)
        };

        private static readonly Among[] a_16 = new[]
        {
            new Among("\u0B95", -1, 1, 0),
            new Among("\u0BA4", -1, 1, 0),
            new Among("\u0BA9", -1, 1, 0),
            new Among("\u0BAA", -1, 1, 0),
            new Among("\u0BAF", -1, 1, 0),
            new Among("\u0BBE", -1, 5, 0),
            new Among("\u0B95\u0BC1", -1, 6, 0),
            new Among("\u0BAA\u0B9F\u0BC1", -1, 1, 0),
            new Among("\u0BA4\u0BC1", -1, 3, 0),
            new Among("\u0BBF\u0BB1\u0BCD\u0BB1\u0BC1", -1, 1, 0),
            new Among("\u0BA9\u0BC8", -1, 1, 0),
            new Among("\u0BB5\u0BC8", -1, 1, 0),
            new Among("\u0BA9\u0BA9\u0BCD", -1, 1, 0),
            new Among("\u0BAA\u0BA9\u0BCD", -1, 1, 0),
            new Among("\u0BB5\u0BA9\u0BCD", -1, 2, 0),
            new Among("\u0BBE\u0BA9\u0BCD", -1, 4, 0),
            new Among("\u0BA9\u0BBE\u0BA9\u0BCD", 15, 1, 0),
            new Among("\u0BAE\u0BBF\u0BA9\u0BCD", -1, 1, 0),
            new Among("\u0BA9\u0BC6\u0BA9\u0BCD", -1, 1, 0),
            new Among("\u0BC7\u0BA9\u0BCD", -1, 5, 0),
            new Among("\u0BA9\u0BAE\u0BCD", -1, 1, 0),
            new Among("\u0BAA\u0BAE\u0BCD", -1, 1, 0),
            new Among("\u0BBE\u0BAE\u0BCD", -1, 5, 0),
            new Among("\u0B95\u0BC1\u0BAE\u0BCD", -1, 1, 0),
            new Among("\u0B9F\u0BC1\u0BAE\u0BCD", -1, 5, 0),
            new Among("\u0BA4\u0BC1\u0BAE\u0BCD", -1, 1, 0),
            new Among("\u0BB1\u0BC1\u0BAE\u0BCD", -1, 1, 0),
            new Among("\u0BC6\u0BAE\u0BCD", -1, 5, 0),
            new Among("\u0BC7\u0BAE\u0BCD", -1, 5, 0),
            new Among("\u0BCB\u0BAE\u0BCD", -1, 5, 0),
            new Among("\u0BBE\u0BAF\u0BCD", -1, 5, 0),
            new Among("\u0BA9\u0BB0\u0BCD", -1, 1, 0),
            new Among("\u0BAA\u0BB0\u0BCD", -1, 1, 0),
            new Among("\u0BC0\u0BAF\u0BB0\u0BCD", -1, 5, 0),
            new Among("\u0BB5\u0BB0\u0BCD", -1, 1, 0),
            new Among("\u0BBE\u0BB0\u0BCD", -1, 5, 0),
            new Among("\u0BA9\u0BBE\u0BB0\u0BCD", 35, 1, 0),
            new Among("\u0BAE\u0BBE\u0BB0\u0BCD", 35, 1, 0),
            new Among("\u0B95\u0BCA\u0BA3\u0BCD\u0B9F\u0BBF\u0BB0\u0BCD", -1, 1, 0),
            new Among("\u0BA9\u0BBF\u0BB0\u0BCD", -1, 5, 0),
            new Among("\u0BC0\u0BB0\u0BCD", -1, 5, 0),
            new Among("\u0BA9\u0BB3\u0BCD", -1, 1, 0),
            new Among("\u0BAA\u0BB3\u0BCD", -1, 1, 0),
            new Among("\u0BB5\u0BB3\u0BCD", -1, 1, 0),
            new Among("\u0BBE\u0BB3\u0BCD", -1, 5, 0),
            new Among("\u0BA9\u0BBE\u0BB3\u0BCD", 44, 1, 0)
        };

        private static readonly Among[] a_17 = new[]
        {
            new Among("\u0B95\u0BBF\u0BB1", -1, -1, 0),
            new Among("\u0B95\u0BBF\u0BA9\u0BCD\u0BB1", -1, -1, 0),
            new Among("\u0BBE\u0BA8\u0BBF\u0BA9\u0BCD\u0BB1", -1, -1, 0),
            new Among("\u0B95\u0BBF\u0BB1\u0BCD", -1, -1, 0),
            new Among("\u0B95\u0BBF\u0BA9\u0BCD\u0BB1\u0BCD", -1, -1, 0),
            new Among("\u0BBE\u0BA8\u0BBF\u0BA9\u0BCD\u0BB1\u0BCD", -1, -1, 0)
        };


        private bool r_fix_va_start()
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
                    slice_from("\u0B93");
                    break;
                }
                case 2: {
                    slice_from("\u0B92");
                    break;
                }
                case 3: {
                    slice_from("\u0B89");
                    break;
                }
                case 4: {
                    slice_from("\u0B8A");
                    break;
                }
            }
            return true;
        }

        private bool r_fix_endings()
        {
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    if (!r_fix_ending())
                        goto lab1;
                    continue;
                lab1: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            return true;
        }

        private bool r_fix_ending()
        {
            int among_var;
            if (current.Length < 4)
            {
                return false;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_5, null);
                if (among_var == 0)
                {
                    goto lab1;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        slice_del();
                        break;
                    }
                    case 2: {
                        {
                            int c2 = limit - cursor;
                            if (find_among_b(a_2, null) == 0)
                            {
                                goto lab1;
                            }
                            cursor = limit - c2;
                        }
                        slice_del();
                        break;
                    }
                    case 3: {
                        slice_from("\u0BB3\u0BCD");
                        break;
                    }
                    case 4: {
                        slice_from("\u0BB2\u0BCD");
                        break;
                    }
                    case 5: {
                        slice_from("\u0B9F\u0BC1");
                        break;
                    }
                    case 6: {
                        if (!B_found_vetrumai_urupu)
                        {
                            goto lab1;
                        }
                        if (!(eq_s_b("\u0BC8")))
                        {
                            goto lab2;
                        }
                        goto lab1;
                    lab2: ;
                        slice_from("\u0BAE\u0BCD");
                        break;
                    }
                    case 7: {
                        slice_from("\u0BCD");
                        break;
                    }
                    case 8: {
                        {
                            int c3 = limit - cursor;
                            if (find_among_b(a_3, null) == 0)
                            {
                                goto lab3;
                            }
                            goto lab1;
                        lab3: ;
                            cursor = limit - c3;
                        }
                        slice_del();
                        break;
                    }
                    case 9: {
                        among_var = find_among_b(a_4, null);
                        switch (among_var) {
                            case 1: {
                                slice_del();
                                break;
                            }
                            case 2: {
                                slice_from("\u0BAE\u0BCD");
                                break;
                            }
                        }
                        break;
                    }
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                if (!(eq_s_b("\u0BCD")))
                {
                    return false;
                }
                {
                    int c4 = limit - cursor;
                    if (find_among_b(a_6, null) == 0)
                    {
                        goto lab5;
                    }
                    {
                        int c5 = limit - cursor;
                        if (!(eq_s_b("\u0BCD")))
                        {
                            {
                                cursor = limit - c5;
                                goto lab6;
                            }
                        }
                        if (find_among_b(a_6, null) == 0)
                        {
                            {
                                cursor = limit - c5;
                                goto lab6;
                            }
                        }
                    lab6: ;
                    }
                    bra = cursor;
                    slice_del();
                    goto lab4;
                lab5: ;
                    cursor = limit - c4;
                    if (find_among_b(a_7, null) == 0)
                    {
                        goto lab7;
                    }
                    bra = cursor;
                    if (!(eq_s_b("\u0BCD")))
                    {
                        goto lab7;
                    }
                    slice_del();
                    goto lab4;
                lab7: ;
                    cursor = limit - c4;
                    {
                        int c6 = limit - cursor;
                        if (find_among_b(a_8, null) == 0)
                        {
                            return false;
                        }
                        cursor = limit - c6;
                    }
                    bra = cursor;
                    slice_del();
                }
            lab4: ;
            }
        lab0: ;
            cursor = limit_backward;
            return true;
        }

        protected override bool stem()
        {
            int among_var;
            bool B_found_a_match;
            B_found_vetrumai_urupu = false;
            {
                int c1 = cursor;
                r_fix_ending();
                cursor = c1;
            }
            if (current.Length < 5)
            {
                return false;
            }
            {
                int c2 = cursor;
                bra = cursor;
                if (!(eq_s("\u0B8E")))
                {
                    goto lab0;
                }
                if (find_among(a_1, null) == 0)
                {
                    goto lab0;
                }
                if (!(eq_s("\u0BCD")))
                {
                    goto lab0;
                }
                ket = cursor;
                slice_del();
                {
                    int c3 = cursor;
                    r_fix_va_start();
                    cursor = c3;
                }
            lab0: ;
                cursor = c2;
            }
            {
                int c4 = cursor;
                bra = cursor;
                if (find_among(a_9, null) == 0)
                {
                    goto lab1;
                }
                if (find_among(a_1, null) == 0)
                {
                    goto lab1;
                }
                if (!(eq_s("\u0BCD")))
                {
                    goto lab1;
                }
                ket = cursor;
                slice_del();
                {
                    int c5 = cursor;
                    r_fix_va_start();
                    cursor = c5;
                }
            lab1: ;
                cursor = c4;
            }
            if (current.Length < 5)
            {
                goto lab2;
            }
            limit_backward = cursor;
            cursor = limit;
            {
                int c6 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_11, null) == 0)
                {
                    goto lab3;
                }
                bra = cursor;
                slice_from("\u0BCD");
            lab3: ;
                cursor = limit - c6;
            }
            cursor = limit_backward;
            r_fix_endings();
        lab2: ;
            {
                int c7 = cursor;
                if (current.Length < 5)
                {
                    goto lab4;
                }
                limit_backward = cursor;
                cursor = limit;
                ket = cursor;
                if (!(eq_s_b("\u0BC1\u0BAE\u0BCD")))
                {
                    goto lab4;
                }
                bra = cursor;
                slice_from("\u0BCD");
                cursor = limit_backward;
                {
                    int c8 = cursor;
                    r_fix_ending();
                    cursor = c8;
                }
            lab4: ;
                cursor = c7;
            }
            {
                int c9 = cursor;
                if (current.Length < 5)
                {
                    goto lab5;
                }
                limit_backward = cursor;
                cursor = limit;
                ket = cursor;
                among_var = find_among_b(a_13, null);
                if (among_var == 0)
                {
                    goto lab5;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("\u0BCD");
                        break;
                    }
                    case 2: {
                        {
                            int c10 = limit - cursor;
                            if (find_among_b(a_3, null) == 0)
                            {
                                goto lab6;
                            }
                            goto lab5;
                        lab6: ;
                            cursor = limit - c10;
                        }
                        slice_from("\u0BCD");
                        break;
                    }
                    case 3: {
                        slice_del();
                        break;
                    }
                }
                cursor = limit_backward;
                r_fix_endings();
            lab5: ;
                cursor = c9;
            }
            {
                int c11 = cursor;
                B_found_vetrumai_urupu = false;
                if (current.Length < 5)
                {
                    goto lab7;
                }
                limit_backward = cursor;
                cursor = limit;
                {
                    int c12 = limit - cursor;
                    {
                        int c13 = limit - cursor;
                        ket = cursor;
                        among_var = find_among_b(a_14, null);
                        if (among_var == 0)
                        {
                            goto lab9;
                        }
                        bra = cursor;
                        switch (among_var) {
                            case 1: {
                                slice_del();
                                break;
                            }
                            case 2: {
                                slice_from("\u0BCD");
                                break;
                            }
                            case 3: {
                                if (!(eq_s_b("\u0BAE")))
                                {
                                    goto lab10;
                                }
                                goto lab9;
                            lab10: ;
                                slice_from("\u0BCD");
                                break;
                            }
                            case 4: {
                                if (current.Length < 7)
                                {
                                    goto lab9;
                                }
                                slice_from("\u0BCD");
                                break;
                            }
                            case 5: {
                                {
                                    int c14 = limit - cursor;
                                    if (find_among_b(a_3, null) == 0)
                                    {
                                        goto lab11;
                                    }
                                    goto lab9;
                                lab11: ;
                                    cursor = limit - c14;
                                }
                                slice_from("\u0BCD");
                                break;
                            }
                            case 6: {
                                {
                                    int c15 = limit - cursor;
                                    if (find_among_b(a_3, null) == 0)
                                    {
                                        goto lab12;
                                    }
                                    goto lab9;
                                lab12: ;
                                    cursor = limit - c15;
                                }
                                slice_del();
                                break;
                            }
                            case 7: {
                                slice_from("\u0BBF");
                                break;
                            }
                        }
                        cursor = limit - c13;
                    }
                    goto lab8;
                lab9: ;
                    cursor = limit - c12;
                    {
                        int c16 = limit - cursor;
                        ket = cursor;
                        if (!(eq_s_b("\u0BC8")))
                        {
                            goto lab7;
                        }
                        {
                            int c17 = limit - cursor;
                            {
                                int c18 = limit - cursor;
                                if (find_among_b(a_6, null) == 0)
                                {
                                    goto lab15;
                                }
                                goto lab14;
                            lab15: ;
                                cursor = limit - c18;
                            }
                            goto lab13;
                        lab14: ;
                            cursor = limit - c17;
                            {
                                int c19 = limit - cursor;
                                if (find_among_b(a_6, null) == 0)
                                {
                                    goto lab7;
                                }
                                if (!(eq_s_b("\u0BCD")))
                                {
                                    goto lab7;
                                }
                                cursor = limit - c19;
                            }
                        }
                    lab13: ;
                        bra = cursor;
                        slice_from("\u0BCD");
                        cursor = limit - c16;
                    }
                }
            lab8: ;
                B_found_vetrumai_urupu = true;
                {
                    int c20 = limit - cursor;
                    ket = cursor;
                    if (!(eq_s_b("\u0BBF\u0BA9\u0BCD")))
                    {
                        goto lab16;
                    }
                    bra = cursor;
                    slice_from("\u0BCD");
                lab16: ;
                    cursor = limit - c20;
                }
                cursor = limit_backward;
                r_fix_endings();
            lab7: ;
                cursor = c11;
            }
            {
                int c21 = cursor;
                limit_backward = cursor;
                cursor = limit;
                ket = cursor;
                among_var = find_among_b(a_10, null);
                if (among_var == 0)
                {
                    goto lab17;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        {
                            int c22 = limit - cursor;
                            if (find_among_b(a_6, null) == 0)
                            {
                                goto lab19;
                            }
                            slice_from("\u0BC1\u0B99\u0BCD");
                            goto lab18;
                        lab19: ;
                            cursor = limit - c22;
                            slice_from("\u0BCD");
                        }
                    lab18: ;
                        break;
                    }
                    case 2: {
                        slice_from("\u0BB2\u0BCD");
                        break;
                    }
                    case 3: {
                        slice_from("\u0BB3\u0BCD");
                        break;
                    }
                    case 4: {
                        slice_del();
                        break;
                    }
                }
                cursor = limit_backward;
            lab17: ;
                cursor = c21;
            }
            {
                int c23 = cursor;
                if (current.Length < 5)
                {
                    goto lab20;
                }
                limit_backward = cursor;
                cursor = limit;
                ket = cursor;
                if (find_among_b(a_12, null) == 0)
                {
                    goto lab20;
                }
                bra = cursor;
                slice_del();
                cursor = limit_backward;
            lab20: ;
                cursor = c23;
            }
            {
                int c24 = cursor;
                while (true)
                {
                    int c25 = cursor;
                    B_found_a_match = false;
                    if (current.Length < 5)
                    {
                        goto lab22;
                    }
                    limit_backward = cursor;
                    cursor = limit;
                    {
                        int c26 = limit - cursor;
                        {
                            int c27 = limit - cursor;
                            ket = cursor;
                            among_var = find_among_b(a_16, null);
                            if (among_var == 0)
                            {
                                goto lab23;
                            }
                            bra = cursor;
                            switch (among_var) {
                                case 1: {
                                    slice_del();
                                    break;
                                }
                                case 2: {
                                    {
                                        int c28 = limit - cursor;
                                        if (find_among_b(a_15, null) == 0)
                                        {
                                            goto lab24;
                                        }
                                        goto lab23;
                                    lab24: ;
                                        cursor = limit - c28;
                                    }
                                    slice_del();
                                    break;
                                }
                                case 3: {
                                    {
                                        int c29 = limit - cursor;
                                        if (find_among_b(a_3, null) == 0)
                                        {
                                            goto lab25;
                                        }
                                        goto lab23;
                                    lab25: ;
                                        cursor = limit - c29;
                                    }
                                    slice_del();
                                    break;
                                }
                                case 4: {
                                    if (!(eq_s_b("\u0B9A")))
                                    {
                                        goto lab26;
                                    }
                                    goto lab23;
                                lab26: ;
                                    slice_from("\u0BCD");
                                    break;
                                }
                                case 5: {
                                    slice_from("\u0BCD");
                                    break;
                                }
                                case 6: {
                                    {
                                        int c30 = limit - cursor;
                                        if (!(eq_s_b("\u0BCD")))
                                        {
                                            goto lab23;
                                        }
                                        cursor = limit - c30;
                                    }
                                    slice_del();
                                    break;
                                }
                            }
                            B_found_a_match = true;
                            cursor = limit - c27;
                        }
                    lab23: ;
                        cursor = limit - c26;
                    }
                    {
                        int c31 = limit - cursor;
                        ket = cursor;
                        if (find_among_b(a_17, null) == 0)
                        {
                            goto lab27;
                        }
                        bra = cursor;
                        slice_del();
                        B_found_a_match = true;
                    lab27: ;
                        cursor = limit - c31;
                    }
                    cursor = limit_backward;
                    r_fix_endings();
                    if (!B_found_a_match)
                    {
                        goto lab22;
                    }
                    continue;
                lab22: ;
                    cursor = c25;
                    break;
                }
                cursor = c24;
            }
            return true;
        }

    }
}

