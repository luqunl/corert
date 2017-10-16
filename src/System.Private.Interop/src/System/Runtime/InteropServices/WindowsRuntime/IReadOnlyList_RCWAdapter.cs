using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Internal.Runtime.Augments;

namespace System.Runtime.InteropServices.WindowsRuntime
{
    // IReadOnlyCollection is generic, so we can't invoke Count without knowing T.  Instead, we 
    // introduce a new non-generic interface for our adapters, so we can have a non-generic helper 
    // for the call.
    public interface IReadOnlyCollectionAdapter
    {
        int Count { get; }
    }
    
    public interface IReadOnlyListAdapter<out T> : IReadOnlyCollectionAdapter
    {
        T this[int index] { get; }
    }

    [DependencyReductionConditionallyDependent(typeof(ICollection<>))]
    [DependencyReductionConditionallyDependent(typeof(IReadOnlyList<>))]
    internal abstract unsafe class IReadOnlyList_RCWAdapter<T> : __ComGenericInterfaceDispatcher, IReadOnlyList<T>, IReadOnlyListAdapter<T>
    {
        IntPtr m_unsafeThis;
        IterableToEnumerableAdapter<T> m_EnumerableAdapter;

        const int idx_GetAt = 6;
        const int idx_get_Size = 7;

        static Func<IntPtr, uint, T> s_GetAt;


        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)IEnumerableRCWAdapter).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)IEnumerableRCWAdapter).GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");

                s_GetAt = s_GetAt ?? DynamicInteropHelpers.GetDelegateForForwardWinRTMethod<Func<IntPtr, uint, T>>(
                    new InteropMethodData(
                        typeof(T),
                        new InteropParameterData[] { new InteropParameterData(typeof(IntPtr)), new InteropParameterData(typeof(uint)) },
                        Marshallers.InteropCallType.ForwardWinRT),
                    idx_GetAt);

                try
                {
                    return s_GetAt(UnsafeThis, (uint)index);
                }
                catch (Exception ex)
                {
                    if (__HResults.E_BOUNDS == ex.HResult)
                        throw new ArgumentOutOfRangeException("index");

                    throw ex;
                }
            }
        }

        public int Count
        {
            get
            {
                IntPtr target = (*((IntPtr**)UnsafeThis))[idx_get_Size];

                uint unsafeSize;
                int result = CalliIntrinsics.StdCall__int(target, UnsafeThis, (void*)&unsafeSize);

                if (result < 0)
                    throw McgMarshal.GetExceptionForHR(result, true /* IsWinRTScenario */);

                if (unsafeSize > (uint)Int32.MaxValue)
                    throw new InvalidOperationException(SR.Excep_CollectionBackingListTooLarge);

                return (int)unsafeSize;
            }
        }

        #region Helpers
        IntPtr UnsafeThis
        {
            get
            {
                if (m_unsafeThis == IntPtr.Zero)
                    m_unsafeThis = McgMarshal.GetInterface(m_comObject, typeof(IReadOnlyList<T>).TypeHandle);
                return m_unsafeThis;
            }
        }
        IterableToEnumerableAdapter<T> IEnumerableRCWAdapter
        {
            get
            {
                if (m_EnumerableAdapter == null)
                {
                    IntPtr enumerableUnsafeThis = McgMarshal.GetInterface(m_comObject, typeof(IEnumerable<T>).TypeHandle);
                    m_EnumerableAdapter = new IterableToEnumerableAdapter<T>(enumerableUnsafeThis);
                }

                return m_EnumerableAdapter;
            }
        }
        #endregion
    }
}
