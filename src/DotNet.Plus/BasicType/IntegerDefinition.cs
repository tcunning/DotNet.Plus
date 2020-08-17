using System;
using System.Diagnostics.CodeAnalysis;

namespace DotNet.Plus.BasicType
{
    public static class IntegerDefinition<TIntegerValue>
        where TIntegerValue : struct, IConvertible
    {
        // ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static readonly TypeCode TypeCode;

        // ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static readonly int Size;

        // ReSharper disable once StaticMemberInGenericType (this is exactly what we want!)
        public static readonly bool IsSigned;

        // This can only return true as the static constructor will throw if it's not an integer 
        public static bool IsInteger => true;

        /// <summary>
        /// Initialize the Integer
        /// </summary>
        /// <exception cref="TypeInitializationException">Will be thrown if the TValue isn't a system integer type</exception>
        static IntegerDefinition()
        {
            TypeCode = System.Type.GetTypeCode(typeof(TIntegerValue));
            switch( TypeCode )
            {
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
