using System;
using System.Collections.Generic;

namespace DotNet.Plus.Collection
{
    /// <summary>
    /// Adds extensions to Lists such as TryTakeFirst
    /// </summary>
    public static class List
    {
        /// <summary>
        /// Will try to take the first item from the list.
        /// </summary>
        /// <typeparam name="TValue">The type of the items in the list</typeparam>
        /// <param name="list">The list who's first item is to be removed</param>
        /// <param name="item">The item removed or default</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        public static bool TryTakeFirst<TValue>(this IList<TValue>? list, out TValue item)
        {
            try
            {
                if( list == null || list.Count == 0 )
                    throw new ArgumentException();

                item = list[0];
                list.RemoveAt(0);
                return true;
            }
            catch
            {
                #pragma warning disable CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                item = default;
                #pragma warning restore CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                return false;
            }

        }
    }
}
