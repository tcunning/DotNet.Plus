using System;
using DotNet.Plus.Core;

namespace DotNet.Plus.Pattern
{
    /// <summary>
    /// A version of IDisposable that can be used to determine if the instance has been disposed.
    /// </summary>
    public interface ICommonDisposable : IDisposable
    {
        /// <summary>
        /// Returns true if the instance has been disposed, other returns false.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// If the instance hasn't already been disposed, this will attempt to dispose it and silently
        /// handle any exceptions.
        /// </summary>
        void TryDispose();
    }

    /// <summary>
    /// A base abstract implementation of <see cref="ICommonDisposable"/>.   This includes it's own
    /// implementation of Dispose() that manages the IsDisposed state and delegates the dispose cleanup
    /// to the abstract method Dispose(bool disposing).
    /// their cleanup.
    /// </summary>
    public abstract class CommonDisposable : ICommonDisposable
    {
        private readonly ISetOnceValue<bool> _isDisposed;

        public bool IsDisposed => _isDisposed.IsSet;

        protected CommonDisposable() : this(new SetOnceValue<bool>())
        {
        }

        protected CommonDisposable(ISetOnceValue<bool> setOnceValue)
        {
            if( setOnceValue.IsSet )
                throw new ArgumentException(nameof(setOnceValue), $"Can't construct {this.GetType()} because {nameof(setOnceValue)} is already set");
            _isDisposed = setOnceValue;
        }

        /// <summary>
        /// Disposes the instance and silently handles any exceptions.
        /// </summary>
        public void TryDispose()
        {
            try
            {
                if( IsDisposed )
                    return;
                Dispose();
            }
            catch { /* ignored */ }
        }

        /// <summary>
        /// Implements the dispose method for <see cref="IDisposable"/>.  If the instance is already
        /// disposed, the method just returns.  Otherwise, it sets the IsDisposed flag and invokes the new
        /// Dispose(bool disposing) method with disposing set to true.  
        /// </summary>
        public void Dispose()
        {
            if( _isDisposed.SetOnce(true) )
                Dispose(true);
            else
                throw new ObjectDisposedException(nameof(this.GetType));
        }

        protected abstract void Dispose(bool disposing);
    }

}
