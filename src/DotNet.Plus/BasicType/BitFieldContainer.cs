using System;
using DotNet.Plus.Core;

namespace DotNet.Plus.BasicType
{
    /// <inheritdoc cref="BitField{TValue, TContainer}"/>
    public interface IBitField<TValue, TContainer>
        where TValue : struct, IConvertible
        where TContainer : struct, IConvertible
    {
        public TContainer Bitmask { get; }
        TValue Decode(TContainer container);
        TContainer Encode(TValue value, TContainer container);
    }
    
    /// <summary>
    /// Terminology: 
    /// <code>
    ///     Byte Index:         Byte 0   Byte 1      Bit
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
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TContainer"></typeparam>
    public readonly struct BitField<TValue, TContainer>
        where TValue : struct, IConvertible
        where TContainer : struct, IConvertible
    {
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
        
        public BitField(byte numBits, int startBitOffset) : this((numBits, startBitOffset))
        {
        }
        
        public BitField(TContainer bitmask) : this(ParseBitmask(bitmask))
        {
        }

        public static (byte numBits, int startBitOffset) ParseBitmask(TContainer bitmask)
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

        //public static BitField<TValue, TContainer> MakeFromBitmask(TContainer bitmask) => new BitField<TValue, TContainer>(bitmask);

        public TValue Decode(TContainer container)
        {
            var containerValue = ConvertUnchecked.ChangeType<UInt64>(container);
            var decodeValue = (containerValue & _bitmask) >> _valueBitsToRight;
            return ConvertUnchecked.ChangeType<TValue>(decodeValue);
        }

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
