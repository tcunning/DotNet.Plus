using System;

namespace DotNet.Plus.BasicType
{
    public enum TypeCodeInteger
    {
        Boolean = TypeCode.Boolean,
        SByte = TypeCode.SByte,
        Int16 = TypeCode.Int16,
        Int32 = TypeCode.Int32,
        Int64 = TypeCode.Int64,
        Byte = TypeCode.Byte,
        UInt16 = TypeCode.UInt16,
        UInt32 = TypeCode.UInt32,
        UInt64 = TypeCode.UInt64,
    }
    
    /// <summary>
    /// Defines what system types ARE Integers  This is useful for generic types that only work with integer
    /// values.
    /// </summary>
    /// <typeparam name="TIntegerValue">The type of the Integer such as Int32 or UInt32, must be an integer type</typeparam>
    public static class IntegerDefinition<TIntegerValue>
        where TIntegerValue : struct, IConvertible
    {
        /// <summary>
        /// The cached system TypeCode of the integer
        /// </summary>
        /// ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static readonly TypeCode TypeCode;

        /// <summary>
        /// The cached system TypeCode of the integer
        /// </summary>
        /// ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static readonly TypeCodeInteger TypeCodeInteger;

        /// <summary>
        /// The size of the integer in bytes, this is useful as generic classes can't get the
        /// size of the generic type at runtime without jumping though some hoops.
        /// </summary>
        /// ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static readonly int Size;

        /// <summary>
        /// True if the Integer is a signed integer
        /// </summary>
        /// ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static readonly bool IsSigned;

        /// <summary>
        /// This will always be true as the type has to be an integer. The static constructor will
        /// throw if TIntegerValue isn't a supported integer type.   This method is useful for
        /// forcing that static initialization if it hasn't already been performed.
        /// </summary>
        /// ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static bool IsInteger => true;

        /// <summary>
        /// Initialize the Integer, will throw if TIntegerValue isn't an integer type. 
        /// </summary>
        /// <exception cref="TypeInitializationException">Will be thrown if the TValue isn't a system integer type
        /// with an inner exception of ArgumentException</exception>
        static IntegerDefinition()
        {
            TypeCode = System.Type.GetTypeCode(typeof(TIntegerValue));
            TypeCodeInteger = (TypeCodeInteger)TypeCode;
            switch( TypeCode )
            {
                case TypeCode.Boolean:
                    Size = sizeof(bool);
                    IsSigned = false;
                    break;

                case TypeCode.SByte:
                    Size = sizeof(sbyte);
                    IsSigned = true;
                    break;

                case TypeCode.Int16:
                    Size = sizeof(Int16);
                    IsSigned = true;
                    break;
                
                case TypeCode.Int32:
                    Size = sizeof(Int32);
                    IsSigned = true;
                    break;
                
                case TypeCode.Int64:
                    Size = sizeof(Int64);
                    IsSigned = true;
                    break;
                
                case TypeCode.Byte:
                    Size = sizeof(byte);
                    IsSigned = false;
                    break;
                
                case TypeCode.UInt16:
                    Size = sizeof(UInt16);
                    IsSigned = false;
                    break;
                
                case TypeCode.UInt32:
                    Size = sizeof(UInt32);
                    IsSigned = false;
                    break;
                
                case TypeCode.UInt64:
                    Size = sizeof(UInt64);
                    IsSigned = false;
                    break;

                default:
                    throw new ArgumentException($"Type code is not an integer system type");
            }
        }
    }
}
