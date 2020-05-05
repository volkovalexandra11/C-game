using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game.Extensions
{
    public static class EnumerableExtensions
    {
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector
        )
            where TKey : IComparable
        {
            return source
                .Aggregate((minEl, nextEl) =>
                            keySelector(minEl).CompareTo(keySelector(nextEl)) > 0
                            ? nextEl : minEl
                );
        }
    }
}
