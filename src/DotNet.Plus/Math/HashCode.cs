using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Plus.Math
{
    /// <summary>
    /// Computes a hash based on a variation of on tht Bernstein Hash.
    /// <see cref="https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode"/>
    /// <code>
    /// Usage:
    ///      public override int GetHashCode() => HashCode.Start.Hash(field1).Hash(field2).Hash(field3);
    /// </code>
    /// </summary>
    public static class HashCode
    {
        /// <summary>
        /// Starting seed
        /// </summary>
        public const int Start = 17;

        /// <summary>
        /// Generates the next hash code for the given hash and object.
        /// </summary>
        /// <typeparam name="TObj">Type of the object</typeparam>
        /// <param name="hash">The current hash code</param>
        /// <param name="obj">Object who's hash is to be added to the given hash</param>
        /// <returns>The new hash code</returns>
        public static int Hash<TObj>(this int hash, TObj obj)
        {
            var newHash = EqualityComparer<TObj>.Default.GetHashCode(obj);
            return unchecked((hash * 31) + newHash);
        }

        /// <summary>
        /// Generates the next hash code for the given hash and object.
        /// </summary>
        /// <typeparam name="TObj">Type of the object</typeparam>
        /// <param name="hash">The current hash code</param>
        /// <param name="objectArray">List of objects who's hash code is to be used to make the new hash</param>
        /// <returns>The new hash code</returns>
        public static int Hash<TObj>(this int hash, params TObj[]? objectArray) =>
            objectArray?.Aggregate(hash, Hash) ?? hash;

        /// <summary>
        /// The byte array has a specific implementation so it can be computed without boxing.
        /// </summary>
        /// <param name="hash">The current hash code</param>
        /// <param name="byteArray">List of integers who's hash code is to be used to make the new hash</param>
        /// <returns>The new hash code</returns>
        public static int Hash(this int hash, params byte[]? byteArray) =>
            hash.Hash((byteArray ?? Enumerable.Empty<byte>()));

        /// <summary>
        /// The byte array has a specific implementation so it can be computed without boxing.
        /// </summary>
        /// <param name="hash">The current hash code</param>
        /// <param name="byteArray">List of integers who's hash code is to be used to make the new hash</param>
        /// <returns>The new hash code</returns>
        public static int Hash(this int hash, IEnumerable<byte>? byteArray) =>
            unchecked(byteArray?.Aggregate(hash, (currentHash, foundByte) => (currentHash * 31) + foundByte)) ?? hash;
    }
}
