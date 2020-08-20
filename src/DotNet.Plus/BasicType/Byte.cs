using System;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Constants for working with bit/byte conversions
    /// </summary>
    public static class Byte
    {
        /// <summary>
        /// Number bases supported for doing a debug dump on an array.
        /// </summary>
        public enum NumberBase
        {
            Binary = 2,
            Hex = 16
        }

        /// <summary>
        /// The number of bytes in 64 bits (8)
        /// </summary>
        public const int BytesIn64Bits = (64 / 8); // 8 bytes

        /// <summary>
        /// The number of Bits in a byte
        /// </summary>
        public const int BitsInByte = 8;
    }
}
