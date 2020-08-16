using System;
using DotNet.Plus.Core;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TBackingType"></typeparam>
    public readonly struct Integer<TBackingType>
        where TBackingType : struct, IConvertible
    {
        public TBackingType Value { get; }

        public Integer(TBackingType value)
        {
            _ = IntegerDefinition<TBackingType>.IsInteger;
            Value = value;
        }

        //public static implicit operator TBackingType(Integer<TBackingType> integer) => integer.Value;
        public static implicit operator sbyte(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<sbyte>(integer.Value);
        public static implicit operator Int16(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Int16>(integer.Value);
        public static implicit operator Int32(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Int32>(integer.Value);
        public static implicit operator Int64(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Int64>(integer.Value);
        public static implicit operator byte(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<byte>(integer.Value);
        public static implicit operator UInt16(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<UInt16>(integer.Value);
        public static implicit operator UInt32(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<UInt32>(integer.Value);
        public static implicit operator UInt64(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<UInt64>(integer.Value);

        public static implicit operator float(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<float>(integer.Value);
        public static implicit operator double(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<double>(integer.Value);
        public static implicit operator Decimal(Integer<TBackingType> integer) => ConvertUnchecked.ChangeType<Decimal>(integer.Value);

        public static implicit operator Integer<TBackingType>(sbyte value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Int16 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Int32 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Int64 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(byte value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(UInt16 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(UInt32 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(UInt64 value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
        public static implicit operator Integer<TBackingType>(Enum value) => new Integer<TBackingType>(ConvertUnchecked.ChangeType<TBackingType>(value));
    }
}
