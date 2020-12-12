using System;
using System.Collections.Generic;
using System.Linq;

namespace Cobalt.Unit.Fact.Map
{
    internal static class Helpers
    {
        public static IEnumerable<T> Yield<T>(T item)
        {
            yield return item;
        }

        public static byte BitCount(uint i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (byte)((((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24);
        }

        public static T[] Copy<T>(T[] arr) => (arr == null) ? null : arr.ToArray();
    }
}
