using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Plus.Core
{
    /// <summary>
    /// Adds extensions to Enumerable types to make them easier to use
    /// </summary>
    public static class EnumerableEx
    {
        /// <summary>
        /// Turns an IEnumerable into a IEnumerable{TItem}.  Only items that match the TItem
        /// type will be included in the new enumeration. 
        /// </summary>
        /// <typeparam name="TItem">Type to filter</typeparam>
        /// <param name="enumerable">An enumerable with no particular information</param>
        /// <returns></returns>
        public static IEnumerable<TItem> ToEnumerable<TItem>(this IEnumerable enumerable) =>
            enumerable.OfType<TItem>();

    }
}
