using System;
using System.Threading.Tasks;

namespace DotNet.Plus.Core
{
    /// <summary>
    /// Helpers to wrap try/catch operations.  This is a more functionality approach to try/catch management
    /// and helps in unit testing failures.
    /// </summary>
    public static class TaskOperation
    {
        /// <summary>
        /// <para>This method executes the given operation in a try/catch block and catches all exceptions.  If an
        /// exception does occur the given defaultValue is returned.</para>
        /// </summary>
        /// <typeparam name="TValue">The Type of the value that should be returned</typeparam>
        /// <param name="operation">The operation to perform</param>
        /// <param name="failureValue">The default value that should be returned when an exception is caught.</param>
        /// <returns>The value for the operation or the defaultValue if the operation throws</returns>
        public static async Task<TValue> TryCatchAsync<TValue>(this Task<TValue> operation, TValue failureValue = default)
        {
            try
            {
                return await operation.ConfigureAwait(false);
            }
            catch { /* ignored */ }

            return failureValue;
        }

        /// <summary>
        /// <para>This method executes the given operation in a try/catch block and catches all exceptions.  If an
        /// exception does occur the given defaultValue is returned.</para>
        /// </summary>
        /// <typeparam name="TValue">The Type of the value that should be returned</typeparam>
        /// <param name="operation">The operation to perform that doesn't return a result</param>
        /// <param name="successValue">Value to return on success.</param>
        /// <param name="failureValue">The default value that should be returned when an exception is caught.</param>
        /// <returns>The value for the operation or the defaultValue if the operation throws</returns>
        public static async Task<TValue> TryCatchAsync<TValue>(this Task operation, TValue successValue, TValue failureValue = default)
        {
            try
            {
                await operation.ConfigureAwait(false);
                return successValue;
            }
            catch { /* ignored */ }

            return failureValue;
        }

        /// <summary>
        /// <para>This method executes the given operation in a try/catch block and catches all exceptions.  If an
        /// exception does occur the given defaultValue is returned.</para>
        /// </summary>
        /// <param name="operation">The operation to perform that doesn't return a result</param>
        /// <returns>The task from the operation</returns>
        public static async Task TryCatchAsync(this Task operation)
        {
            try
            {
                await operation.ConfigureAwait(false);
            }
            catch { /* ignored */ }
        }
    }
}
