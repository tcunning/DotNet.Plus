using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotNet.Plus.Core
{
    public static class Linq
    {
        public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            source.Where((src) => !predicate(src));
    }
}
