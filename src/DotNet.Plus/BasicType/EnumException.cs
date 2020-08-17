using System;

namespace DotNet.Plus.BasicType
{
    public class NotFlagsEnumException : Exception
    {
        public NotFlagsEnumException(string message, Exception? innerException = null) :
            base(message, innerException)
        {
        }
    }


    public class ConvertObjectToEnumException : Exception
    {
        public ConvertObjectToEnumException(string message, Exception? innerException = null) :
            base(message, innerException)
        {
        }
    }

    public class ConvertObjectToEnumException<TEnum> : ConvertObjectToEnumException
        where TEnum : Enum
    {
        public ConvertObjectToEnumException(object fromValue, Exception? innerException = null) :
            this($"Unable to convert {fromValue} into the enum {typeof(TEnum).Name}", innerException)
        {
        }

        public ConvertObjectToEnumException(string message, Exception? innerException = null) :
            base(message, innerException)
        {
        }
    }

}
