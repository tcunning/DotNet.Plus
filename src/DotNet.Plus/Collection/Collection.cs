using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Plus.Core;

namespace DotNet.Plus.Collection
{
    public static class CollectionEx
    {
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis
        // todo: [NotNullWhen(true)] coming in .net 5!
        public static bool TryGetValueAtIndex<TItem>(this ICollection<TItem>? collection, int index, out TItem item)
        {
            try
            {
                if( collection == null )
                    throw new ArgumentNullException(nameof(collection));

                if( index >= collection.Count || index < 0 )
                    throw new ArgumentOutOfRangeException(nameof(index));

                item = collection.ElementAt(index);
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
        
        public static bool TryGetValueAtIndex<TItem>(this ICollection<TItem>? collection, object intIndexObj, out TItem item)
        {
            try
            {
                if( collection == null)
                    throw new ArgumentNullException(nameof(collection));

                int selectedIndex = ConvertUnchecked.ToInt32(intIndexObj);
                return collection.TryGetValueAtIndex(selectedIndex, out item);
            }
            catch
            {
                #pragma warning disable CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                item = default;
                #pragma warning restore CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        /// <returns>True is the item was removed or not found in the list.  Return false if there
        /// was a problem such as the collection being a fixed size.</returns>
        public static bool TryRemove<TItem>(this ICollection<TItem>? collection, TItem item)
        {
            try
            {
                if( collection == null )
                    throw new ArgumentNullException(nameof(collection));

                collection.Remove(item);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
