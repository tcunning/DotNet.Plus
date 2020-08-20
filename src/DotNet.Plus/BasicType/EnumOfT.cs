using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// A set of operations that can be performed on enum values.
    /// </summary>
    /// <typeparam name="TEnum">The Type of enum</typeparam>
    public static partial class Enum<TEnum>
        where TEnum : Enum
    {
        /// <summary>
        /// See if the given object is defined by the enum either by name in the case of a string or by
        /// the native value of the enum. <seealso cref="Enum.IsDefined"/>
        /// </summary>
        /// <param name="fromValue">A string or native representation of an enum value</param>
        /// <returns></returns>
        public static bool IsDefined(object fromValue) => Enum.IsDefined(typeof(TEnum), fromValue);

        /// <summary>
        /// Uses reflection to retrieve an IEnumerable form the Enum type
        /// 
        /// Example usage 
        ///     var myEnumValues = EnumExtensions.GetValues<MyEnum>();
        /// 
        /// See: https://stackoverflow.com/questions/972307/can-you-loop-through-all-enum-values and
        ///      https://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum
        /// </summary>
        /// <typeparam name="TEnum">the Enum type to retrieve an enumeration of</typeparam>
        /// <returns>an IEnumerable of type T</returns>
        /// <exception cref="ArgumentException">thrown when type of T is not Enum</exception>
        public static IEnumerable<TEnum> GetValues()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// Returns true if this is a flags enum and cache the results
        /// </summary>
        public static bool IsFlagsEnum => _isFlagsEnum ??= typeof(TEnum).IsDefined(typeof(FlagsAttribute), inherit: false);

        // ReSharper disable once StaticMemberInGenericType (this is the behavior we want)
        private static bool? _isFlagsEnum;
        
        #region Lookup Default Value
        private static readonly Lazy<TEnum> ValueCache = new Lazy<TEnum>(LookupDefaultValue);

        /// <summary>
        /// Return the value of DefaultValueAttribute if it exists or the "default" value, if there is no DefaultValueAttribute.
        /// </summary>
        public static TEnum DefaultValue => ValueCache.Value;

        /// <summary>
        /// Return the value of DefaultValueAttribute if it exists or the "default" value, if there is no DefaultValueAttribute.
        /// <see cref="DefaultValue"/> which caches the results of this method as the lookup only needs to be done once.
        /// </summary>
        private static TEnum LookupDefaultValue()
        {
            Type enumType = typeof(TEnum);

            // See if we have a DefaultValue attribute, if we don't just return the system default value for the enum.
            //
            if ( !(enumType.GetCustomAttributes(typeof(DefaultValueAttribute), inherit: true)?.FirstOrDefault() is DefaultValueAttribute defaultValueAttribute) )
                return default!;

            // It's super important we don't do the TryConvert with enableDefaultValue true as that could cause an
            // infinite recursion as THIS is the routine that determines the default value.
            //
            if( !Convert(defaultValueAttribute.Value, out var defaultChoice, () => default!) ) {
                var errorMessage = $"Default value {defaultValueAttribute.Value} can't be converted into an enum of type {typeof(TEnum).Name} ";
                System.Diagnostics.Debug.Assert(false, errorMessage);
            }

            return defaultChoice;
        }
        #endregion
    }

}
