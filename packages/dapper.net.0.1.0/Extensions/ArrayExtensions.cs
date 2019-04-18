using System;
using static System.Array;

namespace Dapper.Net.Extensions {

    public static class ArrayExtensions {
        public static T[] Concat<T>(this T[] x, T[] y) {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            var oldLen = x.Length;
            Resize(ref x, x.Length + y.Length);
            Copy(y, 0, x, oldLen, y.Length);
            return x;
        }
    }

}