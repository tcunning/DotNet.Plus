using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Plus.Collection
{
    public static class List
    {
        public static bool TryTakeFirst<TValue>(this IList<TValue>? list, out TValue item)
        {
            if( list == null || list.Count == 0 ) {
                #pragma warning disable CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                item = default;
                #pragma warning restore CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                return false;
            }

            item = list[0];
            list.RemoveAt(0);
            return true;
        }
    }
}
