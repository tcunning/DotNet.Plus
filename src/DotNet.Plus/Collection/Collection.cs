using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Plus.Core;

namespace DotNet.Plus.Collection
{
    /// <summary>
    /// Adds extensions to ICollection's such as TryGetValueAtIndex and TryRemove
    /// </summary>
    public static class CollectionEx
    {
        /// <summary>
        /// Gets the value at the specified based index.
        /// </summary>
        /// <typeparam name="TItem">The type of the item in the collection</typeparam>
        /// <param name="collection">The collection to get the item from</param>
        /// <param name="index">Zero based index of the item</param>
        /// <param name="item">The item found or default if item wasn't able to be found</param>
        /// <returns>True if the item was found, else false</returns>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis
        /// todo: [NotNullWhen(true)] coming in .net 5!
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

        /// <summary>
        /// Gets the value at the specified based index.
        /// </summary>
        /// <typeparam name="TItem">The type of the item in the collection</typeparam>
        /// <param name="collection">The collection to get the item from</param>
        /// <param name="intIndexObj">Zero based index of the item expressed as an object.  An unchecked conversation
        /// will be attempted from the given object to an Int32</param>
        /// <param name="item">The item found or default if item wasn't able to be found</param>
        /// <returns>True if the item was found, else false</returns>
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
        /// Removes the item from the collection if it is in the collection.
        /// </summary>
        /// <typeparam name="TItem">The type of the item in the collection</typeparam>
        /// <param name="collection">The collection to get the item from</param>
        /// <param name="item">The item found or default if item wasn't able to be found</param>
        /// <param name="findBeforeRemove">If true, the item will be looked for in the collection
        /// before it is removed.  If it isn't in the collection then false will be returned.  This
        /// could be a slow operation for some collection types so it's defaulted to off</param>
        /// <returns>True is the item was removed.  If findBeforeRemove is false then true will also be
        /// returned when the item wasn't in the list. Return false if there was a problem such as the
        /// collection being a fixed size or the item wasn't in the collection and findBeforeRemove
        /// was true.</returns>
        public static bool TryRemove<TItem>(this ICollection<TItem>? collection, TItem item, bool findBeforeRemove = false)
        {
            try
            {
                if( collection == null )
                    throw new ArgumentNullException(nameof(collection));

                if( findBeforeRemove && !collection.Contains(item) )
                    return false;

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
