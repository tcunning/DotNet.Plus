using System;
using System.Collections.Generic;

namespace DotNet.Plus.Collection
{
    /// <summary>
    /// Adds extensions to HashSets such as HashSetEquals
    /// </summary>
    public static class HashSetEx
    {
        /// <summary>
        /// Test two hash sets to see if they have the same number of items handling null cases.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="hashSet1"></param>
        /// <param name="hashSet2"></param>
        /// <returns>true if the passed in HashSets are equal (including if they are both null) otherwise false</returns>
        public static bool HashSetEquals<TValue>(this HashSet<TValue>? hashSet1, HashSet<TValue>? hashSet2)
        {
            // If the hash sets references are equal including the case of both being null then
            // they are considered equal
            //
            if( ReferenceEquals(hashSet1, hashSet2) )
                return true;

            // If either HashSet is null and we know the References aren't equal (thus they aren't both null) 
            // then we know they can't be equal!
            //
            if( hashSet1 == null || hashSet2 == null )
                return false;

            return hashSet1.SetEquals(hashSet2);
        }
    }
}
