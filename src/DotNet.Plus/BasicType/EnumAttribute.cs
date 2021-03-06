﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DotNet.Plus.Core;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Enum extensions to get the attributes associated with an enum such as [Description]
    /// </summary>
    public static class EnumAttribute
    {
        /// <summary>
        /// Read an Attribute from an Enum
        /// </summary>
        /// <typeparam name="TAttribute">the Attribute type to be retrieved</typeparam>
        /// <param name="instance">instance of the Enum value</param>
        /// <returns>The found Attribute or null if it wasn't defined for the Enum value</returns>
        public static TAttribute? TryGetAttribute<TAttribute>(this Enum instance)
            where TAttribute : Attribute =>
            TryGetAttributes<TAttribute>(instance).FirstOrDefault();

        /// <summary>
        /// Returns the enumeration of the found Attribute of type TAttribute that are associated with
        /// the given enum instance.
        /// </summary>
        /// <typeparam name="TAttribute">the Attribute type to be retrieved</typeparam>
        /// <param name="instance">instance of the Enum value</param>
        /// <returns>The found Attribute or null if it wasn't defined for the Enum value</returns>
        public static IEnumerable<TAttribute> TryGetAttributes<TAttribute>(this Enum instance)
            where TAttribute : Attribute
        {
            return Operation.TryCatch(() =>
            {
                var field = instance.GetType().GetField(instance.ToString());
                var attributes = field.GetCustomAttributes<TAttribute>();
                return attributes;
            }, Enumerable.Empty<TAttribute>());
        }

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
