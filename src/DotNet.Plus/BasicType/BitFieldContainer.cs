using System;
using DotNet.Plus.Core;

namespace DotNet.Plus.BasicType
{
    /// <inheritdoc cref="BitField{TValue, TContainer}"/>
    public interface IBitField<TValue, TContainer>
        where TValue : struct, IConvertible
        where TContainer : struct, IConvertible
    {
        /// <inheritdoc cref="BitField{TValue, TContainer}.Bitmask"/>
        public TContainer Bitmask { get; }

        /// <inheritdoc cref="BitField{TValue, TContainer}.Decode(TContainer)"/>
        TValue Decode(TContainer container);

        /// <inheritdoc cref="BitField{TValue, TContainer}.Decode(TValue, TContainer)"/>
        TContainer Encode(TValue value, TContainer container);
    }

    /// <summary>
    /// A BitFieldContainer allows a sequence of bits to be identified within a container that is the same
    /// size or larger then the bit sequence.  A value can then be encoded into or out of the sequence of
    /// bits and the BitFieldContainer will take care of all the necessary masking and shifting. 
    /// <code>
    ///     // Define a value that is 4 bits and will be placed in a byte.
    ///     // The container is 16 bits and the value is in the 2nd nibble
    ///     var bitField = new BitField{byte, UInt16}(0b0000_0000_1111_0000);
    ///
    ///     bitField.Encode(0b1010, 0b0000_0000_0000_0000);   // Returns a value of 0b0000_0000_1010_0000
    ///     bitField.Decode(0b0000_0000_1001_0000);           // Returns a value of 0b0000_1010
    /// </code>
    /// </summary>
    /// <typeparam name="TValue">An integer type that will be used to hold the encoded/decoded value.  
    /// BitMask must fit within this type size.</typeparam>
    /// <typeparam name="TContainer">An integer type that will be used as the source/destination of the
    /// decoded/encoded value.</typeparam>
    public readonly struct BitField<TValue, TContainer>
        where TValue : struct, IConvertible
        where TContainer : struct, IConvertible
    {
        /// <summary>
        /// This is the Bitmask that's used to represent where the value will be located within the context of the
        /// TContainer.
        /// </summary>
        public TContainer Bitmask => ConvertUnchecked.ChangeType<TContainer>(_bitmask);
 
        private readonly UInt64 _bitmask;
        private readonly byte _valueBitsToRight;

        private static byte ContainerMaxBits => (byte)(sizeof(UInt64) * Byte.BitsInByte);

        private BitField((byte numBits, int startBitOffset) bitmask)
        {
            if( bitmask.startBitOffset < 0 )
                throw new ArgumentOutOfRangeException(nameof(bitmask.startBitOffset), bitmask.startBitOffset, $"Must be greater then or equal to 0");

            var numBits = bitmask.numBits;
            var startBitOffset = bitmask.startBitOffset;

            var maxValueBits = IntegerDefinition<TValue>.Size * Byte.BitsInByte;
            var containerUsedBits = IntegerDefinition<TContainer>.Size * Byte.BitsInByte;

            if( numBits > maxValueBits )
                throw new ArgumentOutOfRangeException(nameof(numBits), numBits, $"Too many bits specified for type {typeof(TValue)}");

            var bitsToRight = containerUsedBits - numBits - startBitOffset;
            if( bitsToRight < 0 )
                throw new ArgumentOutOfRangeException(nameof(numBits), numBits, $"Too many bits specified for type {typeof(TValue)}");

            _valueBitsToRight = (byte)bitsToRight;

            unchecked {
                _bitmask = 0xFFFF_FFFF_FFFF_FFFFul;
                _bitmask <<= numBits;
                _bitmask >>= numBits;
                _bitmask = ~_bitmask;
                _bitmask >>= startBitOffset;
                _bitmask >>= ContainerMaxBits - containerUsedBits;
            }
        }

        /// <summary>
        /// Creates a BitField with the given number of bits starting at the given startBitOffset.  For example:
        /// <code>
        ///     var bitField = new BitField{byte, UInt16}(3, 1);   // Would have a bitmask of 0b0111_0000_0000_0000
        /// </code>
        /// </summary>
        /// <param name="numBits">The number of bits used for the value</param>
        /// <param name="startBitOffset">A zero based index from the left most bit representing the start of the value.
        /// <code>
        ///                                              Bit 
        ///     Bit Order In Byte: 76543210 76543210    Offset
        ///                        |||||||| ||||||||    ======
        ///                        |||||||| |||||||\__    15  
        ///                        |||||||| ||||||\___    14
        ///                        |||||||| |||||\____    13
        ///                        |||||||| ||||\_____    12
        ///                        |||||||| |||\______    11
        ///                        |||||||| ||\_______    10
        ///                        |||||||| |\________    09
        ///                        |||||||| \_________    08
        ///                        |||||||\___________    07 
        ///                        ||||||\____________    06
        ///                        |||||\_____________    05
        ///                        ||||\______________    04
        ///                        |||\_______________    03
        ///                        ||\________________    02
        ///                        |\_________________    01
        ///                        \__________________    00
        /// </code>
        /// </param>
        public BitField(byte numBits, int startBitOffset) : this((numBits, startBitOffset))
        {
        }

        /// <summary>
        /// Creates BitField with the given bitmask.  
        /// </summary>
        /// <param name="bitmask">The bitmask must contain 0 or more sequential bits starting anywhere within
        /// the container.  There can only be ONE set of bits.  <c>For example 0b1100_0011 would be invalid.</c></param>
        public BitField(TContainer bitmask) : this(ParseBitmask(bitmask))
        {
        }

        internal static (byte numBits, int startBitOffset) ParseBitmask(TContainer bitmask)
        {
            var maxContainerBits = IntegerDefinition<TContainer>.Size * Byte.BitsInByte;
            var maxBits = IntegerDefinition<TValue>.Size * Byte.BitsInByte;
            byte bitsToRight = 0;
            byte numBits = 0;
            bool completedBitmask = false;
            UInt64 container = ConvertUnchecked.ChangeType<UInt64>(bitmask);
            for( int index = 0; index < maxContainerBits; index += 1, container >>= 1 )
            {
                if( (container & 0x01ul) != 0 )
                {
                    if( completedBitmask )
                        throw new ArgumentOutOfRangeException(nameof(bitmask), bitmask, $"Only a single consecutive set of bits is allowed to be set in the bitmask.");
                    numBits += 1;
                }
                else
                {
                    if( numBits > 0 ) {
                        completedBitmask = true;  // But we keep checking in order to validate the given bitmask is in the correct form
                        continue;
                    }

                    bitsToRight += 1;
                }

                if( numBits > maxBits )
                    throw new ArgumentOutOfRangeException(nameof(bitmask), bitmask, $"Too many bits set in the bitmask, the value can contain up to {maxBits}");
            }

            var startBitOffset = maxContainerBits - bitsToRight - numBits;
            return (numBits, startBitOffset);
        }

        /// <summary>
        /// Decodes a value that fits within TValue from the given container.
        /// <code>
        ///     // Define a value that is 4 bits and will be placed in a byte.
        ///     // The container is 16 bits and the value is in the 2nd nibble
        ///     var bitField = new BitField{byte, UInt16}(0b0000_0000_1111_0000);
        ///
        ///     bitField.Decode(0b0000_0000_1001_0000);  // Returns a value of 0b0000_1010
        /// </code>
        /// </summary>
        /// <param name="container">The value is read from the container according to the BitField's bitmask as shifted appropriately</param>
        /// <returns></returns>
        public TValue Decode(TContainer container)
        {
            var containerValue = ConvertUnchecked.ChangeType<UInt64>(container);
            var decodeValue = (containerValue & _bitmask) >> _valueBitsToRight;
            return ConvertUnchecked.ChangeType<TValue>(decodeValue);
        }

        /// <summary>
        /// Decodes a value that fits within TValue from the given container.
        /// <code>
        ///     // Define a value that is 4 bits and will be placed in a byte.
        ///     // The container is 16 bits and the value is in the 2nd nibble
        ///     var bitField = new BitField{byte, UInt16}(0b0000_0000_1111_0000);
        /// 
        ///     bitField.Encode(0b1010, 0b0000_0000_0000_0000);   // Returns a value of 0b0000_0000_1010_0000
        /// </code>
        /// </summary>
        /// <param name="value">The value to be sifted and placed into the container according to BitField's bitmask</param>
        /// <param name="container">The value is applied to the container according to the BitField's bitmask as shifted appropriately</param>
        /// <returns>The original container with the value encoded into the appropriate place defined by the BitField's bitmask</returns>
        public TContainer Encode(TValue value, TContainer container)
        {
            var valueContainer = ConvertUnchecked.ChangeType<UInt64>(value);
            var sourceContainer = ConvertUnchecked.ChangeType<UInt64>(container);
            valueContainer <<= _valueBitsToRight;
            sourceContainer &= ~_bitmask;
            sourceContainer |= (valueContainer & _bitmask);
            return ConvertUnchecked.ChangeType<TContainer>(sourceContainer);
        }
    }
}
