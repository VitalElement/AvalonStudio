using System;
using System.Collections;

namespace CorApi.Portable
{
    /** Exposes an enumerator for Types. */
    public class TypeEnumerator : IEnumerable, IEnumerator
    {
        private TypeEnum m_enum;
        private Type m_ty;

        internal TypeEnumerator(TypeEnum typeEnumerator)
        {
            m_enum = typeEnumerator;
        }

        //
        // ICloneable interface
        //
        public Object Clone()
        {
            Enum clone = null;
            if (m_enum != null)
                m_enum.Clone(out clone);
            return new TypeEnumerator((TypeEnum)clone);
        }

        //
        // IEnumerable interface
        //
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        //
        // IEnumerator interface
        //
        public bool MoveNext()
        {
            if (m_enum == null)
                return false;

            Type[] a = new Type[1];
            int c = 0;
            m_enum.Next(a.Length, a, out c);
            if (c == 1) // S_OK && we got 1 new element
                m_ty = new Type(a[0].NativePointer);
            else
                m_ty = null;
            return m_ty != null;
        }

        public void Reset()
        {
            if (m_enum != null)
                m_enum.Reset();
            m_ty = null;
        }

        public void Skip(int celt)
        {
            m_enum.Skip(celt);
            m_ty = null;
        }

        public Object Current
        {
            get
            {
                return m_ty;
            }
        }

        // Returns total elements in the collection.
        public int Count
        {
            get
            {
                if (m_enum == null)
                {
                    return 0;
                }

                return m_enum.Count;
            }
        }
    }
}
