using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Windows.Foundation.Collections
{
    [ComImport]
    [Guid("02b51929-c1c4-4a7e-8940-0312b5c18500")]
    [WindowsRuntimeImport]
    public unsafe interface IKeyValuePair<K, V>
    {
        K get_Key();
        V get_Value();
    }
}
