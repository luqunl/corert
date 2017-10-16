  

    namespace System.Runtime.InteropServices.WindowsRuntime
	{
  /// <summary>
    /// A managed wrapper for IPropertyValue and IReference<T>
    /// </summary>
    [System.Runtime.CompilerServices.DependencyReductionRootAttribute]
	// TODO: FIX
    // [McgInternalTypeAttribute]
    public class ReferenceImpl<T> : PropertyValueImpl, global::Windows.Foundation.IReference<T>
    {
        private T m_value;

        public ReferenceImpl(T data, int type)
            : base(data, type)
        {
            m_unboxed = true;
            m_value = data;
        }

        internal ReferenceImpl(T data, global::Windows.Foundation.PropertyType type)
            : base(data, (int)type)
        {
            m_unboxed = true;
            m_value = data;
        }

        public T get_Value()
        {
            if (!m_unboxed)
            {
                m_value = (T)m_data;
                m_unboxed = true;
            }
            return m_value;
        }
   } 
	}
