using System;
using System.Threading.Tasks;

namespace DotNet.Plus.Core
{
    public static class TaskOperation
    {
        /// <summary>
        /// <para>This method executes the given operation in a try/catch block and catches all exceptions.  If an
        /// exception does occur the given defaultValue is returned.</para>
        ///
        /// <para>
        /// NOTE: While this is a simple pattern, it was introduced to better support test code coverage analysis.
        ///       It was sometimes challenging to devices tests that always properly testing the returning of the
        ///       default value.  By abstracting this pattern our, we can unit test this pattern independent of
        ///       where it is used.</para>
        /// </summary>
        /// <typeparam name="TValue">The Type of the value that should be returned</typeparam>
        /// <param name="operation">The operation to perform</param>
        /// <param name="defaultValue">The default value that should be returned when an exception is caught.</param>
        /// <returns>The value for the operation or the defaultValue if the operation throws</returns>
        public static async Task<TValue> TryCatchAsync<TValue>(Func<Task<TValue>> operation, TValue defaultValue = default)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch { /* ignored */ }

            return defaultValue;
        }

        /// <summary>
        /// <para>This method executes the given operation in a try/catch block and catches all exceptions.  If an
        /// exception does occur the given defaultValue is returned.</para>
        ///
        /// <para>
        /// NOTE: While this is a simple pattern, it was introduced to better support test code coverage analysis.
        ///       It was sometimes challenging to devices tests that always properly testing the returning of the
        ///       default value.  By abstracting this pattern our, we can unit test this pattern independent of
        ///       where it is used.</para>
        /// </summary>
        /// <typeparam name="TValue">The Type of the value that should be returned</typeparam>
        /// <param name="operation">The operation to perform that doesn't return a result</param>
        /// <param name="successValue">Value to return on success.</param>
        /// <param name="defaultValue">The default value that should be returned when an exception is caught.</param>
        /// <returns>The value for the operation or the defaultValue if the operation throws</returns>
        public static async Task<TValue> TryCatchAsync<TValue>(Func<Task> operation, TValue successValue, TValue defaultValue = default)
        {
            try
            {
                await operation().ConfigureAwait(false);
                return successValue;
            }
            catch { /* ignored */ }

            return defaultValue;
        }

        /// <summary>
        /// <para>This method executes the given operation in a try/catch block and catches all exceptions.  If an
        /// exception does occur the given defaultValue is returned.</para>
        ///
        /// <para>
        /// NOTE: While this is a simple pattern, it was introduced to better support test code coverage analysis.
        ///       It was sometimes challenging to devices tests that always properly testing the returning of the
        ///       default value.  By abstracting this pattern our, we can unit test this pattern independent of
        ///       where it is used.</para>
        /// </summary>
        /// <param name="operation">The operation to perform that doesn't return a result</param>
        /// <returns>The task from the operation</returns>
        public static async Task TryCatchAsync(Func<Task> operation)
        {
            try
            {
                await operation().ConfigureAwait(false);
            }
            catch { /* ignored */ }
        }
    }
}
