using System;
using DotNet.Plus.Core;
using static DotNet.Plus.BasicType.Byte;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// A fixed-point number is an integer that represents a rational, float or double, value.  To convert to/from the
    /// fixed-point number a scale factor is applied.
    ///
    ///       fixedPoint =  realNumber * scale
    ///       realNumber =  fixedPoint / scale
    /// 
    ///     w.f bits     whole      Fraction     Scale factor    Number Of Bytes
    ///     --------   ---------   ----------   ---------------  ---------------
    ///     4.8         4 bits      8 bits      2^8   =  256        2   
    ///     8.8         8           8           2^8   =  256        2
    ///     16.16       16         16           2^16  =  65536      4
    ///
    /// This implementation supports up to 63 bit fixed-point values.  However, loss of precision can happen when
    /// converting to/from floats and doubles.  This implementation will throw overflow exceptions when it is unable
    /// to perform the conversion within known limits.  However, the float/double will lose precision if the fixed-point
    /// number is too big/small.
    ///
    /// This implementation supports fixed point operations of all signed and unsigned integer types, and it supports
    /// float and double rational numbers.
    /// </summary>
    public static class FixedPoint
    {
        /// <summary>
        /// The implementation of fixed point does conversions using 64 bit values thus we end up
        /// capping the number of bits supported.
        /// </summary>
        private const int MaxWholeBits = 63;  // Because of implementation limits

        /// <summary>
        /// Makes a fixed-point value of type TFixedPointValue from the given float value.
        /// </summary>
        /// <typeparam name="TFixedPointValue">The fixed-point type which must be signed or unsigned
        /// integer type such as byte, sbyte, int, ...</typeparam>
        /// <param name="realValue">The value that is to be converted to a fixed-point.  Only the bits
        /// specified in the whole/fractional parts of the real value are used in the converted.</param>
        /// <param name="wholeBits">The number of bits before the decimal</param>
        /// <param name="fractionalBits">The number of bits after the decimal</param>
        /// <returns>A fixed-point value of type TFixedPointValue</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the wholeBits and/or fractionBits won't fit within the specified fixed-point</exception>
        /// <exception cref="OverflowException">If the rational number would cause an overflow for the fixed-point number</exception>
        /// <exception cref="TypeInitializationException">If the TFixedPoint value isn't an integer type</exception>
        public static TFixedPointValue MakeFixedPoint<TFixedPointValue>(this float realValue, byte wholeBits, byte fractionalBits)
            where TFixedPointValue : struct, IConvertible => MakeFixedPoint<TFixedPointValue>((double) realValue, wholeBits, fractionalBits);

        /// <summary>
        /// Makes a fixed-point value of type TFixedPointValue from the given double value.
        /// </summary>
        /// <typeparam name="TFixedPointValue">The fixed-point type which must be signed or unsigned
        /// integer type such as byte, sbyte, int, ...</typeparam>
        /// <param name="realValue">The value that is to be converted to a fixed-point.  Only the bits
        /// specified in the whole/fractional parts of the real value are used in the converted.</param>
        /// <param name="wholeBits">The number of bits before the decimal</param>
        /// <param name="fractionalBits">The number of bits after the decimal</param>
        /// <returns>A fixed-point value of type TFixedPointValue</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the wholeBits and/or fractionBits won't fit within the specified fixed-point</exception>
        /// <exception cref="OverflowException">If the rational number would cause an overflow for the fixed-point number</exception>
        /// <exception cref="TypeInitializationException">If the TFixedPoint value isn't an integer type</exception>
        public static TFixedPointValue MakeFixedPoint<TFixedPointValue>(this double realValue, byte wholeBits, byte fractionalBits)
            where TFixedPointValue : struct, IConvertible
        {
            ThrowIfFixedPointSizeInvalid<TFixedPointValue>(wholeBits, fractionalBits);

            var allowNegative = IntegerDefinition<TFixedPointValue>.IsSigned;
            
            Integer<TFixedPointValue> fixedPoint;
            if( realValue < 0 )
            {
                if( !allowNegative )
                    throw new OverflowException($"Given value {realValue} is negative but is trying to be placed in non signed type {typeof(TFixedPointValue)}");

                if( (Int64)realValue < (double)SmallestWholeValue(wholeBits, allowNegative: allowNegative) )
                    throw new OverflowException($"Given value {realValue} can't fit in {wholeBits}.{fractionalBits}");

                fixedPoint = Convert.ToInt64(realValue * ScaleFactor(fractionalBits)) & (Int64)FixedPointMask(wholeBits, fractionalBits);
            }
            else
            {
                if( (UInt64)realValue > LargestWholeValue(wholeBits, allowNegative: allowNegative) )
                    throw new OverflowException($"Given value {realValue} can't fit in {wholeBits}.{fractionalBits}");

                fixedPoint = Convert.ToUInt64(realValue * ScaleFactor(fractionalBits)) & FixedPointMask(wholeBits, fractionalBits);
            }

            return fixedPoint.Value;
        }

        /// <summary>
        /// Makes a float value from the given fixed-point value.
        /// </summary>
        /// <typeparam name="TFixedPointValue">The fixed-point type which must be signed or unsigned integer type such as byte, sbyte, int, ...</typeparam>
        /// <param name="fixedPointValue">The fixed point value that is to be converted</param>
        /// <param name="wholeBits">The number of bits before the decimal</param>
        /// <param name="fractionalBits">The number of bits after the decimal</param>
        /// <returns>The given fixedPointValue converted to a float</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the wholeBits and/or fractionBits won't fit within the specified fixed-point</exception>
        /// <exception cref="TypeInitializationException">If the TFixedPoint value isn't an integer type</exception>
        public static float MakeFloat<TFixedPointValue>(this TFixedPointValue fixedPointValue, byte wholeBits, byte fractionalBits)
            where TFixedPointValue : struct, IConvertible =>
            (float) MakeDouble(fixedPointValue, wholeBits, fractionalBits);

        /// <summary>
        /// Makes a double value from the given fixed-point value.
        /// </summary>
        /// <typeparam name="TFixedPointValue">The fixed-point type which must be signed or unsigned integer type such as byte, sbyte, int, ...</typeparam>
        /// <param name="fixedPointValue">The fixed point value that is to be converted</param>
        /// <param name="wholeBits">The number of bits before the decimal</param>
        /// <param name="fractionalBits">The number of bits after the decimal</param>
        /// <returns>The given fixedPointValue converted to a double</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the wholeBits and/or fractionBits won't fit within the specified fixed-point</exception>
        /// <exception cref="TypeInitializationException">If the TFixedPoint value isn't an integer type</exception>
        public static double MakeDouble<TFixedPointValue>(this TFixedPointValue fixedPointValue, byte wholeBits, byte fractionalBits)
            where TFixedPointValue : struct, IConvertible
        {
            ThrowIfFixedPointSizeInvalid<TFixedPointValue>(wholeBits, fractionalBits);
            
            Integer<TFixedPointValue> fixedPoint = ConvertUnchecked.ChangeType<UInt64>(fixedPointValue) & FixedPointMask(wholeBits, fractionalBits);
            if( wholeBits > 0 && IntegerDefinition<TFixedPointValue>.IsSigned )
            {
                var totalBits = wholeBits + fractionalBits;
                var valueMask = (0x1ul << totalBits) - 1;
                var negativeBitMask = 0x1ul << (totalBits - 1);  
                if( (fixedPoint & negativeBitMask) != 0 )
                {
                    var signExtendMask = UInt64.MaxValue & ~valueMask;
                    Int64 signedFixedPoint = (Int64)fixedPoint | (Int64)signExtendMask;
                    return (double)signedFixedPoint / ScaleFactor(fractionalBits);
                }
            }

            var realValue = (double)fixedPoint / ScaleFactor(fractionalBits);
            return realValue;
        }

        /// <summary>
        /// The scale factor applied is based on the number of fraction bits.
        /// </summary>
        /// <param name="fractionalBits">Number of fraction bits</param>
        /// <returns>The scale factor used to convert to/from the fixed-point value</returns>
        private static UInt64 ScaleFactor(byte fractionalBits) => 0x1uL << fractionalBits;

        /// <summary>
        /// The fixed-point mask that represents the bits used by the fixed-point value.
        /// </summary>
        /// <param name="wholeBits">The number of bits before the decimal</param>
        /// <param name="fractionalBits">The number of bits after the decimal</param>
        /// <returns></returns>
        public static UInt64 FixedPointMask(byte wholeBits, byte fractionalBits) =>
            (0x1ul << (wholeBits + fractionalBits)) - 1;

        /// <summary>
        /// Validates the wholeBits and fractionalBits against the given TFixedPointValue.
        /// </summary>
        /// <typeparam name="TFixedPointValue">The fixed-point type which must be signed or unsigned integer type such as byte, sbyte, int, ...</typeparam>
        /// <param name="wholeBits">The number of bits before the decimal</param>
        /// <param name="fractionalBits">The number of bits after the decimal</param>
        /// <exception cref="TypeInitializationException">If the TFixedPoint value isn't an integer type</exception>
        private static void ThrowIfFixedPointSizeInvalid<TFixedPointValue>(byte wholeBits, byte fractionalBits)
            where TFixedPointValue : struct, IConvertible
        {
            if( wholeBits > MaxWholeBits )
                throw new ArgumentOutOfRangeException(nameof(wholeBits), wholeBits, $"number of whole bits too big, must be less then {MaxWholeBits}");

            var numBites = wholeBits + fractionalBits;
            if( numBites == 0 )
                throw new ArgumentOutOfRangeException(nameof(fractionalBits), fractionalBits, $"number of TOTAL bits, whole + fractional, can't be 0");

            var maxFixedPointBytes = IntegerDefinition<TFixedPointValue>.Size;
            if( numBites > maxFixedPointBytes * BitsInByte )
                throw new ArgumentOutOfRangeException(nameof(fractionalBits), fractionalBits, $"number of TOTAL bits, whole + fractional, doesn't fit in {typeof(TFixedPointValue)}");
        }

        /// <summary>
        /// Gets the largest whole number that can be represented by the given number of bits.
        /// </summary>
        /// <param name="wholeBits">The number of whole bits to use</param>
        /// <param name="allowNegative">True if space should be allocated to allow negative values</param>
        /// <returns>the largest whole number that can be represented by the given number of bits</returns>
        public static UInt64 LargestWholeValue(byte wholeBits, bool allowNegative)
        {
            if( wholeBits == 0 )
                return 0ul;

            var maxBits = sizeof(UInt64) * BitsInByte;
            if( wholeBits > maxBits )
                throw new ArgumentOutOfRangeException(nameof(wholeBits), wholeBits, $"value must not be greater then {maxBits}");

            if( allowNegative ) 
                return (0x01ul << (wholeBits - 1)) - 1;

            if( wholeBits == sizeof(UInt64) )
                return UInt64.MaxValue;

            return (0x01ul << wholeBits) - 1;
        }

        /// <summary>
        /// Gets the smallest whole number that can be represented by the given number of bits.
        /// </summary>
        /// <param name="wholeBits">The number of whole bits to use</param>
        /// <param name="allowNegative">True if space should be allocated to allow negative values</param>
        /// <returns>the smallest whole number that can be represented by the given number of bits</returns>
        public static Int64 SmallestWholeValue(byte wholeBits, bool allowNegative)
        {
            if( wholeBits == 0 || !allowNegative )
                return 0L;

            var maxBits = sizeof(UInt64) * BitsInByte;
            if( wholeBits > maxBits )
                throw new ArgumentOutOfRangeException(nameof(wholeBits), wholeBits, $"value must not be greater then {maxBits}");

            return (Int64)((0x01ul << wholeBits - 1)) * -1;
        }
    }

}
