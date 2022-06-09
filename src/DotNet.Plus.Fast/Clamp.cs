using System.Runtime.CompilerServices;

namespace DotNet.Plus.Fast
{
    public static class Clamp
    {
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern int Value(int number, int min, int max);

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static extern TValue Value<TValue>(TValue number, TValue min, TValue max) where TValue : struct;
    }
}
