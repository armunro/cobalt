using System;
using System.Collections.Generic;
using System.Linq;

namespace Cobalt.Unit.Fact
{
    internal static class Extensions
    {
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        public static byte BitCount(this UInt32 i)
        {
            i -= ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (byte)((((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24);
        }

        public static T[] Copy<T>(this IEnumerable<T> arr) => arr?.ToArray();
    }
}
