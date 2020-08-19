using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Plus.Core
{
    public static class Operation
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
        public static TValue TryCatch<TValue>(Func<TValue> operation, TValue defaultValue = default)
        {
            try
            {
                return operation();
            }
            catch { /* ignored */ }

            return defaultValue;
        }
    }
}
