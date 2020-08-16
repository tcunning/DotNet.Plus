using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Plus.BasicType
{
    public static class EnumFlagsDebug
    {
        public static string DebugDumpAsFlags<TEnum>(this TEnum enumFlags, string separator = ", ") 
            where TEnum : Enum =>
            DebugDumpAsFlags<TEnum>(enumFlags.ToValue<UInt32>(), separator);

        /// <summary>
        /// Returns a formatted string that represents the values
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumFlagsInt"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
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
