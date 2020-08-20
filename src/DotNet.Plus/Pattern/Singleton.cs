using System;
using System.Linq;
using System.Reflection;

namespace DotNet.Plus.Pattern
{
    /// <inheritdoc />
    /// <summary>
    /// Exception thrown by Singleton when derived type does not contain a non-public default constructor.
    /// </summary>
    public class ConstructorException : Exception
    {
        public ConstructorException(string message, Exception? innerException = null) : base(message, innerException) { }
    }

    /// <summary>
    /// <para>Implements a reusable Singleton pattern.  TSingleton must define a private default constructor,
    /// otherwise a runtime exception will be thrown on construction of the singleton.</para>
    /// 
    /// <para>The concept for this class was based on <seealso cref="https://stackoverflow.com/questions/2319075/generic-singletont"/>
    /// for the reflection technique, and from Microsoft <seealso cref="https://msdn.microsoft.com/en-us/library/ff650316.aspx"/>
    /// for making it thread safe.</para>
    ///
    /// <para>With this solution there is currently no way to validate that the derived class is the same type as the given
    /// TSingleton.  The Single is created solely based on the given TSingleton type.</para>
    /// </summary>
    /// <typeparam name="TSingleton">The type of the singleton to create</typeparam>
    /// <exception cref="ConstructorException">Thrown if TSingleton doesn't have a default constructor or there is an error invoking it.</exception>
    public class Singleton<TSingleton>
        where TSingleton : Singleton<TSingleton>
    {
        private static volatile TSingleton? _instance;

        /// <summary>
        /// Returns the instance and creates it if it hasn't already been created in a thread safe way.  
        /// </summary>
        public static TSingleton Instance
        {
            get
            {
                if( _instance != null )
                    return _instance;

                lock( typeof(TSingleton) )
                {
                    if( _instance != null )
                        return _instance;

                    if( HasPublicConstructor() )
                        throw new ConstructorException($"Singleton type {typeof(TSingleton).Name} has at least one public constructor so it can't be used as singleton.");

                    var instance = InvokeDefaultPrivateConstructor();
                    _instance = instance;

                    return instance;
                }
            }
        }

        /// <summary>
        /// Returns true if the singleton has a private constructor.  Public constructors aren't allowed in a singleton definition
        /// </summary>
        /// <returns>Returns true if the singleton has a private constructor</returns>
        private static bool HasPublicConstructor()
        {
            var publicConstructors = typeof(TSingleton).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            return publicConstructors.Any();
        }

        /// <summary>
        /// Uses reflection to get the default private constructor of TSingleton and invokes it in order
        /// to create an instance of it.
        /// </summary>
        /// <returns>An instance of type TSingleton</returns>
        /// <exception cref="ConstructorException">Thrown if TSingleton doesn't have a default constructor or there is an error invoking it.</exception>
        internal static TSingleton InvokeDefaultPrivateConstructor()
        {
            var privateConstructors = typeof(TSingleton).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            var defaultPrivateConstructor = privateConstructors.FirstOrDefault((ci) => ci.GetParameters().Length == 0);
            if( defaultPrivateConstructor == null )
                throw new ConstructorException($"Singleton type {typeof(TSingleton).Name} has no private default constructor.");

            try {
                return (TSingleton)defaultPrivateConstructor.Invoke(new object[] { });
            }
            catch( Exception ex ) {
                throw new ConstructorException($"Singleton type {typeof(TSingleton).Name} error invoking private constructor", ex);
            }
        }
    }
}
