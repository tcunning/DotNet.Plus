using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Enum extensions for debugging flag enums
    /// </summary>
    public static class EnumFlagsDebug
    {
        /// <summary>
        /// Debug method for dumping a flags enum and showing the values in the enum associated with the
        /// given flags
        /// </summary>
        /// <typeparam name="TEnum">The enum type</typeparam>
        /// <param name="enumFlags">The enum that is to be converted to a delimited string</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>A string useful for debugging that represents the given enumFlags</returns>
        public static string DebugDumpAsFlags<TEnum>(this TEnum enumFlags, string separator = ", ") 
            where TEnum : Enum =>
            DebugDumpAsFlags<TEnum>(enumFlags.ToValue<UInt32>(), separator);

        /// <summary>
        /// Debug method for dumping a flags enum and showing the values in the enum associated with the
        /// given flags
        /// </summary>
        /// <typeparam name="TEnum">The enum type</typeparam>
        /// <param name="enumFlagsInt">Bits flags that will be looked up using TEnum</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>A string useful for debugging that represents the given enumFlags</returns>
        public static string DebugDumpAsFlags<TEnum>(this UInt32 enumFlagsInt, string separator = ", ")
            where TEnum : Enum
        {
            var matchingFlagsList = new List<string>();

            var allPossibleFlags = Enum<TEnum>.GetValues();
            matchingFlagsList.AddRange(from possibleFlag in allPossibleFlags
                                       where (enumFlagsInt & possibleFlag.ToValue<UInt32>()) != 0x0000
                                       select possibleFlag.ToString());
            var formattedFlagsString = matchingFlagsList.Any() ? String.Join(separator, matchingFlagsList) : "none";

            return formattedFlagsString;
        }

        /// <summary>
        /// Debug method for dumping a flags enum and showing the values in the enum associated with the
        /// given flags
        /// </summary>
        /// <typeparam name="TEnum">The enum type</typeparam>
        /// <param name="enumFlagsInt">Bits flags that will be looked up using TEnum</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>A string useful for debugging that represents the given enumFlags</returns>
        public static string DebugDumpAsFlags<TEnum>(this int enumFlagsInt, string separator = ", ")
            where TEnum : Enum
        {
            var matchingFlagsList = new List<string>();

            var allPossibleFlags = Enum<TEnum>.GetValues();
            matchingFlagsList.AddRange(from possibleFlag in allPossibleFlags
                where (enumFlagsInt & possibleFlag.ToValue<int>()) != 0x0000
                select possibleFlag.ToString());
            var formattedFlagsString = matchingFlagsList.Any() ? String.Join(separator, matchingFlagsList) : "none";

            return formattedFlagsString;
        }

    }
}
