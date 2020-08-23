using System;
using DotNet.Plus.Core;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// The Integer types provides for seamless unchecked conversation across the Integer type and system
    /// integers such as Int32, UInt32, etc.  This is useful for generic types that only work with integer
    /// values.
    /// </summary>
    /// <typeparam name="TBackingType">The backing type of the Integer such as Int32 or UInt32, must be an
    /// integer type <seealso cref="IntegerDefinition{TIntegerValue}"/></typeparam>
    public readonly struct Integer<TBackingType>
        where TBackingType : struct, IConvertible
    {
        /// <summary>
        /// The TBackingType must be a valid integer type such as Int32, UInt32, ...
        /// </summary>
        static Integer()
        {
            _ = IntegerDefinition<TBackingType>.IsInteger;
        }

        /// <summary>
        /// The immutable value of the assigned integer
        /// </summary>
        public TBackingType Value { get; }

        /// <summary>
        /// Creates a new Integer with the given value.
        /// </summary>
        /// <param name="value">The value of the Integer</param>
        public Integer(TBackingType value)
        {
            Value = value;
        }

        public static Integer<TBackingType> MakeInteger(TBackingType value) => new Integer<TBackingType>(value);

        #region From Integer to Native Integer type
        /// <summary>
        /// Implicit unchecked conversion from Integer{TBackingType} to specified system integer type.
        /// </summary>
        /// <param name="integer">The Integer to convert</param>
        public static implicit operator sbyte(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<sbyte>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator Int16(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Int16>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator Int32(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Int32>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator Int64(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Int64>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator byte(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<byte>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator UInt16(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<UInt16>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator UInt32(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<UInt32>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator UInt64(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<UInt64>(integer.Value);
        #endregion

        #region From Integer to Non-Integer type
        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator float(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<float>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator double(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<double>(integer.Value);

        /// <inheritdoc cref="implicit operator sbyte(Integer{TBackingType})"/>
        public static implicit operator Decimal(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Decimal>(integer.Value);
        #endregion

        #region From Native type to Integer
        public static implicit operator Integer<TBackingType>(sbyte value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Int16 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Int32 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Int64 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(byte value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(UInt16 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(UInt32 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(UInt64 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Enum value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        #endregion
    }

    
}
