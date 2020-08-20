using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Plus.Core
{
    /// <summary>
    /// Adds extensions to Linq such as WhereNot
    /// </summary>
    public static class Linq
    {
        /// <summary>
        /// A simple filter like where but takes the NOT of the predicate
        /// </summary>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <param name="source">source to filter based on the predicate</param>
        /// <param name="predicate">Returns false to include the item in the last and true to exclude it</param>
        /// <returns>The items not filtered by the predicate</returns>
        public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            source.Where((src) => !predicate(src));
    }
}
