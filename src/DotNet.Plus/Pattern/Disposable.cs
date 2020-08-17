using System;

namespace DotNet.Plus.Pattern
{
    public static class Disposable
    {
        /// <summary>
        /// An extension method on all <see cref="IDisposable"/> that disposes the instance
        /// and silently handles any exceptions.
        /// </summary>
        /// <param name="instance">The instance to dispose</param>
        public static void TryDispose(this IDisposable instance)
        {
            try {
                instance.Dispose();
            }
            catch { /* ignored */ }
        }
    }
}
