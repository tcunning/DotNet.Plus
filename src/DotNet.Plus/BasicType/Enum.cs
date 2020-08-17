using System;
using System.ComponentModel;

namespace DotNet.Plus.BasicType
{
    public static class EnumEx
    {
        public static TValue ToValue<TValue>(this Enum value)
            where TValue : IConvertible =>
            (TValue)Convert.ChangeType(value, typeof(TValue));
        
        public static bool TryGetDescription(this Enum instance, out string? description) 
        {
            var attribute = instance.TryGetAttribute<DescriptionAttribute>();
            description = attribute?.Description;
            return description != null;
        }

        public static string? Description(this Enum instance)
            => TryGetDescription(instance, out var description) ? description : instance.ToString();

    }
}
