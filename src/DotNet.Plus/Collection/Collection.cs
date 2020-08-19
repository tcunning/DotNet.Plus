using System;
using System.Collections.Generic;
using System.Linq;

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
            if( collection == null || !(intIndexObj is int selectedIndex) )
            {
                #pragma warning disable CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                item = default;
                #pragma warning restore CS8601 // Possible null reference assignment, currently no way to express this with templates in C# w/o defining TItem as a struct or class
                return false;
            }

            return collection.TryGetValueAtIndex(selectedIndex, out item);
        }

        public static void TryRemove<TItem>(this ICollection<TItem>? collection, TItem item)
        {
            try
            {
                if( collection?.Contains(item) == true )
                    collection.Remove(item);
            }
            catch
            {
                /* Ignored */
            }
        }
    }
}
