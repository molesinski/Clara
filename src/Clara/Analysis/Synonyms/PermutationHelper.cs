using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    internal static class PermutationHelper
    {
        public static IEnumerable<ListSlim<TItem>> Permutate<TItem>(ListSlim<TItem> list)
            where TItem : notnull
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var length = list.Count;

            if (length == 0)
            {
                yield break;
            }

            if (length == 1)
            {
                yield return list;
                yield break;
            }

            var transform = new int[length];
            var result = new ListSlim<TItem>(list);

            for (var i = 0; i < length; i++)
            {
                transform[i] = i;
            }

            while (true)
            {
                for (var i = 0; i < length; i++)
                {
                    result[i] = list[transform[i]];
                }

                yield return result;

                var j = length - 2;

                while (j != -1 && transform[j] >= transform[j + 1])
                {
                    j--;
                }

                if (j == -1)
                {
                    break;
                }

                var k = length - 1;

                while (transform[k] <= transform[j])
                {
                    k--;
                }

                Swap(ref transform[j], ref transform[k]);

                j++;
                k = length - 1;

                while (j < k)
                {
                    Swap(ref transform[j++], ref transform[k--]);
                }
            }

            static void Swap(ref int left, ref int right)
            {
                var temp = left;
                left = right;
                right = temp;
            }
        }
    }
}
