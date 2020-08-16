using System;
using static System.Math;

namespace DotNet.Plus.Math
{
    /// <summary>
    /// Represents the direction to snap the value in.
    /// </summary>
    public enum SnapDirection
    {
        /// <summary>When the value is in the middle of two other values it will round to the upper value.</summary>
        NearestRoundUp,

        /// <summary> When the value is in the middle of two other values it will round to the lower value.</summary>
        NearestRoundDown,

        /// <summary> When the value is in the middle of two other values it will be rounded to the upper value.</summary>
        AlwaysRoundUp,

        /// <summary> When the value is in the middle of two other values it will be rounded to the lower value.</summary>
        AlwaysRoundDown
    }

    /// <summary>
    /// Used to control the rounding of a number
    /// </summary>
    public static class Round
    {
        /// <summary>
        /// Constant used to specify an always round up 
        /// </summary>
        public const int AlwaysRoundUpThreshold = 1;

        /// <summary>
        /// Constant used to specify an always round down 
        /// </summary>
        public const int AlwaysRoundDownThreshold = 0;

        /// <summary>
        /// Converts the give value to an int rounded up or down based on the give threshold.
        /// If the value is less then the threshold the value is rounded down, if the value is
        /// greater then or equal to the threshold the value is rounded up.
        /// </summary>
        /// <example>
        ///     Value   Threshold   Result
        ///     =========================
        ///     1.4        0          1
        ///     1.5        0          1
        ///     1.6        0          1
        ///     1.7        0          1
        ///     1.8        0          1
        /// 
        ///     1.4        1          2
        ///     1.5        1          2
        ///     1.6        1          2
        ///     1.7        1          2
        ///     1.8        1          2
        ///
        ///     1.4        .5         1
        ///     1.5        .5         2
        ///     1.6        .5         2
        ///     1.7        .5         2
        ///     1.8        .5         2
        /// 
        ///     1.4        .6         1
        ///     1.5        .6         1
        ///     1.6        .6         2
        ///     1.7        .6         2
        ///     1.8        .6         2
        ///     
        ///     1.4        .7         1
        ///     1.5        .7         1
        ///     1.6        .7         1
        ///     1.7        .7         2
        ///     1.8        .7         2
        /// </example>
        /// <param name="value">The value that is to be rounded</param>
        /// <param name="threshold">A value from 0.0 to 1.0 used to determine how rounding will be performed.</param>   
        /// <returns>If the threshold is 0.0, a Floor will be performed.  If the value is 1.0, a Ceiling will
        /// be performed.  When the fractional part of the value is less then the threshold the value will be
        /// rounded down.  When the fractional part of the value is greater then or equal to the threshold the
        /// value will be rounded up.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Will be returned if the threshold greater then 0.0 or less then 1.0</exception>
        public static int ToInt(this double value, double threshold = 0.5)
        {
            if( threshold < 0.0 )
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be > 0");

            if( threshold > 1.0 )
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be < 1");

            if( threshold < double.Epsilon )
                return (int)Floor(value);

            if( threshold >= 1.0 )
                return (int)Ceiling(value);

            var floor = Floor(value);
            return value - floor >= threshold ? (int)floor + 1 : (int)floor;
        }

        /// <summary>
        /// Converts the give value to an int rounded up or down based on the give threshold.
        /// If the value is less then the threshold the value is rounded down, if the value is
        /// greater then or equal to the threshold the value is rounded up.
        /// </summary>
        /// <example>
        ///     Value   Threshold   Result
        ///     =========================
        ///     1.4        0          1
        ///     1.5        0          1
        ///     1.6        0          1
        ///     1.7        0          1
        ///     1.8        0          1
        /// 
        ///     1.4        1          2
        ///     1.5        1          2
        ///     1.6        1          2
        ///     1.7        1          2
        ///     1.8        1          2
        ///
        ///     1.4        .5         1
        ///     1.5        .5         2
        ///     1.6        .5         2
        ///     1.7        .5         2
        ///     1.8        .5         2
        /// 
        ///     1.4        .6         1
        ///     1.5        .6         1
        ///     1.6        .6         2
        ///     1.7        .6         2
        ///     1.8        .6         2
        ///     
        ///     1.4        .7         1
        ///     1.5        .7         1
        ///     1.6        .7         1
        ///     1.7        .7         2
        ///     1.8        .7         2
        /// </example>
        /// <param name="value">The value that is to be rounded</param>
        /// <param name="threshold">A value from 0.0 to 1.0 used to determine how rounding will be performed.</param>   
        /// <returns>If the threshold is 0.0, a Floor will be performed.  If the value is 1.0, a Ceiling will
        /// be performed.  When the fractional part of the value is less then the threshold the value will be
        /// rounded down.  When the fractional part of the value is greater then or equal to the threshold the
        /// value will be rounded up.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Will be returned if the threshold greater then 0.0 or less then 1.0</exception>
        public static int ToInt(this float value, float threshold = 0.5f)
        {
            if( threshold < 0.0f )
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be > 0");

            if( threshold > 1.0f )
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be < 1");

            if( threshold < float.Epsilon )
                return (int)Floor(value);

            if( threshold >= 1.0f )
                return (int)Ceiling(value);

            var floor = (float)Floor(value);
            return value - floor >= threshold ? (int)floor + 1 : (int)floor;
        }

        /// <summary>
        /// Converts the give value to an int rounded up or down based on the give threshold.
        /// If the value is less then the threshold the value is rounded down, if the value is
        /// greater then or equal to the threshold the value is rounded up.
        /// </summary>
        /// <example>
        ///     Value   Threshold   Result
        ///     =========================
        ///     1.4        0          1
        ///     1.5        0          1
        ///     1.6        0          1
        ///     1.7        0          1
        ///     1.8        0          1
        /// 
        ///     1.4        1          2
        ///     1.5        1          2
        ///     1.6        1          2
        ///     1.7        1          2
        ///     1.8        1          2
        ///
        ///     1.4        .5         1
        ///     1.5        .5         2
        ///     1.6        .5         2
        ///     1.7        .5         2
        ///     1.8        .5         2
        /// 
        ///     1.4        .6         1
        ///     1.5        .6         1
        ///     1.6        .6         2
        ///     1.7        .6         2
        ///     1.8        .6         2
        ///     
        ///     1.4        .7         1
        ///     1.5        .7         1
        ///     1.6        .7         1
        ///     1.7        .7         2
        ///     1.8        .7         2
        /// </example>
        /// <param name="value">The value that is to be rounded</param>
        /// <param name="threshold">A value from 0.0 to 1.0 used to determine how rounding will be performed.</param>   
        /// <returns>If the threshold is 0.0, a Floor will be performed.  If the value is 1.0, a Ceiling will
        /// be performed.  When the fractional part of the value is less then the threshold the value will be
        /// rounded down.  When the fractional part of the value is greater then or equal to the threshold the
        /// value will be rounded up.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Will be returned if the threshold greater then 0.0 or less then 1.0</exception>
        public static int ToInt(this decimal value, decimal threshold = 0.5M)
        {
            if( threshold < 0.0M )
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be > 0");

            if( threshold > 1.0M )
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be < 1");

            if( threshold <= 0.0M )
                return (int)Floor(value);

            if( threshold >= 1.0M )
                return (int)Ceiling(value);

            var floor = Floor(value);
            return value - floor >= threshold ? (int)floor + 1 : (int)floor;
        }

        /// <summary>
        /// Snaps the given value to be one of the values in the given list based on the snapDirection.
        /// </summary>
        /// <param name="value">The value to snap</param>
        /// <param name="snapDirection">Used to control how the given value snaps</param>
        /// <param name="snapList">A sorted list from low to high of values to snap to or an empty list</param>
        /// <returns>A value from the snapList that the passed in value was "snapped".  If the snapList is
        /// empty, the given value is return unchanged.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Will be returned if when resolving the
        /// snap value the snap list encounters an out of order snapList OR an invalid snapDirection. </exception>
        public static int SnapTo(this int value, SnapDirection snapDirection, params int[] snapList )
        {
            if( snapList == null || snapList.Length == 0 )
                return value;

            var index = 0;
            var snapValueFirst = snapList[index++];

            if( snapList.Length == 1 || value <= snapValueFirst )
                return snapValueFirst;

            do
            {
                var snapValueSecond = snapList[index++];

                if( snapValueFirst >= snapValueSecond )
                    throw new ArgumentOutOfRangeException($"Expected {snapValueFirst} to be less then {snapValueSecond}");

                if( value == snapValueSecond )
                    return snapValueSecond;

                if( value < snapValueSecond )
                {
                    var firstDiff = Abs(snapValueFirst - value);
                    var secondDiff = Abs(snapValueSecond - value);

                    int SnapToNearest() => (firstDiff < secondDiff ? snapValueFirst : snapValueSecond);

                    switch( snapDirection )
                    {
                        case SnapDirection.NearestRoundUp:
                            return firstDiff == secondDiff ? snapValueSecond : SnapToNearest();
                        case SnapDirection.NearestRoundDown:
                            return firstDiff == secondDiff ? snapValueFirst : SnapToNearest();
                        case SnapDirection.AlwaysRoundDown:
                            return snapValueFirst;
                        case SnapDirection.AlwaysRoundUp:
                            return snapValueSecond;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(snapDirection), snapDirection, "Unknown snap direction");
                    }
                }

                snapValueFirst = snapValueSecond;
            } while( index < snapList.Length );

            return snapValueFirst;
        }

        /// <summary>
        /// Snaps the given value to be one of the values in the given list based on the snapDirection.
        /// </summary>
        /// <param name="value">The value to snap</param>
        /// <param name="snapDirection">Used to control how the given value snaps</param>
        /// <param name="snapList">A sorted list from low to high of values to snap to or an empty list</param>
        /// <returns>A value from the snapList that the passed in value was "snapped".  If the snapList is
        /// empty, the given value is return unchanged.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Will be returned if when resolving the
        /// snap value the snap list encounters an out of order snapList OR an invalid snapDirection. </exception>
        public static double SnapTo(this double value, SnapDirection snapDirection, params double[] snapList)
        {
            if( snapList == null || snapList.Length == 0 )
                return value;

            var index = 0;
            var snapValueFirst = snapList[index++];

            if( snapList.Length == 1 || value <= snapValueFirst )
                return snapValueFirst;

            do
            {
                var snapValueSecond = snapList[index++];

                if( snapValueFirst >= snapValueSecond )
                    throw new ArgumentOutOfRangeException($"Expected {snapValueFirst} to be less then {snapValueSecond}");

                if( Abs(value - snapValueSecond) < double.Epsilon )
                    return snapValueSecond;

                if( value < snapValueSecond )
                {
                    var firstDiff = Abs(snapValueFirst - value);
                    var secondDiff = Abs(snapValueSecond - value);

                    bool IsEqualDistance() => Abs(firstDiff - secondDiff) < double.Epsilon;
                    double SnapToNearest() => (firstDiff < secondDiff ? snapValueFirst : snapValueSecond);

                    switch( snapDirection )
                    {
                        case SnapDirection.NearestRoundUp:
                            return IsEqualDistance() ? snapValueSecond : SnapToNearest();
                        case SnapDirection.NearestRoundDown:
                            return IsEqualDistance() ? snapValueFirst : SnapToNearest();
                        case SnapDirection.AlwaysRoundDown:
                            return snapValueFirst;
                        case SnapDirection.AlwaysRoundUp:
                            return snapValueSecond;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(snapDirection), snapDirection, "Unknown snap direction");
                    }
                }

                snapValueFirst = snapValueSecond;
            } while( index < snapList.Length );

            return snapValueFirst;
        }
    }
}
