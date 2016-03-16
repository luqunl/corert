// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Runtime
{
    internal static class BinderIntrinsics
    {
#if CORERT
        public static void DebugBreak()
        {
            EH.FallbackFailFast(RhFailFastReason.InternalError, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TailCall_RhpThrowEx(Exception e)
        {
            throw e;
        }
#else // CORERT
        // NOTE: We rely on the RID of DebugBreak, so it must be kept as the first method declared in this class.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void DebugBreak()
        {
            // Ignore body of method, it will be generated by the binder.
        }

        // NOTE: We rely on the RID of GetReturnAddress, so it must be kept as the second method declared in this class.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static IntPtr GetReturnAddress()
        {
            // Ignore body of method, it will be generated by the binder.
            return IntPtr.Zero;
        }

        // NOTE: We rely on the RID of TailCall_RhpThrowEx, so it must be kept as the third method declared in this class.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void TailCall_RhpThrowEx(Exception e)
        {
            // Ignore body of method, it will be generated by the binder.
        }
#endif // CORERT
    }
}
