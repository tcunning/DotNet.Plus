using System;
using System.Reflection;

namespace DotNet.Plus.Core
{
    public static class Reflection
    {
        public static MethodInfo? TryGetMethod(this Type type, string name, BindingFlags bindingFlags, params Type[] argTypes)
        {
            return Operation.TryCatch(() =>
            {
                var method = type.GetMethod(name, bindingFlags, binder: null, types: argTypes, modifiers: null);
                return method;
            });
        }



        public static TProperty GetProperty<TProperty>(this object instance, string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance) =>
            GetProperty<TProperty>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, propertyName, bindingFlags);

        public static TProperty GetProperty<TProperty>(this Type objectType, object? instance, string propertyName, BindingFlags bindingFlags)
        {
            PropertyInfo? property = objectType.GetProperty(propertyName, bindingFlags);
            if( property == null )
                throw new ArgumentException($"Property {propertyName} was not found for Type {objectType.Name}", nameof(propertyName));

            return (TProperty) property.GetValue(instance, null);
        }



        public static TField GetField<TField>(this object instance, string fieldName, BindingFlags bindingFlags) =>
            GetField<TField>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, fieldName, bindingFlags);

        public static TField GetField<TField>(this Type objectType, object instance, string fieldName, BindingFlags bindingFlags)
        {
            var field = objectType.GetField(fieldName, bindingFlags);
            if( field == null )
                throw new ArgumentException($"Field {fieldName} was not found for Type {objectType.Name}", nameof(fieldName));

            return (TField)field.GetValue(instance);
        }



        public static void SetProperty<TProperty>(this object instance, string propertyName, TProperty value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance) =>
            SetProperty<TProperty>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, propertyName, value, bindingFlags);

        public static void SetProperty<TProperty>(this Type objectType, object? instance, string propertyName, TProperty value, BindingFlags bindingFlags)
        {
            PropertyInfo? property = objectType.GetProperty(propertyName, bindingFlags);
            if( property == null )
                throw new ArgumentException($"Property {propertyName} was not found for Type {objectType.Name}", nameof(propertyName));

            property.SetValue(instance, value);
        }


        public static void SetField<TProperty>(this object instance, string fieldName, TProperty value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance) =>
            SetField<TProperty>(instance?.GetType() ?? throw new ArgumentNullException(nameof(instance)), instance, fieldName, value, bindingFlags);

        public static void SetField<TField>(this Type objectType, object? instance, string fieldName, TField value, BindingFlags bindingFlags)
        {
            var field = objectType.GetField(fieldName, bindingFlags);
            if( field == null )
                throw new ArgumentException($"Field {fieldName} was not found for Type {objectType.Name}", nameof(fieldName));

            field.SetValue(instance, value);
        }

    }
}
