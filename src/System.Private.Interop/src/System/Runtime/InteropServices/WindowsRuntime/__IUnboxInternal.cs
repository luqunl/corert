using System;
using System.Runtime.InteropServices;

namespace System.Runtime.InteropServices.WindowsRuntime
{
    /// <summary>
    /// Interface to help get unboxed value from IReference<T>/IReferenceArray<T>/IKeyValuePair<K,V>
    /// </summary>
//    [System.Runtime.CompilerServices.DipendencyReductionRootAttribute]
    internal interface __IUnboxInternal
    {
        Object get_Value(Object obj);
    }
}
