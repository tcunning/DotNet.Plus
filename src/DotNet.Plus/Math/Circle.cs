using System;
using System.Runtime.CompilerServices;
using static System.Math;

namespace DotNet.Plus.Math
{
    /// <summary>
    /// Math operations for circles
    /// </summary>
    public static class Circle
    {
        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="angle">The angle to convert in degrees</param>
        /// <returns>The angle in radians</returns>
        /// <example>
        /// <code>
        /// var pi = Circle.DegreeToRadian(180);
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DegreeToRadian(double angle) => PI * angle / 180.0;

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="angle">The angle to convert in degrees</param>
        /// <returns>The angle in radians</returns>
        /// <example>
        /// <code>
        /// var pi = Circle.DegreeToRadian(180);
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RadianToDegree(double angle) => angle * 180.0 / PI ;

    }
}
