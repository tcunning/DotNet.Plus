using System;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Enum extensions such as converting an enum to a value.
    /// </summary>
    public static class EnumEx
    {
        public static TValue ToValue<TValue>(this Enum value)
            where TValue : IConvertible =>
            (TValue)Convert.ChangeType(value, typeof(TValue));
        
    }
}
