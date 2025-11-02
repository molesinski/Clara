// Generated from tamil.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from tamil.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
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
            new Among("\u0B95", -1, -1, 0),
            new Among("\u0B9A", -1, -1, 0),
            new Among("\u0B9F", -1, -1, 0),
            new Among("\u0BA4", -1, -1, 0),
            new Among("\u0BAA", -1, -1, 0),
            new Among("\u0BB1", -1, -1, 0)
        };

        private static readonly Among[] a_8 = new[]
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

        private static readonly Among[] a_9 = new[]
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

        private static readonly Among[] a_10 = new[]
        {
            new Among("\u0B85", -1, -1, 0),
            new Among("\u0B87", -1, -1, 0),
            new Among("\u0B89", -1, -1, 0)
        };

        private static readonly Among[] a_11 = new[]
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

        private static readonly Among[] a_12 = new[]
        {
            new Among("\u0B95", -1, -1, 0),
            new Among("\u0B9A", -1, -1, 0),
            new Among("\u0B9F", -1, -1, 0),
            new Among("\u0BA4", -1, -1, 0),
            new Among("\u0BAA", -1, -1, 0),
            new Among("\u0BB1", -1, -1, 0)
        };

        private static readonly Among[] a_13 = new[]
        {
            new Among("\u0B95\u0BB3\u0BCD", -1, 4, 0),
            new Among("\u0BC1\u0B99\u0BCD\u0B95\u0BB3\u0BCD", 0, 1, 0),
            new Among("\u0B9F\u0BCD\u0B95\u0BB3\u0BCD", 0, 3, 0),
            new Among("\u0BB1\u0BCD\u0B95\u0BB3\u0BCD", 0, 2, 0)
        };

        private static readonly Among[] a_14 = new[]
        {
            new Among("\u0BBE", -1, -1, 0),
            new Among("\u0BC7", -1, -1, 0),
            new Among("\u0BCB", -1, -1, 0)
        };

        private static readonly Among[] a_15 = new[]
        {
            new Among("\u0BAA\u0BBF", -1, -1, 0),
            new Among("\u0BB5\u0BBF", -1, -1, 0)
        };

        private static readonly Among[] a_16 = new[]
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

        private static readonly Among[] a_17 = new[]
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

        private static readonly Among[] a_18 = new[]
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

        private static readonly Among[] a_19 = new[]
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

        private static readonly Among[] a_20 = new[]
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

        private static readonly Among[] a_21 = new[]
        {
            new Among("\u0B95", -1, -1, 0),
            new Among("\u0B9A", -1, -1, 0),
            new Among("\u0B9F", -1, -1, 0),
            new Among("\u0BA4", -1, -1, 0),
            new Among("\u0BAA", -1, -1, 0),
            new Among("\u0BB1", -1, -1, 0)
        };

        private static readonly Among[] a_22 = new[]
        {
            new Among("\u0B95", -1, -1, 0),
            new Among("\u0B9A", -1, -1, 0),
            new Among("\u0B9F", -1, -1, 0),
            new Among("\u0BA4", -1, -1, 0),
            new Among("\u0BAA", -1, -1, 0),
            new Among("\u0BB1", -1, -1, 0)
        };

        private static readonly Among[] a_23 = new[]
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

        private static readonly Among[] a_24 = new[]
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

        private static readonly Among[] a_25 = new[]
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

        private static readonly Among[] a_26 = new[]
        {
            new Among("\u0B95\u0BBF\u0BB1", -1, -1, 0),
            new Among("\u0B95\u0BBF\u0BA9\u0BCD\u0BB1", -1, -1, 0),
            new Among("\u0BBE\u0BA8\u0BBF\u0BA9\u0BCD\u0BB1", -1, -1, 0),
            new Among("\u0B95\u0BBF\u0BB1\u0BCD", -1, -1, 0),
            new Among("\u0B95\u0BBF\u0BA9\u0BCD\u0BB1\u0BCD", -1, -1, 0),
            new Among("\u0BBE\u0BA8\u0BBF\u0BA9\u0BCD\u0BB1\u0BCD", -1, -1, 0)
        };


        private bool r_has_min_length()
        {
            return current.Length > 4;
        }

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

        private bool r_remove_question_prefixes()
        {
            bra = cursor;
            if (!(eq_s("\u0B8E")))
            {
                return false;
            }
            if (find_among(a_1, null) == 0)
            {
                return false;
            }
            if (!(eq_s("\u0BCD")))
            {
                return false;
            }
            ket = cursor;
            slice_del();
            {
                int c1 = cursor;
                r_fix_va_start();
                cursor = c1;
            }
            return true;
        }

        private bool r_fix_ending()
        {
            int among_var;
            if (current.Length <= 3)
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
                        {
                            int c3 = limit - cursor;
                            if (!(eq_s_b("\u0BC8")))
                            {
                                goto lab2;
                            }
                            goto lab1;
                        lab2: ;
                            cursor = limit - c3;
                        }
                        slice_from("\u0BAE\u0BCD");
                        break;
                    }
                    case 7: {
                        slice_from("\u0BCD");
                        break;
                    }
                    case 8: {
                        {
                            int c4 = limit - cursor;
                            if (find_among_b(a_3, null) == 0)
                            {
                                goto lab3;
                            }
                            goto lab1;
                        lab3: ;
                            cursor = limit - c4;
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
                    int c5 = limit - cursor;
                    if (find_among_b(a_6, null) == 0)
                    {
                        goto lab5;
                    }
                    {
                        int c6 = limit - cursor;
                        if (!(eq_s_b("\u0BCD")))
                        {
                            {
                                cursor = limit - c6;
                                goto lab6;
                            }
                        }
                        if (find_among_b(a_7, null) == 0)
                        {
                            {
                                cursor = limit - c6;
                                goto lab6;
                            }
                        }
                    lab6: ;
                    }
                    bra = cursor;
                    slice_del();
                    goto lab4;
                lab5: ;
                    cursor = limit - c5;
                    if (find_among_b(a_8, null) == 0)
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
                    cursor = limit - c5;
                    {
                        int c7 = limit - cursor;
                        if (find_among_b(a_9, null) == 0)
                        {
                            return false;
                        }
                        cursor = limit - c7;
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

        private bool r_remove_pronoun_prefixes()
        {
            bra = cursor;
            if (find_among(a_10, null) == 0)
            {
                return false;
            }
            if (find_among(a_11, null) == 0)
            {
                return false;
            }
            if (!(eq_s("\u0BCD")))
            {
                return false;
            }
            ket = cursor;
            slice_del();
            {
                int c1 = cursor;
                r_fix_va_start();
                cursor = c1;
            }
            return true;
        }

        private bool r_remove_plural_suffix()
        {
            int among_var;
            limit_backward = cursor;
            cursor = limit;
            ket = cursor;
            among_var = find_among_b(a_13, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    {
                        int c1 = limit - cursor;
                        if (find_among_b(a_12, null) == 0)
                        {
                            goto lab1;
                        }
                        slice_from("\u0BC1\u0B99\u0BCD");
                        goto lab0;
                    lab1: ;
                        cursor = limit - c1;
                        slice_from("\u0BCD");
                    }
                lab0: ;
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
            return true;
        }

        private bool r_remove_question_suffixes()
        {
            if (!r_has_min_length())
                return false;
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_14, null) == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                slice_from("\u0BCD");
            lab0: ;
                cursor = limit - c1;
            }
            cursor = limit_backward;
            r_fix_endings();
            return true;
        }

        private bool r_remove_command_suffixes()
        {
            if (!r_has_min_length())
                return false;
            limit_backward = cursor;
            cursor = limit;
            ket = cursor;
            if (find_among_b(a_15, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            cursor = limit_backward;
            return true;
        }

        private bool r_remove_um()
        {
            if (!r_has_min_length())
                return false;
            limit_backward = cursor;
            cursor = limit;
            ket = cursor;
            if (!(eq_s_b("\u0BC1\u0BAE\u0BCD")))
            {
                return false;
            }
            bra = cursor;
            slice_from("\u0BCD");
            cursor = limit_backward;
            {
                int c1 = cursor;
                r_fix_ending();
                cursor = c1;
            }
            return true;
        }

        private bool r_remove_common_word_endings()
        {
            int among_var;
            if (!r_has_min_length())
                return false;
            limit_backward = cursor;
            cursor = limit;
            ket = cursor;
            among_var = find_among_b(a_17, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_from("\u0BCD");
                    break;
                }
                case 2: {
                    {
                        int c1 = limit - cursor;
                        if (find_among_b(a_16, null) == 0)
                        {
                            goto lab0;
                        }
                        return false;
                    lab0: ;
                        cursor = limit - c1;
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
            return true;
        }

        private bool r_remove_vetrumai_urupukal()
        {
            int among_var;
            B_found_vetrumai_urupu = false;
            if (!r_has_min_length())
                return false;
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                {
                    int c2 = limit - cursor;
                    ket = cursor;
                    among_var = find_among_b(a_20, null);
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
                            slice_from("\u0BCD");
                            break;
                        }
                        case 3: {
                            {
                                int c3 = limit - cursor;
                                if (!(eq_s_b("\u0BAE")))
                                {
                                    goto lab2;
                                }
                                goto lab1;
                            lab2: ;
                                cursor = limit - c3;
                            }
                            slice_from("\u0BCD");
                            break;
                        }
                        case 4: {
                            if (current.Length < 7)
                            {
                                goto lab1;
                            }
                            slice_from("\u0BCD");
                            break;
                        }
                        case 5: {
                            {
                                int c4 = limit - cursor;
                                if (find_among_b(a_18, null) == 0)
                                {
                                    goto lab3;
                                }
                                goto lab1;
                            lab3: ;
                                cursor = limit - c4;
                            }
                            slice_from("\u0BCD");
                            break;
                        }
                        case 6: {
                            {
                                int c5 = limit - cursor;
                                if (find_among_b(a_19, null) == 0)
                                {
                                    goto lab4;
                                }
                                goto lab1;
                            lab4: ;
                                cursor = limit - c5;
                            }
                            slice_del();
                            break;
                        }
                        case 7: {
                            slice_from("\u0BBF");
                            break;
                        }
                    }
                    cursor = limit - c2;
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                {
                    int c6 = limit - cursor;
                    ket = cursor;
                    if (!(eq_s_b("\u0BC8")))
                    {
                        return false;
                    }
                    {
                        int c7 = limit - cursor;
                        {
                            int c8 = limit - cursor;
                            if (find_among_b(a_21, null) == 0)
                            {
                                goto lab7;
                            }
                            goto lab6;
                        lab7: ;
                            cursor = limit - c8;
                        }
                        goto lab5;
                    lab6: ;
                        cursor = limit - c7;
                        {
                            int c9 = limit - cursor;
                            if (find_among_b(a_22, null) == 0)
                            {
                                return false;
                            }
                            if (!(eq_s_b("\u0BCD")))
                            {
                                return false;
                            }
                            cursor = limit - c9;
                        }
                    }
                lab5: ;
                    bra = cursor;
                    slice_from("\u0BCD");
                    cursor = limit - c6;
                }
            }
        lab0: ;
            B_found_vetrumai_urupu = true;
            {
                int c10 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("\u0BBF\u0BA9\u0BCD")))
                {
                    goto lab8;
                }
                bra = cursor;
                slice_from("\u0BCD");
            lab8: ;
                cursor = limit - c10;
            }
            cursor = limit_backward;
            r_fix_endings();
            return true;
        }

        private bool r_remove_tense_suffixes()
        {
            while (true)
            {
                int c1 = cursor;
                if (!r_remove_tense_suffix())
                    goto lab0;
                continue;
            lab0: ;
                cursor = c1;
                break;
            }
            return true;
        }

        private bool r_remove_tense_suffix()
        {
            bool B_found_a_match;
            int among_var;
            B_found_a_match = false;
            if (!r_has_min_length())
                return false;
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                {
                    int c2 = limit - cursor;
                    ket = cursor;
                    among_var = find_among_b(a_25, null);
                    if (among_var == 0)
                    {
                        goto lab0;
                    }
                    bra = cursor;
                    switch (among_var) {
                        case 1: {
                            slice_del();
                            break;
                        }
                        case 2: {
                            {
                                int c3 = limit - cursor;
                                if (find_among_b(a_23, null) == 0)
                                {
                                    goto lab1;
                                }
                                goto lab0;
                            lab1: ;
                                cursor = limit - c3;
                            }
                            slice_del();
                            break;
                        }
                        case 3: {
                            {
                                int c4 = limit - cursor;
                                if (find_among_b(a_24, null) == 0)
                                {
                                    goto lab2;
                                }
                                goto lab0;
                            lab2: ;
                                cursor = limit - c4;
                            }
                            slice_del();
                            break;
                        }
                        case 4: {
                            {
                                int c5 = limit - cursor;
                                if (!(eq_s_b("\u0B9A")))
                                {
                                    goto lab3;
                                }
                                goto lab0;
                            lab3: ;
                                cursor = limit - c5;
                            }
                            slice_from("\u0BCD");
                            break;
                        }
                        case 5: {
                            slice_from("\u0BCD");
                            break;
                        }
                        case 6: {
                            {
                                int c6 = limit - cursor;
                                if (!(eq_s_b("\u0BCD")))
                                {
                                    goto lab0;
                                }
                                cursor = limit - c6;
                            }
                            slice_del();
                            break;
                        }
                    }
                    B_found_a_match = true;
                    cursor = limit - c2;
                }
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c7 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_26, null) == 0)
                {
                    goto lab4;
                }
                bra = cursor;
                slice_del();
                B_found_a_match = true;
            lab4: ;
                cursor = limit - c7;
            }
            cursor = limit_backward;
            r_fix_endings();
            return B_found_a_match;
        }

        protected override bool stem()
        {
            B_found_vetrumai_urupu = false;
            {
                int c1 = cursor;
                r_fix_ending();
                cursor = c1;
            }
            if (!r_has_min_length())
                return false;
            {
                int c2 = cursor;
                r_remove_question_prefixes();
                cursor = c2;
            }
            {
                int c3 = cursor;
                r_remove_pronoun_prefixes();
                cursor = c3;
            }
            {
                int c4 = cursor;
                r_remove_question_suffixes();
                cursor = c4;
            }
            {
                int c5 = cursor;
                r_remove_um();
                cursor = c5;
            }
            {
                int c6 = cursor;
                r_remove_common_word_endings();
                cursor = c6;
            }
            {
                int c7 = cursor;
                r_remove_vetrumai_urupukal();
                cursor = c7;
            }
            {
                int c8 = cursor;
                r_remove_plural_suffix();
                cursor = c8;
            }
            {
                int c9 = cursor;
                r_remove_command_suffixes();
                cursor = c9;
            }
            {
                int c10 = cursor;
                r_remove_tense_suffixes();
                cursor = c10;
            }
            return true;
        }

    }
}

