using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Array extensions for formatting strings that represent the byte array's content.
    /// </summary>
    public static class ByteArrayDebug
    {
        /// <summary>
        /// Returns the string representation of the given array in the specified number base. 
        /// </summary>
        /// <param name="buffer">The array of bytes, as an IReadOnlyList that can support anything that implements an indexer w/ enumeration.</param>
        /// <param name="startIndex">Zero base starting index</param>
        /// <param name="length">Number of bytes from the array to convert to ASCII</param>
        /// <param name="separator">Separator to use for the bytes</param>
        /// <param name="toBase">The number base to format the output string</param>
        /// <returns>An ASCII string representing the contents of the buffer in the specified base</returns>
        /// <example>
        /// Hex output example: "04 ff 21 fE 00 05"
        /// Binary output example: "00000100 11111111 00100001 11111110 00000000 00000101"
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException">If there is an issue with the startIndex, length, or toBase</exception>
        public static string DebugDump(this IReadOnlyList<byte> buffer, int startIndex, int length, string separator = " ", Byte.NumberBase toBase = Byte.NumberBase.Hex)
        {
            var stopIndex = startIndex + length;

            switch( toBase )
            {
                case Byte.NumberBase.Binary:
                    return string.Join(separator, buffer.Where((_, index) => index >= startIndex && index < stopIndex).Select(value => Convert.ToString(value, 2).PadLeft(8, '0')));

                case Byte.NumberBase.Hex:
                    return string.Join(separator, buffer.Where((_, index) => index >= startIndex && index < stopIndex).Select(value => Convert.ToString(value, 16).PadLeft(2, '0')));

                default:
                    throw new ArgumentOutOfRangeException(nameof(toBase), toBase, "Unsupported number base");
            }
        }

        /// <summary>
        /// Returns the string representation of the given array in the specified number base. 
        /// </summary>
        /// <param name="buffer">The array of bytes, as an IReadOnlyList that can support anything that implements an indexer w/ enumeration.</param>
        /// <param name="startIndex">Zero base starting index</param>
        /// <param name="separator">Separator to use for the bytes</param>
        /// <param name="toBase">The number base to format the output string</param>
        /// <returns>An ASCII string representing the contents of the buffer in the specified base</returns>
        /// <example>
        /// Hex output example: "04 ff 21 fE 00 05"
        /// Binary output example: "00000100 11111111 00100001 11111110 00000000 00000101"
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException">If there is an issue with the startIndex, length, or toBase</exception>
        public static string DebugDump(this IReadOnlyList<byte> buffer, int startIndex = 0, string separator = " ", Byte.NumberBase toBase = Byte.NumberBase.Hex) =>
            DebugDump(buffer, startIndex, buffer.Count - startIndex, separator, toBase);

        /// <summary>
        /// Returns the string representation of the given array in the specified number base. 
        /// </summary>
        /// <param name="buffer">The array of bytes, as an ArraySegment</param>
        /// <param name="separator">Separator to use for the bytes</param>
        /// <param name="toBase">The number base to format the output string</param>
        /// <returns>An ASCII string representing the contents of the buffer in the specified base</returns>
        /// <example>
        /// Hex output example: "04 ff 21 fE 00 05"
        /// Binary output example: "00000100 11111111 00100001 11111110 00000000 00000101"
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException">If there is an issue with the startIndex, length, or toBase</exception>
        public static string DebugDump(this ArraySegment<byte> buffer, string separator = " ", Byte.NumberBase toBase = Byte.NumberBase.Hex) =>
            DebugDump(buffer.Array, startIndex: buffer.Offset, length: buffer.Count, separator: separator, toBase: toBase);
    }
}
