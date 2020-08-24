using System;
using DotNet.Plus.BasicType;
using Byte = System.Byte;

namespace DotNet.Plus.Core
{
    /// <summary>
    /// <para>This is a version of ChangeType that supports unchecked conversion between basic C# types.
    /// An issue with the System.Convert implementation is that it doesn't allow unchecked conversions which
    /// is very useful when doing low level type conversions when working with real hardware.</para>
    ///
    /// <para>Decimal types are special in that they will still throw in cases where their values have to get
    /// truncated to fit.</para>
    ///
    /// <para>NOTE: The implementation of this could be a great use of Source Generators, when they become
    /// available, as there is a lot of duplicate code with subtle changes.  An IDEAL candidate for a Source
    /// Generator. https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/</para>
    /// </summary>
    /// <example>
    /// <code>
    /// Convert.ChangeType(-1, TypeCode.UInt16);           // Will result in System.OverflowException: Value was either too large or too small for a UInt16.
    /// ConvertUnchecked.ChangeType(-1, TypeCode.UInt16);  // Will result in -1
    /// </code>
    /// <code>
    /// enum Test { A = -1, B = 0, C = 1 }
    /// Convert.ChangeType(Test.A, TypeCode.Byte);           // Will result in System.OverflowException: Value was either too large or too small for an unsigned byte.
    /// ConvertUnchecked.ChangeType(Test.A, TypeCode.Byte);  // Will result in -1
    /// </code>
    /// </example>
    public static class ConvertUnchecked
    {
        /// <summary>
        /// Changes the type of the given object based on the given typeCode using hard unchecked casts. 
        /// 
        /// <para>This implementation also supports the unchecked conversion of enums based on their underlying type.</para>
        /// </summary>
        /// <param name="value">The value to convert to the type corresponding to the given typeCode.  If value is
        /// null, null will be returned</param>
        /// <param name="typeCode">The TypeCode used for the conversion</param>
        /// <returns>The value converted, an exception, or null if the given value was null</returns>
        public static object? ChangeType(object? value, TypeCode typeCode)
        {
            if( value == null )
                return null;

            if( !(value is IConvertible) )
                throw new InvalidCastException($"{value.GetType().Name} doesn't implement {nameof(IConvertible)}");
            
            unchecked
            {
                // An enum is interesting as it will identify its underlying type as its TypeCode
                //
                switch( typeCode )
                {
                    case TypeCode.Boolean: return ToBoolean(value);
                    case TypeCode.Byte: return ToByte(value);
                    case TypeCode.Decimal: return ToDecimal(value);
                    case TypeCode.Double: return ToDouble(value);
                    case TypeCode.Int16: return ToInt16(value);
                    case TypeCode.Int32: return ToInt32(value);
                    case TypeCode.Int64: return ToInt64(value);
                    case TypeCode.SByte: return ToSByte(value);
                    case TypeCode.Single: return ToSingle(value);
                    case TypeCode.UInt16: return ToUInt16(value);
                    case TypeCode.UInt32: return ToUInt32(value);
                    case TypeCode.UInt64: return ToUInt64(value);

                    case TypeCode.Char:
                    case TypeCode.DateTime:
                    case TypeCode.DBNull:
                    case TypeCode.Empty:
                    case TypeCode.Object:
                    case TypeCode.String:
                        return Convert.ChangeType(value, typeCode);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, null);
                }
            }
        }

        /// <summary>
        /// Changes the type of the given object based on the given conversionType using hard unchecked casts. 
        /// 
        /// <para>This implementation also supports the unchecked conversion of enums based on their underlying type.</para>
        /// </summary>
        /// <param name="value">The value to convert to the conversionType.  If value is null, null will be returned</param>
        /// <param name="conversionType">The type being converted to</param>
        /// <returns>The value converted, an exception, or null if the given value was null</returns>
        public static object? ChangeType(object? value, Type conversionType)
            => ChangeType(value, Type.GetTypeCode(conversionType));

        public static TConversion ChangeType<TConversion>(object value)
            where TConversion : struct, IConvertible => 
            (TConversion)ChangeType(value, Type.GetTypeCode(typeof(TConversion)))!;

        #region Boolean
        public static bool ToBoolean(object valueObject)
        {
            switch( valueObject )
            {
                case bool value: return ToBoolean(value);
                case byte value: return ToBoolean(value);
                case decimal value: return ToBoolean(value);
                case double value: return ToBoolean(value);
                case Int16 value: return ToBoolean(value);
                case Int32 value: return ToBoolean(value);
                case Int64 value: return ToBoolean(value);
                case SByte value: return ToBoolean(value);
                case float value: return ToBoolean(value);
                case UInt16 value: return ToBoolean(value);
                case UInt32 value: return ToBoolean(value);
                case UInt64 value: return ToBoolean(value);
                case Enum value: return ToBoolean(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                default:
                    throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Boolean)}", nameof(valueObject));
            }
        }
        
        public static bool ToBoolean(bool value) => value;
        public static bool ToBoolean(byte value) => value != 0;
        public static bool ToBoolean(decimal value) => value != 0;
        public static bool ToBoolean(double value) => value < 0 || value > double.Epsilon;
        public static bool ToBoolean(Int16 value) => value != 0;
        public static bool ToBoolean(Int32 value) => value != 0;
        public static bool ToBoolean(Int64 value) => value != 0;
        public static bool ToBoolean(SByte value) => value != 0;
        public static bool ToBoolean(float value) => value < 0 || value > float.Epsilon;
        public static bool ToBoolean(UInt16 value) => value != 0;
        public static bool ToBoolean(UInt32 value) => value != 0;
        public static bool ToBoolean(UInt64 value) => value != 0;
        #endregion

        #region Byte
        public static byte ToByte(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToByte(value);
                    case byte value: return ToByte(value);
                    case decimal value: return ToByte(value);
                    case double value: return ToByte(value);
                    case Int16 value: return ToByte(value);
                    case Int32 value: return ToByte(value);
                    case Int64 value: return ToByte(value);
                    case SByte value: return ToByte(value);
                    case float value: return ToByte(value);
                    case UInt16 value: return ToByte(value);
                    case UInt32 value: return ToByte(value);
                    case UInt64 value: return ToByte(value);
                    case Enum value: return ToByte(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Byte)}", nameof(valueObject));
                }
            }
        }

        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
        public static byte ToByte(byte value) => unchecked((byte)value);
        public static byte ToByte(decimal value) => unchecked((byte)value);   // Will throw if value can't fit
        public static byte ToByte(double value) => unchecked((byte)value);
        public static byte ToByte(Int16 value) => unchecked((byte)value);
        public static byte ToByte(Int32 value) => unchecked((byte)value);
        public static byte ToByte(Int64 value) => unchecked((byte)value);
        public static byte ToByte(SByte value) => unchecked((byte)value);
        public static byte ToByte(float value) => unchecked((byte)value);
        public static byte ToByte(UInt16 value) => unchecked((byte)value);
        public static byte ToByte(UInt32 value) => unchecked((byte)value);
        public static byte ToByte(UInt64 value) => unchecked((byte)value);
        #endregion

        #region Decimal
        public static decimal ToDecimal(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToDecimal(value);
                    case byte value: return ToDecimal(value);
                    case decimal value: return ToDecimal(value);
                    case double value: return ToDecimal(value);
                    case Int16 value: return ToDecimal(value);
                    case Int32 value: return ToDecimal(value);
                    case Int64 value: return ToDecimal(value);
                    case SByte value: return ToDecimal(value);
                    case float value: return ToDecimal(value);
                    case UInt16 value: return ToDecimal(value);
                    case UInt32 value: return ToDecimal(value);
                    case UInt64 value: return ToDecimal(value);
                    case Enum value: return ToDecimal(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Decimal)}", nameof(valueObject));
                }
            }
        }

        public static decimal ToDecimal(bool value) => value ? (decimal)1 : (decimal)0;
        public static decimal ToDecimal(byte value) => unchecked((decimal)value);
        public static decimal ToDecimal(decimal value) => unchecked((decimal)value);
        public static decimal ToDecimal(double value) => unchecked((decimal)value);
        public static decimal ToDecimal(Int16 value) => unchecked((decimal)value);
        public static decimal ToDecimal(Int32 value) => unchecked((decimal)value);
        public static decimal ToDecimal(Int64 value) => unchecked((decimal)value);
        public static decimal ToDecimal(SByte value) => unchecked((decimal)value);
        public static decimal ToDecimal(float value) => unchecked((decimal)value);
        public static decimal ToDecimal(UInt16 value) => unchecked((decimal)value);
        public static decimal ToDecimal(UInt32 value) => unchecked((decimal)value);
        public static decimal ToDecimal(UInt64 value) => unchecked((decimal)value);
        #endregion

        #region Double
        public static double ToDouble(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToDouble(value);
                    case byte value: return ToDouble(value);
                    case decimal value: return ToDouble(value);
                    case double value: return ToDouble(value);
                    case Int16 value: return ToDouble(value);
                    case Int32 value: return ToDouble(value);
                    case Int64 value: return ToDouble(value);
                    case SByte value: return ToDouble(value);
                    case float value: return ToDouble(value);
                    case UInt16 value: return ToDouble(value);
                    case UInt32 value: return ToDouble(value);
                    case UInt64 value: return ToDouble(value);
                    case Enum value: return ToDouble(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Double)}", nameof(valueObject));
                }
            }
        }

        public static double ToDouble(bool value) => value ? (double)1 : (double)0;
        public static double ToDouble(byte value) => unchecked((double)value);
        public static double ToDouble(decimal value) => unchecked((double)value);   // Will throw if value can't fit
        public static double ToDouble(double value) => unchecked((double)value);
        public static double ToDouble(Int16 value) => unchecked((double)value);
        public static double ToDouble(Int32 value) => unchecked((double)value);
        public static double ToDouble(Int64 value) => unchecked((double)value);
        public static double ToDouble(SByte value) => unchecked((double)value);
        public static double ToDouble(float value) => unchecked((double)value);
        public static double ToDouble(UInt16 value) => unchecked((double)value);
        public static double ToDouble(UInt32 value) => unchecked((double)value);
        public static double ToDouble(UInt64 value) => unchecked((double)value);
        #endregion

        #region Int16
        public static Int16 ToInt16(object valueObject)
        {
            switch( valueObject )
            {
                case bool value: return ToInt16(value);
                case byte value: return ToInt16(value);
                case decimal value: return ToInt16(value);
                case double value: return ToInt16(value);
                case Int16 value: return ToInt16(value);
                case Int32 value: return ToInt16(value);
                case Int64 value: return ToInt16(value);
                case SByte value: return ToInt16(value);
                case float value: return ToInt16(value);
                case UInt16 value: return ToInt16(value);
                case UInt32 value: return ToInt16(value);
                case UInt64 value: return ToInt16(value);
                case Enum value: return ToInt16(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                default:
                    throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Int16)}", nameof(valueObject));
            }
        }

        public static Int16 ToInt16(bool value) => value ? (Int16)1 : (Int16)0;
        public static Int16 ToInt16(byte value) => unchecked((Int16)value);
        public static Int16 ToInt16(decimal value) => unchecked((Int16)value);   // Will throw if value can't fit
        public static Int16 ToInt16(double value) => unchecked((Int16)value);
        public static Int16 ToInt16(Int16 value) => unchecked((Int16)value);
        public static Int16 ToInt16(Int32 value) => unchecked((Int16)value);
        public static Int16 ToInt16(Int64 value) => unchecked((Int16)value);
        public static Int16 ToInt16(SByte value) => unchecked((Int16)value);
        public static Int16 ToInt16(float value) => unchecked((Int16)value);
        public static Int16 ToInt16(UInt16 value) => unchecked((Int16)value);
        public static Int16 ToInt16(UInt32 value) => unchecked((Int16)value);
        public static Int16 ToInt16(UInt64 value) => unchecked((Int16)value);
        #endregion

        #region Int32
        public static Int32 ToInt32(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToInt32(value);
                    case byte value: return ToInt32(value);
                    case decimal value: return ToInt32(value);
                    case double value: return ToInt32(value);
                    case Int16 value: return ToInt32(value);
                    case Int32 value: return ToInt32(value);
                    case Int64 value: return ToInt32(value);
                    case SByte value: return ToInt32(value);
                    case float value: return ToInt32(value);
                    case UInt16 value: return ToInt32(value);
                    case UInt32 value: return ToInt32(value);
                    case UInt64 value: return ToInt32(value);
                    case Enum value: return ToInt32(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Int32)}", nameof(valueObject));
                }
            }
        }

        public static Int32 ToInt32(bool value) => value ? (Int32)1 : (Int32)0;
        public static Int32 ToInt32(byte value) => unchecked((Int32)value);
        public static Int32 ToInt32(decimal value) => unchecked((Int32)value);   // Will throw if value can't fit
        public static Int32 ToInt32(double value) => unchecked((Int32)value);
        public static Int32 ToInt32(Int16 value) => unchecked((Int32)value);
        public static Int32 ToInt32(Int32 value) => unchecked((Int32)value);
        public static Int32 ToInt32(Int64 value) => unchecked((Int32)value);
        public static Int32 ToInt32(SByte value) => unchecked((Int32)value);
        public static Int32 ToInt32(float value) => unchecked((Int32)value);
        public static Int32 ToInt32(UInt16 value) => unchecked((Int32)value);
        public static Int32 ToInt32(UInt32 value) => unchecked((Int32)value);
        public static Int32 ToInt32(UInt64 value) => unchecked((Int32)value);
        #endregion

        #region Int64
        public static Int64 ToInt64(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToInt64(value);
                    case byte value: return ToInt64(value);
                    case decimal value: return ToInt64(value);
                    case double value: return ToInt64(value);
                    case Int16 value: return ToInt64(value);
                    case Int32 value: return ToInt64(value);
                    case Int64 value: return ToInt64(value);
                    case SByte value: return ToInt64(value);
                    case float value: return ToInt64(value);
                    case UInt16 value: return ToInt64(value);
                    case UInt32 value: return ToInt64(value);
                    case UInt64 value: return ToInt64(value);
                    case Enum value: return ToInt64(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Int64)}", nameof(valueObject));
                }
            }
        }

        public static Int64 ToInt64(bool value) => value ? (Int64)1 : (Int64)0;
        public static Int64 ToInt64(byte value) => unchecked((Int64)value);
        public static Int64 ToInt64(decimal value) => unchecked((Int64)value);   // Will throw if value can't fit
        public static Int64 ToInt64(double value) => unchecked((Int64)value);
        public static Int64 ToInt64(Int16 value) => unchecked((Int64)value);
        public static Int64 ToInt64(Int32 value) => unchecked((Int64)value);
        public static Int64 ToInt64(Int64 value) => unchecked((Int64)value);
        public static Int64 ToInt64(SByte value) => unchecked((Int64)value);
        public static Int64 ToInt64(float value) => unchecked((Int64)value);
        public static Int64 ToInt64(UInt16 value) => unchecked((Int64)value);
        public static Int64 ToInt64(UInt32 value) => unchecked((Int64)value);
        public static Int64 ToInt64(UInt64 value) => unchecked((Int64)value);
        #endregion
        
        #region SByte
        public static sbyte ToSByte(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToSByte(value);
                    case byte value: return ToSByte(value);
                    case decimal value: return ToSByte(value);
                    case double value: return ToSByte(value);
                    case Int16 value: return ToSByte(value);
                    case Int32 value: return ToSByte(value);
                    case Int64 value: return ToSByte(value);
                    case SByte value: return ToSByte(value);
                    case float value: return ToSByte(value);
                    case UInt16 value: return ToSByte(value);
                    case UInt32 value: return ToSByte(value);
                    case UInt64 value: return ToSByte(value);
                    case Enum value: return ToSByte(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(SByte)}", nameof(valueObject));
                }
            }
        }

        public static sbyte ToSByte(bool value) => value ? (sbyte)1 : (sbyte)0;
        public static sbyte ToSByte(byte value) => unchecked((sbyte)value);
        public static sbyte ToSByte(decimal value) => unchecked((sbyte)value);   // Will throw if value can't fit
        public static sbyte ToSByte(double value) => unchecked((sbyte)value);
        public static sbyte ToSByte(Int16 value) => unchecked((sbyte)value);
        public static sbyte ToSByte(Int32 value) => unchecked((sbyte)value);
        public static sbyte ToSByte(Int64 value) => unchecked((sbyte)value);
        public static sbyte ToSByte(SByte value) => unchecked((sbyte)value);
        public static sbyte ToSByte(float value) => unchecked((sbyte)value);
        public static sbyte ToSByte(UInt16 value) => unchecked((sbyte)value);
        public static sbyte ToSByte(UInt32 value) => unchecked((sbyte)value);
        public static sbyte ToSByte(UInt64 value) => unchecked((sbyte)value);
        #endregion

        #region Single / Float
        public static float ToSingle(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToSingle(value);
                    case byte value: return ToSingle(value);
                    case decimal value: return ToSingle(value);
                    case double value: return ToSingle(value);
                    case Int16 value: return ToSingle(value);
                    case Int32 value: return ToSingle(value);
                    case Int64 value: return ToSingle(value);
                    case SByte value: return ToSingle(value);
                    case float value: return ToSingle(value);
                    case UInt16 value: return ToSingle(value);
                    case UInt32 value: return ToSingle(value);
                    case UInt64 value: return ToSingle(value);
                    case Enum value: return ToSingle(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(Single)}", nameof(valueObject));
                }
            }
        }

        public static float ToSingle(bool value) => value ? (float)1 : (float)0;
        public static float ToSingle(byte value) => unchecked((float)value);
        public static float ToSingle(decimal value) => unchecked((float)value);   // Will throw if value can't fit
        public static float ToSingle(double value) => unchecked((float)value);
        public static float ToSingle(Int16 value) => unchecked((float)value);
        public static float ToSingle(Int32 value) => unchecked((float)value);
        public static float ToSingle(Int64 value) => unchecked((float)value);
        public static float ToSingle(SByte value) => unchecked((float)value);
        public static float ToSingle(float value) => unchecked((float)value);
        public static float ToSingle(UInt16 value) => unchecked((float)value);
        public static float ToSingle(UInt32 value) => unchecked((float)value);
        public static float ToSingle(UInt64 value) => unchecked((float)value);
        #endregion

        #region UInt16
        public static UInt16 ToUInt16(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToUInt16(value);
                    case byte value: return ToUInt16(value);
                    case decimal value: return ToUInt16(value);
                    case double value: return ToUInt16(value);
                    case Int16 value: return ToUInt16(value);
                    case Int32 value: return ToUInt16(value);
                    case Int64 value: return ToUInt16(value);
                    case SByte value: return ToUInt16(value);
                    case float value: return ToUInt16(value);
                    case UInt16 value: return ToUInt16(value);
                    case UInt32 value: return ToUInt16(value);
                    case UInt64 value: return ToUInt16(value);
                    case Enum value: return ToUInt16(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(UInt16)}", nameof(valueObject));
                }
            }
        }

        public static UInt16 ToUInt16(bool value) => value ? (UInt16)1 : (UInt16)0;
        public static UInt16 ToUInt16(byte value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(decimal value) => unchecked((UInt16)value);   // Will throw if value can't fit
        public static UInt16 ToUInt16(double value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(Int16 value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(Int32 value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(Int64 value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(SByte value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(float value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(UInt16 value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(UInt32 value) => unchecked((UInt16)value);
        public static UInt16 ToUInt16(UInt64 value) => unchecked((UInt16)value);
        #endregion

        #region UInt32
        public static UInt32 ToUInt32(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToUInt32(value);
                    case byte value: return ToUInt32(value);
                    case decimal value: return ToUInt32(value);
                    case double value: return ToUInt32(value);
                    case Int16 value: return ToUInt32(value);
                    case Int32 value: return ToUInt32(value);
                    case Int64 value: return ToUInt32(value);
                    case SByte value: return ToUInt32(value);
                    case float value: return ToUInt32(value);
                    case UInt16 value: return ToUInt32(value);
                    case UInt32 value: return ToUInt32(value);
                    case UInt64 value: return ToUInt32(value);
                    case Enum value: return ToUInt32(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(UInt32)}", nameof(valueObject));
                }
            }
        }

        public static UInt32 ToUInt32(bool value) => value ? (UInt32)1 : (UInt32)0;
        public static UInt32 ToUInt32(byte value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(decimal value) => unchecked((UInt32)value);   // Will throw if value can't fit
        public static UInt32 ToUInt32(double value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(Int16 value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(Int32 value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(Int64 value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(SByte value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(float value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(UInt16 value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(UInt32 value) => unchecked((UInt32)value);
        public static UInt32 ToUInt32(UInt64 value) => unchecked((UInt32)value);
        #endregion

        #region UInt64
        public static UInt64 ToUInt64(object valueObject)
        {
            unchecked
            {
                switch( valueObject )
                {
                    case bool value: return ToUInt64(value);
                    case byte value: return ToUInt64(value);
                    case decimal value: return ToUInt64(value);
                    case double value: return ToUInt64(value);
                    case Int16 value: return ToUInt64(value);
                    case Int32 value: return ToUInt64(value);
                    case Int64 value: return ToUInt64(value);
                    case SByte value: return ToUInt64(value);
                    case float value: return ToUInt64(value);
                    case UInt16 value: return ToUInt64(value);
                    case UInt32 value: return ToUInt64(value);
                    case UInt64 value: return ToUInt64(value);
                    case Enum value: return ToUInt64(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
                    default:
                        throw new ArgumentException($"Unable to convert {valueObject.GetType().Name} to {nameof(UInt64)}", nameof(valueObject));
                }
            }
        }

        public static UInt64 ToUInt64(bool value) => value ? (UInt64)1 : (UInt64)0;
        public static UInt64 ToUInt64(byte value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(decimal value) => unchecked((UInt64)value);   // Will throw if value can't fit
        public static UInt64 ToUInt64(double value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(Int16 value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(Int32 value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(Int64 value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(SByte value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(float value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(UInt16 value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(UInt32 value) => unchecked((UInt64)value);
        public static UInt64 ToUInt64(UInt64 value) => unchecked((UInt64)value);
        #endregion
    }
}
