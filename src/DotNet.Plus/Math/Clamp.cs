using System;
using System.Runtime.CompilerServices;

namespace DotNet.Plus.Math
{
    /// <summary>
    /// Clamps values so they are within given min/max range
    /// </summary>
    public static class Clamp
    {
        /// <summary>
        /// Clamps a value so that it's in the give min/max range.  If the value is below the
        /// minimum value, the minimum value is returned.  If the value is above the maximum
        /// value, the maximum value will be returned.  If the value is within the minimum and
        /// maximum, the value will be returned unchanged.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value that will be returned</param>
        /// <param name="max">The maximum value that will be returned</param>
        /// <returns>Returns the passed in value, but forces it to be within the given min
        /// and max range.</returns>
        /// <example>
        /// <code>
        /// uint value = 55;
        /// var clampedValue = Clamp.Value(value, 10, 100);
        /// </code>
        /// </example>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the given minimum
        /// value is greater then the given maximum value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue Value<TValue>(TValue value, TValue min, TValue max) where TValue : struct, IComparable
        {
            if( min.CompareTo(max) > 0 ) // min is greater then max
                throw new ArgumentOutOfRangeException(nameof(max), $"Max value needs to be larger then min value");

            if( value.CompareTo(min) < 0 )
                return min;

            if( value.CompareTo(max) > 0 )
                return max;

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ValueInt(int value, int min, int max)
        {
            if (min > max) 
                throw new ArgumentOutOfRangeException(nameof(max), $"Max value needs to be larger then min value");

            if (value <= min)
                return min;

            if (value >= max)
                return max;

            return value;
        }

    }
}
