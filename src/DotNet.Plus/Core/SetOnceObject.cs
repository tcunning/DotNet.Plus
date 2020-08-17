using System;
using System.Threading;

namespace DotNet.Plus.Core
{
    /// <summary>
    /// Allows an object to only be set once for object types.
    /// </summary>
    /// <typeparam name="TValue">The type of the object to set</typeparam>
    public interface ISetOnceObject<TValue>
        where TValue : class
    {
        /// <summary>
        /// Returns true if the object has been set
        /// </summary>
        bool IsSet { get; }

        /// <summary>
        /// Return the set object.  If the object hasn't been set yet it returns the default
        /// value of TValue (aka null).
        /// </summary>
        TValue? Value { get; }

        /// <summary>
        /// Set the object, but will only set the object one time.
        /// </summary>
        /// <param name="value">The object to set.  An exception will be thrown if this is null w/o
        /// an object already being set.</param>
        /// <returns>True if the object was set by this call, otherwise false is returned which means
        /// the object has already been set.</returns>
        /// <exception cref="ArgumentNullException">If the given value is null AND the value hasn't been
        /// set.</exception>
        bool SetOnce(TValue value);
    }

    /// <summary>
    /// Holds an object that can only be set once, in a thread safe way.
    /// </summary>
    /// <typeparam name="TValue">The type of the object to set</typeparam>
    public class SetOnceObject<TValue> : ISetOnceObject<TValue>
        where TValue : class
    {
        /// <summary>
        /// Return the set object.  If the object hasn't been set yet it returns the default
        /// value of TValue (aka null).
        /// </summary>
        public TValue? Value => _value;
        private TValue? _value = default;

        /// <summary>
        /// Returns true if the object has been set
        /// </summary>
        public bool IsSet => _value != default;

        /// <summary>
        /// Set the object, but will only set the object one time, in a thread safe manor.
        /// </summary>
        /// <param name="value">The object to set.  An exception will be thrown if this is null w/o
        /// an object already being set.</param>
        /// <returns>True if the object was set by this call, otherwise false is returned which means
        /// the object has already been set.</returns>
        /// <exception cref="ArgumentNullException">If the given value is null AND the value hasn't been
        /// set.</exception>
        public bool SetOnce(TValue value)
        {
            var originalValue = Interlocked.CompareExchange(ref this._value, value, default);
            if( originalValue != default )
                return false; // The value has already been set

            if( value == default )
                throw new ArgumentNullException(nameof(value), $"Can't set value to it's default value.");

            return true;
        }
    }


}
