using System;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Extensions to help convert from another type into an enum.
    /// </summary>
    /// <typeparam name="TEnum">The Type of enum</typeparam>
    public static partial class Enum<TEnum>
        where TEnum : Enum
    {
        /// <summary>
        /// Tries to convert the fromValue into the an enum of type TValue, suppressing any exceptions.  The following
        /// conversions are attempted:
        ///    - Is a string is given, the string must exactly match
        ///    - If it is a flags enum the underlying type of the enum must match exactly
        ///    - It is isn't a flags enum the value will attempt to be converted into the underlying type of the enum
        /// </summary>
        /// <param name="fromValue">This can be the native type of the enum (int, uint, ...) or a string.  If it is a string,
        /// it must match the case of values in the enum</param>
        /// <returns>The fromValue converted to TValue or a default value if the direct conversion wasn't possible.</returns>
        public static TEnum TryConvert(object fromValue)
        {
            try
            {
                Convert(fromValue, out var toValue, enableDefaultValue: true);
                return toValue;
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Tries to convert the fromValue into the an enum of type TValue, suppressing any exceptions.  The following
        /// conversions are attempted:
        ///    - Is a string is given, the string must exactly match
        ///    - If it is a flags enum the underlying type of the enum must match exactly
        ///    - It is isn't a flags enum the value will attempt to be converted into the underlying type of the enum
        /// </summary>
        /// <param name="fromValue">This can be the native type of the enum (int, uint, ...) or a string.  If it is a
        /// string, it must match the case of values in the enum</param>
        /// <param name="toValue">This will be the result of the conversion</param>
        /// <param name="enableDefaultValue">If true we the conversion will use the default value of the enum, either
        /// the system default or the value specified in the DefaultValueAttribute, when the fromValue can't be converted
        /// or isn't a member of the enum</param>
        /// <returns>True is the fromValue was converted into the toValue, false if the conversion failed and the default
        /// value was used (see enableDefaultValue).</returns>
        public static bool TryConvert(object fromValue, out TEnum toValue, bool enableDefaultValue = true)
        {
            try
            {
                return Convert(fromValue, out toValue, enableDefaultValue);
            }
            catch
            {
                toValue = enableDefaultValue ? DefaultValue : default!;
                return false;
            }
        }

        /// <summary>
        /// Tries to convert the fromValue into the an enum of type TValue, suppressing any exceptions.  The following
        /// conversions are attempted:
        ///    - Is a string is given, the string must exactly match
        ///    - If it is a flags enum the underlying type of the enum must match exactly
        ///    - It is isn't a flags enum the value will attempt to be converted into the underlying type of the enum
        /// </summary>
        /// <param name="fromValue">This can be the native type of the enum (int, uint, ...) or a string.  If it is a
        /// string, it must match the case of values in the enum</param>
        /// <param name="toValue">This will be the result of the conversion</param>
        /// <param name="enableDefaultValue">If true we the conversion will use the default value of the enum (either
        /// the system default or the value specified in the
        /// DefaultValueAttribute) when the fromValue can't be converted or isn't a member of the enum</param>
        /// <returns>True is the fromValue was converted into the toValue, false if the conversion failed and the
        /// default value was used (see enableDefaultValue) </returns>
        /// <exception cref="ConvertObjectToEnumException">This will be thrown if enableDefaultValue is false and
        /// fromValue couldn't be converted into a TValue</exception>  
        /// <exception cref="InvalidOperationException">If the given fromValue can't be converted to a enum convertable type (string, int, ...)</exception>
        public static bool Convert(object fromValue, out TEnum toValue, bool enableDefaultValue = false) =>
            Convert(fromValue, out toValue, () => enableDefaultValue ? DefaultValue : throw new ConvertObjectToEnumException<TEnum>(fromValue));

        /// <summary>
        /// Tries to convert the fromValue into the an enum of type TValue, suppressing any exceptions.  The following
        /// conversions are attempted:
        ///    - Is a string is given, the string must exactly match
        ///    - If it is a flags enum the underlying type of the enum must match exactly
        ///    - It is isn't a flags enum the value will attempt to be converted into the underlying type of the enum
        /// </summary>
        /// <param name="fromValue">This can be the native type of the enum (int, uint, ...) or a string.  If it is a
        /// string, it must match the case of values in the enum</param>
        /// <param name="toValue">This will be the result of the conversion</param>
        /// <param name="makeDefaultValue">This is used when the converted fromValue isn't found in the TEnum to get the
        /// default value that will be returned in toValue.  If null is passed it, this will use DefaultValue.</param>
        /// <returns>false if conversion couldn't take place and the default value was returned.</returns>
        /// <exception cref="InvalidOperationException">If the given fromValue can't be converted to a enum convertible type (string, int, ...)</exception>
        public static bool Convert(object fromValue, out TEnum toValue, Func<TEnum>? makeDefaultValue)
        {
            Type enumType = typeof(TEnum);

            // If from version is the same type then there is nothing to convert.
            //
            if( fromValue is TEnum fromValueEnum )
            {
                toValue = fromValueEnum;
                return true;
            }

            if( fromValue is Enum )
            {
                toValue = (makeDefaultValue == null ? DefaultValue : makeDefaultValue())!;  // An TEnum can't be null because it's an enum!
                return false;
            }

            // If this isn't a [Flags] attributed enum then we make sure the value is defined in our enum.  Otherwise, we just
            // convert the value as is because its defined as being a bit field (Flags)
            //
            if (!IsFlagsEnum )
            {

                // If the fromValue isn't a string, we will try to convert it to the underlying type of the enum.  This allows
                // us to pass in a ulong for an enum that is represented by an int and allow the lookup to work properly.  Otherwise,
                // the passed in type has to exactly match the type of the enum or an exception will be thrown.
                //
                if (!(fromValue is string) && fromValue.GetType() != Enum.GetUnderlyingType(enumType) && (fromValue is IConvertible convertibleValue))
                    fromValue = System.Convert.ChangeType(convertibleValue, Enum.GetUnderlyingType(enumType));

                // Make sure the value is in our enum before we allow the conversation
                //
                bool foundEnum = IsDefined(fromValue);
                if (!foundEnum)
                {
                    toValue = (makeDefaultValue == null ? DefaultValue : makeDefaultValue())!;  // An TEnum can't be null because it's an enum!
                    return false;
                }
            }

            if (fromValue is string stringValue)
                toValue = (TEnum)Enum.Parse(enumType, stringValue);
            else
                toValue = (TEnum)Enum.ToObject(enumType, fromValue);

            return true;
        }
    }
}
