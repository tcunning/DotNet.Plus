using System;
using DotNet.Plus.Core;

namespace DotNet.Plus.Pattern
{
    /// <summary>
    /// Adds extensions to IDisposable such as for allowing easy dispose
    /// </summary>
    public static class Disposable
    {
        /// <summary>
        /// An extension method on all <see cref="IDisposable"/> that disposes the instance
        /// and silently handles any exceptions.
        /// </summary>
        /// <param name="instance">The instance to dispose</param>
        public static void TryDispose(this IDisposable instance) =>
            Operation.TryCatch(instance.Dispose);
    }
}
