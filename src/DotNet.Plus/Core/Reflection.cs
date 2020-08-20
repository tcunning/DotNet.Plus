using System;
using System.Reflection;

namespace DotNet.Plus.Core
{
    /// <summary>
    /// Adds extensions to Reflection to make it easier to get method get/set properties, etc
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Uses reflection to get a method matching the given signature.
        /// </summary>
        /// <param name="type">A type who's method is to be looked up</param>
        /// <param name="name">The name of the method</param>
        /// <param name="bindingFlags">Flags that identify the method's access</param>
        /// <param name="argTypes">Variable arguments that represent the argument types of the desired method</param>
        /// <returns>The found method or null if the method couldn't be found.</returns>
        public static MethodInfo? TryGetMethod(this Type type, string name, BindingFlags bindingFlags, params Type[] argTypes)
        {
            return Operation.TryCatch(() =>
            {
                var method = type.GetMethod(name, bindingFlags, binder: null, types: argTypes, modifiers: null);
                return method;
            });
        }

        /// <summary>
        /// Uses reflection to get a property matching the given signature.
        /// </summary>
        /// <param name="instance">The instance who's property is to be looked up</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="bindingFlags">Flags that identify the properties access</param>
        /// <returns>The found property</returns>
        /// <typeparam name="TProperty">The type of the property, for example int</typeparam>
        /// <exception cref="ArgumentException">If the property couldn't be found</exception>
        public static TProperty GetProperty<TProperty>(this object instance, string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance) =>
            GetProperty<TProperty>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, propertyName, bindingFlags);

        /// <summary>
        /// Uses reflection to get a property matching the given signature.
        /// </summary>
        /// <param name="objectType">A type who's property is to be looked up</param>
        /// <param name="instance">The instance who's property is to be retrieved, should be of type objectType</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="bindingFlags">Flags that identify the properties access</param>
        /// <returns>The found property</returns>
        /// <typeparam name="TProperty">The type of the property, for example int</typeparam>
        /// <exception cref="ArgumentException">If the property couldn't be found</exception>
        public static TProperty GetProperty<TProperty>(this Type objectType, object? instance, string propertyName, BindingFlags bindingFlags)
        {
            PropertyInfo? property = objectType.GetProperty(propertyName, bindingFlags);
            if( property == null )
                throw new ArgumentException($"Property {propertyName} was not found for Type {objectType.Name}", nameof(propertyName));

            return (TProperty) property.GetValue(instance, null);
        }

        /// <summary>
        /// Uses reflection to get a field matching the given signature.
        /// </summary>
        /// <param name="instance">The instance who's field is to be looked up</param>
        /// <param name="fieldName">The name of the field</param>
        /// <param name="bindingFlags">Flags that identify the field's access</param>
        /// <returns>The found field</returns>
        /// <typeparam name="TField">The type of the field, for example int</typeparam>
        /// <exception cref="ArgumentException">If the field couldn't be found</exception>
        public static TField GetField<TField>(this object instance, string fieldName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance) =>
            GetField<TField>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, fieldName, bindingFlags);

        /// <summary>
        /// Uses reflection to get a field matching the given signature.
        /// </summary>
        /// <param name="objectType">A type who's field is to be looked up</param>
        /// <param name="instance">The instance who's field is to be looked up</param>
        /// <param name="fieldName">The name of the field</param>
        /// <param name="bindingFlags">Flags that identify the field's access</param>
        /// <returns>The found field</returns>
        /// <typeparam name="TField">The type of the field, for example int</typeparam>
        /// <exception cref="ArgumentException">If the field couldn't be found</exception>
        public static TField GetField<TField>(this Type objectType, object instance, string fieldName, BindingFlags bindingFlags)
        {
            var field = objectType.GetField(fieldName, bindingFlags);
            if( field == null )
                throw new ArgumentException($"Field {fieldName} was not found for Type {objectType.Name}", nameof(fieldName));

            return (TField)field.GetValue(instance);
        }

        /// <summary>
        /// Uses reflection to set a property matching the given signature.
        /// </summary>
        /// <param name="instance">The instance who's property is to be set</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to set</param>
        /// <param name="bindingFlags">Flags that identify the properties access</param>
        /// <typeparam name="TProperty">The type of the property, for example int</typeparam>
        /// <exception cref="ArgumentException">If the property couldn't be found</exception>
        public static void SetProperty<TProperty>(this object instance, string propertyName, TProperty value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance) =>
            SetProperty<TProperty>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, propertyName, value, bindingFlags);

        /// <summary>
        /// Uses reflection to set a property matching the given signature.
        /// </summary>
        /// <param name="objectType">A type who's property is to be looked up</param>
        /// <param name="instance">The instance who's property is to be set, should be of type objectType</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to set</param>
        /// <param name="bindingFlags">Flags that identify the properties access</param>
        /// <returns>The found property</returns>
        /// <typeparam name="TProperty">The type of the property, for example int</typeparam>
        /// <exception cref="ArgumentException">If the property couldn't be found</exception>
        public static void SetProperty<TProperty>(this Type objectType, object? instance, string propertyName, TProperty value, BindingFlags bindingFlags)
        {
            PropertyInfo? property = objectType.GetProperty(propertyName, bindingFlags);
            if( property == null )
                throw new ArgumentException($"Property {propertyName} was not found for Type {objectType.Name}", nameof(propertyName));

            property.SetValue(instance, value);
        }

        /// <summary>
        /// Uses reflection to set a field matching the given signature.
        /// </summary>
        /// <param name="instance">The instance who's field is to be set</param>
        /// <param name="fieldName">The name of the field to be set</param>
        /// <param name="value">The value to set</param>
        /// <param name="bindingFlags">Flags that identify the field's access</param>
        /// <typeparam name="TField">The type of the field, for example int</typeparam>
        /// <exception cref="ArgumentException">If the field couldn't be found</exception>
        public static void SetField<TField>(this object instance, string fieldName, TField value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance) =>
            SetField<TField>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, fieldName, value, bindingFlags);

        /// <summary>
        /// Uses reflection to set a field matching the given signature.
        /// </summary>
        /// <param name="objectType">A type who's field is to be looked up</param>
        /// <param name="instance">The instance who's field is to be set</param>
        /// <param name="fieldName">The name of the field</param>
        /// <param name="value">The value to set</param>
        /// <param name="bindingFlags">Flags that identify the field's access</param>
        /// <typeparam name="TField">The type of the field, for example int</typeparam>
        /// <exception cref="ArgumentException">If the field couldn't be found</exception>
        public static void SetField<TField>(this Type objectType, object? instance, string fieldName, TField value, BindingFlags bindingFlags)
        {
            var field = objectType.GetField(fieldName, bindingFlags);
            if( field == null )
                throw new ArgumentException($"Field {fieldName} was not found for Type {objectType.Name}", nameof(fieldName));

            field.SetValue(instance, value);
        }
    }
}
