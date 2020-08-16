using System;
using System.Collections.Generic;
using DotNet.Plus.BasicType;

namespace DotNet.Plus.Endian
{
    public static partial class Endianness
    {
        #region UInt32
        /// <summary>
        /// Reads bytes from the given buffer and formats them via the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="buffer">The byte buffer.  This supports a variety of data types such as byte arrays,
        /// lists, etc.</param>
        /// <param name="startOffset">The zero-based index of the starting byte to read, there must be at
        /// least 4 bytes in the buffer starting from this index.</param>
        /// <param name="endian">Specifies the format, <see cref="Endianness"/>, of the value in the given buffer.</param>
        /// <returns>The value read from the buffer using the specified <see cref="Endianness"/> formatted as a C# value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to read data from buffer because it's too small, etc</exception>
        public static UInt32 ToUInt32(this IReadOnlyList<byte> buffer, int startOffset = 0, EndianFormat endian = EndianFormat.Big) =>
            (UInt32)ToUInt64(buffer, sizeof(UInt32), startOffset, signExtend: false, endian);

        /// <summary>
        /// Writes the value into the given buffer using the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="value">The value that is to be written to the given buffer in the specified <see cref="Endianness"/></param>
        /// <param name="buffer">The destination buffer identified by the given ArraySegment.  There must be at least 4 bytes
        /// of space available in the buffer.</param>
        /// <param name="endian">The data will be written to the buffer in this <see cref="Endianness"/>.</param>
        /// <returns>The passed in buffer, this allows for call chaining to other operations.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to write data into the buffer</exception>
        public static ArraySegment<byte> ToBuffer(this UInt32 value, ArraySegment<byte> buffer, EndianFormat endian = EndianFormat.Big) =>
            ToBuffer(value, sizeof(UInt32), buffer, endian);

        /// <summary>
        /// Writes the value into the given buffer using the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="value">The value that is to be written to the given buffer in the specified <see cref="Endianness"/></param>
        /// <param name="buffer">The destination buffer.  There must be at least 4 bytes of space available in the buffer
        /// from the specified offset.</param>
        /// <param name="startOffset">The zero-based index of the starting byte to write</param>
        /// <param name="endian">The data will be written to the buffer in this <see cref="Endianness"/>.</param>
        /// <returns>The passed in buffer, this allows for call chaining to other operations.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to write data into the buffer</exception>
        public static byte[] ToBuffer(this UInt32 value, byte[] buffer, int startOffset = 0, EndianFormat endian = EndianFormat.Big) =>
            ToBuffer(value, buffer.ToArraySegment(startOffset), endian).Array;

        /// <summary>
        /// Writes the value into a newly allocated buffer (byte array) using the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="value">The value that is to be written to the created buffer in the specified <see cref="Endianness"/></param>
        /// <param name="endian">The data will be written to the buffer in this <see cref="Endianness"/>.</param>
        /// <returns>The passed in buffer, this allows for call chaining to other operations.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to write data into the buffer</exception>
        public static byte[] ToBufferNew(this UInt32 value, EndianFormat endian = EndianFormat.Big) =>
            ToBuffer(value, new byte[sizeof(UInt32)], startOffset: 0, endian: endian);
        #endregion

        #region Int32
        /// <summary>
        /// Reads bytes from the given buffer and formats them via the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="buffer">The byte buffer.  This supports a variety of data types such as byte arrays,
        /// lists, etc.</param>
        /// <param name="startOffset">The zero-based index of the starting byte to read, there must be at
        /// least 4 bytes in the buffer starting from this index.</param>
        /// <param name="endian">Specifies the format, <see cref="Endianness"/>, of the value in the given buffer.</param>
        /// <returns>The value read from the buffer using the specified endianness formatted as a C# value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to read data from buffer because it's too small, etc</exception>
        public static Int32 ToInt32(this IReadOnlyList<byte> buffer, int startOffset = 0, EndianFormat endian = EndianFormat.Big) =>
            (Int32)ToUInt64(buffer, sizeof(Int32), startOffset, signExtend: true, endian);

        /// <summary>
        /// Writes the value into the given buffer using the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="value">The value that is to be written to the given buffer in the specified <see cref="Endianness"/></param>
        /// <param name="buffer">The destination buffer identified by the given ArraySegment.  There must be at least 4 bytes
        /// of space available in the buffer.</param>
        /// <param name="endian">The data will be written to the buffer in this <see cref="Endianness"/>.</param>
        /// <returns>The passed in buffer, this allows for call chaining to other operations.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to write data into the buffer</exception>
        public static ArraySegment<byte> ToBuffer(this Int32 value, ArraySegment<byte> buffer, EndianFormat endian = EndianFormat.Big) =>
            ToBuffer((UInt64)value, sizeof(Int32), buffer, endian);

        /// <summary>
        /// Writes the value into the given buffer using the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="value">The value that is to be written to the given buffer in the specified <see cref="Endianness"/></param>
        /// <param name="buffer">The destination buffer.  There must be at least 4 bytes of space available in the buffer
        /// from the specified offset.</param>
        /// <param name="startOffset">The zero-based index of the starting byte to write</param>
        /// <param name="endian">The data will be written to the buffer in this <see cref="Endianness"/>.</param>
        /// <returns>The passed in buffer, this allows for call chaining to other operations.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to write data into the buffer</exception>
        public static byte[] ToBuffer(this Int32 value, byte[] buffer, int startOffset = 0, EndianFormat endian = EndianFormat.Big) =>
            ToBuffer(value, buffer.ToArraySegment(startOffset), endian).Array;

        /// <summary>
        /// Writes the value into a newly allocated buffer (byte array) using the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="value">The value that is to be written to the created buffer in the specified <see cref="Endianness"/></param>
        /// <param name="endian">The data will be written to the buffer in this <see cref="Endianness"/>.</param>
        /// <returns>The passed in buffer, this allows for call chaining to other operations.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to write data into the buffer</exception>
        public static byte[] ToBufferNew(this Int32 value, EndianFormat endian = EndianFormat.Big) =>
            ToBuffer(value, new byte[sizeof(Int32)], startOffset: 0, endian: endian);
        #endregion
    }
}