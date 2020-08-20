using System;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// This will be thrown when an operation is performed on an enum that was expected to be a [FLAGS]
    /// enum.
    /// </summary>
    public class NotFlagsEnumException : Exception
    {
        public NotFlagsEnumException(string message, Exception? innerException = null) :
            base(message, innerException)
        {
        }
    }

    /// <summary>
    /// This will be thrown when an object wasn't able to be converted to an Enum.
    /// </summary>
    public class ConvertObjectToEnumException : Exception
    {
        public ConvertObjectToEnumException(string message, Exception? innerException = null) :
            base(message, innerException)
        {
        }
    }

    /// <summary>
    /// This will be thrown when an object wasn't able to be converted to an Enum with more detailed
    /// information.
    /// </summary>
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
