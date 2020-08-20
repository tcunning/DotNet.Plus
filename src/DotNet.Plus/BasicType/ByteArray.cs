using System;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Extensions for working with byte arrays such as creating ArraySegments, Filling, and Clearing
    /// </summary>
    public static class ByteArray
    {
        /// <summary>
        /// Easy way to create an array segment from an array.
        /// </summary>
        /// <param name="byteArray">An array of bytes</param>
        /// <param name="offset">Starting offset</param>
        /// <returns>An array segment starting at offset and going until the end of the array</returns>
        public static ArraySegment<byte> ToArraySegment(this byte[] byteArray, int offset = 0) =>
            new ArraySegment<byte>(byteArray, offset, byteArray.Length - offset);

        /// <summary>
        /// Easy way to create an array segment from an array.
        /// </summary>
        /// <param name="byteArray">An array of bytes</param>
        /// <param name="offset">Starting offset</param>
        /// <param name="size">Number of bytes to include in the array segment</param>
        /// <returns>An array segment starting at offset and going for size bytes</returns>
        public static ArraySegment<byte> ToArraySegment(this byte[] byteArray, int offset, int size ) =>
            new ArraySegment<byte>(byteArray, offset, size);

        /// <summary>
        /// File a byte with the give fillValue
        /// </summary>
        /// <param name="byteArray">An array of bytes that will be filled</param>
        /// <param name="fillValue">The fill byte</param>
        /// <param name="offset">Starting index to fill</param>
        /// <returns>The buff will be filled with the given fillValue until the end of the byteArray</returns>
        public static byte[] Fill(this byte[] byteArray, byte fillValue, int offset = 0) =>
            Fill(byteArray, fillValue, offset, byteArray.Length - offset);

        /// <summary>
        /// File a byte array with the give fillValue
        /// </summary>
        /// <param name="byteArray">An array of bytes that will be filled</param>
        /// <param name="fillValue">The fill byte</param>
        /// <param name="offset">Starting index to fill, the byte will be filled from this starting index until the end of the array.</param>
        /// <param name="fillLength">The number of bytes to fill</param>
        /// <returns>The buff will be filled with the given fillValue until fillLength bytes have been filled</returns>
        public static byte[] Fill(this byte[] byteArray, byte fillValue, int offset, int fillLength)
        {
            if( fillLength == 0 )
                return byteArray;

            if( offset < 0 )
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"offset must be 0 or greater");

            if( fillLength < 0 )
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"size must be 0 or greater");

            var endIndex = offset + fillLength;
            for( int index = offset; index < endIndex; index += 1 )
                byteArray[index] = fillValue;

            return byteArray;
        }

        /// <summary>
        /// Fills the given byteArray with 0x00 from the start of the array to the end of the array.
        /// </summary>
        /// <param name="byteArray">An array of bytes that will be cleared</param>
        /// <returns>The buff will be filled 0x00 and returned</returns>
        public static byte[] Clear(this byte[] byteArray) => Fill(byteArray, 0x00);
    }
}
