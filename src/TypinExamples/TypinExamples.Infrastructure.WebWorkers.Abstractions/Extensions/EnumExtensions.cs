namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Extensions
{
    using System;
    using System.Runtime.CompilerServices;

    public static class EnumExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static unsafe bool HasFlags<T>(T* first, T* second) where T : unmanaged, Enum
        {
            byte* pf = (byte*)first;
            byte* ps = (byte*)second;

            for (int i = 0; i < sizeof(T); ++i)
            {
                if ((pf[i] & ps[i]) != ps[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <remarks>Faster analog of Enum.HasFlag</remarks>
        /// <inheritdoc cref="Enum.HasFlag"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasFlags<T>(this T first, T second) where T : unmanaged, Enum
        {
            return HasFlags(&first, &second);
        }
    }
}
