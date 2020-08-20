using System;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Enum extensions for working with [FLAG] enums such as being able to set and clear flags
    /// </summary>
    public static class EnumFlags
    {
        // Will clear or set the given flags based on setFlags. If setFlags are true then all the given flags will be set,
        // otherwise they will be cleared.
        //
        public static TEnum UpdateFlag<TEnum>(this TEnum enumFlags, TEnum flags, bool setFlags) 
            where TEnum : Enum =>
            setFlags ? SetFlag(enumFlags, flags) : ClearFlag(enumFlags, flags);

        /// <summary>
        /// Returns the given enumFlags with the specified setFlags added.  The enum must be
        /// attributed as a Flags enum
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum</typeparam>
        /// <param name="enumFlags">The enum who's flags are to be set</param>
        /// <param name="setFlags">The flags to set from the given enumFlags</param>
        /// <returns>The given enumFlags with the setFlags set</returns>
        /// <exception cref="ArgumentException">Unable to covert the given enum to a support Integer type</exception>
        public static TEnum SetFlag<TEnum>(this TEnum enumFlags, TEnum setFlags)
            where TEnum : Enum
        {
            if( !Enum<TEnum>.IsFlagsEnum )
                throw new NotFlagsEnumException($"The type {typeof(TEnum).Name} isn't a flags enum.");

            var enumType = Enum.GetUnderlyingType(typeof(TEnum));
            var typeCode = Type.GetTypeCode(enumType);
            var flagsObj = Convert.ChangeType(enumFlags, typeCode);
            var maskObj = Convert.ChangeType(setFlags, typeCode);

            if( flagsObj.Equals(maskObj) || 0.Equals(maskObj) )
                return enumFlags;

            switch( InjectTypeCode ?? typeCode )
            {
                case TypeCode.SByte:
                    return (TEnum) (object) (sbyte) ((sbyte) flagsObj | (sbyte) maskObj);
                case TypeCode.Int16:
                    return (TEnum) (object) (Int16) ((Int16) flagsObj | (Int16) maskObj);
                case TypeCode.Int32:
                    return (TEnum) (object) (Int32) ((Int32) flagsObj | (Int32) maskObj);
                case TypeCode.Int64:
                    return (TEnum) (object) (Int64) ((Int64) flagsObj | (Int64) maskObj);
                case TypeCode.Byte:
                    return (TEnum) (object) (byte) ((byte) flagsObj | (byte) maskObj);
                case TypeCode.UInt16:
                    return (TEnum) (object) (UInt16) ((UInt16) flagsObj | (UInt16) maskObj);
                case TypeCode.UInt32:
                    return (TEnum) (object) (UInt32) ((UInt32) flagsObj | (UInt32) maskObj);
                case TypeCode.UInt64:
                    return (TEnum) (object) (UInt64) ((UInt64) flagsObj | (UInt64) maskObj);
                default:
                    throw new ArgumentException($"Enum underlying type of {typeCode} not supported/expected");
            }
        }

        /// <summary>
        /// Returns the given enumFlags with the specified clearFlags removed.  The enum must be
        /// attributed as a Flags enum
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum</typeparam>
        /// <param name="enumFlags">The enum who's flags are to be set</param>
        /// <param name="clearFlags">The flags to clear from the given enumFlags</param>
        /// <returns>The given enumFlags with the clearFlags removed</returns>
        /// <exception cref="ArgumentException">Unable to covert the given enum to a support Integer type</exception>
        public static TEnum ClearFlag<TEnum>(this TEnum enumFlags, TEnum clearFlags)
            where TEnum : Enum
        {
            if( !Enum<TEnum>.IsFlagsEnum )
                throw new NotFlagsEnumException($"The type {typeof(TEnum).Name} isn't a flags enum.");

            var enumType = Enum.GetUnderlyingType(typeof(TEnum));
            var typeCode = Type.GetTypeCode(enumType);
            var flagsObj = Convert.ChangeType(enumFlags, typeCode);
            var maskObj = Convert.ChangeType(clearFlags, typeCode);

            if( 0.Equals(maskObj) )
                return enumFlags;

            switch( InjectTypeCode ?? typeCode )
            {
                case TypeCode.SByte: return (TEnum)(object)(sbyte)((sbyte)flagsObj & ~(sbyte)maskObj);
                case TypeCode.Int16: return (TEnum)(object)(Int16)((Int16)flagsObj & ~(Int16)maskObj);
                case TypeCode.Int32: return (TEnum)(object)(Int32)((Int32)flagsObj & ~(Int32)maskObj);
                case TypeCode.Int64: return (TEnum)(object)(Int64)((Int64)flagsObj & ~(Int64)maskObj);
                case TypeCode.Byte: return (TEnum)(object)(byte)((byte)flagsObj & ~(byte)maskObj);
                case TypeCode.UInt16: return (TEnum)(object)(UInt16)((UInt16)flagsObj & ~(UInt16)maskObj);
                case TypeCode.UInt32: return (TEnum)(object)(UInt32)((UInt32)flagsObj & ~(UInt32)maskObj);
                case TypeCode.UInt64: return (TEnum)(object)(UInt64)((UInt64)flagsObj & ~(UInt64)maskObj);
                default:
                    throw new ArgumentException($"Enum underlying type of {typeCode} not supported/expected");
            }
        }

        /// <summary>
        /// This is used for unit testing to allow us to Inject a just TypeCode in order to trigger certain error
        /// conditions.
        /// </summary>
        private static TypeCode? InjectTypeCode { get; set; } = null;
    }
}
