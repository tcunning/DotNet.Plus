using System;
using System.Threading;

namespace DotNet.Plus.Core
{
    /// <summary>
    /// Allows a value to only be set once for value types.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to set</typeparam>
    public interface ISetOnceValue<TValue>
        where TValue : struct, IConvertible
    {
        /// <summary>
        /// Returns true if the object has been set
        /// </summary>
        bool IsSet { get; }

        /// <summary>
        /// Return the set value.  If the value hasn't been set yet it returns the default
        /// value of TValue.
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Set the value, but will only set the value one time.
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <returns>True if the value was set by this call, otherwise false is returned which means
        /// the value has already been set.</returns>
        bool SetOnce(TValue value);
    }

    /// <summary>
    /// Allows a value to only be set once, in a thread safe way, for value types.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to set</typeparam>
    public class SetOnceValue<TValue> : ISetOnceValue<TValue>
        where TValue : struct, IConvertible
    {
        private object? _valueObject = default;

        /// <summary>
        /// Return the set value.  If the value hasn't been set yet it returns the default
        /// value of TValue.
        /// </summary>
        public TValue Value =>
            _valueObject == null ?
            default : 
            (TValue)(_valueObject = (TValue)Convert.ChangeType(_valueObject, typeof(TValue)));

        /// <summary>
        /// Returns true if the value has been set
        /// </summary>
        public bool IsSet => _valueObject != default;

        /// <summary>
        /// Set the value, but will only set the value one time, in a thread safe manor.
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <returns>True if the value was set by this call, otherwise false is returned which means
        /// the value has already been set.</returns>
        public bool SetOnce(TValue value)
        {
            var originalValue = Interlocked.CompareExchange(ref this._valueObject, value, default);
            return originalValue == default;
        }
    }


}
