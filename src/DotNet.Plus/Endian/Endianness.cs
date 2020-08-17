using System;
using System.Collections.Generic;
using static DotNet.Plus.BasicType.Byte;

namespace DotNet.Plus.Endian
{
    /// <summary>
    /// Endianness is the ordering or sequencing of bytes that represent numeric values.  It is expressed as big endian or little endian.
    /// A big-endian system stores the most significant byte at the smallest memory address and the least significant byte at the largest.
    /// A little-endian system, in contrast, stores the least-significant byte at the smallest address.
    /// </summary>
    public enum EndianFormat
    {
        /// <summary>
        /// A big-endian system stores the most significant byte at the smallest memory address and the least significant byte at the largest.
        /// </summary>
        /// <example>
        /// Take the number 0x12345678
        ///
        ///   Pointer/  Big   
        ///   Address   Endian
        ///   ================
        ///     a:      0x12  
        ///     a+1:    0x34  
        ///     a+2:    0x56  
        ///     a+3:    0x78  
        /// </example>
        Big,

        /// <summary>
        /// A little-endian system stores the least-significant byte at the smallest address and the most significant byte at the largest.
        /// </summary>
        /// <example>
        /// Take the number 0x12345678
        ///
        ///   Pointer/  Little
        ///   Address   Endian
        ///   =================
        ///     a:       0x78
        ///     a+1:     0x56
        ///     a+2:     0x34
        ///     a+3:     0x12
        /// </example>
        Little
    }
    
    /// <summary>
    /// Endianness is the ordering or sequencing of bytes that represent numeric values.  It is expressed as big endian or little endian.
    /// A big-endian system stores the most significant byte at the smallest memory address and the least significant byte at the largest.
    /// A little-endian system, in contrast, stores the least-significant byte at the smallest address.
    /// </summary>
    /// <example>
    /// Take the number 0x12345678
    ///
    ///   Pointer/  Big     Little
    ///   Address   Endian  Endian
    ///   =========================
    ///     a:      0x12     0x78
    ///     a+1:    0x34     0x56
    ///     a+2:    0x56     0x34
    ///     a+3:    0x78     0x12
    /// </example>
    public static partial class Endianness
    {
        /// <summary>
        /// This is used to sign extend a UInt64 bit value when the source value is less then 64 bits
        /// thus we only need 8 entries in the table in order to define all possibilities.  Note: we
        /// do a simple table lookup to get the mask as it's super fast and simple.
        /// </summary>
        private static readonly UInt64[] _signExtendMask = new UInt64[]
        {
            0xFFFF_FFFF_FFFF_FF00,
            0xFFFF_FFFF_FFFF_0000,
            0xFFFF_FFFF_FF00_0000,
            0xFFFF_FFFF_0000_0000,
            0xFFFF_FF00_0000_0000,
            0xFFFF_0000_0000_0000,
            0xFF00_0000_0000_0000,
            0x0000_0000_0000_0000
        };

        /// <summary>
        /// Reads bytes from the given buffer and formats them via the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="buffer">The byte buffer.  This supports a variety of data types such as byte arrays, lists, etc.</param>
        /// <param name="numBytes">The number of bytes, up to a maximum of 8, to read into the UInt64.</param>
        /// <param name="startOffset">The zero-based index of the starting byte to read, there must be at least numBytes in the
        /// buffer starting from this index.</param>
        /// <param name="signExtend">If true the sign bit will be extended to fill the UInt64 based on the high bit of the value
        /// read (using numBytes).  For example, if numBytes is 3, meaning read a 24 bit value, the high bit of that 24 bit value
        /// will be sign extended to the rest of the UInt64.  This is used to preserve the "sign" of the value even though we are
        /// reading into an unsigned value.</param>
        /// <param name="endian">Specifies the format, <see cref="Endianness"/>, of the value in the given buffer.</param>
        /// <returns>The value read from the buffer using the specified <see cref="Endianness"/> formatted as a UInt64 value.
        /// Up to 8 bytes can be read from the buffer, but less then 8 bytes may be read.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to read data from buffer because it's too small, or too many numBytes specified</exception>
        public static UInt64 ToUInt64(this IReadOnlyList<byte> buffer, int numBytes, int startOffset, bool signExtend, EndianFormat endian = EndianFormat.Big)
        {
            if( numBytes == 0 )
                return 0;

            if( buffer.Count <= 0 )
                throw new ArgumentOutOfRangeException(nameof(buffer.Count), buffer.Count, $"Buffer size of {buffer.Count} is too small");

            if( numBytes < 0 )
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, $"must be greater then zero");

            if( signExtend && numBytes > _signExtendMask.Length )
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, $"unable to sign extend, value must be less then or equal to {BytesIn64Bits}");

            if( numBytes > BytesIn64Bits )
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, $"must be less then or equal to {BytesIn64Bits}");
            
            if( numBytes > buffer.Count )
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, $"must be less then or equal to the given buffer length of {buffer.Count}");

            //          Big Endian                              Little Endian
            //  index   bufferIndex  shift     Shift Bitmask     bufferIndex
            //    0     buffer[7]      0     0x00000000000000FF   buffer[0]
            //    1     buffer[6]      8     0x000000000000FF00   buffer[1]
            //    2     buffer[5]     16     0x0000000000FF0000   buffer[2]
            //    3     buffer[4]     24     0x00000000FF000000   buffer[3]
            //    4     buffer[3]     32     0x000000FF00000000   buffer[4]
            //    5     buffer[2]     40     0x0000FF0000000000   buffer[5]
            //    6     buffer[1]     48     0x00FF000000000000   buffer[6]
            //    7     buffer[0]     56     0xFF00000000000000   buffer[7]
            UInt64 value = 0;
            var shift = 0;
            var bufferIndex = startOffset + (endian == EndianFormat.Big ? numBytes - 1 : 0);
            var bufferIndexInc = endian == EndianFormat.Big ? -1 : 1;
            for( var index = 0; index < numBytes; index += 1, shift += BitsInByte, bufferIndex += bufferIndexInc )
            {
                var maskedData = ((UInt64)buffer[bufferIndex]) << shift;
                value |= maskedData;
            }

            // Sign extend high bit to properly map negative values
            //
            if( signExtend )
            {
                int signedByteMsbIndex = numBytes - 1;
                UInt64 signMask = (UInt64)0x80 << (signedByteMsbIndex * BitsInByte);
                var signedValue = (value & signMask) != 0x00;
                if( signedValue )
                    value |= _signExtendMask[signedByteMsbIndex];
            }

            return value;
        }

        /// <summary>
        /// Reads bytes from the given buffer and formats them via the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="buffer">The byte buffer.  This supports a variety of data types such as byte arrays, lists, etc.</param>
        /// <param name="numBytes">The number of bytes, up to a maximum of 8, to read into the UInt64.</param>
        /// <param name="startOffset">The zero-based index of the starting byte to read, there must be at least numBytes in the
        /// buffer starting from this index.</param>
        /// <param name="endian">Specifies the format, <see cref="Endianness"/>, of the value in the given buffer.</param>
        /// <returns>The value read from the buffer using the specified <see cref="Endianness"/> formatted as a UInt64 value.
        /// Up to 8 bytes can be read from the buffer, but less then 8 bytes may be read.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to read data from buffer because it's too small, or too many numBytes specified</exception>
        public static UInt64 ToInt64(this IReadOnlyList<byte> buffer, int numBytes, int startOffset, EndianFormat endian = EndianFormat.Big) =>
            ToUInt64(buffer, numBytes, startOffset, signExtend: true, endian: endian);

        /// <summary>
        /// Writes the value into the given buffer using the specified <see cref="Endianness"/>.
        /// </summary>
        /// <param name="value">The value that is to be written to the given buffer in the specified <see cref="Endianness"/></param>
        /// <param name="numBytes">The number of bytes, up to a maximum of 8, to write to the buffer.</param>
        /// <param name="buffer">The destination buffer identified by the given ArraySegment.  There must be at least numBytes bytes
        /// of space available in the buffer.</param>
        /// <param name="endian">The data will be written to the buffer in this <see cref="Endianness"/>.</param>
        /// <returns>The passed in buffer, this allows for call chaining to other operations.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If unable to write data into the buffer</exception>
        public static ArraySegment<byte> ToBuffer(this UInt64 value, int numBytes, ArraySegment<byte> buffer, EndianFormat endian = EndianFormat.Big)
        {
            if( numBytes == 0 )
                return buffer;

            if( buffer.Count <= 0 )
                throw new ArgumentOutOfRangeException(nameof(buffer.Count), buffer.Count, $"Buffer size of {buffer.Count} is too small");

            if( numBytes < 0 )
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, $"must be greater then zero");

            if( numBytes > BytesIn64Bits )
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, $"must be less then or equal to {BytesIn64Bits}");

            if( numBytes > buffer.Count )
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, $"must be less then or equal to the given buffer length of {buffer.Count}");

            //          Big Endian                              Little Endian
            //  index   bufferIndex  shift     Shift Bitmask     bufferIndex
            //    0     buffer[7]      0     0x00000000000000FF   buffer[0]
            //    1     buffer[6]      8     0x000000000000FF00   buffer[1]
            //    2     buffer[5]     16     0x0000000000FF0000   buffer[2]
            //    3     buffer[4]     24     0x00000000FF000000   buffer[3]
            //    4     buffer[3]     32     0x000000FF00000000   buffer[4]
            //    5     buffer[2]     40     0x0000FF0000000000   buffer[5]
            //    6     buffer[1]     48     0x00FF000000000000   buffer[6]
            //    7     buffer[0]     56     0xFF00000000000000   buffer[7]
            var bitMask = (UInt64)0xFF;   // This needs to be cast or it will default to a smaller size
            var shift = 0;
            var bufferIndex = buffer.Offset + (endian == EndianFormat.Big ? numBytes - 1 : 0);
            var bufferIndexInc = endian == EndianFormat.Big ? -1 : 1;
            for( var index = 0; index < numBytes; index += 1, bitMask <<= BitsInByte, shift += BitsInByte, bufferIndex += bufferIndexInc )
            {
                UInt64 maskedData = value & bitMask;
                buffer.Array[bufferIndex] = (byte)(maskedData >> shift);
            }

            return buffer;
        }
    }
}
